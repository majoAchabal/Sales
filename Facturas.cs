using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using QRCoder; 

namespace Prototipo
{
    public partial class Facturas : Form
    {
        private Menu menu;
        public int Id { get; set; }
        public string NumeroFactura { get; set; }
        public string CodigoAutorizacion { get; set; } // permiso oficial para facturar
        public DateTime FechaEmision { get; set; }
        public string NitComprador { get; set; }
        public string NombreComprador { get; set; }
        public decimal ImporteTotal { get; set; }
        public string CodigoControl { get; set; } // verifica autenticidad de la factura
        public string TipoTransaccion { get; set; }
        public string Moneda { get; set; }
        public decimal TipoCambio { get; set; }
        public string Detalle { get; set; }
        public decimal TotalVenta { get; set; }

        // Atributos adicionales para la factura electrónica
        public string CUIS { get; private set; }  // Código Único de Inicio de Sistema
        public string CUFD { get; private set; }  // Código Único de Facturación Diaria
        public string FirmaDigital { get; private set; }  // Firma digital de la factura

        private ConexionSQL Conexion;
        private const int TamanoQR = 20;

        public GestionAutorizaciones gestionAutorizacion;
        public CertificadoDigital certificadoDigital;

        public string cert;
        public string pass = "12387687675e654ej34";
        public Facturas(Menu menu, ConexionSQL conexion)
        {
            InitializeComponent();

            this.menu = menu;
            this.Conexion = conexion;

            // Initialize these instances here in the constructor
            this.gestionAutorizacion = new GestionAutorizaciones(Conexion);
            this.certificadoDigital = new CertificadoDigital(cert, pass);

            this.CUIS = gestionAutorizacion.CUIS;
            this.CUFD = gestionAutorizacion.CUFD;
        }

        //CERRAR
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
            menu.Show();
        }
        private void btnCerrar_MouseHover(object sender, EventArgs e)
        {
            btnCerrar.Image = Properties.Resources.btncerrarseleccionado;
        }

        private void btnCerrar_MouseLeave(object sender, EventArgs e)
        {
            btnCerrar.Image = Properties.Resources.btncerrar;
        }
        //

        //BUSCAR

        private void btnBuscar_Click(object sender, EventArgs e)
        {

        }

        private void btnBuscar_MouseHover(object sender, EventArgs e)
        {
            btnBuscar.Image = Properties.Resources.btnbuscarseleccionado;
        }

        private void btnBuscar_MouseLeave(object sender, EventArgs e)
        {
            btnBuscar.Image = Properties.Resources.btnbuscar;
        }
        //

        //CREAR

        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (inputNombre.Text == "" | inputNIT.Text == "" | inputFecha.Value == null | cbMoneda.SelectedItem == null | cbTipoTransaccion.SelectedItem == null)
            {
                MessageBox.Show("Ingrese todos los datos completos");
            }

