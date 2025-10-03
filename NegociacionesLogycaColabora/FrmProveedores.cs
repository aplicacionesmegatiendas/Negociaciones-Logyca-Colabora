using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NegociacionesLogycaColabora
{
    public partial class FrmProveedores : Form
    {
        bool invalid = false;

        public FrmProveedores()
        {
            InitializeComponent();
        }

        private void Limpiar()
        {
            txt_gln.Text = "";
            txt_nit.Text = "";
            txt_razon_soc.Text = "";
            txt_email_notif.Text = "";
        }

        private bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        private void ListarProveedores()
        {
            Datos datos = new Datos();
            dgv_proveedores.AutoGenerateColumns = false;
            dgv_proveedores.Columns[0].DataPropertyName = "pr_gln";
            dgv_proveedores.Columns[1].DataPropertyName = "pr_nit";
            dgv_proveedores.Columns[2].DataPropertyName = "pr_razon_soc";
            dgv_proveedores.Columns[3].DataPropertyName = "pr_email_notif";
            dgv_proveedores.Columns[4].DataPropertyName = "pr_id";
            dgv_proveedores.DataSource = datos.ListarProveedores();
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (txt_nit.Text.Trim().Equals("") || txt_razon_soc.Text.Trim().Equals(""))
            {
                MessageBox.Show("Falta el Nit y la Razón Social", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txt_email_notif.Text.Trim().Equals(""))
            {
                MessageBox.Show("Falta el Email de notificación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_email_notif.Focus();
                return;
            }
            if (IsValidEmail(txt_email_notif.Text.Trim()) == false)
            {
                MessageBox.Show("Email de notificación no valido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_email_notif.Focus();
                return;
            }

            try
            {
                Datos guardar = new Datos();
                guardar.GuardarProveedor(txt_gln.Text.Trim(), txt_nit.Text.Trim(), txt_razon_soc.Text.Trim(), txt_email_notif.Text.Trim());
                ListarProveedores();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_buscar_info_Click(object sender, EventArgs e)
        {
            if (txt_gln.Text.Trim().Equals(""))
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string gln_proveedor = txt_gln.Text.Trim();
                Datos datos = new Datos();
                string[] info = null;
                info = datos.ObtenerNitProveedor(gln_proveedor);
                if (info != null)
                {
                    txt_nit.Text = info[0];
                    txt_razon_soc.Text = info[3];
                }
                else
                {
                    MessageBox.Show("No hay información para el proveedor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            try
            {
                ListarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_proveedores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                if (dgv_proveedores.Rows.Count > 0 && dgv_proveedores.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("¿Confirma eliminar este registro?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Datos datos = new Datos();
                            datos.EliminarProveedor(Convert.ToInt32(dgv_proveedores[4, dgv_proveedores.CurrentRow.Index].Value));
                            ListarProveedores();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void dgv_proveedores_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_proveedores.Rows.Count > 0)
            {
                txt_gln.Text = dgv_proveedores[0, dgv_proveedores.CurrentRow.Index].Value.ToString();
                txt_email_notif.Text= dgv_proveedores[3, dgv_proveedores.CurrentRow.Index].Value.ToString();
                txt_email_notif.Focus();
                txt_email_notif.SelectAll();
                btn_buscar_info.PerformClick();
            }
        }
    }
}
