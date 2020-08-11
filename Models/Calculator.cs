using System.Linq;

namespace Contador_Luz.Models
{
    public static class Calculator
    {
        public static void GenerarVatiosConsumidos(int AcumLocal, string IdLocal)
        {
            var lc = from loc in Home.Locales
                          where loc.Id == IdLocal
                          select (Local) loc;

            Local Local = (Local) lc;

            Local.VatiosConsumidos = AcumLocal - Local.AcumuladoVatiosAnterior;
        }

        public static void GenerarValoresPago()
        {
            foreach(var lc in Home.Locales)
            {
                int Pago = lc.VatiosConsumidos * Home.ValorVatio;
                lc.Pago = Pago;
            }
        }

        public static void GenerarDescuentoSubsidio() 
        {   
            foreach(var lc in Home.Locales)
            {
                int Descuento = (Home.Subsidio * lc.PorcentajeSubAplicado) / 100 ;
                lc.Pago = lc.Pago - Descuento;
            }
        }
    }
}