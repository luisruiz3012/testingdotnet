using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VentasApi.Models;

namespace VentasApi.Controllers
{
    [ApiController]
    [Route("api/departamentos")]
    public class DepartmentosController : ControllerBase
    {
        private readonly DB _db;

        public DepartmentosController()
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

                    string query = "SELECT * FROM Departamentos";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    var departamentos = new List<Departamento>();


                    while (reader.Read())
                    {
                        var departamento = new Departamento
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString()
                        };

                        departamentos.Add(departamento);
                    }

                    conn.Close();


                    if (departamentos.Count > 0)
                    {
                        return Ok(departamentos);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("id")]
        public dynamic GetById(int id)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = $"SELECT * FROM departamentos WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Departamento departamento = new Departamento
                        {
                            Id = (int)(reader["Id"]),
                            Nombre = reader["Nombre"].ToString()
                        };

                        conn.Close();

                        return Ok(departamento);
                    }

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("id")]
        public dynamic DeleteById(int id)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM departamentos WHERE id = @Id";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Id", id);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return NotFound(new { message = "Department not found" });
                    }

                    string deleteQuery = "DELETE FROM departamentos WHERE id = @Id";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@Id", id);
                    deleteCmd.ExecuteNonQuery();

                    conn.Close();

                    return StatusCode(202, new { message = "Deleted successfully" });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("")]
        public dynamic Create([FromBody] Departamento departamento)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO Departamentos (nombre) VALUES (@Nombre)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    return StatusCode(201, new { message = "Created Successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public dynamic Update(int id, [FromBody] Departamento departamento)
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "UPDATE Departamentos SET nombre = @Nombre WHERE id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    return Ok(new { message = "Updated successfully" });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
