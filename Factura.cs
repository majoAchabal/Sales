using MySql.Data.MySqlClient;
using QRCoder;
using System;
using System.Drawing;
using System.Text;

namespace Prototipo
{
    public class Factura
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
        public decimal TotalVenta { get; set; }

        // Atributos adicionales para la factura electrónica
        public string CUIS { get; private set; }
        public string CUFD { get; private set; }
        public string FirmaDigital { get; private set; }

        private ConexionSQL Conexion = new ConexionSQL();
        private const int TamanoQR = 20; // Tamaño de los píxeles del QR

        // Constructor que recibe las instancias de GestionAutorizacion y CertificadoDigital
        public Factura(GestionAutorizaciones gestionAutorizacion, CertificadoDigital certificadoDigital)
        {
            this.CUIS = gestionAutorizacion.CUIS;
            this.CUFD = gestionAutorizacion.CUFD;

            GenerarCodigoAutorizacion();
            GenerarCodigoControl();
            GenerarNumeroFactura();
            GenerarFirmaDigital(certificadoDigital);
        }

        // Método para generar el código de control
        public void GenerarCodigoControl()
        {
            string datosParaCodificar = $"{NumeroFactura}{NitComprador}{FechaEmision:yyyyMMdd}{ImporteTotal}{CUFD}";
            CodigoControl = Convert.ToBase64String(Encoding.UTF8.GetBytes(datosParaCodificar)).Substring(0, 20);
        }

        // Método para calcular el importe total con descuentos y bonificaciones
        public void CalcularImporteTotal(decimal subtotal, decimal descuentos = 0, decimal bonificaciones = 0)
        {
            ImporteTotal = subtotal - descuentos - bonificaciones;
        }

        // Método para generar la firma digital utilizando CertificadoDigital
        private void GenerarFirmaDigital(CertificadoDigital certificadoDigital)
        {
            string data = $"{NumeroFactura}{CodigoAutorizacion}{FechaEmision}{NitComprador}{NombreComprador}{ImporteTotal}{CUIS}{CUFD}";
            this.FirmaDigital = certificadoDigital.FirmarDocumento(data);
        }

        // Método para validar los datos de la factura antes de guardarla
        public bool ValidarFactura()
        {
            return !string.IsNullOrEmpty(NumeroFactura) && !string.IsNullOrEmpty(CodigoAutorizacion) &&
                   !string.IsNullOrEmpty(NitComprador) && !string.IsNullOrEmpty(NombreComprador) &&
                   ImporteTotal > 0 && !string.IsNullOrEmpty(CUIS) && !string.IsNullOrEmpty(CUFD) &&
                   !string.IsNullOrEmpty(FirmaDigital);
        }

        // Método para guardar la factura en la base de datos
        public void GuardarEnBaseDeDatos()
        {
            if (ValidarFactura())
            {
                using (var conn = this.Conexion.AbrirConexion())
                {
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
            else
            {
                Console.WriteLine("Error: La factura no es válida para guardar.");
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
                   $"Número de Factura:{NumeroFactura}\n" +
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

        // Método para generar el número de factura en formato secuencial
        public void GenerarNumeroFactura()
        {
            using (var conn = this.Conexion.AbrirConexion())
            {
                string query = "SELECT NumeroFactura FROM facturas ORDER BY Id DESC LIMIT 1";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string ultimoNumero = result.ToString();
                        int numero = int.Parse(ultimoNumero) + 1;
                        NumeroFactura = numero.ToString("D5");
                    }
                    else
                    {
                        NumeroFactura = "00001";
                    }
                }
            }
        }

        public void GenerarCodigoAutorizacion()
        {
            CodigoAutorizacion = $"AUT-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        // Asignar NIT desde venta
        public void AsignarNitDesdeVenta(int idVenta)
        {
            using (var conn = this.Conexion.AbrirConexion())
            {
                string query = @"SELECT clientes.nitclientes
                                FROM clientes
                                INNER JOIN venta ON clientes.nitclientes = venta.clienteventa
                                WHERE venta.idventa = @IdVenta";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdVenta", idVenta);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.NitComprador = reader.GetString("nitclientes");
                            Console.WriteLine($"NIT del comprador asignado desde la venta: {this.NitComprador}");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró un cliente asociado a esta venta.");
                        }
                    }
                }
            }
        }
    }
}
