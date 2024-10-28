using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    internal class Cliente
    {
        public int Nit {  get; set; }
        public string Name { get; set; }
        public string Celular { get; set; }

        public ConexionSQL Conexion = new ConexionSQL();

        public Cliente()
        {

        }

        public Cliente(int nit, string name, string celular)
        {
            Nit = nit;
            Name = name;
            Celular = celular;
        }

        public void AgregarCliente(int nit, string nombre, string cel)
        {
            string query = "insert into clientes(nitclientes, nombrecliente, celularcliente) values (@nit, @name, @celular)";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nit", nit);
                    cmd.Parameters.AddWithValue("@name", nombre);
                    cmd.Parameters.AddWithValue("@celular", cel);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente agregado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar el cliente: {ex.Message}");
                    }
                }
            }
        }

        public Cliente BuscarCliente(int nit)
        {
            string query = "select nitclientes, nombrecliente, celularcliente from clientes where nitclientes = @nit";
            ConexionSQL conexion = new ConexionSQL();
            Cliente cliente = null;

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nit", nit);

                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            cliente = new Cliente
                            {
                                Nit = reader.GetInt32("nitclientes"),
                                Name = reader.GetString("nombrecliente"),
                                Celular = reader.GetString("celularcliente")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return cliente;
        }

        public void ActualizarCliente(int nit, string nombre, string cel)
        {
            string query = "UPDATE clientes SET nombrecliente = @name, celularcliente = @cel WHERE nitclientes = @nit";
            using (var conn = Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nit", nit);
                    cmd.Parameters.AddWithValue("@name", nombre);
                    cmd.Parameters.AddWithValue("@cel", cel);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente actualizado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar el cliente: {ex.Message}");
                    }
                }
            }
        }

        public void EliminarCliente(int nit)
        {
            string query = "DELETE FROM clientes WHERE nitclientes = @nit";
            ConexionSQL conexion = new ConexionSQL();

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nit", nit);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el cliente con el NIT especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    internal class Cliente
    {
        public int Nit {  get; set; }
        public string Name { get; set; }
        public string Celular { get; set; }

        public ConexionSQL Conexion = new ConexionSQL();

        public Cliente()
        {

        }

        public Cliente(int nit, string name, string celular)
        {
            Nit = nit;
            Name = name;
            Celular = celular;
        }

        public void AgregarCliente(int nit, string nombre, string cel)
        {
            string query = "insert into clientes(nitclientes, nombrecliente, celularcliente) values (@nit, @name, @celular)";
            using (var conn = this.Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nit", nit);
                    cmd.Parameters.AddWithValue("@name", nombre);
                    cmd.Parameters.AddWithValue("@celular", cel);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente agregado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar el cliente: {ex.Message}");
                    }
                }
            }
        }

        public Cliente BuscarCliente(int nit)
        {
            string query = "select nitclientes, nombrecliente, celularcliente from clientes where nitclientes = @nit";
            ConexionSQL conexion = new ConexionSQL();
            Cliente cliente = null;

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nit", nit);

                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            cliente = new Cliente
                            {
                                Nit = reader.GetInt32("nitclientes"),
                                Name = reader.GetString("nombrecliente"),
                                Celular = reader.GetString("celularcliente")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return cliente;
        }

        public void ActualizarCliente(int nit, string nombre, string cel)
        {
            string query = "UPDATE clientes SET nombrecliente = @name, celularcliente = @cel WHERE nitclientes = @nit";
            using (var conn = Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nit", nit);
                    cmd.Parameters.AddWithValue("@name", nombre);
                    cmd.Parameters.AddWithValue("@cel", cel);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente actualizado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar el cliente: {ex.Message}");
                    }
                }
            }
        }

        public void EliminarCliente(int nit)
        {
            string query = "DELETE FROM clientes WHERE nitclientes = @nit";
            ConexionSQL conexion = new ConexionSQL();

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nit", nit);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el cliente con el NIT especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

