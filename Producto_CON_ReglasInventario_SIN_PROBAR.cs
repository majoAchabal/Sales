using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    public class Producto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int StockDisponible { get; set; }
        private ConexionSQL Conexion = new ConexionSQL();

        // Agregar una propiedad para las reglas de inventario
        public ReglasInventario ReglaInventario { get; set; }

        public Producto() { }

        public Producto(int categoriaId, string nombre, decimal precio, int stockDisponible)
        {
            this.CategoriaId = categoriaId;
            this.Nombre = nombre;
            this.Precio = precio;
            this.StockDisponible = stockDisponible;
        }

        // Método para revisar si se debe generar una alerta de inventario
        public void RevisarInventario()
        {
            if (ReglaInventario != null && ReglaInventario.VerificarInventario(StockDisponible))
            {
                GenerarAlerta();
            }
        }

        // Método para generar una alerta al proveedor
        private void GenerarAlerta()
        {
            MessageBox.Show($"Alerta: El producto '{Nombre}' (ID: {Id}) está por debajo del stock mínimo. Contactar al proveedor con ID {ReglaInventario.ProveedorID} para reabastecer.");
        }

        // Crear producto
        public void CrearProducto(int categoriaId, string nombre, decimal precio, int stockDisponible)
        {
            string query = "INSERT INTO productos(categoriaproducto, nombreproducto, precioproducto, stockdisponible) VALUES (@CategoriaId, @Nombre, @Precio, @Stock)";
            using (var conn = this.Conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CategoriaId", categoriaId);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Precio", precio);
                cmd.Parameters.AddWithValue("@Stock", stockDisponible);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto agregado correctamente.");
                    RevisarInventario(); // Revisa el inventario después de crear el producto
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al agregar el producto: {ex.Message}");
                }
            }
        }

        // Modificar producto
        public void Modificar(int id, int categoriaId, string nombre, decimal precio, int stock)
        {
            this.Id = id;
            this.CategoriaId = categoriaId;
            this.Nombre = nombre;
            this.Precio = precio;
            this.StockDisponible = stock;

            string query = "UPDATE productos SET categoriaproducto = @CategoriaId, nombreproducto = @Nombre, precioproducto = @Precio, stockdisponible = @Stock WHERE idproductos = @Id";
            using (var conn = this.Conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@CategoriaId", categoriaId);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Precio", Precio);
                cmd.Parameters.AddWithValue("@Stock", StockDisponible);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto modificado correctamente.");
                    RevisarInventario(); // Revisa el inventario después de modificar el producto
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al modificar el producto: {ex.Message}");
                }
            }
        }

        // Eliminar producto
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

        // Buscar producto
        public Producto Buscar(int id)
        {
            string query = "SELECT idproductos, categoriaproducto, nombreproducto, precioproducto, stockdisponible FROM productos WHERE idproductos = @Id";
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
                            producto = new Producto
                            {
                                Id = reader.GetInt32("idproductos"),
                                CategoriaId = reader.GetInt32("categoriaproducto"),
                                Nombre = reader.GetString("nombreproducto"),
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

        // Incrementar cantidad
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
                    RevisarInventario(); // Revisa el inventario después de incrementar
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al incrementar la cantidad: {ex.Message}");
                }
            }
        }

        // Decrementar cantidad
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
                        RevisarInventario(); // Revisa el inventario después de decrementar
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
