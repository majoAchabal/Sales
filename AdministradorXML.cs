using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Prototipo
{
    public class AdministradorXML
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
                    EnviarFactura(facturaId);

                    // Si se envió con éxito, actualizar el estado en la base de datos a "enviado"
                    ActualizarEstadoFactura(facturaId, "enviado");
                    Console.WriteLine($"Factura {facturaId} enviada exitosamente.");
                }
                catch (Exception ex)
                {
                    // Si el envío falla, actualizar el estado a "pendiente" y loguear el error
                    Console.WriteLine($"Error al enviar la factura {facturaId}: {ex.Message}. Se mantiene en pendientes.");
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
            // Aquí pondrías el código real para enviar el XML de la factura.
            if (new Random().Next(0, 3) == 0) // Simula una falla 1/3 del tiempo
            {
                throw new Exception("Fallo simulado en el envío.");
            }

            // Simulación de envío exitoso
            Console.WriteLine($"Factura {facturaId} enviada correctamente.");
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

        // Método para detener el temporizador al finalizar
        public void DetenerReintento()
        {
            reintentoTimer.Stop();
            reintentoTimer.Dispose();
            Console.WriteLine("Reintento de envío de facturas detenido.");
        }
    }
}
