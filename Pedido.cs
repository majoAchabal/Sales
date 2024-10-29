using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;

namespace Prototipo
{
    internal class Pedido
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
        public int ProveedorId { get; set; }

        public ConexionSQL Conexion = new ConexionSQL();

        public Pedido() { }

        public Pedido(int id, string fecha, string estado, decimal total, int proveedorId)
        {
            Id = id;
            Fecha = fecha;
            Estado = estado;
            Total = total;
            ProveedorId = proveedorId;
        }

        public void CambiarEstado(int id, string nuevoEstado)
        {
            Estado = nuevoEstado;

            string query = "UPDATE pedidos SET estado = @estado WHERE idpedidos = @id";
            using (var conn = Conexion.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Estado actualizado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar el estado del pedido: {ex.Message}");
                    }
                }
            }
        }


        public void EliminarPedido(int id)
        {
            string query = "DELETE FROM pedidos WHERE idpedidos = @id;";
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
                        MessageBox.Show("Pedido eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el Pedido con el id especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public Pedido BuscarPedido(int id) {

            string query = "SELECT * FROM pedidos WHERE idpedidos = @id";
            ConexionSQL conexion = new ConexionSQL();
            Pedido pedido = null;

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

                            pedido = new Pedido
                            {
                                 ProveedorId = reader.GetInt32(reader.GetOrdinal("proveedorpedido")),
                                 Fecha = reader.GetString(reader.GetOrdinal("fechapedido")),
                                 Estado = reader.GetString(reader.GetOrdinal("estadopedido")),
                                 Total = reader.GetDecimal(reader.GetOrdinal("totalpedido"))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar el pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return pedido;
        }



    }
}
