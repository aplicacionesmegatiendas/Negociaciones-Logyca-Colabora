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
	public partial class FrmDimensionesImagen : Form
	{
		string ruta_archivo;
		string nombre_archivo;
		string nuevo_nombre;
		public FrmDimensionesImagen(string ruta_archivo, string nombre_archivo, string nuevo_nombre)
		{
			InitializeComponent();
			this.ruta_archivo = ruta_archivo;
			this.nombre_archivo = nombre_archivo;
			this.nuevo_nombre = nuevo_nombre;
		}

		private void SoloNumeros(object sender, KeyPressEventArgs e)
		{
			if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
			{
				e.Handled = true;
			}
			else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8))
			{
				e.Handled = true;
			}
		}

		public void RedimensionarImagen(string rutaOriginal, int nuevoAncho, int nuevoAlto)
		{
			using (var imagenOriginal = new Bitmap(rutaOriginal))
			{
				var imagenRedimensionada = new Bitmap(nuevoAncho, nuevoAlto);
				using (var grafico = Graphics.FromImage(imagenRedimensionada))
				{
					grafico.DrawImage(imagenOriginal, 0, 0, nuevoAncho, nuevoAlto);
				}

				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.Filter = "Imagen JPEG|*.jpg";
					saveFileDialog.Title = "Guardar Imagen";
					saveFileDialog.FileName = nuevo_nombre;
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						imagenRedimensionada.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
						MessageBox.Show("Listo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
						Close();
					}
				}
			}
		}

		private void btn_cerrar_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FrmDimensionesImagen_Load(object sender, EventArgs e)
		{
			try
			{
				pbx_imagen.Image = Image.FromFile(ruta_archivo + "\\" + nombre_archivo);
				txt_ancho.Text = pbx_imagen.Image.Width.ToString();
				txt_alto.Text = pbx_imagen.Image.Height.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btn_guardar_Click(object sender, EventArgs e)
		{
			if (txt_alto.Text.Trim().Equals(string.Empty) || txt_ancho.Text.Trim().Equals(string.Empty))
			{
				MessageBox.Show("Escriba el ancho, el alto y el nombre de la nueva imagen", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			try
			{
				RedimensionarImagen(ruta_archivo + "\\" + nombre_archivo, Convert.ToInt32(txt_ancho.Text), Convert.ToInt32(txt_alto.Text));
				pbx_imagen.Image.Dispose();
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
