using System.Linq;

namespace Contador_Luz.Models
{
    public static class Calculator
    {
        public static void GenerarVatiosConsumidos(int AcumLocal, string IdLocal)
        {

            foreach (var lc in Home.Locales)
            {
                if (lc.Id == IdLocal)
                {
                    lc.VatiosConsumidos = AcumLocal - lc.AcumuladoVatiosAnterior;
                    lc.AcumuladoVatiosHoy = AcumLocal;
                }
            }
        }

        public static void GenerarValoresPago()
        {
            foreach (var lc in Home.Locales)
            {
                int Pago = lc.VatiosConsumidos * Home.ValorVatio;
                lc.Pago = Pago;
            }
        }

        public static void GenerarDescuentoSubsidio()
        {
            foreach (var lc in Home.Locales)
            {
                int Descuento = (Home.Subsidio * lc.PorcentajeSubAplicado) / 100;
                lc.Pago = lc.Pago - Descuento;
            }
        }
    }
}