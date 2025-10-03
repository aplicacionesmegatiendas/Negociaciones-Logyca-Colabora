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
    public partial class FrmConsultaProcesados : Form
    {
        public FrmConsultaProcesados()
        {
            InitializeComponent();
        }

        private void ListarProveedores()
        {
            cmb_proveedores.DisplayMember = "do_razon_social";
            cmb_proveedores.ValueMember = "do_nit";
            Datos dato = new Datos();

            DataTable dt = dato.ListarProvedoresDocumentos();
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.InsertAt(dr, 0);

            cmb_proveedores.DataSource = dt;
            cmb_proveedores.SelectedIndex = 0;
        }

        private void BuscarProcesados(string desde, string hasta, string proveedor)
        {
            dgv_items.AutoGenerateColumns = false;
            dgv_items.Columns[2].DataPropertyName = "do_numero_doc";
            dgv_items.Columns[3].DataPropertyName = "accion";
            dgv_items.Columns[4].DataPropertyName = "do_fecha";
            dgv_items.Columns[5].DataPropertyName = "do_razon_social";
            dgv_items.Columns[6].DataPropertyName = "gtin";
            dgv_items.Columns[7].DataPropertyName = "descripcion";
            dgv_items.Columns[0].DataPropertyName = "aceptado";
            dgv_items.Columns[1].DataPropertyName = "motivo";
            dgv_items.Columns[8].DataPropertyName = "comprador";
            Datos datos = new Datos();
            dgv_items.DataSource = datos.BuscarProcesados(desde, hasta, proveedor);
            foreach (DataGridViewColumn col in dgv_items.Columns)
            {
                if (col.Index != 5)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }

        private void FrmConsultaProcesados_Load(object sender, EventArgs e)
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

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                string proveedor = "";
                if (cmb_proveedores.SelectedIndex >= 0)
                {
                    proveedor = cmb_proveedores.SelectedValue.ToString();
                }
                BuscarProcesados(dtp_desde.Text, dtp_hasta.Text, proveedor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {

        }

        private void btn_exportar_Click(object sender, EventArgs e)
        {
            if (dgv_items.RowCount==0)
            {
                MessageBox.Show("No hay información para exportar.","Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    html += "<h3>" + dgv_items[3, 0].Value + "</h3>";
                    String tabla = "<table id='detalle'>";
                    tabla += "<tr>";
                    tabla += "<th>Aceptado</th>";
                    tabla += "<th>Motivo</th>";
                    tabla += "<th>Nro. Doc.</th>";
                    tabla += "<th>Fecha</th>";
                    tabla += "<th>Proveedor</th>";
                    tabla += "<th>Referencia</th>";
                    tabla += "<th>Descripción</th>";
                    tabla += "<th>Comprados</th>";
                    tabla += "</tr>";

                    for (int i = 0; i < dgv_items.RowCount; i++)
                    {
                        tabla += "<tr>";
                        if (Convert.ToBoolean(dgv_items[0, i].Value) == true)
                        {
                            tabla += "<td><center>✔</center></td>";
                        }
                        else
                        {
                            tabla += "<td></td>";
                        }
                        
                        tabla += "<td>" + dgv_items[1, i].Value + "</td>";
                        tabla += "<td>" + dgv_items[2, i].Value + "</td>";
                        tabla += "<td>" + dgv_items[4, i].Value + "</td>";
                        tabla += "<td>" + dgv_items[5, i].Value + "</td>";
                        tabla += "<td>" + dgv_items[6, i].Value + "</td>";
                        tabla += "<td>" + dgv_items[7, i].Value + "</td>";
                        tabla += "<td>" + dgv_items[8, i].Value + "</td>";
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
