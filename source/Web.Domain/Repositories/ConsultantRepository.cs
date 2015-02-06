using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class ConsultantRepository
    {
        private const int ContractAgreementType = 459;//TODO: Verify
        private const int ContractAgreementSubType_FloThru = 172;//TODO: Verify

        public Consultant Find(int id)
        {
            return ConsultantMockData.Contractors.SingleOrDefault(c => c.Id == id);
        }

        public IEnumerable<ConsultantGroup> FindAlumni(string query, int clientId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.ClientApp))
            {
                //TODO: Verify Assumptions
                // Date & Inactive Column Usage
                // StatusType column --- should we be filtering based on some value?
                // Are there cases where Rate is NULL? Should we include those? Test DB Has some.
                const string contractQuery = @"SELECT DISTINCT U.UserID Id, U.FirstName, U.LastName, "
                                             + "A.CandidateID ConsultantId, A.CompanyID ClientId, "
                                             + "A.StartDate, A.EndDate, CRD.BillRate Rate, S.Name SpecializationName "
                                             + "FROM [Users] AS U, [Agreement] AS A, "
                                             + "[Agreement_ContractDetail] AS CD, "
                                             + "[Agreement_ContractRateDetail] AS CRD, [Specialization] as S "
                                             + "WHERE U.UserID=A.CandidateID "
                                             + "AND A.AgreementID=CD.AgreementID "
                                             + "AND A.AgreementID=CRD.AgreementID "
                                             + "AND CD.SpecializationID=S.SpecializationID "
                                             + "AND A.AgreementType=@AgreementType AND A.AgreementSubType=@AgreementSubType "
                                             + "AND A.CompanyID=@CompanyId "
                                             + "AND A.Inactive = 0 "
                                             + "AND ( (U.FirstName+' '+U.LastName) LIKE @Query "
                                             + "OR S.Name LIKE @Query) ";

                //query and map contracts to consultants
                var lookup = new Dictionary<int, Consultant>();
                db.Connection.Query<Consultant, Contract, Consultant>(contractQuery,
                    (c, contract) =>
                    {
                        Consultant consultant;
                        if (!lookup.TryGetValue(c.Id, out consultant))
                        {
                            lookup.Add(c.Id, consultant = c);
                        }
                        consultant.Contracts.Add(contract);
                        return consultant;
                    },
                    new
                    {
                        AgreementType = ContractAgreementType, 
                        AgreementSubType = ContractAgreementSubType_FloThru,
                        CompanyId=clientId, 
                        Query="%"+query+"%"
                    },
                    splitOn:"ConsultantId");

                //filter out people that have current contracts
                //ie. not alumni
                var matchingConsultants = lookup.Values
                    .Where(c=> !c.Contracts.Any(contract=>contract.EndDate>DateTime.UtcNow));


                return GroupConsultantsByContractSpecialization(matchingConsultants);
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
                        groups.Add(specializationName, new ConsultantGroup {Specialization = specializationName});
                    }
                    var group = groups[specializationName];
                    @group.Consultants.Add(new ConsultantSummary(consultant, specializationName));
                }
            }
            return groups.Select(g => g.Value).OrderBy(g => g.Specialization);
        }
    }
}