            else
            {
                Factura factura = new Factura(gestionAutorizacion, certificadoDigital);

                // Set necessary properties
                factura.NitComprador = inputNIT.Text;
                factura.NombreComprador = inputNombre.Text;
                factura.FechaEmision = inputFecha.Value;
                factura.TipoTransaccion = cbTipoTransaccion.SelectedItem.ToString();
                factura.Moneda = cbMoneda.SelectedItem.ToString();

                // Calculate and set the ImporteTotal
                int idVenta = ObtenerIdVentaPorFactura(factura.Id); // Replace with your actual method to retrieve the Venta ID
                factura.ImporteTotal = CalcularImporteTotal(idVenta);
                factura.CodigoAutorizacion = GenerarCodigoAutorizacion();
                factura.CodigoControl = GenerarCodigoControl();
                factura.NumeroFactura = GenerarNumeroFactura();

                if (cbMoneda.Text == "Dolar")
                {
                    factura.TipoCambio = 6.96M;
                }

                else
                {
                    factura.TipoCambio = 0.14M;
                }

                factura.TotalVenta = ObtenerTotalVenta(idVenta);
                cert = certificadoDigital.CargarCertificado();
                AsignarFirmaDigital(certificadoDigital, pass);

                // Save Factura to the database
                GuardarEnBaseDeDatos(factura);

                MessageBox.Show("Factura creada y guardada correctamente.");
            }

        }

        private void btnCrear_MouseHover(object sender, EventArgs e)
        {
            btnCrear.Image = Properties.Resources.btncrearseleccionado;
        }

        private void btnCrear_MouseLeave(object sender, EventArgs e)
        {
            btnCrear.Image = Properties.Resources.btncrear;
        }
        //

        //ELIMINAR

        private void btnEliminar_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_MouseHover(object sender, EventArgs e)
        {
            btnEliminar.Image = Properties.Resources.btneliminarseleccionado;
        }

        private void btnEliminar_MouseLeave(object sender, EventArgs e)
        {
            btnEliminar.Image = Properties.Resources.btneliminar;
        }
        //



        //FACTURAS

        private void Facturas_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titulo;
        }

        private void inputNombre_Enter(object sender, EventArgs e)
        {

        }



        //GENERAR

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            GenerarFacturas generarfactura = new GenerarFacturas(); // Asegúrate de que AdminUsuarios esté definido
            generarfactura.Show();
            this.Hide();
        }

        private void btnGenerar_MouseHover(object sender, EventArgs e)
        {
            btnGenerar.Image = Properties.Resources.btngenerarseleccionado;
        }

        private void btnGenerar_MouseLeave(object sender, EventArgs e)
        {
            btnGenerar.Image = Properties.Resources.btnGenerar;
        }

        public int ObtenerIdVentaPorFactura(int idFactura)
        {
            int idVenta = 0;

            using (var conn = this.Conexion.AbrirConexion())
            {
                conn.Open();
                string query = "SELECT idventa FROM venta WHERE numerofactura = @idFactura";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idFactura", idFactura);

                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idVenta = Convert.ToInt32(result);
                    }
                    else
                    {
                        Console.WriteLine("No se encontró una venta asociada a esta factura.");
                    }
                }
            }

            return idVenta;
        }

        public decimal CalcularImporteTotal(int idVenta)
        {
            ImporteTotal = ObtenerTotalVenta(idVenta);
            return ImporteTotal;
        }
        public decimal ObtenerTotalVenta(int idVenta)
        {
            decimal totalVenta = 0;

            string query = @"SELECT SUM(total) AS TotalVenta 
                             FROM detallesventa 
                             WHERE idventa = @idVenta";

            using (var conn = this.Conexion.AbrirConexion())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);

                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        totalVenta = Convert.ToDecimal(result);
                    }
                }
            }

            return totalVenta;
        }

        public string GenerarCodigoAutorizacion()
        {
            // Genera un código de autorización único.
            return $"AUT-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        public string GenerarCodigoControl()
        {
            string datosParaCodificar = $"{NumeroFactura}{NitComprador}{FechaEmision:yyyyMMdd}{ImporteTotal}{CUFD}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(datosParaCodificar)).Substring(0, 20);
        }
        public string GenerarNumeroFactura()
        {
            string nuevoNumeroFactura = "00001"; // Default to "00001" if there are no existing records

            using (var conn = this.Conexion.AbrirConexion())
            {
                conn.Open();

                // Query to get the last NumeroFactura in descending order
                string query = "SELECT NumeroFactura FROM facturas ORDER BY Id DESC LIMIT 1";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Get the last NumeroFactura and parse it as an integer
                        string ultimoNumero = result.ToString();
                        int numero = int.Parse(ultimoNumero) + 1; // Increment by 1

                        // Format with leading zeros to ensure 5-digit representation
                        nuevoNumeroFactura = numero.ToString("D5");
                    }
                }
            }

            return nuevoNumeroFactura;
        }

        internal void AsignarFirmaDigital(CertificadoDigital certificadoDigital, string data)
        {
            cert = certificadoDigital.CargarCertificado();
            FirmaDigital = certificadoDigital.FirmarDocumento(data);
        }

        internal void GuardarEnBaseDeDatos(Factura factura)
        {
            if (ValidarFactura())
            {
                using (var conn = this.Conexion.AbrirConexion())
                {
                    conn.Open();
                    string query = @"INSERT INTO Facturas (NumeroFactura, CodigoAutorizacion, FechaEmision, NitComprador, NombreComprador, ImporteTotal, CodigoControl, TipoTransaccion, Moneda, TipoCambio, Detalle, CUIS, CUFD, FirmaDigital)
                                     VALUES (@NumeroFactura, @CodigoAutorizacion, @FechaEmision, @NitComprador, @NombreComprador, @ImporteTotal, @CodigoControl, @TipoTransaccion, @Moneda, @TipoCambio, @Detalle, @CUIS, @CUFD, @FirmaDigital)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NumeroFactura", factura.NumeroFactura);
                        cmd.Parameters.AddWithValue("@CodigoAutorizacion", factura.CodigoAutorizacion);
                        cmd.Parameters.AddWithValue("@FechaEmision", factura.FechaEmision);
                        cmd.Parameters.AddWithValue("@NitComprador", factura.NitComprador);
                        cmd.Parameters.AddWithValue("@NombreComprador", factura.NombreComprador);
                        cmd.Parameters.AddWithValue("@ImporteTotal", factura.ImporteTotal);
                        cmd.Parameters.AddWithValue("@CodigoControl", factura.CodigoControl);
                        cmd.Parameters.AddWithValue("@TipoTransaccion", factura.TipoTransaccion);
                        cmd.Parameters.AddWithValue("@Moneda", factura.Moneda);
                        cmd.Parameters.AddWithValue("@TipoCambio", factura.TipoCambio);
                        cmd.Parameters.AddWithValue("@Detalle", factura.Detalle);
                        cmd.Parameters.AddWithValue("@CUIS", factura.CUIS);
                        cmd.Parameters.AddWithValue("@CUFD", factura.CUFD);
                        cmd.Parameters.AddWithValue("@FirmaDigital", factura.FirmaDigital);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Factura guardada en la base de datos.");
                    }
                }
            }
        }
        public bool ValidarFactura()
        {
            if (string.IsNullOrEmpty(NumeroFactura) || string.IsNullOrEmpty(CodigoAutorizacion) ||
                string.IsNullOrEmpty(NitComprador) || string.IsNullOrEmpty(NombreComprador) ||
                ImporteTotal <= 0 || string.IsNullOrEmpty(CUIS) || string.IsNullOrEmpty(CUFD) ||
                string.IsNullOrEmpty(FirmaDigital))
            {
                MessageBox.Show("Error: Datos faltantes o incorrectos en la factura.");
                return false;
            }
            return true;
        }

    }
}