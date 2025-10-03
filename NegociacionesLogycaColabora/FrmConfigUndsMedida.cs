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
    public partial class FrmConfigUndsMedida : Form
    {
        public FrmConfigUndsMedida()
        {
            InitializeComponent();
        }

        private void ListarUnidadesMedida()
        {
            dgv_unidades.AutoGenerateColumns = false;
            dgv_unidades.Columns[0].DataPropertyName = "un_id";
            dgv_unidades.Columns[1].DataPropertyName = "un_unidad_megatiendas";
            dgv_unidades.Columns[2].DataPropertyName = "un_unidad_logyca";
            Datos datos = new Datos();
            dgv_unidades.DataSource = datos.ListarEquivalenciaUnidadesMedida();
        }

        private void FrmConfigUndsMedida_Load(object sender, EventArgs e)
        {
            try
            {
                ListarUnidadesMedida();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (txt_und_mega.Text.Trim().Equals("") || txt_und_logyca.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba la unidad de medida Megatiendas y la unidad de medida Logyca", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                Datos guardar = new Datos();
                guardar.GuardarUnidadMedida(txt_und_mega.Text.Trim(), txt_und_logyca.Text.Trim());

                MessageBox.Show("Unidad de medida guardada exitosamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_und_mega.Text = ""; txt_und_logyca.Text = "";
                txt_und_mega.Focus();
                ListarUnidadesMedida();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_unidades_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                if (dgv_unidades.Rows.Count > 0 && dgv_unidades.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("¿Confirma eliminar este registro?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Datos datos = new Datos();
                            datos.EliminarUnidadMedida(Convert.ToInt32(dgv_unidades[0, dgv_unidades.CurrentRow.Index].Value));
                            ListarUnidadesMedida();
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
