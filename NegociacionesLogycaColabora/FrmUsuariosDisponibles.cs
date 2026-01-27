using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NegociacionesLogycaColabora
{
    public partial class FrmUsuariosDisponibles : Form
    {
        TextBox usuario;
        public FrmUsuariosDisponibles(TextBox usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void ListarUsuarios()
        {
            Datos datos=new Datos();
            dgv_usuarios.AutoGenerateColumns = false;
            dgv_usuarios.Columns[0].DataPropertyName = "us_id";
            dgv_usuarios.Columns[1].DataPropertyName = "us_nombre";
            dgv_usuarios.DataSource = datos.ListarUsuariosDisponibles();
            dgv_usuarios.ClearSelection();
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUsuariosDisponibles_Load(object sender, EventArgs e)
        {
            try
            {
                ListarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_seleccionar_Click(object sender, EventArgs e)
        {
            if (dgv_usuarios.SelectedRows.Count==0)
            {
                MessageBox.Show("Seleccione un usuario de la lista", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dgv_usuarios.Focus();
                return;
            }

            try
            {
                usuario.Text = Convert.ToString(dgv_usuarios[1, dgv_usuarios.CurrentRow.Index].Value);
                usuario.Tag = Convert.ToString(dgv_usuarios[0, dgv_usuarios.CurrentRow.Index].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
