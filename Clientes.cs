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
    public partial class Clientes : Form
    {
        private Menu menu;
        private Cliente clienteDB = new Cliente();


        private bool isFirstClickNombre = true;
        private bool isFirstClickNIT = true;
        private bool isFirstClickCel = true;


        public Clientes(Menu menu)
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
            if (int.TryParse(inputNIT.Text, out int nit))
            {
                Cliente cliente = clienteDB.BuscarCliente(nit);
                if (cliente != null)
                {
                    inputNombre.Text = cliente.Name;
                    inputCelular.Text = cliente.Celular;
                    MessageBox.Show("Cliente encontrado.");
                }
                else
                {
                    MessageBox.Show("Cliente no encontrado.");
                }
            }
            else
            {
                MessageBox.Show("Por favor ingrese un NIT válido.");
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
            if (int.TryParse(inputNIT.Text, out int nit) && !string.IsNullOrEmpty(inputNombre.Text) && !string.IsNullOrEmpty(inputCelular.Text))
            {
                clienteDB.AgregarCliente(nit, inputNombre.Text, inputCelular.Text);
            }
            else
            {
                MessageBox.Show("Por favor llene todos los campos.");
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
            if (int.TryParse(inputNIT.Text, out int nit))
            {
                clienteDB.EliminarCliente(nit);
            }
            else
            {
                MessageBox.Show("Por favor ingrese un NIT válido.");
            }
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

        //MODIFICAR

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(inputNIT.Text, out int nit) && !string.IsNullOrEmpty(inputNombre.Text) && !string.IsNullOrEmpty(inputCelular.Text))
            {
                clienteDB.ActualizarCliente(nit, inputNombre.Text, inputCelular.Text);
            }
            else
            {
                MessageBox.Show("Por favor llene todos los campos.");
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

        //CLIENTES

        private void Clientes_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titulo;
        }

        //

        //INPUT NOMBRE

        private void inputNombre_Enter(object sender, EventArgs e)
        {
            if (isFirstClickNombre)
            {
                inputNombre.Clear();
                isFirstClickNombre = false;
            }
        }

        //INPUT NIT

        private void inputNIT_Enter(object sender, EventArgs e)
        {
            if (isFirstClickNIT)
            {
                inputNIT.Clear();
                isFirstClickNIT = false;
            }
        }

        //INPUT CELULAR

        private void inputCelular_Enter(object sender, EventArgs e)
        {
            if (isFirstClickCel)
            {
                inputCelular.Clear();
                isFirstClickCel = false;
            }
        }

        //
    }
}
