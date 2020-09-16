using System;
using System.Collections.Generic;
using System.Data;

namespace Contador_Luz.Models
{
    public static class Engine
    {
        static Boolean flagBDLocal = true;
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

        public static void AlmacenarInfoBD()
        {
            //BD AZURE
            int idPago = GenerarIdPago();
            GuardarPagoLocales(idPago);

            //BD LOCAL
            int idPagoBdLocal = GenerarIdPago(flagBDLocal);
            GuardarPagoLocales(idPago,flagBDLocal);
        }

        public static void GuardarPagoLocales(int idPago, Boolean FlagBDlocal = false)
        {

            string[] rtas = new string[2];
            int numLocal = 0;


            foreach (Local local in Home.Locales)
            {
                try
                {
                    Query query;

                    if (FlagBDlocal)
                    {
                        string StringConnectionsBdLocal = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Data Source=4NDR3S_B3RN4L;Initial Catalog=PRUEBAS";
                        query = new Query(StringConnectionsBdLocal);
                    }
                    else
                    {
                        query = new Query();
                    }

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    query.Nombre = "Sp_Contador_Vatios";
                    query.AgregarParametro("@Opcion", 3);
                    query.AgregarParametro("@IdPago", idPago);
                    query.AgregarParametro("@IdLocal", local.Id);
                    query.AgregarParametro("@PorcSubAplicado", local.PorcentajeSubAplicado);
                    query.AgregarParametro("@ValorPago", local.Pago);
                    query.AgregarParametro("@VatiosConsumidosLc", local.VatiosConsumidos);
                    query.AgregarParametro("@AcumuladoVatiosLc", local.AcumuladoVatiosHoy);
                    ds = query.EjecutarProcedimiento();

                    dt = ds.Tables[0];
                    rtas[numLocal] = (Convert.ToString(dt.Rows[0][0]) + " Local " + local.Nombre);
                    numLocal++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    numLocal++;
                }
            }

            foreach (var msg in rtas)
            {
                Console.WriteLine(msg);
            }
        }

        public static int GenerarIdPago(Boolean flagBDLocal = false)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int id = 0;
            Query query;

            if (flagBDLocal)
            {
                string StringConnectionsBdLocal = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Data Source=4NDR3S_B3RN4L;Initial Catalog=PRUEBAS";
                query = new Query(StringConnectionsBdLocal);
            }
            else
            {
                query = new Query();
            }

            try
            {
                query.Nombre = "Sp_Contador_Vatios";
                query.AgregarParametro("@Opcion", 2);
                query.AgregarParametro("@TotalVatiosConsumidos", Home.TotalVatiosConsumidosHoy);
                query.AgregarParametro("@ValorVatio", Home.ValorVatio);
                query.AgregarParametro("@ValorTotal", Home.ValorTotal);
                query.AgregarParametro("@Subsidio", Home.Subsidio);

                ds = query.EjecutarProcedimiento();
                dt = ds.Tables[0];

                id = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return id;
        }
    
    }
}