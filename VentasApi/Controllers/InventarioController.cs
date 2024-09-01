using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VentasApi.Models;

namespace VentasApi.Controllers
{
    [ApiController]
    [Route("api/inventario")]
    public class InventarioController : ControllerBase
    {
        private readonly DB _db;
        public InventarioController()
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

                    string query = "SELECT * FROM inventario";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Inventario> productos = new List<Inventario>();

                    while (reader.Read())
                    {
                        Inventario producto = new Inventario
                        {
                            Id = (int)reader["Id"],
                            Producto = reader["Producto"].ToString(),
                            Precio = (decimal)reader["Precio"],
                            inventario = (int)reader["Inventario"]
                        };

                        productos.Add(producto);
                    }

                    conn.Close();

                    if (productos.Count > 0)
                    {
                        return Ok(productos);
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

                    string query = "SELECT * FROM inventario WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Inventario producto = new Inventario
                        {
                            Id = (int)reader["Id"],
                            Producto = reader["Producto"].ToString(),
                            Precio = (decimal)reader["Precio"],
                            inventario = (int)reader["Inventario"]
                        };

                        conn.Close();

                        return Ok(producto);
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
        public dynamic Create([FromBody] Inventario producto)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO inventario (producto, precio, inventario) VALUES (@Producto, @Precio, @Inventario)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Producto", producto.Producto);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Inventario", producto.inventario);
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    conn.Open();
                    string query2 = "SELECT * FROM inventario WHERE producto = @Producto";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@Producto", producto.Producto);
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
        public dynamic Update(int id, [FromBody] Inventario producto)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM inventario WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return NotFound();
                    }

                    conn.Close();

                    conn.Open();

                    string query2 = "UPDATE inventario SET producto = @Producto, precio = @Precio, inventario = @Inventario WHERE id = @Id";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@Producto", producto.Producto);
                    cmd2.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd2.Parameters.AddWithValue("@Inventario", producto.inventario);
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

                    string query = "SELECT * FROM inventario WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read()) { return NotFound(); }

                    conn.Close();

                    conn.Open();

                    string query2 = "DELETE FROM inventario WHERE id = @Id";
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
