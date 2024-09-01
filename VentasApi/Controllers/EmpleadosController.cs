using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VentasApi.Models;

namespace VentasApi.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    public class EmpleadosController : ControllerBase
    {
        private readonly DB _db;
        public EmpleadosController()
        {
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

                    string query = "SELECT * FROM Empleados";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Empleado> empleados = new List<Empleado>();

                    while (reader.Read())
                    {
                        Empleado empleado = new Empleado {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Departamento_Id = (int)reader["Departamento_id"],
                        };

                        empleados.Add(empleado);
                    }

                    if (empleados.Count > 0)
                    {
                        return Ok(empleados);
                    }

                    return NotFound();
                }
            } catch (Exception ex)
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
                using(var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM Empleados WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Empleado empleado = new Empleado
                        {
                            Id = (int)reader["id"],
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Departamento_Id = (int)reader["Departamento_id"]
                        };

                        return Ok(empleado);
                    }

                    return NotFound();
                }
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error {ex.Message}");
            }
        }

        [HttpPost]
        [Route("")]
        public dynamic Create([FromBody]  Empleado empleado)
        {
            try
            {
                using(var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO empleados (nombre, apellido, departamento_id) VALUES (@Nombre, @Apellido, @Departamento_Id)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@Departamento_Id", empleado.Departamento_Id);
                    cmd.ExecuteNonQuery();

                    return new { empleado };
                }
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public dynamic Update(int id, [FromBody]  Empleado empleado)
        {
            try
            {
                using(var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "UPDATE Empleados SET nombre = @Nombre, apellido = @Apellido, departamento_id = @Departamento_Id WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@Departamento_id", empleado.Departamento_Id);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();

                    return StatusCode(200, new { messge = "Updated successfully" });

                }
            } catch (Exception ex)
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

                    string query = "SELECT * FROM empleados WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return NotFound();
                    }

                    conn.Close();

                    conn.Open();
                    string query2 = "DELETE FROM empleados WHERE id = @Id";
                    cmd = new SqlCommand(query2, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    return new { message = "Deleted successfully" };
                }
            } catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
