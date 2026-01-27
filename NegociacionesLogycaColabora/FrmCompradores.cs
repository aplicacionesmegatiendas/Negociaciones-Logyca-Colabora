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
    public partial class FrmCompradores : Form
    {
        int id_comprador = -1;

        public FrmCompradores()
        {
            InitializeComponent();
        }

        private void ListarCompradores()
        {
            Datos datos = new Datos();
            dgv_compradores.AutoGenerateColumns = false;
            dgv_compradores.Columns[0].DataPropertyName = "co_id";
            dgv_compradores.Columns[1].DataPropertyName = "co_gln";
            dgv_compradores.Columns[2].DataPropertyName = "co_nombre";
            dgv_compradores.Columns[3].DataPropertyName = "co_email";
            dgv_compradores.Columns[4].DataPropertyName = "us_nombre";
            dgv_compradores.DataSource = datos.ListarCompradores();
        }

        private void FrmCompradores_Load(object sender, EventArgs e)
        {
            //dgv_compradores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            //this.dgv_compradores.ColumnHeadersHeight = 20;
            try
            {
                ListarCompradores();

                dgv_compradores.ClearSelection();
                txt_gln.Text = "";
                txt_nombre.Text = "";
                txt_email.Text = "";
                txt_usuario.Text = "";
                id_comprador = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_usuarios_Click(object sender, EventArgs e)
        {
            new FrmUsuariosDisponibles(txt_usuario).ShowDialog(this);
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (txt_gln.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el Gln del comprador", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_gln.Focus();
                return;
            }

            if (txt_nombre.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el nombre del comprador", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_nombre.Focus();
                return;
            }

            if (txt_email.Text.Equals(""))
            {
                MessageBox.Show("Escriba el email", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_usuarios.Focus();
                return;
            }


            if (txt_usuario.Text.Equals(""))
            {
                MessageBox.Show("Seleccione el usuario que se le asignara al comprador", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_usuarios.Focus();
                return;
            }

            try
            {
                int res = -1;
                Datos datos = new Datos();
                int us=-1;
                if (txt_usuario.Tag!=null)
                {
                    us=Convert.ToInt32(txt_usuario.Tag);
                    res = datos.GuardarComprador(txt_gln.Text.Trim(), txt_nombre.Text.Trim(), txt_email.Text.Trim(), us, id_comprador);
                }
                else
                {
                    res = datos.GuardarComprador(txt_gln.Text.Trim(), txt_nombre.Text.Trim(), txt_email.Text.Trim(),us , id_comprador);
                }
                
               if (res<=0)
               {
                   MessageBox.Show("El gln de proveedor ya existe", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   txt_gln.Focus();
                   txt_gln.SelectAll();
               }
               else
               {
                   ListarCompradores();
                   
                   Conectores.CrearEstructuraDirectorios(txt_gln.Text.Trim());
                   
                   txt_gln.Text = "";
                   txt_nombre.Text = "";
                   txt_email.Text = "";
                   txt_usuario.Text = "";
                   txt_usuario.Tag = null;

                   txt_gln.Focus();
               }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void dgv_compradores_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_compradores.RowCount>0)
            {
                txt_gln.Text = Convert.ToString(dgv_compradores[1, dgv_compradores.CurrentRow.Index].Value);
                txt_nombre.Text = Convert.ToString(dgv_compradores[2, dgv_compradores.CurrentRow.Index].Value);
                txt_email.Text = Convert.ToString(dgv_compradores[3, dgv_compradores.CurrentRow.Index].Value);
                txt_usuario.Text = Convert.ToString(dgv_compradores[4, dgv_compradores.CurrentRow.Index].Value);

                id_comprador = Convert.ToInt32(dgv_compradores[0, dgv_compradores.CurrentRow.Index].Value);
            }
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            if (id_comprador==-1)
            {
                MessageBox.Show("Seleccione el comprador que desea eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (MessageBox.Show("¿Confirma elimiar este comprador?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                {
                    Datos datos = new Datos();
                    datos.EliminarComprador(id_comprador);

                    ListarCompradores();

                    dgv_compradores.ClearSelection();

                    txt_gln.Text = "";
                    txt_nombre.Text = "";
                    txt_email.Text = "";
                    txt_usuario.Text = "";
                    txt_usuario.Tag = null;

                    txt_gln.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
