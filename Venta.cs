using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Prototipo
{
    public class Venta
    {
        public int Id { get; set; }
        public int Vendedor { get; set; }
        public int Cliente { get; set; }
        public string Fecha { get; set; }
        public string NumeroFactura { get; private set; }

        private ConexionSQL Conexion = new ConexionSQL();
        private Factura factura;

        public Venta() { }

        public Venta(int vendedor, int cliente, string fecha, GestionAutorizaciones gestionAutorizacion, CertificadoDigital certificadoDigital)
        {
            this.Vendedor = vendedor;
            this.Cliente = cliente;
            this.Fecha = fecha;
            this.factura = new Factura(gestionAutorizacion, certificadoDigital);
            this.NumeroFactura = factura.NumeroFactura;
        }


        // Method to get client data
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

        // Method to generate an invoice number
        private string GenerarNumeroFactura()
        {
            // Assuming Factura class has a proper constructor for generating an invoice number
            // Assuming you have a GestionAutorizaciones and CertificadoDigital instance
            GestionAutorizaciones gestionAutorizacion = new GestionAutorizaciones("connectionStringHere");
            CertificadoDigital certificadoDigital = new CertificadoDigital("path/to/certificado.pfx", "password");

            Factura factura = new Factura(gestionAutorizacion, certificadoDigital);

            factura.GenerarNumeroFactura();
            return factura.NumeroFactura;
        }

        // Method to register a sale in the database
        public void RegistrarVenta()
        {
            if (string.IsNullOrEmpty(NumeroFactura))
            {
                MessageBox.Show("No se ha generado un número de factura.");
                return;
            }

            string query = "INSERT INTO venta (empleadoventa, clienteventa, fechaventa, numerofactura) VALUES (@Vendedor, @Cliente, @Fecha, @NumeroFactura);";
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

        // Method to delete a sale by ID
        public void EliminarVenta(int id)
        {
            string query = "DELETE FROM venta WHERE idventa = @Id";
            using (var conn = this.Conexion.AbrirConexion())
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

        // Method to search for a sale by ID
        public Venta BuscarVenta(int id)
        {
            string query = "SELECT empleadoventa, clienteventa, fechaventa, numerofactura FROM venta WHERE idventa = @Id";
            Venta venta = null;

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
                            venta = new Venta
                            {
                                Id = id,
                                Vendedor = reader.GetInt32("empleadoventa"),
                                Cliente = reader.GetInt32("clienteventa"),
                                Fecha = reader.GetString("fechaventa"),
                                NumeroFactura = reader.GetString("numerofactura")
                            };
                        }
                        else
                        {
                            MessageBox.Show("No se encontró una venta con el ID especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
