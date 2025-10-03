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
    public partial class FrmCrearUsuarios : Form
    {
        public FrmCrearUsuarios()
        {
            InitializeComponent();
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (txt_usuario.Text.Trim().Equals("") || txt_contra.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el nombre de usuario y la contraseña", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cmb_tipo.SelectedIndex==-1)
            {
                MessageBox.Show("Seleccione el tipo de usuario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmb_tipo.Focus();
                return;
            }

            try
            {
                Usuario usuario = new Usuario();
                usuario.CrearUsuario(txt_usuario.Text.Trim(), Seguridad.Encriptar(txt_contra.Text.Trim()),cmb_tipo.SelectedIndex+2);
                MessageBox.Show("Usuario creado correctamente", "Aviso",MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_usuario.Text = "";
                txt_contra.Text = "";
                txt_usuario.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_usuario.Focus();
                txt_usuario.SelectAll();
            }
        }

        private void btn_listaUsusarios_Click(object sender, EventArgs e)
        {
            new FrmUsuarios().ShowDialog(this);
        }
    }
}
