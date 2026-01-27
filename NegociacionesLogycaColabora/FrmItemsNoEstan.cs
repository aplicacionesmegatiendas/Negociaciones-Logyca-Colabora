using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NegociacionesLogycaColabora
{
    public partial class FrmItemsNoEstan : Form
    {
        string razon_soc;
        string nit;
        string nomb_doc;
        string nro_doc;
        bool estan;
        public FrmItemsNoEstan(string razon_soc, string nit, string nomb_doc, string nro_doc, bool estan)
        {
            InitializeComponent();

            this.razon_soc = razon_soc;
            this.nit = nit;
            this.nomb_doc = nomb_doc;
            this.nro_doc = nro_doc;
            this.estan = estan;
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }

        private void Notificar()
        {
            Datos notificar = new Datos();

            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.StartupPath + "\\" + Application.ProductName + ".exe");
            AppSettingsSection section = config.AppSettings;
            string imagen = section.Settings["imagen"].Value;

            string html = "<html>";
            html += "<head>";
            html += "<title>Pricat</title>";
            html += "<style>" +
                    "table, th, td {" +
                        "border: 1px dashed gray;" +
                        "border-collapse: collapse;" +
                    "}" +
                    "th, td {" +
                        "padding: 5px;" +
                        "text-align: left;" +
                    "}" +
                    "table {" +
                        "width: 100%;" +
                    "}" +
                    "</style>";
            html += "</head>";
            html += "<body>";

            html += "<img src=" + imagen + " width='100%' height='100px' />";
            html += "<p>Cartagena " + MonthName(DateTime.Now.Date.Month) + " " + DateTime.Now.Date.Day.ToString() + " de " + DateTime.Now.Date.Year.ToString() + "</p>";
            html += "<h2>Señores:</h2>";
            html += "<h3>" + razon_soc + "</h3>";
            html += "<h4>" + nit + "</h4>";
            html += "<p>Cordial saludo</p>";
            if (estan.Equals(false))
            {
                html += "<p>Se les informa que los siguientes items del documento " + nomb_doc + " número: " + nro_doc + " no fueron aceptados porque no estan inscritos en el catalogo de productos de Megatiendas.</p>";
            }
            else
            {
                html += "<p>Se les informa que los siguientes items del documento " + nomb_doc + " número: " + nro_doc + " no fueron aceptados porque ya estan inscritos en el catalogo de productos de Megatiendas.</p>";
            }

            String tabla = "<table>";
            tabla += "<tr>";
            tabla += "<th>Gtin</th>";
            tabla += "</tr>";

            for (int i = 0; i < dgv_items.Rows.Count; i++)
            {
                tabla += "<tr>";
                tabla += "<td>" + dgv_items.Rows[i].Cells[0].Value.ToString().Trim() + "</td>";
                tabla += "</tr>";
            }
            tabla += "</table>";
            html += tabla;
            html += "</body>";
            html += "</html>";

            string email_notif = notificar.ObtenerEmailProveedor(Datos.GlnProveedor);
            if (email_notif.Trim().Equals(""))
            {
                MessageBox.Show("No hay email para notificar al proveedor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            notificar.EnviarCorreoProveedor(email_notif, html, nomb_doc);
            MessageBox.Show("Se le ha enviado la notificación al proveedor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FrmItemsNoEstan_Load(object sender, EventArgs e)
        {
            if (estan.Equals(false))
            {
                lbl_nota.Text = "Los Gtin que aparecen en el listado no tienen información en la base de datos.";
            }
            else
            {
                lbl_nota.Text = "Los Gtin que aparecen en el listado ya tienen información en la base de datos.";
            }
            lbl_nro.Text = dgv_items.Rows.Count.ToString();
        }

        private void btn_notificar_Click(object sender, EventArgs e)
        {
            try
            {
                Notificar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
