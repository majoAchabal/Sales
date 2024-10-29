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
    public partial class Empleados : Form
    {
        private Menu menu;


        private bool isFirstClickNombre = true;
        private bool isFirstClickApellido = true;
        private bool isFirstClickCel = true;
        private bool isFirstClickUser = true;
        


        public Empleados(Menu menu)
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
            int id;
            if (int.TryParse(inputId.Text, out id))
            {
                Empleado empleado = new Empleado();
                Empleado resultado = empleado.BuscarEmpleado(id);

                if (resultado != null)
                {
                    inputNombre.Text = resultado.Name;
                    inputApellido.Text = resultado.Surname;
                    cbCargo.Text = resultado.Cargo;
                    inputCelular.Text = resultado.Celular;
                    inputUsuario.Text = resultado.Username;
                }
                else
                {
                    MessageBox.Show("Empleado no encontrado.");
                }
            }
            else
            {
                MessageBox.Show("Por favor ingrese un ID válido.");
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
            Empleado empleado = new Empleado();
            empleado.AddEmpleado(
                inputNombre.Text,
                inputApellido.Text,
                cbCargo.Text,
                inputCelular.Text,
                inputUsuario.Text
            );
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
            int id;
            if (int.TryParse(inputId.Text, out id))
            {
                Empleado empleado = new Empleado();
                empleado.EliminarEmpleado(id);
            }
            else
            {
                MessageBox.Show("Por favor ingrese un ID válido.");
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
            int id;
            if (int.TryParse(inputId.Text, out id))
            {
                Empleado empleado = new Empleado();
                empleado.UpdateEmpleado(
                    id,
                    inputNombre.Text,
                    inputApellido.Text,
                    cbCargo.Text,
                    inputCelular.Text,
                    inputUsuario.Text
                );
            }
            else
            {
                MessageBox.Show("Por favor ingrese un ID válido.");
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

        //EMPLEADOS
        private void Empleados_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titulo;
        }

        //INPUT NOMBRE

        private void inputNombre_Enter(object sender, EventArgs e)
        {
            if (isFirstClickNombre)
            {
                inputNombre.Clear();
                isFirstClickNombre = false;
            }
        }

        //INPUT APELLIDO

        private void inputApellido_Enter(object sender, EventArgs e)
        {
            if (isFirstClickApellido)
            {
                inputApellido.Clear();
                isFirstClickApellido = false;
            }
        }

        //INPUT CEL

        private void inputCelular_Enter(object sender, EventArgs e)
        {
            if (isFirstClickCel)
            {
                inputCelular.Clear();
                isFirstClickCel = false;
            }
        }

        //INPUT USUARIO

        private void inputUsuario_Enter(object sender, EventArgs e)
        {
            if (isFirstClickUser)
            {
                inputUsuario.Clear();
                isFirstClickUser = false;
            }
        }

        //
    }
}
