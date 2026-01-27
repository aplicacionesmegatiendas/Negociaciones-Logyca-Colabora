using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NegociacionesLogycaColabora
{
    public partial class FrmLogin : Form
    {

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            Datos.salir = true;
            Application.Exit();
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (txt_usuario.Text.Trim().Equals("") || txt_contra.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el nombre de usuario y la contraseña", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                Usuario us = new Usuario();
                if (chkAdmin.Checked == true)
                {
                    int res = -1;
                    res = us.IniciarSesionAdmin(txt_usuario.Text.Trim(), Seguridad.Encriptar(txt_contra.Text.Trim()));
                    if (res == 1)
                    {
                        this.Hide();
                        FrmAdmin _FrmAdmin = new FrmAdmin(0);
                        _FrmAdmin.ShowDialog(this);

                    }
                    else
                    {
                        MessageBox.Show("Nombre de usuario o contraseña incorrecto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    string[] glnNombre = null;//AQUI SE GUARDA EL GLN DE COMPRADOR Y EL NOMBRE
                    glnNombre = us.IniciarSesion(txt_usuario.Text.Trim(), Seguridad.Encriptar(txt_contra.Text.Trim()));
                    if (glnNombre == null)
                    {
                        MessageBox.Show("Nombre de usuario y/o contraseña incorrecta o usuario no asociado a un comprador.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Datos.GlnComprador = glnNombre[0];
                        Datos.Comprador = glnNombre[1];
                        Datos.Usuario = txt_usuario.Text;
                        Datos.TipoUsuario = glnNombre[2];
                        //this.Hide();
                        //FrmArchivosPendientes _FrmArchivosPendientes = new FrmArchivosPendientes();
                        //_FrmArchivosPendientes.Show();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkAdmin_CheckedChanged(object sender, EventArgs e)
        {
            txt_usuario.Focus();
            txt_usuario.SelectAll();
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.F2)
            {
                txt_contra.UseSystemPasswordChar = false;
            }
        }

        private void FrmLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                txt_contra.UseSystemPasswordChar = true;
            }
        }
    }
}
