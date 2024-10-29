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
    public partial class AdminUsuarios : Form
    {
        public AdminUsuarios()
        {
            InitializeComponent();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (inputUsuario.Text == "" || inputPassword.Text == "" || cbCargo.SelectedItem == null)
            {
                MessageBox.Show("Ingrese los datos completos del usuario a registrar");
            }
            else
            {
                Usuario usuario = new Usuario();
                usuario.AddUser(inputUsuario.Text, inputPassword.Text, cbCargo.SelectedItem.ToString());
                MessageBox.Show("Usuario con usename: " + inputUsuario.Text + " creado correctamente");
                inputUsuario.Text = "";
                inputPassword.Text = "";
                cbCargo.SelectedItem = null;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (inputUsuario.Text == "")
            {
                MessageBox.Show("Ingrese el usuername del usuario que quiere buscar");
                inputUsuario.Text = "";
                inputPassword.Text = "";
                cbCargo.SelectedItem = null;
            }
            else
            {
                Usuario user = new Usuario();
                user = user.BuscarUsuario(inputUsuario.Text);
                if (user == null)
                {
                    MessageBox.Show("Usuario no encontrado");
                }
                else
                {
                    inputPassword.Text = user.Username;
                    cbCargo.Text = user.Rol;
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (inputUsuario.Text == "")
            {
                MessageBox.Show("Ingrese el usuername del usuario que quiere eliminar");
                inputUsuario.Text = "";
                inputPassword.Text = "";
                cbCargo.SelectedItem = null;
            }
            else
            {
                Usuario user = new Usuario();
                user = user.BuscarUsuario(inputUsuario.Text);
                if (user == null)
                {
                    MessageBox.Show("Usuario no encontrado");
                    inputUsuario.Text = "";
                    inputPassword.Text = "";
                    cbCargo.SelectedItem = null;
                }
                else
                {
                    DialogResult answer = MessageBox.Show("¿Estás seguro que quiere eliminar el usuario " + user.Username + "?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (answer == DialogResult.Yes)
                    {
                        user.EliminarUsuario(inputUsuario.Text);
                        Console.WriteLine("Usuario eliminado.");
                    }
                    else
                    {
                        Console.WriteLine("Operación cancelada.");
                    }

                    inputUsuario.Text = "";
                    inputPassword.Text = "";
                    cbCargo.SelectedItem = null;

                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu menu = new Menu();
            menu.Show();
        }
    }
}
