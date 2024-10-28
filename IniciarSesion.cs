using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class IniciarSesion : Form
    {
        private ConexionSQL conexion = new ConexionSQL();
        public IniciarSesion()
        {
            InitializeComponent();

        }

        private bool ValidarUser(string username, string password)
        {
            bool valid = false;
            string query = "SELECT COUNT(1) FROM usuarios WHERE username = @username AND password = @password";

            using (var conn = conexion.AbrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al validar las credenciales: {ex.Message}");
                    return false;
                }
            }
        }

        //Variables Globales
        private bool isFirstClickUsername = true;
        private bool isFirstClickPassword = true;
        //

        //ELEMENTOS

        //INPUTUSERNAME
        private void inputUsername_TextChanged(object sender, EventArgs e)
        {

        }
        private void inputUsername_Enter(object sender, EventArgs e)
        {

            if (isFirstClickUsername)
            {
                inputUsername.Clear();
                isFirstClickUsername = false;
            }
        }

        //

        //FORM
        private void FormIniciarSesion_Load(object sender, EventArgs e)
        {
            // Establece el enfoque en el TextBox
            this.ActiveControl = tituloIniciarSesion;
            CenterElements();
        }

        private void FormIniciarSesion_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void FormIniciarSesion_Resize(object sender, EventArgs e)
        {
            CenterElements();
        }


        //

        //INPUTPASSWORD


        private void inputPassword_Enter(object sender, EventArgs e)
        {
            if (isFirstClickPassword)
            {
                inputPassword.Clear();
                isFirstClickPassword = false;
            }
        }

        //

        //BOTON INICIAR SESION

        private void btnIniciarSesion_MouseHover(object sender, EventArgs e)
        {
            btnIniciarSesion.Image = Properties.Resources.btnIniciarSesionSeleccionado;


        }

        private void btnIniciarSesion_MouseLeave(object sender, EventArgs e)
        {
            btnIniciarSesion.Image = Properties.Resources.botonIniciarSesion;
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            //IMPLEMENTAR AUTENTICACION DE USUARIO
            string username = inputUsername.Text;
            string password = inputPassword.Text;

            if (ValidarUser(username, password))
            {
                Menu nuevaVentana = new Menu();
                nuevaVentana.ShowDialog();
                this.Close();
            } else
            {
                MessageBox.Show("Username o password incorrectos. Intente otra vez.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
        }

        //
        
        //


        //FUNCION PARA CENTRAR ELEMENTOS

        private void CenterElements()
        {
            // Obtener el ancho del formulario
            int formWidth = this.ClientSize.Width;

            // Centrar el inputUsername
            inputUsername.Left = (formWidth - inputUsername.Width) / 2;

            // Centrar el tituloIniciarSesion
            tituloIniciarSesion.Left = (formWidth - tituloIniciarSesion.Width) / 2;

            // Centrar Linea1
            Linea1.Left = (formWidth - Linea1.Width) / 2;

            // Centrar el inputPassword
            inputPassword.Left = (formWidth - inputPassword.Width) / 2;

            // Centrar Linea2
            Linea2.Left = (formWidth - Linea2.Width) / 2;

            // Centrar el btnIniciarSesion
            btnIniciarSesion.Left = (formWidth - btnIniciarSesion.Width) / 2;
        }

        //

    }
}
