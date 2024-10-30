using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Prototipo
{
    public partial class GenerarFacturas : Form
    {
        private int facturaId;
        private Factura factura;
        private AdministradorXML administradorXML;
        public GenerarFacturas(int facturaId, Factura factura)
        {
            InitializeComponent();
            this.facturaId = facturaId;
            this.factura = factura;
            this.administradorXML = new AdministradorXML();

            if (factura != null)
            {

                inputID.Text = factura.NumeroFactura;
                inputCodAuto.Text = factura.CodigoAutorizacion;
                inputFecha.Value = factura.FechaEmision;
                inputNIT.Text = factura.NitComprador;
                inputNombre.Text = factura.NombreComprador;
                inputTotal.Text = factura.ImporteTotal.ToString();
                inputMoneda.Text = factura.Moneda;
                inputCambio.Text = factura.TipoCambio.ToString();
                inputTransaccion.Text = factura.TipoTransaccion;
                inputCodCon.Text = factura.CodigoControl;
                lvCarrito = factura.Detalle;
                inputCUIS.Text = factura.CUIS;
                inputCUFD.Text = factura.CUFD;
                inputFirma.Text = factura.FirmaDigital;

                MostrarQR();

                administradorXML.IntentarReenvioPendientes();

            }

            else
            {
                MessageBox.Show("No se encontró ninguna factura con el ID especificado.");
            }
        }

        //Generar imagen QR
        private void MostrarQR()
        {
            Bitmap qrImage = factura.GenerarCodigoQR();
            if(qrImage != null)
            {
                pictureBoxQR.Image = qrImage;
                pictureBoxQR.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                MessageBox.Show("Error al generar el codigo QR")   
            }

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }

    }
}
