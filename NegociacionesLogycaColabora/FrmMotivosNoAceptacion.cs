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
    public partial class FrmMotivosNoAceptacion : Form
    {
        public FrmMotivosNoAceptacion()
        {
            InitializeComponent();
        }

        private void ListarMotivos()
        {
            Datos datos = new Datos();
            dgv_motivos.AutoGenerateColumns = false;
            dgv_motivos.Columns[0].DataPropertyName = "mo_descripcion";
            dgv_motivos.Columns[1].DataPropertyName = "mo_id";
            dgv_motivos.DataSource = datos.ListarMotivos();
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (txt_motivo.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el motivo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_motivo.Focus();
                return;
            }
            try
            {
                Datos guardar = new Datos();
                guardar.GuardarMotivo(txt_motivo.Text.Trim());
                ListarMotivos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmMotivosNoAceptacion_Load(object sender, EventArgs e)
        {
            try
            {
                ListarMotivos();
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
