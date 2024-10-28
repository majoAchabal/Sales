using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Prototipo
{
    internal class ConexionSQL
    {
        string connectionString = "Server=localhost;Port=3306;Database=sales;Uid=root;Pwd=12345;";
        public MySqlConnection AbrirConexion()
        {
            MySqlConnection conexion = new MySqlConnection(this.connectionString);
            try
            {
                conexion.Open();
                Console.WriteLine("Conexión a la base de datos abierta exitosamente.");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}");
            }
            return conexion;
        }

        public void CerrarConexion(MySqlConnection conexion)
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Open)
                {
                    conexion.Close();
                    Console.WriteLine("Conexión a la base de datos cerrada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar la conexión: {ex.Message}");
            }
        }
    }


}
