namespace VentasApi.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public decimal Total { get; set; }
        public int IdCliente { get; set; }
        public int EmpleadoId { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
