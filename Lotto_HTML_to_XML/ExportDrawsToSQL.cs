using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotto
{
    class ExportDrawsToSQL
    {
        public SQLUtils.ConnectionString connectionString { get; private set; }

        //Constructors
        public ExportDrawsToSQL(SQLUtils.ConnectionString ConnString)
        {
            this.connectionString = ConnString;
        }
        public ExportDrawsToSQL(string serverName, string databaseName, bool trustedConnection)
        {
            this.connectionString = new SQLUtils.ConnectionString(serverName, databaseName, trustedConnection);
        }

        // Clear the table to avoid rasing Primary Key violation exception while calling ExportAllDrawsListToSQL method
        private void ClearTable(SqlConnection conn, string TableName)
        {
            string del = string.Format(@"DELETE FROM {0};", TableName);
            conn.Open();
            SqlCommand cmdDel = new SqlCommand(del, conn);
            Console.WriteLine("Liczba usunietych rows: " + cmdDel.ExecuteNonQuery().ToString());
            conn.Close();
        }

        // Sends List<> of all draws to SQL database
        public void ExportDrawsListToSQL(string TableName, string url = ConvertHTMLDrawsToXML.SOURCE_LINK_TO_ALL)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString.ToString()))
            {
                string qry = string.Format(@"select * from {0}", TableName);
                string qryInsertDataFromList = string.Format(@"insert into {0} (NrLosowania, DataLosowania, Plus, Liczba1, Liczba2, Liczba3, Liczba4, Liczba5, Liczba6, Liczba7, Liczba8, Liczba9,Liczba10, Liczba11, Liczba12, Liczba13, Liczba14, Liczba15, Liczba16, Liczba17, Liczba18, Liczba19, Liczba20) 
                                                   values(@NrLosowania, @DataLosowania, @Plus, @Liczba1, @Liczba2, @Liczba3, @Liczba4, @Liczba5, @Liczba6, @Liczba7, @Liczba8, @Liczba9, @Liczba10, @Liczba11, @Liczba12, @Liczba13, @Liczba14, @Liczba15, @Liczba16, @Liczba17, @Liczba18, @Liczba19, @Liczba20)", TableName);

                DataSet dataSetLosowania = new DataSet();
                SqlDataAdapter dataAdapterLosowanie = new SqlDataAdapter(qry, conn);

                ClearTable(conn, TableName); //Clear table to avoid duplicating data

                dataAdapterLosowanie.Fill(dataSetLosowania, TableName);

                DataTable dt = dataSetLosowania.Tables[TableName];
                int counter = 0; //Counter for displaying process of adding new rows to a database in a console windows

                foreach (var los in ConvertHTMLDrawsToXML.GetDrawsList(url)) //Adding new rows to a dataset
                {
                    DataRow newRow = dt.NewRow();
                    newRow["NrLosowania"] = los.NrLosowania;
                    newRow["DataLosowania"] = los.DataLosowania;
                    if (los.Plus != null)
                        newRow["Plus"] = los.Plus;
                    for (int i = 1; i <= 20; i++) //Loop for adding data fill columns from "Liczba1" to "Liczba20"
                    {
                        newRow[string.Format("Liczba{0}", i.ToString())] = los.Liczby[i - 1];
                    }
                    dt.Rows.Add(newRow);

                    Console.WriteLine("Dodano rząd " + (++counter).ToString());
                }

                // Create command
                SqlCommand cmd = new SqlCommand(qryInsertDataFromList, conn);

                // Map parameters
                cmd.Parameters.Add("@NrLosowania", SqlDbType.SmallInt, 15, "NrLosowania");
                cmd.Parameters.Add("@DataLosowania", SqlDbType.DateTime, 15, "DataLosowania");
                cmd.Parameters.Add("@Plus", SqlDbType.TinyInt, 25, "Plus");
                for (int i = 1; i <= 20; i++) //Loop for adding parameters "Liczba1" to "Liczba20"
                {
                    cmd.Parameters.Add(string.Format("@Liczba{0}", i.ToString()), SqlDbType.TinyInt, 50, string.Format("Liczba{0}", i.ToString()));
                }

                // Insert values
                dataAdapterLosowanie.InsertCommand = cmd;
                dataAdapterLosowanie.Update(dataSetLosowania, TableName);

                conn.Close();
            }
        }

        /*public void UpdateDatabase(SQLUtils.ConnectionString connectionString, string tableName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString.ToString())
            {
                string lastDrawDate = "" 
            }*/
    }
        
}
