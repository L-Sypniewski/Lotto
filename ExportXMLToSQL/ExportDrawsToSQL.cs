﻿using System;
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
            Console.WriteLine("Liczba usunietych rows: " + cmdDel.ExecuteNonQuery().ToString());
            conn.Close();
        }

        // Sends List<> of all draws to SQL database
        public void ExportDrawsListToSQL(List<Draw> drawsList)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString.ToString()))
            {
                string qry = string.Format(@"select * from {0}", this.connectionString.tableName);
                string qryInsertDataFromList = string.Format(@"insert into {0} (NrLosowania, DataLosowania, Plus, Liczba1, Liczba2, Liczba3, Liczba4, Liczba5, Liczba6, Liczba7, Liczba8, Liczba9,Liczba10, Liczba11, Liczba12, Liczba13, Liczba14, Liczba15, Liczba16, Liczba17, Liczba18, Liczba19, Liczba20) 
                                                   values(@NrLosowania, @DataLosowania, @Plus, @Liczba1, @Liczba2, @Liczba3, @Liczba4, @Liczba5, @Liczba6, @Liczba7, @Liczba8, @Liczba9, @Liczba10, @Liczba11, @Liczba12, @Liczba13, @Liczba14, @Liczba15, @Liczba16, @Liczba17, @Liczba18, @Liczba19, @Liczba20)", this.connectionString.tableName);

                DataSet dataSetLosowania = new DataSet();
                SqlDataAdapter dataAdapterLosowanie = new SqlDataAdapter(qry, conn);

                ClearTable(conn); //Clear table to avoid duplicating data

                dataAdapterLosowanie.Fill(dataSetLosowania, this.connectionString.tableName);

                DataTable dt = dataSetLosowania.Tables[this.connectionString.tableName];
                int counter = 0; //Counter for displaying process of adding new rows to a database in a console windows

                foreach (var los in drawsList) //Adding new rows to a dataset
                {
                    DataRow newRow = dt.NewRow();
                    newRow["NrLosowania"] = los.DrawNo;
                    newRow["DataLosowania"] = los.DrawDate;
                    if (los.Plus != null)
                        newRow["Plus"] = los.Plus;
                    for (int i = 1; i <= 20; i++) //Loop for adding data fill columns from "Liczba1" to "Liczba20"
                    {
                        newRow[string.Format("Liczba{0}", i.ToString())] = los.Numbers[i - 1];
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
                dataAdapterLosowanie.Update(dataSetLosowania, this.connectionString.tableName);

                conn.Close();
            }
        }
    }
        
}