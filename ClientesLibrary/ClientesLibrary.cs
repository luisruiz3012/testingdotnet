using ClientesLibrary.Database;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ClientesLibrary.Models;

namespace ClientesLibrary
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

                string query = "SELECT * FROM clientes".Replace("--", "").Replace("'", "").Replace("%", "");
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

                return clientes;
            }
        }

        public dynamic GetById(int id)
        {
            Cliente cliente = null;

            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM clientes WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Cliente clienteEncontrado = new Cliente
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString()
                    };

                    cliente = clienteEncontrado;
                }
                conn.Close();

                return cliente;
            }
        }

        public dynamic Create(Cliente cliente)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO clientes (nombre, apellido) VALUES (@Nombre, @Apellido)".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                cmd.ExecuteNonQuery();

                conn.Close();

                conn.Open();
                string query2 = "SELECT * FROM clientes WHERE nombre = @Nombre AND apellido = @Apellido".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd2.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                SqlDataReader reader = cmd2.ExecuteReader();


                if (!reader.Read())
                {
                    conn.Close();
                    return null;
                }

                return new { message = "Created successfully" };
            }
        }

        public dynamic Update(int id, Cliente cliente)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM clientes WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                conn.Close();

                conn.Open();

                string query2 = "UPDATE clientes SET nombre = @Nombre, apellido = @Apellido WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd2.Parameters.AddWithValue("@Apellido", cliente.Apellido);
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

                string query = "SELECT * FROM clientes WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read()) { return null; }

                conn.Close();

                conn.Open();

                string query2 = "DELETE FROM clientes WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Id", id);
                cmd2.ExecuteNonQuery();

                conn.Close();

                return new { message = "Deleted successfully" };
            }
        }
    }
}
