using System.Data.SqlClient;

namespace TestMonitorAndUsers
{
    class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=JAN\SQLEXPRESS;Initial Catalog=OrdrsIP;Integrated Security=True");

        public void Openconnection()
        {
            if(sqlConnection.State==System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void Closeconnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }
}
