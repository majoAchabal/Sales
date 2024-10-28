using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace pruebas_Facturacion.Models
{
    internal class Factura
    {
        // Atributos de la factura
        public int Id { get; set; }
        public string NumeroFactura { get; set; }
        public string CodigoAutorizacion { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NitComprador { get; set; }
        public string NombreComprador { get; set; }
        public decimal ImporteTotal { get; set; }
        public string CodigoControl { get; set; }
        public string TipoTransaccion { get; set; }
        public string Moneda { get; set; }
        public decimal TipoCambio { get; set; }
        public string Detalle { get; set; }

        // Atributos adicionales para la factura electrónica
        public string CUIS { get; private set; }  // Código Único de Inicio de Sistema
        public string CUFD { get; private set; }  // Código Único de Facturación Diaria
        public string FirmaDigital { get; private set; }  // Firma digital de la factura

        private string connectionString = "tu_conexion_mysql_aqui";  // Cadena de conexión a MySQL
        private const int TamanoQR = 20; // Tamaño de los píxeles del QR

        // Constructor que recibe las instancias de GestionAutorizacion y CertificadoDigital
        public Factura(GestionAutorizacion gestionAutorizacion, CertificadoDigital certificadoDigital)
        {
            // Obtener CUIS y CUFD de la instancia de GestionAutorizacion
            this.CUIS = gestionAutorizacion.CUIS;
            this.CUFD = gestionAutorizacion.CUFD;

            // Generar la firma digital utilizando CertificadoDigital
            GenerarFirmaDigital(certificadoDigital);
        }

        // Método para generar el código de control (puedes personalizar la implementación)
        public void GenerarCodigoControl()
        {
            CodigoControl = "GenerarCódigo";  // Implementar el cálculo del código de control
        }

        // Método para calcular el importe total en caso de bonificaciones, descuentos, etc.
        public void CalcularImporteTotal(decimal subtotal, decimal descuentos = 0, decimal bonificaciones = 0)
        {
            ImporteTotal = subtotal - descuentos - bonificaciones;
        }

        // Método para generar la firma digital utilizando CertificadoDigital
        private void GenerarFirmaDigital(CertificadoDigital certificadoDigital)
        {
            // Usamos una representación de la factura como cadena para firmar
            string data = $"{NumeroFactura}{CodigoAutorizacion}{FechaEmision}{NitComprador}{NombreComprador}{ImporteTotal}{CUIS}{CUFD}";
            this.FirmaDigital = certificadoDigital.FirmarDocumento(data);
        }

        // Método para validar los datos de la factura antes de guardarla
        public bool ValidarFactura()
        {
            if (string.IsNullOrEmpty(NumeroFactura) || string.IsNullOrEmpty(CodigoAutorizacion) ||
                string.IsNullOrEmpty(NitComprador) || string.IsNullOrEmpty(NombreComprador) ||
                ImporteTotal <= 0 || string.IsNullOrEmpty(CUIS) || string.IsNullOrEmpty(CUFD) ||
                string.IsNullOrEmpty(FirmaDigital))
            {
                Console.WriteLine("Error: Datos faltantes o incorrectos en la factura.");
                return false;
            }
            return true;
        }

        // Método para guardar la factura en la base de datos
        public void GuardarEnBaseDeDatos()
        {
            if (ValidarFactura())
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Facturas (NumeroFactura, CodigoAutorizacion, FechaEmision, NitComprador, NombreComprador, ImporteTotal, CodigoControl, TipoTransaccion, Moneda, TipoCambio, Detalle, CUIS, CUFD, FirmaDigital)
                                     VALUES (@NumeroFactura, @CodigoAutorizacion, @FechaEmision, @NitComprador, @NombreComprador, @ImporteTotal, @CodigoControl, @TipoTransaccion, @Moneda, @TipoCambio, @Detalle, @CUIS, @CUFD, @FirmaDigital)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NumeroFactura", NumeroFactura);
                        cmd.Parameters.AddWithValue("@CodigoAutorizacion", CodigoAutorizacion);
                        cmd.Parameters.AddWithValue("@FechaEmision", FechaEmision);
                        cmd.Parameters.AddWithValue("@NitComprador", NitComprador);
                        cmd.Parameters.AddWithValue("@NombreComprador", NombreComprador);
                        cmd.Parameters.AddWithValue("@ImporteTotal", ImporteTotal);
                        cmd.Parameters.AddWithValue("@CodigoControl", CodigoControl);
                        cmd.Parameters.AddWithValue("@TipoTransaccion", TipoTransaccion);
                        cmd.Parameters.AddWithValue("@Moneda", Moneda);
                        cmd.Parameters.AddWithValue("@TipoCambio", TipoCambio);
                        cmd.Parameters.AddWithValue("@Detalle", Detalle);
                        cmd.Parameters.AddWithValue("@CUIS", CUIS);
                        cmd.Parameters.AddWithValue("@CUFD", CUFD);
                        cmd.Parameters.AddWithValue("@FirmaDigital", FirmaDigital);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Factura guardada en la base de datos.");
                    }
                }
            }
        }

        // Método para generar la imagen QR de la factura
        public Bitmap GenerarCodigoQR()
        {
            string contenidoQR = GenerarContenidoQR();
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(contenidoQR, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                return qrCode.GetGraphic(TamanoQR);
            }
        }

        // Método para crear el contenido del QR con los datos de la factura
        private string GenerarContenidoQR()
        {
            return $"NIT:{NitComprador}\n" +
                   $"Número de Factura:{Id}\n" +
                   $"CUFD:{CUFD}\n" +
                   $"Monto Total:{ImporteTotal}\n" +
                   $"Fecha Emisión:{FechaEmision:yyyy-MM-dd}";
        }

        // Método para guardar la imagen QR en una ubicación específica
        public void GuardarQR(Bitmap qrImage, string rutaArchivo)
        {
            if (qrImage == null)
                throw new ArgumentNullException(nameof(qrImage), "La imagen QR no puede ser nula.");
            if (string.IsNullOrEmpty(rutaArchivo))
                throw new ArgumentException("La ruta de archivo no puede estar vacía.", nameof(rutaArchivo));

            qrImage.Save(rutaArchivo, System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine($"Código QR guardado en: {rutaArchivo}");
        }
    }
}
