using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VentasApi.Models;

namespace VentasApi.Controllers
{
    [ApiController]
    [Route("api/facturas")]
    public class FacturasController : ControllerBase
    {
        private readonly DB _db;
        public FacturasController() {
            _db = new DB();
        }

        [HttpGet]
        [Route("")]
        public dynamic Get()
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM facturas";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Factura> facturas = new List<Factura>();

                    while (reader.Read())
                    {
                        Factura factura = new Factura
                        {
                            Id = (int)reader["Id"],
                            Fecha = reader["Fecha"].ToString(),
                            Total = (decimal)reader["Total"],
                            IdCliente = (int)reader["Id_cliente"],
                            EmpleadoId = (int)reader["Empleado_id"]
                        };

                        facturas.Add(factura);
                    }

                    if (facturas.Count > 0)
                    {
                        return Ok(facturas);
                    }

                    return NotFound();
                }
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("")]
        public dynamic Create([FromBody] Factura factura) { 
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "EXEC ventas @Total @IdCliente @EmpleadoId @Cantidad @PrecioUnitario";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    return new { };
                }
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error {ex.Message}");
            }
        }
    }
}
