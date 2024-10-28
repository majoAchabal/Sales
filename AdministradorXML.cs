using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Timers;

namespace pruebas_Facturacion.Models
{
    internal class AdministradorXML
    {
        private readonly string connectionString;
        private readonly Timer reintentoTimer;

        public AdministradorXML(string connectionString)
        {
            this.connectionString = connectionString;

            // Configurar el temporizador para el reintento cada 10 minutos (600000 ms)
            reintentoTimer = new Timer(600000);
            reintentoTimer.Elapsed += (sender, e) => IntentarReenvioPendientes();
            reintentoTimer.AutoReset = true;
            reintentoTimer.Enabled = true;
        }

        // Método para intentar el reenvío de todas las facturas pendientes
        public void IntentarReenvioPendientes()
        {
            List<int> facturasPendientes = ObtenerFacturasPendientes();

            foreach (var facturaId in facturasPendientes)
            {
                try
                {
                    // Aquí iría el código de envío real, por ejemplo:
                    EnviarFactura(facturaId);

                    // Si se envió con éxito, actualizar el estado en la base de datos a "enviado"
                    ActualizarEstadoFactura(facturaId, "enviado");
                    Console.WriteLine($"Factura {facturaId} enviada exitosamente.");
                }
                catch (Exception)
                {
                    // Si el envío falla, actualizar el estado a "fallido" o mantenerlo como "pendiente"
                    Console.WriteLine($"Error al enviar la factura {facturaId}. Se mantiene en pendientes.");
                    ActualizarEstadoFactura(facturaId, "pendiente");
                }
            }
        }

        // Método para obtener IDs de facturas con estado "pendiente"
        private List<int> ObtenerFacturasPendientes()
        {
            List<int> facturasPendientes = new List<int>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id FROM Facturas WHERE Estado = 'pendiente'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        facturasPendientes.Add(reader.GetInt32(0));
                    }
                }
            }

            return facturasPendientes;
        }

        // Método para enviar una factura específica (aquí simulado)
        private void EnviarFactura(int facturaId)
        {
            // Simulación del envío: lanzar una excepción si falla
            // Aquí pondrías el código real para enviar el XML de la factura.
            if (new Random().Next(0, 3) == 0) // Simula una falla 1/3 del tiempo
            {
                throw new Exception("Fallo simulado en el envío.");
            }
        }

        // Método para actualizar el estado de una factura en la base de datos
        private void ActualizarEstadoFactura(int facturaId, string nuevoEstado)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Facturas SET Estado = @Estado WHERE Id = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@Id", facturaId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
