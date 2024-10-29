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
    public partial class Carrito : Form
    {
        private Productos productos;
        private List<DetalleVenta> detalleVentas;
        public Carrito(Productos productos, List<DetalleVenta> detalleVentas)
        {
            InitializeComponent();
            this.productos = productos;
            this.detalleVentas = detalleVentas;
        }

        //CERRAR

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
            productos.Show();
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

        //CARRITO
        private void Carrito_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titulo;
            CargarCarrito();
        }

        private void CargarCarrito()
        {
            lvCarrito.Items.Clear();
            decimal total = 0;

            foreach (var detalle in detalleVentas)
            {
                if (detalle.Producto != null)
                {
                    ListViewItem item = new ListViewItem(detalle.Producto.Nombre);
                    item.SubItems.Add(detalle.Cantidad.ToString());
                    item.SubItems.Add(detalle.Producto.Precio.ToString("C"));
                    item.SubItems.Add(detalle.Total.ToString("C"));

                    lvCarrito.Items.Add(item);
                    total += detalle.Total;
                }
            }

            txtTotal.Text = total.ToString("C");
        }

        //
    }
}
