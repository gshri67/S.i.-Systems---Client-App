using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SiSystems.ClientApp.Web.Domain
{
    public class DatabaseContext: IDisposable
    {
        private const string ConnectionStringConfigPropertyName = "ApplicationDb";

        private IDbConnection _connection;
        public IDbConnection Connection { get { return _connection ?? (_connection = CreateConnection()); } }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringConfigPropertyName].ConnectionString);
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
