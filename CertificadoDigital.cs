using MySql.Data.MySqlClient;
using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Prototipo
{
    public class CertificadoDigital
    {
        public int Id { get; set; }
        private string rutaCertificado;
        private SecureString contraseña;
        private X509Certificate2 certificado;
        private ConexionSQL conexionSQL = new ConexionSQL();

        // Constructor para inicializar la ruta del archivo PFX y su contraseña
        public CertificadoDigital(string rutaCertificado, string contraseña)
        {
            this.rutaCertificado = rutaCertificado;
            this.contraseña = new SecureString();
            foreach (char c in contraseña)
                this.contraseña.AppendChar(c);
            this.contraseña.MakeReadOnly();
        }

        // Método para cargar el certificado desde el archivo PFX
        public void CargarCertificado()
        {
            try
            {
                certificado = new X509Certificate2(rutaCertificado, contraseña,
                    X509KeyStorageFlags.MachineKeySet |
                    X509KeyStorageFlags.PersistKeySet |
                    X509KeyStorageFlags.Exportable);
                Console.WriteLine("Certificado cargado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el certificado: {ex.Message}");
                throw;
            }
        }

        // Firma digital de datos de factura y devuelve la firma en Base64
        public string FirmarDocumento(string data)
        {
            if (certificado == null)
                throw new InvalidOperationException("Certificado no cargado.");

            try
            {
                using (RSA rsa = certificado.GetRSAPrivateKey())
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] signedBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    return Convert.ToBase64String(signedBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al firmar el documento: {ex.Message}");
                throw;
            }
        }

        // Inserta un nuevo registro de certificado en la base de datos
        public void GuardarCertificadoEnBD()
        {
            string query = "INSERT INTO certificados (ruta_certificado, contrasena) VALUES (@Ruta, @Contrasena)";
            using (var conn = conexionSQL.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ruta", rutaCertificado);
                    cmd.Parameters.AddWithValue("@Contrasena", ConvertToUnsecureString(contraseña));

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Certificado guardado en la base de datos correctamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al guardar el certificado en la base de datos: {ex.Message}");
                    }
                }
            }
        }

        // Método auxiliar para convertir SecureString a string
        private string ConvertToUnsecureString(SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        // Método para cargar el certificado desde la base de datos
        public void CargarCertificadoDesdeBD(int id)
        {
            string query = "SELECT ruta_certificado, contrasena FROM certificados WHERE id = @Id";
            using (var conn = conexionSQL.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                rutaCertificado = reader.GetString("ruta_certificado");
                                string contrasena = reader.GetString("contrasena");
                                this.contraseña = new SecureString();
                                foreach (char c in contrasena)
                                    this.contraseña.AppendChar(c);
                                this.contraseña.MakeReadOnly();
                                Console.WriteLine("Certificado cargado desde la base de datos.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al cargar el certificado desde la base de datos: {ex.Message}");
                    }
                }
            }
        }

        // Método para actualizar el certificado en la base de datos
        public void ActualizarCertificadoEnBD(int id)
        {
            string query = "UPDATE certificados SET ruta_certificado = @Ruta, contrasena = @Contrasena WHERE id = @Id";
            using (var conn = conexionSQL.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ruta", rutaCertificado);
                    cmd.Parameters.AddWithValue("@Contrasena", ConvertToUnsecureString(contraseña));
                    cmd.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Certificado actualizado correctamente en la base de datos.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al actualizar el certificado: {ex.Message}");
                    }
                }
            }
        }

        // Método para eliminar el certificado de la base de datos
        public void EliminarCertificadoEnBD(int id)
        {
            string query = "DELETE FROM certificados WHERE id = @Id";
            using (var conn = conexionSQL.AbrirConexion())
            {
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Certificado eliminado de la base de datos correctamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al eliminar el certificado: {ex.Message}");
                    }
                }
            }
        }
    }
}
