namespace NegociacionesLogycaColabora
{
	partial class FrmDimensionesImagen
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pbx_imagen = new System.Windows.Forms.PictureBox();
			this.btn_cerrar = new System.Windows.Forms.Button();
			this.txt_alto = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btn_guardar = new System.Windows.Forms.Button();
			this.txt_ancho = new System.Windows.Forms.TextBox();
			this.lbl_tit_fecha_act = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pbx_imagen)).BeginInit();
			this.SuspendLayout();
			// 
			// pbx_imagen
			// 
			this.pbx_imagen.Location = new System.Drawing.Point(12, 12);
			this.pbx_imagen.Name = "pbx_imagen";
			this.pbx_imagen.Size = new System.Drawing.Size(83, 81);
			this.pbx_imagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbx_imagen.TabIndex = 35;
			this.pbx_imagen.TabStop = false;
			// 
			// btn_cerrar
			// 
			this.btn_cerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_cerrar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_cerrar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_cerrar.Location = new System.Drawing.Point(113, 99);
			this.btn_cerrar.Name = "btn_cerrar";
			this.btn_cerrar.Size = new System.Drawing.Size(95, 33);
			this.btn_cerrar.TabIndex = 5;
			this.btn_cerrar.Text = "&CERRAR";
			this.btn_cerrar.UseVisualStyleBackColor = false;
			this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
			// 
			// txt_alto
			// 
			this.txt_alto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txt_alto.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txt_alto.Location = new System.Drawing.Point(154, 60);
			this.txt_alto.MaxLength = 8;
			this.txt_alto.Name = "txt_alto";
			this.txt_alto.Size = new System.Drawing.Size(54, 22);
			this.txt_alto.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(105, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Alto:";
			// 
			// btn_guardar
			// 
			this.btn_guardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			this.btn_guardar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			this.btn_guardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_guardar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_guardar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_guardar.Location = new System.Drawing.Point(12, 99);
			this.btn_guardar.Name = "btn_guardar";
			this.btn_guardar.Size = new System.Drawing.Size(95, 33);
			this.btn_guardar.TabIndex = 4;
			this.btn_guardar.Text = "&GUARDAR";
			this.btn_guardar.UseVisualStyleBackColor = false;
			this.btn_guardar.Click += new System.EventHandler(this.btn_guardar_Click);
			// 
			// txt_ancho
			// 
			this.txt_ancho.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txt_ancho.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txt_ancho.Location = new System.Drawing.Point(154, 32);
			this.txt_ancho.MaxLength = 8;
			this.txt_ancho.Name = "txt_ancho";
			this.txt_ancho.Size = new System.Drawing.Size(54, 22);
			this.txt_ancho.TabIndex = 1;
			// 
			// lbl_tit_fecha_act
			// 
			this.lbl_tit_fecha_act.AutoSize = true;
			this.lbl_tit_fecha_act.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_tit_fecha_act.Location = new System.Drawing.Point(105, 36);
			this.lbl_tit_fecha_act.Name = "lbl_tit_fecha_act";
			this.lbl_tit_fecha_act.Size = new System.Drawing.Size(43, 13);
			this.lbl_tit_fecha_act.TabIndex = 0;
			this.lbl_tit_fecha_act.Text = "Ancho:";
			// 
			// FrmDimensionesImagen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(222, 149);
			this.ControlBox = false;
			this.Controls.Add(this.pbx_imagen);
			this.Controls.Add(this.btn_cerrar);
			this.Controls.Add(this.txt_alto);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btn_guardar);
			this.Controls.Add(this.txt_ancho);
			this.Controls.Add(this.lbl_tit_fecha_act);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "FrmDimensionesImagen";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Dimensiones Imagen";
			this.Load += new System.EventHandler(this.FrmDimensionesImagen_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbx_imagen)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbx_imagen;
		private System.Windows.Forms.Button btn_cerrar;
		private System.Windows.Forms.TextBox txt_alto;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btn_guardar;
		private System.Windows.Forms.TextBox txt_ancho;
		private System.Windows.Forms.Label lbl_tit_fecha_act;
	}
}