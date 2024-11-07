using Microsoft.Data.SqlClient;

namespace SV21T1020546.DataLayers.SQLServer
{
    public abstract class BaseDAL
    {
        protected string _connectionString = "";

        public BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }

    }
}
