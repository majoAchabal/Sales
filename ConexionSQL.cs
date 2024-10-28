using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Prototipo
{
    internal class ConexionSQL
    {
        string connectionString = "Server=127.0.0.1;Port=3306;Database=SalesApp;Uid=root;Pwd=Fabi79736610;";
        public SQLiteConnection AbrirConexion()
        {
            SQLiteConnection conexion = new SQLiteConnection(this.connectionString);
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

        public void CerrarConexion(SQLiteConnection conexion)
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
