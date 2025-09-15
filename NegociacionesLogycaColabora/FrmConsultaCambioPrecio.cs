using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NegociacionesLogycaColabora
{
    public partial class FrmConsultaCambioPrecio : Form
    {
        public FrmConsultaCambioPrecio()
        {
            InitializeComponent();
        }


        private void FrmConsultaCambioPrecio_Load(object sender, EventArgs e)
        {
            try
            {
                Datos datos = new Datos();

                cmb_proveedores.DisplayMember = "do_razon_social";
                cmb_proveedores.ValueMember = "do_nit";
                DataTable dt = datos.ListarProvedoresDocumentos();
                DataRow dr = dt.NewRow();
                dr[0] = "";
                dr[1] = "";
                dt.Rows.InsertAt(dr, 0);
                cmb_proveedores.DataSource = dt;
                cmb_proveedores.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                Datos datos = new Datos();
                string proveedor = "";
                if (cmb_proveedores.SelectedIndex >= 0)
                {
                    proveedor = cmb_proveedores.SelectedValue.ToString();
                }
                dgv_items.DataSource = datos.BuscarProcesadosCambioPrecio(dtp_desde.Text, dtp_hasta.Text, proveedor);

                foreach (DataGridViewColumn col in dgv_items.Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_exportar_Click(object sender, EventArgs e)
        {
            if (dgv_items.RowCount == 0)
            {
                MessageBox.Show("No hay información para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (_saveFileDialog.ShowDialog().Equals(DialogResult.OK))
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    string html = "<html>";
                    html += "<head>";
                    html += "<meta charset='utf-8'>";
                    html += "<title>Pricat</title>";
                    html += "<style>" +
                            "body {" +
                                "font-family: 'Tahoma', serif;" +
                            "}" +
                            "table, th, td {" +
                                "border: 1px solid gray;" +
                                "border-collapse: collapse;" +
                            "}" +
                            "th, td {" +
                                "padding: 5px;" +
                                "text-align: left;" +
                            "}" +
                            "#detalle {" +
                                "width: 100%;" +
                            "}" +
                            "#totales {" +
                                "float: right;" +
                            "}" +
                            "</style>";
                    html += "</head>";
                    html += "<body>";
                    html += "<h3>Cambio de Precio</h3>";
                    String tabla = "<table id='detalle'>";

                    tabla += "<tr>";
                    for (int i = 0; i < dgv_items.ColumnCount; i++)
                    {
                        tabla += "<th>" + dgv_items.Columns[i].HeaderText.Trim() + "</th>";
                    }
                    tabla += "</tr>";

                    for (int i = 0; i < dgv_items.RowCount; i++)
                    {
                        tabla += "<tr>";
                        for (int j = 0; j < dgv_items.ColumnCount; j++)
                        {
                            if (j==0)
                            {
                                if (Convert.ToBoolean(dgv_items[0, i].Value)==true)
                                {
                                    tabla += "<td><center>✔</center></td>";
                                }
                                else
                                {
                                    tabla += "<td></td>";
                                }
                            }
                            else
                            {
                                tabla += "<td>" + dgv_items[j, i].Value + "</td>";
                            }
                        }
                        tabla += "</tr>";
                    }
                    tabla += "</table>";
                    html += tabla;

                    html += "</body>";
                    html += "</html>";

                    StreamWriter w;
                    FileStream fs = new FileStream(_saveFileDialog.FileName, FileMode.Create,
                                              FileAccess.ReadWrite);
                    w = new StreamWriter(fs);
                    w.Write(html);
                    w.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.Cursor = Cursors.Default;
        }
    }
}
