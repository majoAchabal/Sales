using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pruebas_Facturacion.Models
{
    internal class GestionAutorizacion
    {
        public string CUIS { get; private set; }
        public string CUFD { get; private set; }
        public DateTime FechaVencimientoCUIS { get; private set; }
        public DateTime FechaVencimientoCUFD { get; private set; }
        private readonly string connectionString; // Cadena de conexión a la base de datos

        public GestionAutorizacion(string connectionString)
        {
            this.connectionString = connectionString;
            CargarUltimaAutorizacion(); // Cargar la última autorización al iniciar
        }

        // Método para cargar el último registro de autorización desde la base de datos
        private void CargarUltimaAutorizacion()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 1 CUIS, CUFD, FechaVencimientoCUIS, FechaVencimientoCUFD FROM Autorizaciones ORDER BY FechaGeneracion DESC";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CUIS = reader.GetString(0);
                        CUFD = reader.GetString(1);
                        FechaVencimientoCUIS = reader.GetDateTime(2);
                        FechaVencimientoCUFD = reader.GetDateTime(3);
                        Console.WriteLine("Autorización cargada desde la base de datos.");
                    }
                    else
                    {
                        // Si no hay autorizaciones en la base de datos, generar una nueva
                        GenerarNuevaAutorizacion();
                    }
                }
            }
        }

        // Generar y almacenar una nueva autorización
        public void GenerarNuevaAutorizacion()
        {
            CUIS = GeneradorCodigos.GenerarCUIS();
            CUFD = GeneradorCodigos.GenerarCUFD();
            FechaVencimientoCUIS = DateTime.Now.AddMonths(1);
            FechaVencimientoCUFD = DateTime.Now.AddDays(1);

            GuardarAutorizacionEnBD();
        }

        // Guardar la autorización en la base de datos
        private void GuardarAutorizacionEnBD()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Autorizaciones (CUIS, CUFD, FechaVencimientoCUIS, FechaVencimientoCUFD) VALUES (@CUIS, @CUFD, @FechaVencimientoCUIS, @FechaVencimientoCUFD)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CUIS", CUIS);
                    command.Parameters.AddWithValue("@CUFD", CUFD);
                    command.Parameters.AddWithValue("@FechaVencimientoCUIS", FechaVencimientoCUIS);
                    command.Parameters.AddWithValue("@FechaVencimientoCUFD", FechaVencimientoCUFD);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Autorización guardada en la base de datos.");
                }
            }
        }

        public void RenovarCUIS()
        {
            if (IsCUISVencido())
            {
                CUIS = GeneradorCodigos.GenerarCUIS();
                FechaVencimientoCUIS = DateTime.Now.AddMonths(1);
                GuardarAutorizacionEnBD();
                Console.WriteLine("CUIS renovado automáticamente.");
            }
        }

        public void RenovarCUFD()
        {
            if (IsCUFDVencido())
            {
                CUFD = GeneradorCodigos.GenerarCUFD();
                FechaVencimientoCUFD = DateTime.Now.AddDays(1);
                GuardarAutorizacionEnBD();
                Console.WriteLine("CUFD renovado automáticamente.");
            }
        }

        public bool IsCUISVencido() => DateTime.Now > FechaVencimientoCUIS;

        public bool IsCUFDVencido() => DateTime.Now > FechaVencimientoCUFD;

        public void NotificarVencimientoProximo()
        {
            TimeSpan umbralAlerta = TimeSpan.FromDays(1);

            if (FechaVencimientoCUIS - DateTime.Now < umbralAlerta)
            {
                Console.WriteLine("Alerta: El CUIS está próximo a vencer. Renovación recomendada.");
            }

            if (FechaVencimientoCUFD - DateTime.Now < umbralAlerta)
            {
                Console.WriteLine("Alerta: El CUFD está próximo a vencer. Renovación recomendada.");
            }
        }
    }
}



