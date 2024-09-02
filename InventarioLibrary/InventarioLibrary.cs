using InventarioLibrary.Database;
using InventarioLibrary.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace InventarioLibrary
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

                string query = "SELECT * FROM inventario".Replace("--", "").Replace("'", "").Replace("%", "");
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
                    return productos;
                }

                return null;
            }
        }

        public dynamic GetById(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM inventario WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
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

                    return producto;
                }

                return null;
            }
        }

        public dynamic Create(Inventario producto)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO inventario (producto, precio, inventario) VALUES (@Producto, @Precio, @Inventario)".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Producto", producto.Producto);
                cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                cmd.Parameters.AddWithValue("@Inventario", producto.inventario);
                cmd.ExecuteNonQuery();

                conn.Close();

                conn.Open();
                string query2 = "SELECT * FROM inventario WHERE producto = @Producto".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Producto", producto.Producto);
                SqlDataReader reader = cmd2.ExecuteReader();


                if (reader.Read())
                {
                    conn.Close();
                    return new { message = "Created successfully" };
                }

                conn.Close();
                return null;
            }
        }

        public dynamic Update(int id, Inventario producto)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM inventario WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                conn.Close();

                conn.Open();

                string query2 = "UPDATE inventario SET producto = @Producto, precio = @Precio, inventario = @Inventario WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Producto", producto.Producto);
                cmd2.Parameters.AddWithValue("@Precio", producto.Precio);
                cmd2.Parameters.AddWithValue("@Inventario", producto.inventario);
                cmd2.Parameters.AddWithValue("@Id", id);
                cmd2.ExecuteNonQuery();

                return new { message = "Updated successfully" };
            }
        }

        public dynamic Delete(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM inventario WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read()) { return null; }

                conn.Close();

                conn.Open();

                string query2 = "DELETE FROM inventario WHERE id = @Id".Replace("--", "").Replace("'", "").Replace("%", "");
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Id", id);
                cmd2.ExecuteNonQuery();

                return new { message = "Deleted successfully" };
            }
        }
    }
}
