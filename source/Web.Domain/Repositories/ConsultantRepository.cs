﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface IConsultantRepository
    {
        Consultant Find(int id);

        /// <summary>
        /// Find alumni consultant candidates for a specific client.
        /// Does not include candidates that have current active or pending contracts with the specifed client.
        /// </summary>
        /// <param name="query">Text to search for in candidate name or contract specialization.</param>
        /// <param name="clientIds">Client company IDs that alumni must have worked for in the past.</param>
        /// <returns>A list of consultants, grouped by specialization.</returns>
        IEnumerable<ConsultantGroup> FindAlumni(string query, IEnumerable<int> clientIds);
    }

    public class ConsultantRepository : IConsultantRepository
    {
        public Consultant Find(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                string consultantQuery = @"SELECT U.UserID Id, U.FirstName, U.LastName, UE.PrimaryEmail as EmailAddress, "
                                             + "ISNULL(CRI.ReferenceValue, " + MatchGuideConstants.ResumeRating.NotChecked + ") Rating, "
                                             + "CRI.ResumeText, "
                                             + "A.CandidateID ConsultantId, A.CompanyID ClientId, CD.JobTitle Title, "
                                             + "A.StartDate, A.EndDate, CRD.PayRate Rate, S.Name SpecializationName, S.Description SpecializationNameShort "
                                             + "FROM [Users] AS U "
                                            //ResumeInfo gives us rating, if present
                                             + "LEFT JOIN [Candidate_ResumeInfo] as CRI on CRI.UserID=U.UserID, "
                                             //Include Email
                                             + "[User_Email] as UE, "
                                            //Contracts
                                             + "[Agreement] AS A, "
                                             + "[Agreement_ContractDetail] AS CD, "
                                             + "[Agreement_ContractRateDetail] AS CRD, [Specialization] as S "
                                             + "WHERE U.UserID=UE.UserID "
                                             + "AND U.UserID=A.CandidateID "
                                             + "AND A.AgreementID=CD.AgreementID "
                                             + "AND A.AgreementID=CRD.AgreementID "
                                             + "AND CD.SpecializationID=S.SpecializationID "
                                            //Only Include FloThru contracts for now
                                             + "AND A.AgreementType=" + MatchGuideConstants.AgreementTypes.Contract + " "
                                             + "AND A.AgreementSubType=" + MatchGuideConstants.AgreementSubTypes.FloThru + " "
                                            //Match on specific user id
                                             + "AND U.UserID = @UserId";
                
                var consultants = new Dictionary<int, Consultant>();
                db.Connection.Query(consultantQuery,
                    CreateContractConsultantMappingFunction(consultants),
                    new { UserId = id },
                    //Each row contains a Consultant and a Contract
                    //Tell Dapper where the object boundaries are by specifying column
                    splitOn: "ConsultantId");

                var consultant = consultants.Values.FirstOrDefault();

                if (consultant != null)
                {
                    //get specializations..
                    const string specializationQuery = @"set nocount on
                                                        DECLARE @t TABLE(Id int, SpecializationId int, Name varchar(50), YearsOfExperience int)
                                                        insert @t
	                                                        SELECT s.SkillId, cs.SpecID, SkillName, cs.ExpID
	                                                        FROM Skill s
	                                                        JOIN Candidate_SkillsMatrix cs on cs.SkillID = s.SkillID
	                                                        WHERE UserId = @UserId
                                                        set nocount off 
                                                        SELECT * FROM @t
                                                        SELECT SpecializationId Id, Name, Description FROM Specialization WHERE SpecializationID in (select t.SpecializationId from @t t)";

                    var multi = db.Connection.QueryMultiple(specializationQuery, new { UserId = id });
                    var skills = multi.Read<Skill>();
                    var specializations = multi.Read<Specialization>();
                    foreach (var specialization in specializations)
                    {
                        specialization.Skills = skills.Where(sk => sk.SpecializationId == specialization.Id);
                    }
                    consultant.Specializations = specializations;
                }

                return consultant;
            }
        }

        /// <summary>
        /// Find alumni consultant candidates for a specific client.
        /// Does not include candidates that have current active or pending contracts with the specifed client.
        /// </summary>
        /// <param name="query">Text to search for in candidate name</param>
        /// <param name="clientIds">Client company ID that alumni must have worked for in the past.</param>
        /// <returns>A list of consultants, grouped by specialization.</returns>
        public IEnumerable<ConsultantGroup> FindAlumni(string query, IEnumerable<int> clientIds)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                string contractQuery = @"SELECT DISTINCT U.UserID Id, U.FirstName, U.LastName, "
                                             +"ISNULL(CRI.ReferenceValue, "+MatchGuideConstants.ResumeRating.NotChecked+") Rating, "
                                             + "A.CandidateID ConsultantId, A.CompanyID ClientId, CD.JobTitle Title, "
                                             + "A.StartDate, A.EndDate, CRD.PayRate Rate, S.Name SpecializationName, S.Description SpecializationNameShort "
                                             + "FROM [Users] AS U "
                                             //ResumeInfo gives us rating, if present
                                             + "LEFT JOIN [Candidate_ResumeInfo] as CRI on CRI.UserID=U.UserID, "  
                                             //Contracts
                                             + "[Agreement] AS A, "
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
                                             + "AND A.CompanyID IN @CompanyIds "
                                             + "AND A.Inactive = 0 "
                                             //Text query used to match on full name or resume
                                             + "AND ( (U.FirstName+' '+U.LastName) LIKE @LikeQuery "
                                             + "OR CONTAINS(CRI.ResumeText, @FullTextQuery)) "
                                            //Filter CandidateIDs with active or pending contracts with client
                                             + "AND U.UserID NOT IN ("
                                                 + "SELECT A.CandidateID FROM [Agreement] AS A "
                                                 + "WHERE A.CompanyID in @CompanyIds "
                                                 + "AND A.AgreementType=" + MatchGuideConstants.AgreementTypes.Contract+" "
                                                 + "AND A.AgreementSubType=" + MatchGuideConstants.AgreementSubTypes.FloThru + " "
                                                 + "AND (A.StatusType=" + MatchGuideConstants.ContractStatusTypes.Active + " "
                                                 + "OR A.StatusType=" + MatchGuideConstants.ContractStatusTypes.Pending + ") "
                                                 + "AND A.EndDate > GETUTCDATE() "
                                                 + "AND A.Inactive = 0 "
                                             + ") ";


                //Query will return row per contract, with consultant info repeated
                //Map contracts to consultants to get our desired data model.
                var consultants = new Dictionary<int, Consultant>();
                db.Connection.Query(contractQuery,
                    CreateContractConsultantMappingFunction(consultants),
                    new
                    {
                        CompanyIds = clientIds,
                        LikeQuery = "%" + query + "%",
                        FullTextQuery = FullTextSearchExpression.Create(query)
                    },
                    //Each row contains a Consultant and a Contract
                    //Tell Dapper where the object boundaries are by specifying column
                    splitOn: "ConsultantId");

                return GroupConsultantsByContractSpecialization(consultants.Values);
            }
        }

        private Func<Consultant, Contract, Consultant> CreateContractConsultantMappingFunction(Dictionary<int, Consultant> consultantLookup)
        {
            //map contract to consultant
            //reuse consultant instance if it is already present in dictionary to avoid duplicates
            return (c, contract) =>
            {
                Consultant consultant;
                if (!consultantLookup.TryGetValue(c.Id, out consultant))
                {
                    consultantLookup.Add(c.Id, consultant = c);
                }
                consultant.Contracts.Add(contract);
                return consultant;
            };
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
