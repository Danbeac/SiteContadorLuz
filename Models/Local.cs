namespace Contador_Luz.Models
{
    public class Local 
    {
        public string Nombre {get; set;}

        public string Id {get; private set;}
        
        public int VatiosConsumidos {get;set;}

        public int AcumuladoVatiosAnterior {get;set;}

        public int AcumuladoVatiosHoy {get; set;}

        public int Pago {get;set;}

        public int PorcentajeSubAplicado {get;set;}

        public Local(string id,string nombre,int acumuladoVatiosAnterior)
        {
            this.Nombre = nombre;
            this.Id = id;
            this.AcumuladoVatiosAnterior = acumuladoVatiosAnterior;

        }

    }
}