using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotto
{
    public static class SQLQueries
    {
        public static int procedure_CountEachNumberOccurencesOutput(string connectionString, int parameter)
        {
            #region ReturnValue
            //using (SqlConnection cn = new SqlConnection(con))
            //{
            //    cn.Open();
            //    using (SqlCommand cmd = new SqlCommand("procedure_CountEachNumberOccurencesReturn", cn))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        SqlParameter param = cmd.Parameters.Add("@number", SqlDbType.Int);
            //        param.Direction = ParameterDirection.Input;
            //        param.Value = parameter;
            //        SqlParameter returnValue = new SqlParameter();
            //        returnValue.Direction = ParameterDirection.ReturnValue;
            //        cmd.Parameters.Add(returnValue);
            //        return Convert.ToInt32(cmd.ExecuteScalar());
            //    }
            //}
            #endregion

            #region OutputValue Entity Framework
            //using (SqlConnection cn = new SqlConnection(con))
            //{
            //    cn.Open();
            //    using (SqlCommand cmd = new SqlCommand("procedure_CountEachNumberOccurencesOutput", cn))
            //    {
            //        var outParam = new SqlParameter();
            //        int outputValue = 0;


            //        outParam.SqlDbType = SqlDbType.Int;
            //        outParam.Direction = ParameterDirection.Output;

            //        DbContext dbContext = new DbContext(this.connectionString);
            //        var data = dbContext.Database.SqlQuery<int>("sp_CountEachNumberOccurencesOutput @number, @count_number OUT",
            //                                                    new SqlParameter("number", parameter),
            //                                                    new SqlParameter("count_number", outputValue), outParam);
            //        outputValue = data.ToList()[0];
            //        return outputValue;
            //    }
            //}
            #endregion

            #region OutputValue
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("procedure_CountEachNumberOccurencesOutput", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = cmd.Parameters.Add("@number", SqlDbType.Int);
                    param.Direction = ParameterDirection.Input;
                    param.Value = parameter;

                    SqlParameter outputParameter = cmd.Parameters.Add("@count_number", SqlDbType.Int);
                    outputParameter.Direction = ParameterDirection.Output;
                    outputParameter.Value = 0;

                    cmd.ExecuteNonQuery();
                    con.Close();
                    int OutputValue = (int)cmd.Parameters["@count_number"].Value;
                    return OutputValue;
                }
            }
            #endregion
        }

        public static void procedure_GetLastDrawDate(string connectionString, out DateTime date, out int drawNumber)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("procedure_GetLastDrawDate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter outputParameterDate = cmd.Parameters.Add("@last_draw_date", SqlDbType.Date);
                    outputParameterDate.Direction = ParameterDirection.Output;
                    SqlParameter outputParameterDrawNumber = cmd.Parameters.Add("@last_draw_number", SqlDbType.SmallInt);
                    outputParameterDrawNumber.Direction = ParameterDirection.Output;

                    cmd.ExecuteScalar();
                    con.Close();
                    date = (DateTime)cmd.Parameters["@last_draw_date"].Value;
                    drawNumber = Convert.ToInt32(cmd.Parameters["@last_draw_number"].Value);
                }
            }
        }

        public static void ExportEachNumberOccurencesToMathematicaMatrix(SQLUtils.ConnectionString con, string Path, int Rows, int Columns)
        {
            StringBuilder sb = new StringBuilder();
            int k = 1;
            sb.Append("{");
            for (int j = 0; j < Rows; j++) //rows
            {
                for (int i = 0; i < Columns; i++) //columns
                {
                    if (i == 0)
                        sb.Append("{");
                    sb.Append(SQLQueries.procedure_CountEachNumberOccurencesOutput(con.ToString(), k++));
                    if (i != Columns - 1)
                        sb.Append(", ");
                    else
                        sb.Append("}");
                }
                if (j != Rows - 1)
                    sb.Append(",");
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter(Path);
            sb.Append("}");
            file.WriteLine(sb.ToString());
            file.Close();
        }

        public static bool procedure_ValidateDatabaseRowNumbers(string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("procedure_ValidateDatabaseRowNumbers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter returnValue = new SqlParameter();
                    returnValue.SqlDbType = SqlDbType.Int;
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);
                    cmd.ExecuteNonQuery();
                    return Convert.ToBoolean((int)returnValue.Value);
                }
            }
        }
    }
}
