using System;
using System.Data.SqlClient;

namespace Lotto
{
    public static class SQLUtils
    {
        // Creates new database. Returns 0 if succeeded, throws an exception if failed
        public static int CreateNewDatabase(ConnectionString connectionString)
        {
            string newDatabaseName = connectionString.databaseName;
            connectionString.databaseName = "master";
            string str = string.Format(@"CREATE DATABASE {0}", newDatabaseName);
            
            using (SqlConnection con = new SqlConnection(connectionString.ToString()))
            {
                SqlCommand myCommand = new SqlCommand(str, con);
                try
                {
                    con.Open();
                    Console.WriteLine("");
                    myCommand.ExecuteNonQuery();
                    return 0;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw new Exception(ex.Message); // ErrorCode -2146232060 if database with a given name already exists
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // Checks if connection with a database succeeded
        public static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        public static bool IsServerConnected(ConnectionString connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public struct ConnectionString
        {
            public string serverName, databaseName, tableName;
            public bool trustedConnection;

            public ConnectionString(string serverName, string databaseName, bool trustedConnection, string tableName = null)
            {
                this.serverName = serverName;
                this.databaseName = databaseName;
                this.trustedConnection = trustedConnection;
                this.tableName = tableName;
            }
            public override string ToString()
            {
                return string.Format(@"Server={0};Database={1};Trusted_Connection={2}", this.serverName, this.databaseName, this.trustedConnection.ToString());
            }
        }
    }
}
