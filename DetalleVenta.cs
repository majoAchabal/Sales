using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    public class DetalleVenta
    {
        public int IdVenta { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }

        private ConexionSQL Conexion = new ConexionSQL();

        public DetalleVenta() { }

        public DetalleVenta(int idVenta, Producto producto, int cantidad)
        {
            IdVenta = idVenta;
            Producto = producto;
            Cantidad = cantidad;
            Total = calcularTotal();
        }

        public void agregar(int idProducto, int cantidad)
        {
            Producto producto = new Producto().Buscar(idProducto);
            if (producto != null)
            {
                if (Producto != null && Producto.Id == producto.Id)
                {
                    Cantidad += cantidad;
                }
                else
                {
                    Producto = producto;
                    Cantidad = cantidad;
                }
                Total = calcularTotal();
                MessageBox.Show("Producto agregado al carrito.");
            }
            else
            {
                MessageBox.Show("El producto no existe.");
            }
        }

        public void eliminarProducto(int idProducto)
        {
            if (Producto != null && Producto.Id == idProducto)
            {
                Producto = null;
                Cantidad = 0;
                Total = 0;
                MessageBox.Show("Producto eliminado del carrito.");
            }
            else
            {
                MessageBox.Show("El producto no está en el carrito.");
            }
        }

        public decimal calcularTotal()
        {
            if (Producto != null)
            {
                return Producto.Precio * Cantidad;
            }
            return 0;
        }

        public void finalizarCompra()
        {
            if (Producto != null && Cantidad > 0)
            {
                string query = "INSERT INTO detallesventa (iddetallesventa, productoid, cantidaddetalleventa, total) VALUES (@IdVenta, @ProductId, @Cantidad, @Total)";
                using (var conn = this.Conexion.AbrirConexion())
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdVenta", IdVenta);
                    cmd.Parameters.AddWithValue("@ProductId", Producto.Id);
                    cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
                    cmd.Parameters.AddWithValue("@Total", Total);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Compra finalizada con éxito.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al finalizar la compra: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay productos en el carrito para finalizar la compra.");
            }
        }
    }
}
