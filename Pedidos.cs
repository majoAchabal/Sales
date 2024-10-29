using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Pedidos : Form
    {
        private Menu menu;
        private Pedido pedido;
        public Pedidos(Menu menu)
        {
            InitializeComponent();
            this.menu = menu;

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
            int pedidoId;
            if (int.TryParse(inputID.Text, out pedidoId))
            {
                Pedido pedidoBuscado = pedido.BuscarPedido(pedidoId);
                if (pedidoBuscado != null)
                {
                    inputFecha.Text = pedidoBuscado.Fecha;
                    cbEstado.Text = pedidoBuscado.Estado;
                    inputTotal.Text = pedidoBuscado.Total.ToString("C");
                    cbProveedor.Text = pedidoBuscado.ProveedorId.ToString();
                }
                else
                {
                    MessageBox.Show("Pedido no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            Productos productosForm = new Productos(menu, "Pedidos");
            productosForm.Show();
            this.Hide();
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

        private void btnEliminar_MouseHover(object sender, EventArgs e)
        {
            btnEliminar.Image = Properties.Resources.btneliminarseleccionado;
        }

        private void btnEliminar_MouseLeave(object sender, EventArgs e)
        {
            btnEliminar.Image = Properties.Resources.btneliminar;
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int pedidoId;
            if (int.TryParse(inputID.Text, out pedidoId))
            {
                pedido.EliminarPedido(pedidoId);
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //

        //MODIFICAR

        private void btnModificar_Click(object sender, EventArgs e)
        {
            int pedidoId;
            if (int.TryParse(inputID.Text, out pedidoId))
            {
                string nuevoEstado = cbEstado.Text;
                if (!string.IsNullOrEmpty(nuevoEstado))
                {
                    pedido.CambiarEstado(pedidoId, nuevoEstado);
                    cbEstado.Text = nuevoEstado;
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa un nuevo estado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnModificar_MouseHover(object sender, EventArgs e)
        {
            btnModificar.Image = Properties.Resources.btnmodifiicarseleccionado;
        }

        private void btnModificar_MouseLeave(object sender, EventArgs e)
        {
            btnModificar.Image = Properties.Resources.btnmodificar;
        }

        //

        //PEDIDOS
        private void Pedidos_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titulo;
        }

        private bool isFirstClickID = true;

        private void inputID_Enter(object sender, EventArgs e)
        {
            if (isFirstClickID)
            {
                inputID.Clear();
                isFirstClickID = false;
            }
        }

        private void LimpiarCampos()
        {
            inputFecha.Text = "";
            cbEstado.Text = "";
            inputTotal.Text = "";
            cbProveedor.Text = "";
            inputID.Clear();
        }

        //
    }
}
