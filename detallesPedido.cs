using System;
using MySql.Data.MySqlClient;

namespace Prototipo
{
    internal class detallesPedido
    {
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public int CantidadPedida { get; set; }

        // Instance of the ConexionSQL class
        public ConexionSQL ConexionSQL = new ConexionSQL();

        public detallesPedido() { }

        public detallesPedido(int idPedido, int idProducto, int cantidadPedida)
        {
            IdPedido = idPedido;
            IdProducto = idProducto;
            CantidadPedida = cantidadPedida;
        }

        // Method to add a product to the order
        public void AgregarProducto(int idProducto, int cantidadPedida)
        {
            using (MySqlConnection conexion = ConexionSQL.AbrirConexion())
            {
                string query = "INSERT INTO detallespedido (idpedido, idproducto, cantidadpedido) VALUES (@idpedido, @idproducto, @cantidad)";
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@idpedido", IdPedido);
                command.Parameters.AddWithValue("@idproducto", idProducto);
                command.Parameters.AddWithValue("@cantidad", cantidadPedida);
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Producto {idProducto} agregado al pedido {IdPedido} con cantidad {cantidadPedida}.");
        }

        // Method to decrease the quantity of a product in the order
        public void QuitarProducto(int idProducto, int cantidad)
        {
            using (MySqlConnection conexion = ConexionSQL.AbrirConexion())
            {
                string query = "UPDATE detallespedido SET cantidadpedido = cantidadpedido - @cantidad WHERE idpedido = @idpedido AND idproducto = @idproducto";
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@idpedido", IdPedido);
                command.Parameters.AddWithValue("@idproducto", idProducto);
                command.Parameters.AddWithValue("@cantidad", cantidad);
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Cantidad de producto {idProducto} reducida en {cantidad} en el pedido {IdPedido}.");
        }

        // Method to completely remove a product from the order
        public void EliminarProducto(int idProducto)
        {
            using (MySqlConnection conexion = ConexionSQL.AbrirConexion())
            {
                string query = "DELETE FROM detallespedido WHERE idpedido = @idpedido AND idproducto = @idproducto";
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@idpedido", IdPedido);
                command.Parameters.AddWithValue("@idproducto", idProducto);
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Producto {idProducto} eliminado del pedido {IdPedido}.");
        }

        // Method to finalize the order
        public void Pedir()
        {
            using (MySqlConnection conexion = ConexionSQL.AbrirConexion())
            {
                string query = "UPDATE pedidos SET estado = 'Finalizado' WHERE idpedidos = @idpedido";
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@idpedido", IdPedido);
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Pedido {IdPedido} finalizado.");
        }
    }
}
