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
            Engine.AlmacenarInfoBD();

            ViewBag.Locales = Home.Locales;
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
