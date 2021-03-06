﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public enum DatabaseSelect
    {
        MatchGuide,
        ClientApp
    }

    public class DatabaseContext: IDisposable
    {
        private const string MatchGuideConnectionStringConfigPropertyName = "MatchGuideDb";
        private const string ClientAppConnectionStringConfigPropertyName = "ClientAppDb";
        private readonly DatabaseSelect _type;

        public DatabaseContext(DatabaseSelect type)
        {
            _type = type;
        }

        private IDbConnection _connection;
        public IDbConnection Connection { get { return _connection ?? (_connection = CreateConnection(_type)); } }

        private IDbConnection CreateConnection(DatabaseSelect type)
        {
            var propName = type == DatabaseSelect.MatchGuide
                ? MatchGuideConnectionStringConfigPropertyName
                : ClientAppConnectionStringConfigPropertyName;

            return new SqlConnection(ConfigurationManager.ConnectionStrings[propName].ConnectionString);
        }

        #region IDisposable Implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
        #endregion
    }
}
