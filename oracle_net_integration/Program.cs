using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using oracle_net_integration.Models;

namespace oracle_net_integration
{
    class Program
    {
        static void Main(string[] args)
        {


            List<TaskTbl> data = new List<TaskTbl>();

            data.Add(new TaskTbl()
            {
                executed = "This test is executed successfully!!!",
                nao_conform = 1,
                obs = "more description is here"
            });

            data.Add(new TaskTbl()
            {
                executed = "This test is executed successfully!!!",
                nao_conform = 2,
                obs = "more description is here"
            });

            //METHOD THAT REQUIRED IN THE TASK
            SaveRecords(data);

            Console.WriteLine("Press ENTER to exit");
            Console.Read();


        }

        private static void SaveRecords(List<TaskTbl> data)
        {
            try
            {
                using (var con = GetConnection())
                {
                    foreach (var rec in data)
                    {
                        var param = new DynamicParameters();
                        param.Add("@p_executed", rec.executed);
                        param.Add("@p_nao_conform", rec.nao_conform);
                        param.Add("@p_obs", rec.obs);

                        con.Execute("SCHEME.wk_req_wbs_execute.wbs_execute", param, commandType: CommandType.StoredProcedure);
                        Console.WriteLine("Request saved!: {0}", rec.nao_conform);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        private static OracleConnection GetConnection()
        {
            var conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL)));User Id=USER;Password=PASS;");
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }
    }
}


