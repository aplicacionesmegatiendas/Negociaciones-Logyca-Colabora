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
    public partial class FrmActividades : Form
    {
        public FrmActividades()
        {
            InitializeComponent();
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListarActividades(string fecha_in, string fecha_fin)
        {
            dgv_datos.AutoGenerateColumns = false;
            dgv_datos.Columns[0].DataPropertyName = "lg_usuario";
            dgv_datos.Columns[1].DataPropertyName = "co_gln";
            dgv_datos.Columns[2].DataPropertyName = "co_nombre";
            dgv_datos.Columns[3].DataPropertyName = "lg_fecha";
            dgv_datos.Columns[4].DataPropertyName = "lg_accion";
            dgv_datos.Columns[5].DataPropertyName = "lg_archivo";
            Datos datos = new Datos();
            dgv_datos.DataSource = datos.ListarActividades(fecha_in, fecha_fin);
        }

        private void FrmActividades_Load(object sender, EventArgs e)
        {
            dgv_datos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgv_datos.ColumnHeadersHeight = 30;
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                ListarActividades(dt_fi.Value.ToString("yyyyMMdd") + " 00:00:00", dt_ff.Value.ToString("yyyyMMdd") + " 23:59:59");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
