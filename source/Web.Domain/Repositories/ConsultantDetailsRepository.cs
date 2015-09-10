using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Repositories
{
    public interface IConsultantDetailsRepository
    {
        ConsultantDetails GetConsultantDetails(int userId);
    }

    public class ConsultantDetailsRepository : IConsultantDetailsRepository
    {
        public ConsultantDetails GetConsultantDetails(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT CorpName AS CorporationName
                          FROM Candidate_Corporation
                          WHERE UserId = @UserId";
                
                var details = db.Connection.Query<ConsultantDetails>(query, new { UserId = userId}).FirstOrDefault();

                return details;
            }
        }
    }
}
