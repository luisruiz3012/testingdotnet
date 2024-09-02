using EmpleadosLibrary.Database;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using EmpleadosLibrary.Models;

namespace EmpleadosLibrary
{
    public class Metodos
    {
        private readonly DB _db;

        public Metodos()
        {
            _db = new DB();
        }

        public dynamic Get()
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Empleados".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Empleado> empleados = new List<Empleado>();

                while (reader.Read())
                {
                    Empleado empleado = new Empleado
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        Departamento_Id = (int)reader["Departamento_id"],
                    };

                    empleados.Add(empleado);
                }

                if (empleados.Count > 0)
                {
                    return empleados;
                }

                return null;
            }
        }

        public dynamic GetById(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Empleados WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
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

                    return empleado;
                }

                return null;
            }
        }

        public dynamic Create(Empleado empleado)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO empleados (nombre, apellido, departamento_id) VALUES (@Nombre, @Apellido, @Departamento_Id)".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                cmd.Parameters.AddWithValue("@Departamento_Id", empleado.Departamento_Id);
                cmd.ExecuteNonQuery();

                string query2 = "SELECT * FROM empleados WHERE nombre = @Nombre, apellido = @Apellido, departamento_id = @Departamento_Id)".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd2.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                cmd2.Parameters.AddWithValue("@Departamento_Id", empleado.Departamento_Id);
                SqlDataReader reader = cmd2.ExecuteReader();

                if (!reader.Read())
                {
                    conn.Close();
                    return null;
                }

                conn.Close();
                return empleado;
            }
        }

        public dynamic Update(int id, Empleado empleado)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM empleados WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", empleado.Id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (! reader.Read())
                {
                    conn.Close();
                    return null;
                }

                conn.Close();
                conn.Open();

                string query2 = "UPDATE Empleados SET nombre = @Nombre, apellido = @Apellido, departamento_id = @Departamento_Id WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd2.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                cmd2.Parameters.AddWithValue("@Departamento_id", empleado.Departamento_Id);
                cmd2.Parameters.AddWithValue("@Id", id);
                cmd2.ExecuteNonQuery();

                conn.Close();
                return new { messge = "Updated successfully" };

            }
        }

        public dynamic Delete(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM empleados WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                conn.Close();

                conn.Open();
                string query2 = "DELETE FROM empleados WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                cmd = new SqlCommand(query2, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();

                return new { message = "Deleted successfully" };
            }
        }
    }
}
