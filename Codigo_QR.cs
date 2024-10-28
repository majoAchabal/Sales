using System;
using System.Drawing;
using QRCoder;

namespace Facturacion
{
    public class GeneradorQR
    {
        private const int TamanoQR = 20; // Tamaño de los píxeles del QR

        // Método para generar la imagen QR de una factura
        public Bitmap GenerarCodigoQR(Factura factura)
        {
            if (factura == null)
                throw new ArgumentNullException(nameof(factura), "La factura no puede ser nula.");

            string contenidoQR = GenerarContenidoQR(factura);

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(contenidoQR, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                return qrCode.GetGraphic(TamanoQR);
            }
        }

        // Método para crear el contenido del QR con los datos de la factura
        private string GenerarContenidoQR(Factura factura)
        {
            return $"NIT:{factura.NIT}\n" +
                   $"Número de Factura:{factura.Id}\n" +
                   $"CUFD:{factura.CUFD}\n" +
                   $"Monto Total:{factura.Total}\n" +
                   $"Fecha Emisión:{factura.FechaEmision:yyyy-MM-dd}";
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
