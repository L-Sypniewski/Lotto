using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotto
{
    public class ExportDrawsToSQL
    {
        public SQLUtils.ConnectionString connectionString { get; private set; }

        //Constructors
        public ExportDrawsToSQL(SQLUtils.ConnectionString ConnString)
        {
            if (ConnString.tableName == null)
                throw new ArgumentException(string.Format("Class {0} requires a SQLUtils.ConnectionString with non-null tableName.", "ExportDrawsToSQL"));
            this.connectionString = ConnString;
        }
        public ExportDrawsToSQL(string serverName, string databaseName, bool trustedConnection, string tableName)
        {
            if (tableName == null)
                throw new ArgumentException(string.Format("Class {0} requires a SQLUtils.Connection string with non-null tableName.", "ExportDrawsToSQL"));
            this.connectionString = new SQLUtils.ConnectionString(serverName, databaseName, trustedConnection, tableName);
        }

        // Clear the table to avoid rasing Primary Key violation exception while calling ExportAllDrawsListToSQL method
        private void ClearTable(SqlConnection conn)
        {
            string del = string.Format(@"DELETE FROM {0};", this.connectionString.tableName);
            conn.Open();
            SqlCommand cmdDel = new SqlCommand(del, conn);
            Console.WriteLine("Number of deleted rows: " + cmdDel.ExecuteNonQuery().ToString());
            conn.Close();
        }

        // Sends List<> of all draws to SQL database
        public void ExportDrawsListToSQL(List<Draw> drawsList, bool sortNumbers = false)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString.ToString()))
            {
                string qry = string.Format(@"select * from {0}", this.connectionString.tableName);
                string qryInsertDataFromList = string.Format(@"insert into {0} (DrawNo, DrawDate, Plus, Number1, Number2, Number3, Number4, Number5, Number6, Number7, Number8, Number9,Number10, Number11, Number12, Number13, Number14, Number15, Number16, Number17, Number18, Number19, Number20) 
                                                   values(@DrawNo, @DrawDate, @Plus, @Number1, @Number2, @Number3, @Number4, @Number5, @Number6, @Number7, @Number8, @Number9, @Number10, @Number11, @Number12, @Number13, @Number14, @Number15, @Number16, @Number17, @Number18, @Number19, @Number20)", this.connectionString.tableName);

                DataSet dataSetDraws = new DataSet();
                SqlDataAdapter dataAdapterDraw = new SqlDataAdapter(qry, conn);
                ClearTable(conn); //Clear table to avoid duplicating data
                dataAdapterDraw.Fill(dataSetDraws, this.connectionString.tableName);
                DataTable dt = dataSetDraws.Tables[this.connectionString.tableName];

                int counter = 0; //Counter for displaying process of adding new rows to a database in a console windows
                foreach (var los in drawsList) //Adding new rows to a dataset
                {
                    if (sortNumbers == true)
                        los.Numbers.Sort();
                    DataRow newRow = dt.NewRow();
                    newRow["DrawNo"] = los.DrawNo;
                    newRow["DrawDate"] = los.DrawDate;
                    if (los.Plus != null)
                        newRow["Plus"] = los.Plus;
                    for (int i = 1; i <= 20; i++) //Loop for adding data fill columns from "Number1" to "Number20"
                    {
                        newRow[string.Format("Number{0}", i.ToString())] = los.Numbers[i - 1];
                    }
                    dt.Rows.Add(newRow);
                    #if DEBUG
                        Console.WriteLine("Added row " + (++counter).ToString());
                    #else
                        ++counter;
                    #endif
                  
                }
                Console.WriteLine(string.Format("\n{0} rows have been added.", counter.ToString()));

                // Create command
                SqlCommand cmd = new SqlCommand(qryInsertDataFromList, conn);

                // Map parameters
                cmd.Parameters.Add("@DrawNo", SqlDbType.SmallInt, 15, "DrawNo");
                cmd.Parameters.Add("@DrawDate", SqlDbType.DateTime, 15, "DrawDate");
                cmd.Parameters.Add("@Plus", SqlDbType.TinyInt, 25, "Plus");
                for (int i = 1; i <= 20; i++) //Loop for adding parameters "Number1" to "Number20"
                {
                    cmd.Parameters.Add(string.Format("@Number{0}", i.ToString()), SqlDbType.TinyInt, 50, string.Format("Number{0}", i.ToString()));
                }

                // Insert values
                dataAdapterDraw.InsertCommand = cmd;
                dataAdapterDraw.Update(dataSetDraws, this.connectionString.tableName);
                conn.Close();
            }
        }
    }
        
}
