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
    public partial class FrmItemEspejo : Form
    {
        public FrmItemEspejo()
        {
            InitializeComponent();
        }

        private void BuscarItems(string param)
        {
            ItemEspejo datos = new ItemEspejo();
            dgv_items.AutoGenerateColumns = false;
            dgv_items.Columns[0].DataPropertyName = "f120_id";
            dgv_items.Columns[1].DataPropertyName = "f120_referencia";
            dgv_items.Columns[2].DataPropertyName = "f120_descripcion";
            dgv_items.Columns[3].DataPropertyName = "f120_descripcion_corta";
            dgv_items.Columns[4].DataPropertyName = "f120_ind_compra";
            dgv_items.Columns[5].DataPropertyName = "f120_ind_venta";
            dgv_items.Columns[6].DataPropertyName = "f120_ind_manufactura";
            dgv_items.DataSource = datos.ListarItemsEspejo(txt_descripcion_barra.Text.Trim(), param);
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (rdb_descripcion.Checked.Equals(true))
                {
                    BuscarItems("desc");
                }
                else if (rdb_barra.Checked.Equals(true))
                {
                    BuscarItems("barra");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor = Cursors.Default;
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (dgv_items.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione el item.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                if (chk_imp_inv.Checked)
                    ItemEspejo.LlenarGrupoImpTipoInv(Convert.ToString(dgv_items[0, dgv_items.CurrentRow.Index].Value));
                if (chk_criterios_clasificacion.Checked)
                    ItemEspejo.LlenarCriteriosClasificacion(Convert.ToString(dgv_items[0, dgv_items.CurrentRow.Index].Value));
                if (chk_parametros_planeacion.Checked)
                    ItemEspejo.LlenarParametrosPlaneacion(Convert.ToString(dgv_items[0, dgv_items.CurrentRow.Index].Value));
                if (chk_listas_precio.Checked)
                    ItemEspejo.LlenarListaPrecios(Convert.ToString(dgv_items[0, dgv_items.CurrentRow.Index].Value));
                if (chk_portafolio_barra.Checked)
                {
                    ItemEspejo.LlenarPortafolios(Convert.ToString(dgv_items[0, dgv_items.CurrentRow.Index].Value));
                    ItemEspejo.LlenarBarra(Convert.ToString(dgv_items[0, dgv_items.CurrentRow.Index].Value));
                }
                ItemEspejo.DescripcionLarga = Convert.ToString(dgv_items[2, dgv_items.CurrentRow.Index].Value);
                ItemEspejo.DescripcionCorta = Convert.ToString(dgv_items[3, dgv_items.CurrentRow.Index].Value);
                ItemEspejo.IndCompra = Convert.ToInt32(dgv_items[4, dgv_items.CurrentRow.Index].Value);
                ItemEspejo.IndVenta = Convert.ToInt32(dgv_items[5, dgv_items.CurrentRow.Index].Value);
                ItemEspejo.IndManufactura = Convert.ToInt32(dgv_items[6, dgv_items.CurrentRow.Index].Value);
                ItemEspejo.Seleccion = true;
                if (chk_descripcion_tecnica.Checked)
                    ItemEspejo.LlenarDescripcionTecnica(Convert.ToString(dgv_items[0, dgv_items.CurrentRow.Index].Value));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor = Cursors.Default;
        }
    }
}
