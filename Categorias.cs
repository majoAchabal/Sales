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
    public partial class Categorias : Form
    {
        private Menu menu;

        private bool isFirstClickID = true;
        private bool isFirstClickNombre = true;
        private bool isFirstClickDescrip = true;
        private bool isFirstClickDesc = true;
        

        public Categorias(Menu menu)
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

            if (!int.TryParse(inputID.Text.Trim(), out int id))
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Categoria categoria = new Categoria();
            categoria = categoria.BuscarCategoriaPorID(id);

            if (categoria != null)
            {
                inputName.Text = categoria.Name;
                inputDescripcion.Text = categoria.Description;
                inputDescuento.Text = categoria.Descuento?.ToString();
            }
            else
            {
                MessageBox.Show("No se encontró ninguna categoría con ese ID.", "Resultado de búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            string name = inputName.Text.Trim();
            string description = inputDescripcion.Text.Trim();

            // Validación para campo Descuento como double
            if (!double.TryParse(inputDescuento.Text.Trim(), out double descuento))
            {
                MessageBox.Show("Por favor, ingrese un valor numérico válido para el descuento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación de campo Nombre y Descripcion como strings
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Por favor, ingrese un nombre.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Por favor, ingrese una descripción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Categoria categoria = new Categoria(name, description, descuento);
            categoria.AgregarCategoria();
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
            if (!int.TryParse(inputID.Text.Trim(), out int id))
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Categoria categoria = new Categoria();
            categoria.EliminarCategoria(id);
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
            if (!int.TryParse(inputID.Text.Trim(), out int id))
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar nombre como string
            string newName = inputName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Por favor, ingrese un Nombre válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar descripción como varchar
            string newDescription = inputDescripcion.Text.Trim();
            if (string.IsNullOrEmpty(newDescription) || newDescription.Length > 255)
            {
                MessageBox.Show("Por favor, ingrese una Descripción válida (hasta 255 caracteres).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar descuento como double
            if (!double.TryParse(inputDescuento.Text.Trim(), out double descuento))
            {
                MessageBox.Show("Por favor, ingrese un valor numérico válido para el descuento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ocultar Modificar y mostrar Aceptar
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

        //BOTON ACEPTAR

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(inputID.Text.Trim(), out int id))
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newName = inputName.Text.Trim();
            string newDescription = inputDescripcion.Text.Trim();

            // Validación para campo Descuento como double
            double? newDescuento = null;
            if (!string.IsNullOrEmpty(inputDescuento.Text.Trim()) && double.TryParse(inputDescuento.Text.Trim(), out double descuento))
            {
                newDescuento = descuento;
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un valor numérico válido para el descuento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Categoria categoria = new Categoria();
            categoria.ActualizarCategoria(id, newName, newDescription, newDescuento.GetValueOrDefault());

           

            // Restaurar la visibilidad: mostrar Modificar y ocultar Aceptar
            btnModificar.Visible = true;
            btnAceptar.Visible = false;
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

        //CATEGORIA

        private void Categorias_Load(object sender, EventArgs e)
        {
            btnAceptar.Hide();
            this.ActiveControl = titulo;
        }
        //INPUT NOMBRE
        private void inputID_Enter(object sender, EventArgs e)
        {
            if (isFirstClickID)
            {
                inputID.Clear();
                isFirstClickID = false;
            }
        }

        //INPUT DESCRIPCION

        private void inputDescripcion_Enter(object sender, EventArgs e)
        {
            if (isFirstClickDescrip)
            {
                inputDescripcion.Clear();
                isFirstClickDescrip = false;
            }
        }


        //INPUT DESCUENTO
        private void inputDescuento_Enter(object sender, EventArgs e)
        {
            if (isFirstClickDesc)
            {
                inputDescuento.Clear();
                isFirstClickDesc = false;
            }
        }

        //INPUT NOMBRE

        private void inputName_Enter(object sender, EventArgs e)
        {
            if (isFirstClickNombre)
            {
                inputName.Clear();
                isFirstClickNombre = false;
            }
        }

        
    }
}
