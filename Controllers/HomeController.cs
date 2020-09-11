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
            // PruebaQuery();
            // PruebaProcedimiento();

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

            int SubLocalA = Convert.ToInt32(Datos["subLocalA"]);
            int SubLocalB = Convert.ToInt32(Datos["subLocalB"]);

            Calculator.VatiosConsumidosLocales(AcumLocalA, AcumLocalB);
            Calculator.GenerarValoresPago();
            Calculator.SubsidioXLocal(SubLocalA, SubLocalB);

            return View("Results");
        }

        private void PruebaQuery()
        {   
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string conn = "Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Data Source=4NDR3S_B3RN4L;Initial Catalog=Bernal";
            Query query = new Query(conn);
            query.Nombre = "select * from Contador_Luz";
            ds = query.EjecutarQuery();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            
        }


        private void PruebaProcedimiento()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            var Query = new Query();
            Query.Nombre = "SP_Consultas_Basicas";
            Query.AgregarParametro("@opcion",1);
            Query.AgregarParametro("@prm","dog");
            ds = Query.EjecutarProcedimiento();

            dt = ds.Tables[0];
            var sound = dt.Rows[0][0];
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Results()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Results(IFormCollection form)
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
