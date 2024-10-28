using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;

namespace Prototipo
{
    internal class Proveedor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
        public string Telefono { get; set; }

        public ConexionSQL Conexion = new ConexionSQL();
        
        public Proveedor() { }

        public Proveedor(int id, string name, string direction, string telefono)
        {
            Id = id;
            Name = name;
            Direction = direction;
            Telefono = telefono;
        }

        public void AddProveedor(string nombre, string direccion,  string telefono)
        {
            string query = "insert into proveedores(nombreproveedores, direccionproveedores, telefono) values (@nombre, @direccion, @telefono)";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@direccion", direccion);
                    cmd.Parameters.AddWithValue("@telefono", telefono);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Proveedor agregado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar el proveedor: {ex.Message}");
                    }
                }
            }
        }

        public Proveedor BuscarProveedor(int id)
        {
            string query = "select nombreproveedores, direccionproveedores, telefono from proveedores where idproveedores = @id";
            ConexionSQL conexion = new ConexionSQL();
            Proveedor proveedor = null;

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

                            proveedor = new Proveedor
                            {
                                Name = reader.GetString("nombreproveedores"),
                                Direction = reader.GetString("direccionproveedores"),
                                Telefono = reader.GetString("telefono")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar el proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return proveedor;
        }

        public void ActualizarProveedor(int id, string nombre, string direccion, string telefono)
        {
            string query = "update proveedores set nombreproveedores = @nombre, direccionproveedores = @direction, telefono = @telefono where idproveedores = @id";
            using (var conn = Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@direction", direccion);
                    cmd.Parameters.AddWithValue("@telefono", telefono);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Proveedor actualizado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar el proveedor: {ex.Message}");
                    }
                }
            }
        }

        public void ElimiarProveedor(int id)
        {
            string query = "DELETE FROM proveedores WHERE idproveedores = @id";
            ConexionSQL conexion = new ConexionSQL();

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el proveedor con el ID especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
