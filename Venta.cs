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
    internal class Venta
    {
        public int Id {  get; set; }
        public int Vendedor { get; set; }
        public int Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public string NumeroFactura { get; private set; }

        private ConexionSQL Conexion = new ConexionSQL();
        private Factura factura;

        public Venta() { }

        public Venta(int vendedor, int cliente, DateTime fecha)
        {
            this.Vendedor = vendedor;
            this.Cliente = cliente;
            this.Fecha = fecha;
            this.NumeroFactura = ObtenerNumeroFactura();
        }

        public (string Nit, string Nombre) ObtenerDatosCliente(int clienteId)
        {
            Cliente cliente = new Cliente();
            var datosCliente = cliente.BuscarCliente(clienteId);

            if (datosCliente != null)
            {
                return (datosCliente.Nit.ToString(), datosCliente.Name);
            }
            else
            {
                MessageBox.Show("Cliente no encontrado.");
                return (null, null);
            }
        }

        private string ObtenerNumeroFactura()
        {
            Factura factura = new Factura();
            factura.NumeroFactura = factura.Id.ToString();
            return factura.NumeroFactura;
        }


        public void RegistrarVenta()
        {
            string query = "insert into venta(empleadoventa, clienteventa, fechaventa) values (@Vendedor, @Cliente, @Fecha);";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@vendedor", Vendedor);
                    cmd.Parameters.AddWithValue("@cliente", Cliente);
                    cmd.Parameters.AddWithValue("@fecha", Fecha);
                    cmd.Parameters.AddWithValue("@NumeroFactura", NumeroFactura);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Id = (int)cmd.LastInsertedId;
                        MessageBox.Show("Venta agregada correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar la venta: {ex.Message}");
                    }
                }
            }

        }

        public void eliminarVenta(int id)
        {
            string query = "DELETE FROM venta WHERE idventa = @id";
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
                        MessageBox.Show("Venta eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la venta con el ID especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public Venta buscarVenta(int id)
        {
            string query = "select empleadoventa, clienteventa, fechaventa FROM venta where idventa = @id";
            ConexionSQL conexion = new ConexionSQL();
            Venta venta = null;

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

                            venta = new Venta
                            {
                                Vendedor = reader.GetInt32("empleadoventa"),
                                Cliente = reader.GetInt32("clienteventa"),
                                Fecha = reader.GetDateTime("fechaventa")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return venta;
        }

       
    }
}
