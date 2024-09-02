using DepartamentosLibrary.Database;
using DepartamentosLibrary.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DepartamentosLibrary
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

                string query = "SELECT * FROM Departamentos".Replace("--", "").Replace("'", "").Replace("%", "");
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
                    return departamentos;
                }
                else
                {
                    return null;
                }
            }
        }

        public dynamic GetById(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = $"SELECT * FROM departamentos WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
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

                    return departamento;
                }

                return null;
            }
        }

        public dynamic Create(Departamento departamento)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO Departamentos (nombre) VALUES (@Nombre)".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                cmd.ExecuteNonQuery();

                conn.Close();

                conn.Open();

                string query2 = "SELECT * FROM Departamentos WHERE nombre = @Nombre".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                SqlDataReader reader = cmd2.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                conn.Close();
                return new { message = "Created Successfully" };
            }
        }

        public dynamic Update(int id, Departamento departamento)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Departamentos WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                conn.Close();

                conn.Open();

                string query2 = "UPDATE Departamentos SET nombre = @Nombre WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                cmd2.Parameters.AddWithValue("@Id", id);
                cmd2.ExecuteNonQuery();

                conn.Close();
                return new { message = "Updated successfully" };

            }
        }

        public dynamic Delete(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM departamentos WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Id", id);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    return null;
                }

                string deleteQuery = "DELETE FROM departamentos WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                deleteCmd.Parameters.AddWithValue("@Id", id);
                deleteCmd.ExecuteNonQuery();

                conn.Close();

                return new { message = "Deleted successfully" };

            }
        }
    }
}
