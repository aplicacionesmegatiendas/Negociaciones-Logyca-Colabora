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
    public partial class FrmAdmin : Form
    {
        int origen;
        public FrmAdmin(int origen)
        {
            InitializeComponent();
            this.origen = origen;
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            if (origen==0)
            {
                Application.Restart();
                Datos.salir = true;
            }
            else
            {
                this.Close();
            }
        }

        private void btn_unidades_med_Click(object sender, EventArgs e)
        {
            FrmConfigUndsMedida _FrmConfigUndsMedida = new FrmConfigUndsMedida();
            _FrmConfigUndsMedida.ShowDialog(this);
        }

        private void btn_compradores_Click(object sender, EventArgs e)
        {
            FrmCompradores _FrmCompradores = new FrmCompradores();
            _FrmCompradores.ShowDialog(this);
        }

        private void btn_usuarios_Click(object sender, EventArgs e)
        {
            FrmCrearUsuarios _FrmUsuarios = new FrmCrearUsuarios();
            _FrmUsuarios.ShowDialog(this);
        }

        private void btn_act_Click(object sender, EventArgs e)
        {
            FrmActividades _FrmActividades = new FrmActividades();
            _FrmActividades.ShowDialog(this);
        }

        private void btn_proveedores_Click(object sender, EventArgs e)
        {
            FrmProveedores _FrmProveedores = new FrmProveedores();
            _FrmProveedores.ShowDialog(this);
        }

        private void btn_mot_no_aceptacion_Click(object sender, EventArgs e)
        {
            FrmMotivosNoAceptacion _FrmMotivosNoAceptacion = new FrmMotivosNoAceptacion();
            _FrmMotivosNoAceptacion.ShowDialog();
        }

        private void btn_cat_logyca_Click(object sender, EventArgs e)
        {
            FrmConfigCategoriasLogyca confi = new FrmConfigCategoriasLogyca();
            this.Cursor = Cursors.WaitCursor;
            confi.ShowDialog(this);
            this.Cursor = Cursors.Default;
        }
    }
}
