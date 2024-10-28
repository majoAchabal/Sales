using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    internal class Producto
    {
        public int Id { get; set; }
        public Categoria Categoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int StockDisponible { get; set; }

        private ConexionSQL Conexion = new ConexionSQL();

        public Producto() { }

        public Producto(Categoria categoria, string nombre, string descripcion, decimal precio, int stockDisponible)
        {
            this.Categoria = categoria;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Precio = precio;
            this.StockDisponible = stockDisponible;
        }

        public void CrearProducto(Categoria categoria, string nombre, string descripcion, decimal precio, int stockDisponible)
        {
            string query = "insert into productos(categoriaproducto, nombreproducto, descripcionproducto, precioproducto, stockdisponible) values (@CategoriaId, @Nombre, @Descripcion, @Precio, @Stock)";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoriaId", categoria.Id);
                    cmd.Parameters.AddWithValue("@Nombre", Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", Precio);
                    cmd.Parameters.AddWithValue("@Stock", StockDisponible);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Producto agregado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar el producto: {ex.Message}");
                    }
                }
            }
        }

        public void Modificar(int id, Categoria categoria, string nombre, string descripcion, decimal precio, int stock)
        {
            this.Id = id;
            this.Categoria = categoria;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Precio = precio;
            this.StockDisponible = stock;

            string query = "UPDATE productos SET categoriaproducto = @CategoriaId, nombre = @Nombre, descripcion = @Descripcion, precio = @Precio, stock_disponible = @Stock WHERE id = @Id";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@CategoriaId", categoria.Id);
                    cmd.Parameters.AddWithValue("@Nombre", Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", Precio);
                    cmd.Parameters.AddWithValue("@Stock", StockDisponible);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Producto modificado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al modificar el producto: {ex.Message}");
                    }
                }
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM productos WHERE idproductos = @Id";
            using (var conn = this.Conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el producto con el ID especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public Producto Buscar(int id)
        {
            string query = "SELECT idproductos, categoriaproducto, nombreproducto, descripcionproducto, precioproducto, stockdisponible " + "from productos " + "where idproductos = @Id";

            Producto producto = null;

            using (var conn = this.Conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            int categoriaId = reader.GetInt32("categoriaproducto");
                            Categoria categoria = new Categoria().BuscarCategoriaPorID(categoriaId);
                            producto = new Producto
                            {
                                Id = reader.GetInt32("idproductos"),
                                Categoria = categoria,
                                Nombre = reader.GetString("nombreproducto"),
                                Descripcion = reader.GetString("descripcionproducto"),
                                Precio = reader.GetDecimal("precioproducto"),
                                StockDisponible = reader.GetInt32("stockdisponible"),
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return producto;
        }

        public void IncrementarCantidad(int nro)
        {
            StockDisponible += nro;

            string query = "UPDATE productos SET stockdisponible = @Stock WHERE idproductos = @Id";
            using (var conn = this.Conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Stock", StockDisponible);
                cmd.Parameters.AddWithValue("@Id", Id);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cantidad incrementada correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al incrementar la cantidad: {ex.Message}");
                }
            }
        }

        public void DecrementarCantidad(int nro)
        {
            if (StockDisponible >= nro)
            {
                StockDisponible -= nro;

                string query = "UPDATE productos SET stockdisponible = @Stock WHERE idproductos = @Id";
                using (var conn = this.Conexion.AbrirConexion())
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Stock", StockDisponible);
                    cmd.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cantidad decrementada correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al decrementar la cantidad: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay suficiente cantidad disponible.");
            }
        }

    }
}
