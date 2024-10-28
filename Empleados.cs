using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    internal class Empleado
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Cargo { get; set; }
        public string Celular { get; set; }
        public string Username { get; set; }
        
        public ConexionSQL Conexion = new ConexionSQL();
        public Empleado() { }

        public Empleado(int id, string name, string surname, string cargo, string celular, string username)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Cargo = cargo;
            Celular = celular;
            Username = username;
        }

        public void AddEmpleado(string name, string surname, string cargo, string celular, string username)
        {
            string query = "insert into empleados(nombreempleados, apellidoempleados, cargoempleados, celularempleados, usernameempleados) values (@nombre, @apellido, @cargo, @cel, @username)";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", name);
                    cmd.Parameters.AddWithValue("@apellido", surname);
                    cmd.Parameters.AddWithValue("@cargo", cargo);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@cel", celular);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Empleado agregado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar el empleado: {ex.Message}");
                    }
                }
            }
        }

        public Empleado BuscarEmpleado(int id)
        {
            string query = "select nombreempleados, apellidoempleados, cargoempleados, celularempleados, usernameempleados from empleados where idempleados = @id";
            ConexionSQL conexion = new ConexionSQL();
            Empleado empleado = null;

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

                            empleado = new Empleado
                            {
                                Name = reader.GetString("nombreempleados"),
                                Surname = reader.GetString("apellidoempleados"),
                                Cargo = reader.GetString("cargoempleados"),
                                Celular = reader.GetString("celularempleados"),
                                Username = reader.GetString("usernameempleados")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar el empleado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return empleado;
        }

        public void UpdateEmpleado(int id, string name, string surname, string cargo, string celular, string username)
        {
            string query = "update empleados set nombreempleados = @nombre, apellidoempleados = @surname, cargoempleados = @cargo, celularempleados = @cel, usernameempleados = @uaer where idempleados = @id";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", name);
                    cmd.Parameters.AddWithValue("@surname", surname);
                    cmd.Parameters.AddWithValue("@cargo", cargo);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@cel", celular);
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Empleado actualizado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar el empleado: {ex.Message}");
                    }
                }
            }
        }

        public void EliminarEmpleado(int id)
        {
            string query = "DELETE FROM empleados WHERE idempleados = @id";
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
                        MessageBox.Show("Empleado eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el empleado con el ID especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar empleado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
