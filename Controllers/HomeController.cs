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
            PruebaQuery();
            PruebaProcedimiento();

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

            CalcularVatiosConsumidosLocales(AcumLocalA, AcumLocalB);
            Calculator.GenerarValoresPago();
            CalcularSubsidioXLocal(SubLocalA, SubLocalB);

            return View();
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
            Query.AgregarParametro("@opcion",2);
            ds = Query.EjecutarProcedimiento();

            dt = ds.Tables[0];
            var sound = dt.Rows[0][0];
        }

        private void CalcularSubsidioXLocal(int subLocalA, int subLocalB)
        {
            int[] arrSubsidio = new int[2] { subLocalA, subLocalB };
            int i = 0;

            foreach (var lc in Home.Locales)
            {
                Home.Locales[i].PorcentajeSubAplicado = arrSubsidio[i];
                i++;
            }

            Calculator.GenerarDescuentoSubsidio();

        }

        private void CalcularVatiosConsumidosLocales(int acumLocalA, int acumLocalB)
        {
            int[] arrAcumulados = new int[2] { acumLocalA, acumLocalB };
            int i = 0;

            foreach (var lc in Home.Locales)
            {

                string IdLocal = Home.Locales[i].Id;
                Calculator.GenerarVatiosConsumidos(arrAcumulados[i], IdLocal);
                i++;
            }
        }

        public IActionResult Privacy()
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
