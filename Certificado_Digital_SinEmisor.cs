using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Text;

namespace Facturacion
{
    public class CertificadoDigital
    {
        private string rutaCertificado;
        private SecureString contraseña;
        private X509Certificate2 certificado;

        // Constructor para configurar la ruta del archivo PFX y su contraseña
        public CertificadoDigital(string rutaCertificado, string contraseña)
        {
            this.rutaCertificado = rutaCertificado;
            this.contraseña = new SecureString();
            foreach (char c in contraseña)
                this.contraseña.AppendChar(c);
            this.contraseña.MakeReadOnly();
        }

        // Carga el certificado PFX y su clave privada en el sistema
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

        // Firma los datos de la factura y devuelve la firma en Base64
        public string FirmarDocumento(string data)
        {
            if (certificado == null)
                throw new InvalidOperationException("Certificado no cargado.");

            try
            {
                // Extrae la clave privada RSA del certificado
                using (RSA rsa = certificado.GetRSAPrivateKey())
                {
                    // Convierte los datos a un array de bytes
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    // Genera la firma digital con SHA256 y PKCS1
                    byte[] signedBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                    // Convierte la firma a Base64
                    return Convert.ToBase64String(signedBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al firmar el documento: {ex.Message}");
                throw;
            }
        }
    }
}
