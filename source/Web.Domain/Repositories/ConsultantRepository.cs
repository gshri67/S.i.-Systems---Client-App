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
        /// Find consultant candidates for a specific client.
        /// Does not include candidates that have current active or pending contracts with the specifed client.
        /// </summary>
        /// <param name="query">Text to search for in candidate name</param>
        /// <param name="clientIds">Client company ID that alumni must have worked for in the past.</param>
        /// <param name="active">
        /// If true finds all consultants who have an active or pending contract with one of the companies specified in <see cref="clientIds"/>
        /// If false finds all consultants who have no active or pending contracts but do have flo thru contracts in the past.
        /// </param>
        /// <returns>A list of consultants, grouped by specialization.</returns>
        IEnumerable<ConsultantGroup> Find(string query, IEnumerable<int> clientIds, bool active = false);
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
                                SELECT usr.UserID Id, usr.FirstName, usr.LastName, ue.PrimaryEmail as EmailAddress
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
                                            CAST(agr.AgreementSubType AS INT) AgreementSubType,
                                            agrDetail.JobTitle Title,
                                            agr.StartDate, 
                                            agr.EndDate, 
                                            agrRateDetail.PayRate Rate,
                                            spec.Name SpecializationName, 
                                            spec.Description SpecializationNameShort,
                                            contact.UserID ContactId,
											contact.FirstName,
											contact.LastName,
											contactEmail.PrimaryEmail EmailAddress
                                            FROM [Agreement] agr 
                                            LEFT JOIN [Agreement_ContractDetail] agrDetail on agr.AgreementID = agrDetail.AgreementID
                                            LEFT JOIN [Agreement_ContractRateDetail] agrRateDetail on agr.AgreementID = agrRateDetail.AgreementID
                                            LEFT JOIN [Specialization] spec on agrDetail.SpecializationID = spec.SpecializationID
                                            LEFT JOIN Users contact on contact.UserID = agr.ContactID
											JOIN User_Email contactEmail on contactEmail.UserID = contact.UserID
					                        WHERE agr.CandidateId = @UserId
                                                --AND agr.CompanyID in @CompanyIds
						                        AND agr.AgreementType = @CONTRACT
                                                AND agr.AgreementSubType = @FLOTHRU";
                    var floThruContracts = db.Connection.Query<Contract, Contact, Contract>(constants + floThruContractsQuery,
                        (contract, contact) =>
                        {
                            contract.Contact = contact;
                            return contract;
                        }, param: new { UserId = id }, splitOn: "ContactId");
                    consultant.Contracts = floThruContracts;
                }

                return consultant;
            }
        }

        public IEnumerable<ConsultantGroup> Find(string query, IEnumerable<int> clientIds, bool active = false)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                string constants = @"DECLARE @ACTIVE int = " + MatchGuideConstants.ContractStatusTypes.Active + ","
                                            + "@PENDING int = " + MatchGuideConstants.ContractStatusTypes.Pending + ","
                                            + "@FLOTHRU int = " + MatchGuideConstants.AgreementSubTypes.FloThru + ","
                                            + "@CONTRACT int = " + MatchGuideConstants.AgreementTypes.Contract + ","
                                            + "@NOTCHECKED int = " + MatchGuideConstants.ResumeRating.NotChecked;
                string consultantsQuery;
                if (active)
                {
                    consultantsQuery = @"SELECT usr.UserID Id
	                                    ,usr.FirstName
	                                    ,usr.LastName
	                                    ,ISNULL(cri.ReferenceValue, @NOTCHECKED) as Rating
	                                    ,mostRecentContract.StartDate MostRecentContractStartDate
	                                    ,mostRecentContract.EndDate MostRecentContractEndDate
	                                    ,mostRecentContract.PayRate MostRecentContractRate
	                                    ,mostRecentContract.SpecializationID Id, mostRecentContract.Name, mostRecentContract.Description
                                    FROM Users usr
	                                    JOIN User_Email ue on ue.UserID = usr.UserID
	                                    LEFT JOIN [Candidate_ResumeInfo] cri on cri.UserID = usr.UserID
	                                    CROSS APPLY (SELECT	TOP 1	a.CandidateID,
							                                    a.StartDate, 
							                                    a.EndDate,
							                                    crd.PayRate
							                                    ,spec.SpecializationID, spec.Name, spec.Description
				                                    FROM [Agreement] a
				                                    JOIN [Agreement_ContractDetail] cd on cd.AgreementID = a.AgreementID
				                                    JOIN [Specialization] spec on spec.SpecializationID = cd.SpecializationID
				                                    JOIN [Agreement_ContractRateDetail] crd on crd.AgreementID = a.AgreementID
				                                    WHERE a.CandidateID = usr.UserID AND a.StatusType = @ACTIVE
				                                    ORDER BY a.EndDate desc) mostRecentContract
                                    WHERE
                                    --Text query used to match on full name or resume
                                    ((usr.FirstName + ' ' + usr.LastName) LIKE @LikeQuery
                                        OR CONTAINS(cri.ResumeText, @FullTextQuery))
                                    AND 
                                    EXISTS (
	                                    SELECT TOP 1 * 
	                                    FROM [Agreement] agr 
	                                    WHERE agr.CandidateId = usr.UserID
		                                    AND agr.CompanyID IN @CompanyIds
		                                    AND agr.AgreementType = @CONTRACT
		                                    AND agr.StatusType IN (@ACTIVE, @PENDING)
                                    )";
                }
                else
                {
                    consultantsQuery = @"SELECT usr.UserID Id
	                                    ,usr.FirstName
	                                    ,usr.LastName
	                                    ,ISNULL(cri.ReferenceValue, @NOTCHECKED) as Rating
	                                    ,mostRecentContract.StartDate MostRecentContractStartDate
	                                    ,mostRecentContract.EndDate MostRecentContractEndDate
	                                    ,mostRecentContract.PayRate MostRecentContractRate
	                                    ,spec.SpecializationID Id, spec.Name, spec.Description
                                    FROM Users usr
	                                    JOIN User_Email ue on ue.UserID = usr.UserID
	                                    -- Include one record for each of a candidate's specializations
	                                    LEFT JOIN (SELECT DISTINCT SpecId, UserID FROM Candidate_SkillsMatrix WHERE Inactive = 0) skills on usr.UserID = skills.UserID
	                                    LEFT JOIN Specialization spec on spec.SpecializationID = skills.SpecID
	                                    LEFT JOIN [Candidate_ResumeInfo] cri on cri.UserID = usr.UserID
	                                    CROSS APPLY (SELECT	TOP 1	a.CandidateID,
							                                    a.StartDate, 
							                                    a.EndDate,
							                                    crd.PayRate
				                                    FROM [Agreement] a 
				                                    LEFT JOIN [Agreement_ContractRateDetail] crd on crd.AgreementID = a.AgreementID
				                                    WHERE a.CandidateID = usr.UserID
				                                    ORDER BY a.EndDate desc) mostRecentContract
                                    WHERE
                                    --Text query used to match on full name or resume
                                    ((usr.FirstName + ' ' + usr.LastName) LIKE @LikeQuery
                                        OR CONTAINS(cri.ResumeText, @FullTextQuery))
                                    AND EXISTS (
	                                    SELECT TOP 1 * 
	                                    FROM [Agreement] agr 
	                                    WHERE agr.CandidateId = usr.UserID
		                                    AND agr.CompanyID in @CompanyIds
		                                    AND agr.AgreementType = @CONTRACT
		                                    AND agr.AgreementSubType = @FLOTHRU
                                    )
                                    AND NOT EXISTS (
	                                    SELECT TOP 1 * 
	                                    FROM [Agreement] agr 
	                                    WHERE agr.CandidateId = usr.UserID
		                                    AND agr.CompanyID in @CompanyIds
		                                    AND agr.AgreementType = @CONTRACT
		                                    AND agr.StatusType IN (@ACTIVE, @PENDING)
                                    )";
                }
                        
                var lookup = new Dictionary<int, ConsultantGroup>();
                db.Connection.Query<ConsultantSummary, Specialization, ConsultantGroup>(constants + consultantsQuery,
                    (summary, spec) =>
                    {
                        ConsultantGroup group;
                        spec = spec ?? new Specialization { Id = 0, Name = string.Empty };
                        if (!lookup.TryGetValue(spec.Id, out group))
                        {
                            lookup.Add(spec.Id, group = new ConsultantGroup { Specialization = spec.Name, Consultants = new List<ConsultantSummary>() });
                        }
                        group.Consultants.Add(summary);
                        return group;
                    },
                    new
                    {
                        CompanyIds = clientIds,
                        LikeQuery = "%" + query + "%",
                        FullTextQuery = FullTextSearchExpression.Create(query)
                    });

                return lookup.Values;
            }
        }
    }
}
