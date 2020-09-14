using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Contador_Luz.Models
{
    public class Query
    {
        public string ConnectionStrings { get; set; }

        public string Nombre { get; set; }



        Dictionary<string,string> DicParameter = new Dictionary<string,string>();
        Dictionary<string,int> DicParameterInt = new Dictionary<string,int>();
        string stringDefault = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Data Source=4NDR3S_B3RN4L;Initial Catalog=PRUEBAS";

        


        public DataSet EjecutarQuery()
        {   
            var ds = new DataSet();
            var conn = new SqlConnection(ConnectionStrings);
            try
            {
                conn.Open();

                var cmd = new SqlCommand(Nombre, conn);
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                conn.Close();

            }
            catch (Exception ex)
            {   
                var mensaje = ex.Message;
                conn.Close();
            }

                return ds;
        }

        public DataSet EjecutarProcedimiento()
        {   
            var ds = new DataSet();
            var conn = new SqlConnection(ConnectionStrings);
            try
            {
                conn.Open();


                var cmd = new SqlCommand(Nombre, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach(var pmt in DicParameter)
                {
                    cmd.Parameters.Add(new SqlParameter(pmt.Key,pmt.Value));
                }

                foreach(var pmt in DicParameterInt)
                {
                    cmd.Parameters.Add(new SqlParameter(pmt.Key,pmt.Value));
                }

                var da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                conn.Close();

            }
            catch (Exception ex)
            {   
                var mensaje = ex.Message;
                conn.Close();
            }

                return ds;
        }

        public Query(string conn = "")
        {
            if (conn == "")
            {
                this.ConnectionStrings = stringDefault;
            }
            else
            {
                this.ConnectionStrings = conn;
            }
        }

        internal void AgregarParametro(string namParameter, string valParameter)
        {

            DicParameter.Add(namParameter,valParameter.ToString());
        }

        internal void AgregarParametro(string namParameter, int valParameter)
        {
            DicParameterInt.Add(namParameter,valParameter);
        }
    }
}

