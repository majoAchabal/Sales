using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Prototipo
{
    internal class Venta
    {
        public int Id { get; set; }
        public int Vendedor { get; set; }
        public int Cliente { get; set; }
        public string Fecha { get; set; }
        public string NumeroFactura { get; private set; }

        private ConexionSQL Conexion = new ConexionSQL();
        private Factura factura;

        public Venta() { }

        public Venta(int vendedor, int cliente, string fecha)
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
            string query = "insert into venta(empleadoventa, clienteventa, fechaventa, numerofactura) values (@Vendedor, @Cliente, @Fecha, @NumeroFactura);";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Vendedor", Vendedor);
                    cmd.Parameters.AddWithValue("@Cliente", Cliente);
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
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
            string query = "DELETE FROM venta WHERE idventa = @Id";
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
            string query = "SELECT empleadoventa, clienteventa, fechaventa, numerofactura FROM venta WHERE idventa = @Id";
            ConexionSQL conexion = new ConexionSQL();
            Venta venta = null;

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

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
                                Fecha = reader.GetString("fechaventa"),
                                NumeroFactura = reader.GetString("numerofactura")
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
