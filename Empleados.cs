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
        public Empleados()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Menu nuevaVentana = new Menu();
            nuevaVentana.ShowDialog();
            this.Hide();
        }

        private void Empleados_Load(object sender, EventArgs e)
        {

        }
    }
}
