using System.Collections.Generic;

namespace Contador_Luz.Models
{
    public class Home
    {
        public List<Local> Locales {get;set;}
        
        public int VatiosHoy {get;set;}
        public int TotalVatiosConsumidosHoy {get;set;}

        public int ValorVatio {get;set;}

        public int Subsidio {get;set;}

    }
}