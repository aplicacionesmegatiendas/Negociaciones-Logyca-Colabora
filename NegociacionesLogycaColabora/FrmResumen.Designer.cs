namespace NegociacionesLogycaColabora
{
	partial class FrmResumen
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmResumen));
			this.dgv_resumen = new System.Windows.Forms.DataGridView();
			this.btn_cerrar = new System.Windows.Forms.Button();
			this.col_conector = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.col_linea = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.col_tipo_reg = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.col_sub_tipo_reg = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.col_version = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.col_nivel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.col_error = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.col_detalle = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgv_resumen)).BeginInit();
			this.SuspendLayout();
			// 
			// dgv_resumen
			// 
			this.dgv_resumen.AllowUserToAddRows = false;
			this.dgv_resumen.AllowUserToDeleteRows = false;
			this.dgv_resumen.AllowUserToOrderColumns = true;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			this.dgv_resumen.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dgv_resumen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgv_resumen.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dgv_resumen.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(1, 5, 1, 5);
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgv_resumen.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgv_resumen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_resumen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_conector,
            this.col_linea,
            this.col_tipo_reg,
            this.col_sub_tipo_reg,
            this.col_version,
            this.col_nivel,
            this.col_error,
            this.col_detalle});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(217)))), ((int)(((byte)(241)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgv_resumen.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgv_resumen.EnableHeadersVisualStyles = false;
			this.dgv_resumen.Location = new System.Drawing.Point(12, 12);
			this.dgv_resumen.MultiSelect = false;
			this.dgv_resumen.Name = "dgv_resumen";
			this.dgv_resumen.ReadOnly = true;
			this.dgv_resumen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgv_resumen.Size = new System.Drawing.Size(1241, 538);
			this.dgv_resumen.TabIndex = 0;
			// 
			// btn_cerrar
			// 
			this.btn_cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_cerrar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_cerrar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_cerrar.Location = new System.Drawing.Point(1158, 556);
			this.btn_cerrar.Name = "btn_cerrar";
			this.btn_cerrar.Size = new System.Drawing.Size(95, 33);
			this.btn_cerrar.TabIndex = 1;
			this.btn_cerrar.Text = "&CERRAR";
			this.btn_cerrar.UseVisualStyleBackColor = false;
			this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
			// 
			// col_conector
			// 
			this.col_conector.HeaderText = "CONECTOR";
			this.col_conector.Name = "col_conector";
			this.col_conector.ReadOnly = true;
			this.col_conector.Width = 150;
			// 
			// col_linea
			// 
			this.col_linea.HeaderText = "LÍNEA";
			this.col_linea.Name = "col_linea";
			this.col_linea.ReadOnly = true;
			this.col_linea.Width = 120;
			// 
			// col_tipo_reg
			// 
			this.col_tipo_reg.HeaderText = "TIPO REGISTRO";
			this.col_tipo_reg.Name = "col_tipo_reg";
			this.col_tipo_reg.ReadOnly = true;
			this.col_tipo_reg.Width = 120;
			// 
			// col_sub_tipo_reg
			// 
			this.col_sub_tipo_reg.HeaderText = "SUBTIPO REGISTRO";
			this.col_sub_tipo_reg.Name = "col_sub_tipo_reg";
			this.col_sub_tipo_reg.ReadOnly = true;
			this.col_sub_tipo_reg.Width = 120;
			// 
			// col_version
			// 
			this.col_version.HeaderText = "VERSIÓN";
			this.col_version.Name = "col_version";
			this.col_version.ReadOnly = true;
			this.col_version.Width = 120;
			// 
			// col_nivel
			// 
			this.col_nivel.HeaderText = "NIVEL";
			this.col_nivel.Name = "col_nivel";
			this.col_nivel.ReadOnly = true;
			this.col_nivel.Width = 120;
			// 
			// col_error
			// 
			this.col_error.HeaderText = "ERROR";
			this.col_error.Name = "col_error";
			this.col_error.ReadOnly = true;
			this.col_error.Width = 300;
			// 
			// col_detalle
			// 
			this.col_detalle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.col_detalle.HeaderText = "DETALLE";
			this.col_detalle.Name = "col_detalle";
			this.col_detalle.ReadOnly = true;
			// 
			// FrmResumen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1265, 601);
			this.Controls.Add(this.dgv_resumen);
			this.Controls.Add(this.btn_cerrar);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmResumen";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Resumen";
			this.Load += new System.EventHandler(this.FrmResumen_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgv_resumen)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgv_resumen;
		private System.Windows.Forms.Button btn_cerrar;
		private System.Windows.Forms.DataGridViewTextBoxColumn col_conector;
		private System.Windows.Forms.DataGridViewTextBoxColumn col_linea;
		private System.Windows.Forms.DataGridViewTextBoxColumn col_tipo_reg;
		private System.Windows.Forms.DataGridViewTextBoxColumn col_sub_tipo_reg;
		private System.Windows.Forms.DataGridViewTextBoxColumn col_version;
		private System.Windows.Forms.DataGridViewTextBoxColumn col_nivel;
		private System.Windows.Forms.DataGridViewTextBoxColumn col_error;
		private System.Windows.Forms.DataGridViewTextBoxColumn col_detalle;
	}
}