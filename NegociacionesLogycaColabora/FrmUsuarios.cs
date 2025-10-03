using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace NegociacionesLogycaColabora
{
    public partial class FrmUsuarios : Form
    {
        public FrmUsuarios()
        {
            InitializeComponent();
        }

        private void ListarUsuarios()
        {
            Usuario usuario = new Usuario();
            dgv_usuarios.AutoGenerateColumns = false;
            dgv_usuarios.Columns[0].DataPropertyName = "us_nombre";
            dgv_usuarios.Columns[1].DataPropertyName = "us_contraseña";
            dgv_usuarios.Columns[2].DataPropertyName = "us_tipo";
            dgv_usuarios.DataSource = usuario.ListarUsuarios();
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
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

        private void dgv_usuarios_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void dgv_usuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.StartupPath + "\\" + Application.ProductName + ".exe");
                AppSettingsSection section = config.AppSettings;

                char pwdchar = Convert.ToChar(section.Settings["pwdchar"].Value.ToString());

                if (e.ColumnIndex == 1 && e.Value != null)
                {
                    e.Value = new String(pwdchar, e.Value.ToString().Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (dgv_usuarios.SelectedRows.Count==0)
            {
                MessageBox.Show("Seleccione un usuario de la lista.","Aviso",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dgv_usuarios.Focus();
                return;
            }

            if (txt_contraseña.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Escriba la contraseña.","Aviso",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_contraseña.Focus();
                return;
            }

            Usuario usuario = new Usuario();
            try
            {
                   usuario.ActualizarContraseñaUsuario(txt_nombre.Text.Trim(),txt_contraseña.Tag.ToString().Trim(),Seguridad.Encriptar(txt_contraseña.Text.Trim()));
                   ListarUsuarios();
                   MessageBox.Show("Contraseña actualizada correctamente","Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_usuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_usuarios.RowCount > 0)
            {
                txt_nombre.Text = ""; txt_contraseña.Text = ""; txt_contraseña.Tag = null;
                txt_nombre.Text = dgv_usuarios.CurrentRow.Cells[0].Value.ToString().Trim();
                txt_contraseña.Text = Seguridad.Desencriptar(dgv_usuarios.CurrentRow.Cells[1].Value.ToString().Trim());
                txt_contraseña.Tag = dgv_usuarios.CurrentRow.Cells[1].Value.ToString().Trim();
            }
        }
    }
}
