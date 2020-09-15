using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Contador_Luz.Models;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace Contador_Luz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Locales = Home.Locales;
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection Datos)
        {
            ViewBag.Locales = Home.Locales;

            int AcumLocalA = Convert.ToInt32(Datos["Acum_LocalA"]);
            int AcumLocalB = Convert.ToInt32(Datos["Acum_LocalB"]);

            Home.TotalVatiosConsumidosHoy = Convert.ToInt32(Datos["totalVatConsumidos"]);
            Home.ValorVatio = Convert.ToInt32(Datos["valorVatio"]);
            Home.Subsidio = Convert.ToInt32(Datos["subsidio"]);
            ViewBag.Subsidio = Home.Subsidio;

            int SubLocalA = Convert.ToInt32(Datos["subLocalA"]);
            int SubLocalB = Convert.ToInt32(Datos["subLocalB"]);

            Calculator.VatiosConsumidosLocales(AcumLocalA, AcumLocalB);
            Calculator.GenerarValoresPago();
            Calculator.SubsidioXLocal(SubLocalA, SubLocalB);

            return View("Results");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Results()
        {
            ViewBag.Locales = Home.Locales;
            return View();
        }


        public IActionResult Guardar()
        {
            int idPago = GenerarIdPago();
            GuardarPagoLocales(idPago);

            ViewBag.Locales = Home.Locales;
            return View("Index");
        }

        private void GuardarPagoLocales(int idPago)
        {

            string[] rtas = new string[2];
            int numLocal = 0;

            foreach (Local local in Home.Locales)
            {
                try
                {
                    Query query = new Query();
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

        private int GenerarIdPago()
        {
            Query qr = new Query();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int id = 0;

            try
            {
                qr.Nombre = "Sp_Contador_Vatios";
                qr.AgregarParametro("@Opcion", 2);
                qr.AgregarParametro("@TotalVatiosConsumidos", Home.TotalVatiosConsumidosHoy);
                qr.AgregarParametro("@ValorVatio", Home.ValorVatio);
                qr.AgregarParametro("@ValorTotal", Home.ValorTotal);
                qr.AgregarParametro("@Subsidio", Home.Subsidio);

                ds = qr.EjecutarProcedimiento();
                dt = ds.Tables[0];

                id = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            return id;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
