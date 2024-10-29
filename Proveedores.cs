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
    
    public partial class Proveedores : Form
    {
        private Proveedor proveedor = new Proveedor();

        private Menu menu;

        private bool isFirstClickNombre = true;
        private bool isFirstClickDirec = true;
        private bool isFirstClickTel = true;


        public Proveedores(Menu menu)
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
            if (int.TryParse(inputID.Text, out int id))
            {
                Proveedor resultado = proveedor.BuscarProveedor(id);
                if (resultado != null)
                {
                    inputNombre.Text = resultado.Name;
                    inputDireccion.Text = resultado.Direction;
                    inputTelefono.Text = resultado.Telefono;

                    // Habilitar y mostrar el botón Modificar
                    btnModificar.Enabled = true;
                    btnModificar.Visible = true;
                    btnAceptar.Visible = false;
                }
                else
                {
                    MessageBox.Show("Proveedor no encontrado.", "Error de búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnModificar.Enabled = false; // Deshabilitar el botón Modificar si no se encuentra el proveedor
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "ID inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnModificar.Enabled = false;
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
            // Verificar que los campos hayan sido cambiados desde su valor por defecto
            if (!isFirstClickNombre && !isFirstClickDirec && !isFirstClickTel)
            {
                string nombre = inputNombre.Text;
                string direccion = inputDireccion.Text;
                string telefono = inputTelefono.Text;

                // Validación de tipo de datos
                if (!int.TryParse(telefono, out _))
                {
                    MessageBox.Show("El campo Teléfono debe ser un número.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                proveedor.AddProveedor(nombre, direccion, telefono);
            }
            else
            {
                MessageBox.Show("Por favor, rellene todos los campos.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (int.TryParse(inputID.Text, out int id))
            {
                var confirmResult = MessageBox.Show("¿Está seguro de que desea eliminar este proveedor?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    proveedor.ElimiarProveedor(id);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "ID inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // Oculta el botón Modificar y muestra el botón Aceptar
            btnModificar.Visible = false;
            btnAceptar.Visible = true;
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
        
        //PROVEEDORES
        private void Proveedores_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titulo;
            btnAceptar.Visible = false;  // Asegurarse de que Aceptar esté oculto al inicio
            btnModificar.Enabled = false;  // Modificar deshabilitado inicialmente
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

        //INPUT DIRECCION

        private void inputDireccion_Enter(object sender, EventArgs e)
        {
            if (isFirstClickDirec)
            {
                inputDireccion.Clear();
                isFirstClickDirec = false;
            }
        }

        //INPUT TELEFONO

        private void inputTelefono_Enter(object sender, EventArgs e)
        {
            if (isFirstClickTel)
            {
                inputTelefono.Clear();
                isFirstClickTel = false;
            }
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(inputID.Text, out int id))
            {
                string nombre = inputNombre.Text;
                string direccion = inputDireccion.Text;
                string telefono = inputTelefono.Text;

                // Validación de tipos de datos
                if (!int.TryParse(telefono, out _))
                {
                    MessageBox.Show("El campo Teléfono debe ser un número.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(direccion) && !string.IsNullOrEmpty(telefono))
                {
                    proveedor.ActualizarProveedor(id, nombre, direccion, telefono);
                    btnModificar.Visible = true;
                    btnAceptar.Visible = false;
                }
                else
                {
                    MessageBox.Show("Por favor, complete todos los campos para actualizar.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "ID inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAceptar_MouseHover(object sender, EventArgs e)
        {
            btnAceptar.Image = Properties.Resources.btnaceptarseleccionado;
        }

        private void btnAceptar_MouseLeave(object sender, EventArgs e)
        {
            btnAceptar.Image = Properties.Resources.btnaceptar;
        }

        //
    }
}
