namespace VentasApi.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required int Departamento_Id { get; set; }

    }
}
