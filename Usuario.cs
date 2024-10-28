using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Prototipo
{
    internal class Usuario
    {
        public string Username {  get; set; }
        public string Password { get; set; }
        public string Rol {  get; set; }

        public ConexionSQL Conexion = new ConexionSQL();

        public Usuario() { }
        public Usuario(string username, string password, string rol)
        {
            this.Username = username;
            this.Password = password;
            this.Rol = rol;
        }

        public void AddUser(string username, string password, string rol)
        {
            string query = "insert into usuarios(username, passwordd, rol) values (@username, @pass, @rol)";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    cmd.Parameters.AddWithValue("@rol", rol);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario agregado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar el usuario: {ex.Message}");
                    }
                }
            }
        }

        public Usuario BuscarUsuario(string username)
        {
            string query = "select passwordd, rol from usuarios where username = @username";
            ConexionSQL conexion = new ConexionSQL();
            Usuario usuario = null;

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            usuario = new Usuario
                            {
                                Password = reader.GetString("passwordd"),
                                Rol = reader.GetString("rol")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return usuario;
        }

        public void ActualizarUsuario(string username, string password, string rol)
        {
            string query = "update usuarios set passwordd = @pass, rol = @rol where username = @username";
            using (var conn = Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pass", password);
                    cmd.Parameters.AddWithValue("@rol", rol);
                    cmd.Parameters.AddWithValue("@username", username);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario actualizado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar el usuario: {ex.Message}");
                    }
                }
            }
        }

        public void EliminarUsuario(string username)
        {
            string query = "DELETE FROM usuarios WHERE username = @username";
            ConexionSQL conexion = new ConexionSQL();

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el Usuario con el username especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
