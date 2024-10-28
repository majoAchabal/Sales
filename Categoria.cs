using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    internal class Categoria
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Descuento { get; set; }

        private ConexionSQL Conexion = new ConexionSQL();

        public Categoria() { }

        public Categoria(string name, string description, double descuento)
        {
            this.Name = name;
            this.Description = description;
            this.Descuento = descuento;
        }

        public void AgregarCategoria()
        {
            string query = "insert into categorias(nombrecategoria, descripcioncategoria, descuento) values (@Name, @Description, @Descuento);";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@Descuento", Descuento);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Categoría agregada correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar la categoría: {ex.Message}");
                    }
                }
            }
        }

        public Categoria BuscarCategoriaPorID(int id)
        {
            string query = "SELECT idcategorias, nombrecategoria, descripcioncategoria, descuento FROM categorias WHERE idcategorias = @id";
            ConexionSQL conexion = new ConexionSQL();
            Categoria categoria = null;

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            categoria = new Categoria
                            {
                                Id = reader.GetInt32("idcategorias"),
                                Name = reader.GetString("nombrecategoria"),
                                Description = reader.GetString("descripcioncategoria"),
                                Descuento = reader.GetDouble("descuento")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar la categoría: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return categoria;
        }

        public void ActualizarCategoria(int id, string newName, string newDescription, double newDescuento)
        {
            string query = "UPDATE categorias SET nombrecategoria = @Name, descripcioncategoria = @Description, descuento = @Descuento WHERE idcategorias = @id";
            using (var conn = Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@Name", newName);
                    cmd.Parameters.AddWithValue("@Description", newDescription);
                    cmd.Parameters.AddWithValue("@Descuento", newDescuento);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Categoría actualizada correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar la categoría: {ex.Message}");
                    }
                }
            }
        }

        public void EliminarCategoria(int id)
        {
            string query = "DELETE FROM categorias WHERE idcategorias = @id";
            ConexionSQL conexion = new ConexionSQL();

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Categoria eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la categoria con el ID especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar categoria: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }

}

