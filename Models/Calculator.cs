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

         public static void VatiosConsumidosLocales(int acumLocalA, int acumLocalB)
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

        public static void SubsidioXLocal(int subLocalA, int subLocalB)
        {
            int[] arrSubsidio = new int[2] { subLocalA, subLocalB };
            int i = 0;

            foreach (var lc in Home.Locales)
            {
                Home.Locales[i].PorcentajeSubAplicado = arrSubsidio[i];
                i++;
            }

            GenerarDescuentoSubsidio();

        }
    }
}