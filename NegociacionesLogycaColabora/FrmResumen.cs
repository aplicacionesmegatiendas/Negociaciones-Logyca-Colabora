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
	public partial class FrmResumen : Form
	{
		DataTable dt_resumen;
		public FrmResumen(DataTable dt_resumen)
		{
			InitializeComponent();
			this.dt_resumen = dt_resumen;
		}

		private void btn_cerrar_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FrmResumen_Load(object sender, EventArgs e)
		{
			try
			{
                foreach (DataRow row in dt_resumen.Rows)
                {
					dgv_resumen.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7]);
                }
            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
