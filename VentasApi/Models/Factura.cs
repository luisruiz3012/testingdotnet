namespace VentasApi.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public decimal Total { get; set; }
        public int IdCliente { get; set; }
        public int EmpleadoId { get; set; }
    }
}
