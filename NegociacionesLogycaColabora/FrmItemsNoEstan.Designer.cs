namespace NegociacionesLogycaColabora
{
    partial class FrmItemsNoEstan
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmItemsNoEstan));
			this.dgv_items = new System.Windows.Forms.DataGridView();
			this.Col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btn_cerrar = new System.Windows.Forms.Button();
			this.lbl_nota = new System.Windows.Forms.Label();
			this.lbl_nro = new System.Windows.Forms.Label();
			this.btn_notificar = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgv_items)).BeginInit();
			this.SuspendLayout();
			// 
			// dgv_items
			// 
			this.dgv_items.AllowUserToAddRows = false;
			this.dgv_items.AllowUserToDeleteRows = false;
			this.dgv_items.AllowUserToOrderColumns = true;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			this.dgv_items.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dgv_items.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dgv_items.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(1, 5, 1, 5);
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgv_items.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgv_items.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_items.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col1,
            this.Col2});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(217)))), ((int)(((byte)(241)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgv_items.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgv_items.EnableHeadersVisualStyles = false;
			this.dgv_items.Location = new System.Drawing.Point(12, 52);
			this.dgv_items.MultiSelect = false;
			this.dgv_items.Name = "dgv_items";
			this.dgv_items.Size = new System.Drawing.Size(356, 272);
			this.dgv_items.TabIndex = 1;
			// 
			// Col1
			// 
			this.Col1.HeaderText = "GTIN";
			this.Col1.Name = "Col1";
			// 
			// Col2
			// 
			this.Col2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Col2.HeaderText = "DESCRIPCIÓN";
			this.Col2.Name = "Col2";
			// 
			// btn_cerrar
			// 
			this.btn_cerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_cerrar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_cerrar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_cerrar.Location = new System.Drawing.Point(273, 330);
			this.btn_cerrar.Name = "btn_cerrar";
			this.btn_cerrar.Size = new System.Drawing.Size(95, 33);
			this.btn_cerrar.TabIndex = 4;
			this.btn_cerrar.Text = "&CERRAR";
			this.btn_cerrar.UseVisualStyleBackColor = false;
			this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
			// 
			// lbl_nota
			// 
			this.lbl_nota.Location = new System.Drawing.Point(12, 9);
			this.lbl_nota.Name = "lbl_nota";
			this.lbl_nota.Size = new System.Drawing.Size(248, 40);
			this.lbl_nota.TabIndex = 0;
			this.lbl_nota.Text = "...";
			// 
			// lbl_nro
			// 
			this.lbl_nro.AutoSize = true;
			this.lbl_nro.Location = new System.Drawing.Point(12, 327);
			this.lbl_nro.Name = "lbl_nro";
			this.lbl_nro.Size = new System.Drawing.Size(13, 13);
			this.lbl_nro.TabIndex = 2;
			this.lbl_nro.Text = "..";
			// 
			// btn_notificar
			// 
			this.btn_notificar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_notificar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			this.btn_notificar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			this.btn_notificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_notificar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_notificar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_notificar.Location = new System.Drawing.Point(172, 330);
			this.btn_notificar.Name = "btn_notificar";
			this.btn_notificar.Size = new System.Drawing.Size(95, 33);
			this.btn_notificar.TabIndex = 3;
			this.btn_notificar.Text = "&NOTIFICAR";
			this.btn_notificar.UseVisualStyleBackColor = false;
			this.btn_notificar.Visible = false;
			this.btn_notificar.Click += new System.EventHandler(this.btn_notificar_Click);
			// 
			// FrmItemsNoEstan
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(380, 376);
			this.Controls.Add(this.btn_notificar);
			this.Controls.Add(this.lbl_nro);
			this.Controls.Add(this.lbl_nota);
			this.Controls.Add(this.btn_cerrar);
			this.Controls.Add(this.dgv_items);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmItemsNoEstan";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.FrmItemsNoEstan_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgv_items)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_cerrar;
        private System.Windows.Forms.Label lbl_nota;
        public System.Windows.Forms.DataGridView dgv_items;
        private System.Windows.Forms.Label lbl_nro;
        private System.Windows.Forms.Button btn_notificar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col2;
    }
}