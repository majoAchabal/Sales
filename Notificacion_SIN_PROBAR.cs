using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Net.Mail;

namespace Prototipo
{
    public class Notificacion
    {
        private EmailService emailService;

        public Notificacion()
        {
            emailService = new EmailService();
        }

        public void EnviarDescuentoSiAplica(string emailCliente, int puntosActuales, int puntosParaDescuento)
        {
            if (puntosActuales >= puntosParaDescuento)
            {
                string mensaje = $"¡Hola! Tienes {puntosActuales} puntos acumulados y calificas para un descuento especial. ¡Canjea tus puntos en tu próxima compra!";
                emailService.EnviarNotificacionDescuento(emailCliente, mensaje);
            }
        }
    }

}
