using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VentasApi.Models;

namespace VentasApi.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly DB _db;
        public ClientesController()
        {
            _db = new DB();
        }

        [HttpGet]
        public dynamic Get()
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM clientes";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cliente> clientes = new List<Cliente>();

                    while (reader.Read())
                    {
                        Cliente cliente = new Cliente
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString()
                        };

                        clientes.Add(cliente);
                    }

                    conn.Close();

                    if (clientes.Count > 0)
                    {
                        return Ok(clientes);
                    }

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public dynamic GetById(int id)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM clientes WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Cliente cliente = new Cliente
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString()
                        };

                        conn.Close();

                        return Ok(cliente);
                    }

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("")]
        public dynamic Create([FromBody] Cliente cliente)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO clientes (nombre, apellido) VALUES (@Nombre, @Apellido)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    conn.Open();
                    string query2 = "SELECT * FROM clientes WHERE nombre = @Nombre AND apellido = @Apellido";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd2.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    SqlDataReader reader = cmd2.ExecuteReader();


                    if (reader.Read())
                    {
                        conn.Close();
                        return new { message = "Created successfully" };
                    }

                    conn.Close();
                    return StatusCode(400, "Bad request");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public dynamic Update(int id, [FromBody] Cliente cliente)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM clientes WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return NotFound();
                    }

                    conn.Close();

                    conn.Open();

                    string query2 = "UPDATE clientes SET nombre = @Nombre, apellido = @Apellido WHERE id = @Id";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd2.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    cmd2.Parameters.AddWithValue("@Id", id);
                    cmd2.ExecuteNonQuery();

                    return StatusCode(201, new { message = "Updated successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public dynamic Delete(int id)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM clientes WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read()) { return NotFound(); }

                    conn.Close();

                    conn.Open();

                    string query2 = "DELETE FROM clientes WHERE id = @Id";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@Id", id);
                    cmd2.ExecuteNonQuery();

                    return Ok(new { message = "Deleted successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
