﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class ConsultantRepository
    {

        public Consultant Find(int id)
        {
            return ConsultantMockData.Contractors.SingleOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Find alumni consultant candidates for a specific client.
        /// Does not include candidates that have current active or pending contracts with the specifed client.
        /// </summary>
        /// <param name="query">Text to search for in candidate name or contract specialization.</param>
        /// <param name="clientId">Client company ID that alumni must have worked for in the past.</param>
        /// <returns>A list of consultants, grouped by specialization.</returns>
        public IEnumerable<ConsultantGroup> FindAlumni(string query, int clientId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                string contractQuery = @"SELECT DISTINCT U.UserID Id, U.FirstName, U.LastName, "
                                             + "A.CandidateID ConsultantId, A.CompanyID ClientId, "
                                             + "A.StartDate, A.EndDate, CRD.BillRate Rate, S.Name SpecializationName "
                                             + "FROM [Users] AS U, [Agreement] AS A, "
                                             + "[Agreement_ContractDetail] AS CD, "
                                             + "[Agreement_ContractRateDetail] AS CRD, [Specialization] as S "
                                             + "WHERE U.UserID=A.CandidateID "
                                             + "AND A.AgreementID=CD.AgreementID "
                                             + "AND A.AgreementID=CRD.AgreementID "
                                             + "AND CD.SpecializationID=S.SpecializationID "
                                             //Only FloThru contracts
                                             + "AND A.AgreementType=" + MatchGuideConstants.AgreementTypes.Contract + " "
                                             + "AND A.AgreementSubType=" + MatchGuideConstants.AgreementSubTypes.FloThru + " "
                                             //That are between the candidate and the client
                                             + "AND A.CompanyID=@CompanyId "
                                             + "AND A.Inactive = 0 "
                                             //Text query used to match on full name or contract specialization name
                                             + "AND ( (U.FirstName+' '+U.LastName) LIKE @Query "
                                             + "OR S.Name LIKE @Query) "
                                            //Filter CandidateIDs with active or pending contracts with client
                                             + "AND U.UserID NOT IN ("
                                                 + "SELECT A.CandidateID FROM [Agreement] AS A "
                                                 + "WHERE A.CompanyID=@CompanyID "
                                                 + "AND A.AgreementType=" + MatchGuideConstants.AgreementTypes.Contract+" "
                                                 + "AND A.AgreementSubType=" + MatchGuideConstants.AgreementSubTypes.FloThru + " "
                                                 + "AND (A.StatusType=" + MatchGuideConstants.ContractStatusTypes.Active + " "
                                                 + "OR A.StatusType=" + MatchGuideConstants.ContractStatusTypes.Pending + ") "
                                                 + "AND A.EndDate > @Today "
                                                 + "AND A.Inactive = 0 "
                                             + ")";

                //query and map contracts to consultants
                var consultantLookup = new Dictionary<int, Consultant>();
                db.Connection.Query<Consultant, Contract, Consultant>(contractQuery,
                    (c, contract) =>
                    {
                        Consultant consultant;
                        if (!consultantLookup.TryGetValue(c.Id, out consultant))
                        {
                            consultantLookup.Add(c.Id, consultant = c);
                        }
                        consultant.Contracts.Add(contract);
                        return consultant;
                    },
                    new
                    {
                        CompanyId = clientId,
                        Today = DateTime.UtcNow,
                        Query = "%" + query + "%"
                    },
                    splitOn: "ConsultantId");

                return GroupConsultantsByContractSpecialization(consultantLookup.Values);
            }
        }

        private static IEnumerable<ConsultantGroup> GroupConsultantsByContractSpecialization(IEnumerable<Consultant> matchingConsultants)
        {
            var groups = new Dictionary<string, ConsultantGroup>();

            foreach (var consultant in matchingConsultants)
            {
                foreach (var specializationName in consultant.Contracts.Select(c => c.SpecializationName).Distinct())
                {
                    if (!groups.ContainsKey(specializationName))
                    {
                        groups.Add(specializationName, new ConsultantGroup { Specialization = specializationName });
                    }
                    var group = groups[specializationName];
                    @group.Consultants.Add(new ConsultantSummary(consultant, specializationName));
                }
            }
            return groups.Select(g => g.Value).OrderBy(g => g.Specialization);
        }
    }
}
