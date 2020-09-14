using System;
using System.Collections.Generic;
using System.Data;

namespace Contador_Luz.Models
{
    public static class Engine
    {
        public static void CargarLocales()
        {   
            var ListLocales = new List<Local>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            Query query = new Query();
            query.Nombre = "Sp_Contador_Vatios";
            query.AgregarParametro("@Opcion",1);
            ds = query.EjecutarProcedimiento();

            dt = ds.Tables[0];

            foreach(DataRow row in dt.Rows)
            {
                ListLocales.Add(new Local(Convert.ToString(row["Id"]),
                                        Convert.ToString(row["Nombre"]),
                                        Convert.ToInt32(row["Acumulado"])));
            }
            
            Home.Locales = ListLocales;
        }
    }
}