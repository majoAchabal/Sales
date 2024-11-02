using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Net.Mail

namespace Prototipo
{
    public class EmailService
    {
        private string smtpServer = "smtp.gmail.com";
        private int smtpPort = 587;
        private string emailFrom = "salesappsofteng@gmail.com";
        private string password = "clientes2024";

        public void EnviarNotificacionDescuento(string emailTo, string mensaje)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailFrom);
            mail.To.Add(emailTo);
            mail.Subject = "Tienes un descuento disponible!";
            mail.Body = mensaje;

            SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);
            smtp.Credentials = new System.Net.NetworkCredential(emailFrom, password);
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mail);
                Console.WriteLine("Correo enviado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
    }
}
