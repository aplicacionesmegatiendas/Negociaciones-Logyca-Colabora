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
    public partial class FrmConfigCategoriasLogyca : Form
    {
        DataTable dt_plan = null;
        DataTable dt_criterio = null;
        DataTable dt_cat_logyca = null;

        public FrmConfigCategoriasLogyca()
        {
            InitializeComponent();
        }

        private void ObtenerPlanes()
        {
            Datos datos = new Datos();
            dt_plan = datos.ObtenerPlanes();
            lbx_plan.DataSource = dt_plan;
            lbx_plan.DisplayMember = "f105_descripcion";
            lbx_plan.ValueMember = "f105_id";
            lbx_plan.SelectedIndex = 1;
            lbx_plan.SelectedIndex = 0;
        }

        private void ObtenerMayores(string plan)
        {
            Datos datos = new Datos();
            dt_criterio = datos.ObtenerCriteriosMayor(plan);
            lbx_mayor.DataSource = dt_criterio;
            lbx_mayor.DisplayMember = "f106_descripcion";
            lbx_mayor.ValueMember = "f106_id";
        }

        private void ObtenerEquivalenciasPricat()
        {
            Datos datos = new Datos();
            dt_cat_logyca = datos.ListarEquivalenciasCategoriaLogyca();
            dgv_equivalencias.AutoGenerateColumns = false;
            dgv_equivalencias.Columns[0].DataPropertyName = "ca_id";
            dgv_equivalencias.Columns[1].DataPropertyName = "ca_catlogyca";
            dgv_equivalencias.Columns[2].DataPropertyName = "ca_plan";
            dgv_equivalencias.Columns[3].DataPropertyName = "ca_desc_plan";
            dgv_equivalencias.Columns[4].DataPropertyName = "ca_criterio";
            dgv_equivalencias.Columns[5].DataPropertyName = "ca_desc_mayor";
            dgv_equivalencias.DataSource = dt_cat_logyca;
        }

        private void FrmConfigCategoriasLogyca_Load(object sender, EventArgs e)
        {
            try
            {
                ObtenerPlanes();
                ObtenerEquivalenciasPricat();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void txt_buscar_plan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dt_plan.DefaultView.RowFilter = "f105_descripcion Like'%" + txt_buscar_plan.Text + "%'";
                lbx_mayor.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_buscar_criterio_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dt_criterio.DefaultView.RowFilter = "f106_descripcion Like'%" + txt_buscar_criterio.Text + "%'";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_buscar_cat_logyca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dt_cat_logyca.DefaultView.RowFilter = "ca_catlogyca Like'%" + txt_buscar_cat_logyca.Text + "%'";
                //lbx_mayor.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (lbx_plan.SelectedIndex == -1 || lbx_mayor.SelectedIndex == -1 || txt_cat_logyca.Text.Trim().Equals(""))
            {
                MessageBox.Show("Seleccione el plan, el criterio y escriba la categoría antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                Datos datos = new Datos();
                datos.GuardarEquivalenciaCategoriaLogyca(txt_cat_logyca.Text.Trim(), lbx_plan.SelectedValue.ToString(), lbx_plan.Text.Split('-')[1], lbx_mayor.SelectedValue.ToString(), lbx_mayor.Text.Split('-')[1]);
                ObtenerEquivalenciasPricat();
                txt_buscar_cat_logyca.Text = "";
                txt_cat_logyca.Focus(); txt_cat_logyca.SelectAll();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbx_plan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbx_plan.Items.Count > 0)
                {
                    ObtenerMayores(lbx_plan.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_equivalencias_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                if (dgv_equivalencias.Rows.Count > 0 && dgv_equivalencias.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("¿Confirma eliminar este registro?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Datos datos = new Datos();
                            datos.EliminarEquivalenciaCategoriaLogyca(Convert.ToInt32(dgv_equivalencias[0, dgv_equivalencias.CurrentRow.Index].Value));
                            ObtenerEquivalenciasPricat();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
