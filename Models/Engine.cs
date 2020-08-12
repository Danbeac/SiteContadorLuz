using System.Collections.Generic;

namespace Contador_Luz.Models
{
    public static class Engine
    {
        public static void CargarLocales()
        {
            var ListLocales = new List<Local>(){
                                      new Local("1000","Farmacia",2180),
                                      new Local("2000","Fruteria",836)};
            
            Home.Locales = ListLocales;
        }
    }
}