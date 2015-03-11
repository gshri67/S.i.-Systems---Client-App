using System;
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
                string constants = @"DECLARE @ACTIVE int = " + MatchGuideConstants.ContractStatusTypes.Active + ","
                                            + "@PENDING int = " + MatchGuideConstants.ContractStatusTypes.Pending + ","
                                            + "@FLOTHRU int = " + MatchGuideConstants.AgreementSubTypes.FloThru + ","
                                            + "@CONTRACT int = " + MatchGuideConstants.AgreementTypes.Contract + ","
                                            + "@NOTCHECKED int = " + MatchGuideConstants.ResumeRating.NotChecked;

                const string consultantQuery = @"
                                SELECT usr.UserID, usr.FirstName, usr.LastName, ue.PrimaryEmail as EmailAddress
	                                ,ISNULL(cri.ReferenceValue, @NOTCHECKED) as Rating, cri.ResumeText
                                FROM Users usr
	                                LEFT JOIN User_Email ue on ue.UserID = usr.UserID
	                                LEFT JOIN [Candidate_ResumeInfo] cri on cri.UserID = usr.UserID
                                WHERE usr.UserId = @UserId";

                var consultants = db.Connection.Query<Consultant>(constants + consultantQuery, new { UserId = id });

                var consultant = consultants.FirstOrDefault();

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
                    //get contracts..
                    const string floThruContractsQuery = @"
                                            SELECT agr.CandidateID ConsultantId,
                                            agr.CompanyID ClientId,
                                            agrDetail.JobTitle Title,
                                            agr.StartDate, 
                                            agr.EndDate, 
                                            agrRateDetail.PayRate Rate,
                                            spec.Name SpecializationName, 
                                            spec.Description SpecializationNameShort
                                            FROM [Agreement] agr 
                                            LEFT JOIN [Agreement_ContractDetail] agrDetail on agr.AgreementID = agrDetail.AgreementID
                                            LEFT JOIN [Agreement_ContractRateDetail] agrRateDetail on agr.AgreementID = agrRateDetail.AgreementID
                                            LEFT JOIN [Specialization] spec on agrDetail.SpecializationID = spec.SpecializationID
					                        WHERE agr.CandidateId = @UserId
                                                --AND agr.CompanyID in @CompanyIds
						                        AND agr.AgreementType = @CONTRACT
                                                AND agr.AgreementSubType = @FLOTHRU";

                    var floThruContracts = db.Connection.Query<Contract>(constants + floThruContractsQuery, new { UserId = id });
                    consultant.Contracts = floThruContracts;
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
                string constants = @"DECLARE @ACTIVE int = " + MatchGuideConstants.ContractStatusTypes.Active + ","
                                            + "@PENDING int = " + MatchGuideConstants.ContractStatusTypes.Pending + ","
                                            + "@FLOTHRU int = " + MatchGuideConstants.AgreementSubTypes.FloThru + ","
                                            + "@CONTRACT int = " + MatchGuideConstants.AgreementTypes.Contract + ","
                                            + "@NOTCHECKED int = " + MatchGuideConstants.ResumeRating.NotChecked;

                const string contractsExistsSubQuery = @"
                                            SELECT TOP 1 * 
					                        FROM [Agreement] agr 
					                        WHERE agr.CandidateId = usr.UserID
                                                AND agr.CompanyID in @CompanyIds
						                        AND agr.AgreementType = @CONTRACT";

                const string floThruContractExistsSubQuery = contractsExistsSubQuery + " AND agr.AgreementSubType = @FLOTHRU";
                const string activeOrPendingContractExistsSubQuery = contractsExistsSubQuery + " AND agr.StatusType in (@ACTIVE, @PENDING)";

                string consultantsQuery = @"
                        SELECT usr.UserID Id, usr.FirstName, usr.LastName, ue.PrimaryEmail as EmailAddress
	                        ,ISNULL(cri.ReferenceValue, @NOTCHECKED) as Rating
	                        ,cri.ResumeText
	                        ,spec.SpecializationID Id, spec.Name, spec.Description
                        FROM Users usr
	                        JOIN User_Email ue on ue.UserID = usr.UserID
	                        -- Include one record for each of a candidate's specializations
	                        LEFT JOIN (SELECT DISTINCT SpecId, UserID FROM Candidate_SkillsMatrix WHERE Inactive = 0) skills on usr.UserID = skills.UserID
	                        LEFT JOIN Specialization spec on spec.SpecializationID = skills.SpecID
	                        LEFT JOIN [Candidate_ResumeInfo] cri on cri.UserID = usr.UserID
                        WHERE
                        --Text query used to match on full name or resume
                        ((usr.FirstName + ' ' + usr.LastName) LIKE @LikeQuery
                            OR CONTAINS(cri.ResumeText, @FullTextQuery))"
                        //Does not have an active or pending contract
                        + " AND EXISTS ( " + floThruContractExistsSubQuery + " )"
                        //Has previous flothru contract 
                        + " AND NOT EXISTS ( " + activeOrPendingContractExistsSubQuery + " )";

                var lookup = new Dictionary<int, Consultant>();
                db.Connection.Query<Consultant, Specialization, Consultant>(constants + consultantsQuery,
                    (c, s) =>
                    {
                        Consultant consultant;
                        List<Specialization> specializations;
                        if (!lookup.TryGetValue(c.Id, out consultant))
                        {
                            lookup.Add(c.Id, consultant = c);
                            consultant.Specializations = new List<Specialization>();
                        }
                        specializations = consultant.Specializations.ToList();
                        if (s != null)
                        {
                            specializations.Add(s);
                        }
                        consultant.Specializations = specializations;
                        return consultant;
                    },
                    new
                    {
                        CompanyIds = clientIds,
                        LikeQuery = "%" + query + "%",
                        FullTextQuery = FullTextSearchExpression.Create(query)
                    });

                var result = lookup.Values.SelectMany(c => c.Specializations.Select(s => new { s.Name, Summary = new ConsultantSummary(c, s.Name) }))
                    .ToLookup(l => l.Name, l => l.Summary)
                    .Select(kvp => new ConsultantGroup { Specialization = kvp.Key, Consultants = kvp.ToArray() })
                    .ToList();

                var consultantsWithNoSpecializations = lookup.Values.Where(c => !c.Specializations.Any())
                                                .Select(c => new ConsultantSummary(c, string.Empty)).ToList();
                if (consultantsWithNoSpecializations.Any())
                {
                    result.Add(new ConsultantGroup
                    {
                        Specialization = string.Empty,
                        Consultants = consultantsWithNoSpecializations
                    });
                }
                
                return result;
            }
        }
    }
}
