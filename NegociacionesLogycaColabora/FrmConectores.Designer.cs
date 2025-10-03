namespace NegociacionesLogycaColabora
{
    partial class FrmConectores
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConectores));
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this.conectoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.adiciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.itemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.descripciónTecnicaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.criterioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.parametrosDePlaneaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.códigoDeBarrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cotizaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.precioDeVentaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.portafoliosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.todosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cambioDePrecioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.costoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pVPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.todosToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.lbl_tit_und_inv = new System.Windows.Forms.Label();
			this.cmb_proveedores = new System.Windows.Forms.ComboBox();
			this.dtp_fecha_fin = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.dtp_fecha_ini = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btn_buscar = new System.Windows.Forms.Button();
			this.btn_cerrar = new System.Windows.Forms.Button();
			this.dgv_documentos = new System.Windows.Forms.DataGridView();
			this.Col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Col3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Col4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Col5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._menuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgv_documentos)).BeginInit();
			this.SuspendLayout();
			// 
			// _menuStrip
			// 
			this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conectoresToolStripMenuItem});
			this._menuStrip.Location = new System.Drawing.Point(0, 0);
			this._menuStrip.Name = "_menuStrip";
			this._menuStrip.Size = new System.Drawing.Size(621, 24);
			this._menuStrip.TabIndex = 0;
			this._menuStrip.Text = "menuStrip1";
			// 
			// conectoresToolStripMenuItem
			// 
			this.conectoresToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adiciónToolStripMenuItem,
            this.cambioDePrecioToolStripMenuItem});
			this.conectoresToolStripMenuItem.Name = "conectoresToolStripMenuItem";
			this.conectoresToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
			this.conectoresToolStripMenuItem.Text = "Conectores";
			// 
			// adiciónToolStripMenuItem
			// 
			this.adiciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemToolStripMenuItem,
            this.descripciónTecnicaToolStripMenuItem,
            this.criterioToolStripMenuItem,
            this.parametrosDePlaneaciónToolStripMenuItem,
            this.códigoDeBarrasToolStripMenuItem,
            this.cotizaciónToolStripMenuItem,
            this.precioDeVentaToolStripMenuItem,
            this.portafoliosToolStripMenuItem,
            this.pumToolStripMenuItem,
            this.todosToolStripMenuItem});
			this.adiciónToolStripMenuItem.Enabled = false;
			this.adiciónToolStripMenuItem.Name = "adiciónToolStripMenuItem";
			this.adiciónToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.adiciónToolStripMenuItem.Text = "Adición";
			// 
			// itemToolStripMenuItem
			// 
			this.itemToolStripMenuItem.Name = "itemToolStripMenuItem";
			this.itemToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.itemToolStripMenuItem.Text = "Item";
			this.itemToolStripMenuItem.Click += new System.EventHandler(this.itemToolStripMenuItem_Click);
			// 
			// descripciónTecnicaToolStripMenuItem
			// 
			this.descripciónTecnicaToolStripMenuItem.Name = "descripciónTecnicaToolStripMenuItem";
			this.descripciónTecnicaToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.descripciónTecnicaToolStripMenuItem.Text = "Descripción técnica";
			this.descripciónTecnicaToolStripMenuItem.Click += new System.EventHandler(this.descripciónTecnicaToolStripMenuItem_Click);
			// 
			// criterioToolStripMenuItem
			// 
			this.criterioToolStripMenuItem.Name = "criterioToolStripMenuItem";
			this.criterioToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.criterioToolStripMenuItem.Text = "Criterio";
			this.criterioToolStripMenuItem.Click += new System.EventHandler(this.criterioToolStripMenuItem_Click);
			// 
			// parametrosDePlaneaciónToolStripMenuItem
			// 
			this.parametrosDePlaneaciónToolStripMenuItem.Name = "parametrosDePlaneaciónToolStripMenuItem";
			this.parametrosDePlaneaciónToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.parametrosDePlaneaciónToolStripMenuItem.Text = "Parámetros de planeación";
			this.parametrosDePlaneaciónToolStripMenuItem.Click += new System.EventHandler(this.parametrosDePlaneaciónToolStripMenuItem_Click);
			// 
			// códigoDeBarrasToolStripMenuItem
			// 
			this.códigoDeBarrasToolStripMenuItem.Name = "códigoDeBarrasToolStripMenuItem";
			this.códigoDeBarrasToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.códigoDeBarrasToolStripMenuItem.Text = "Código de barras";
			this.códigoDeBarrasToolStripMenuItem.Click += new System.EventHandler(this.códigoDeBarrasToolStripMenuItem_Click);
			// 
			// cotizaciónToolStripMenuItem
			// 
			this.cotizaciónToolStripMenuItem.Name = "cotizaciónToolStripMenuItem";
			this.cotizaciónToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.cotizaciónToolStripMenuItem.Text = "Cotización";
			this.cotizaciónToolStripMenuItem.Click += new System.EventHandler(this.cotizaciónToolStripMenuItem_Click);
			// 
			// precioDeVentaToolStripMenuItem
			// 
			this.precioDeVentaToolStripMenuItem.Name = "precioDeVentaToolStripMenuItem";
			this.precioDeVentaToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.precioDeVentaToolStripMenuItem.Text = "Precio de venta";
			this.precioDeVentaToolStripMenuItem.Click += new System.EventHandler(this.precioDeVentaToolStripMenuItem_Click);
			// 
			// portafoliosToolStripMenuItem
			// 
			this.portafoliosToolStripMenuItem.Name = "portafoliosToolStripMenuItem";
			this.portafoliosToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.portafoliosToolStripMenuItem.Text = "Portafolios";
			this.portafoliosToolStripMenuItem.Click += new System.EventHandler(this.portafoliosToolStripMenuItem_Click);
			// 
			// todosToolStripMenuItem
			// 
			this.todosToolStripMenuItem.Name = "todosToolStripMenuItem";
			this.todosToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.todosToolStripMenuItem.Text = "Todos";
			this.todosToolStripMenuItem.Click += new System.EventHandler(this.todosToolStripMenuItem_Click);
			// 
			// cambioDePrecioToolStripMenuItem
			// 
			this.cambioDePrecioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.costoToolStripMenuItem,
            this.pVPToolStripMenuItem,
            this.todosToolStripMenuItem1});
			this.cambioDePrecioToolStripMenuItem.Enabled = false;
			this.cambioDePrecioToolStripMenuItem.Name = "cambioDePrecioToolStripMenuItem";
			this.cambioDePrecioToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.cambioDePrecioToolStripMenuItem.Text = "Cambio de precio";
			// 
			// costoToolStripMenuItem
			// 
			this.costoToolStripMenuItem.Name = "costoToolStripMenuItem";
			this.costoToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.costoToolStripMenuItem.Text = "Costo";
			this.costoToolStripMenuItem.Click += new System.EventHandler(this.costoToolStripMenuItem_Click);
			// 
			// pVPToolStripMenuItem
			// 
			this.pVPToolStripMenuItem.Name = "pVPToolStripMenuItem";
			this.pVPToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.pVPToolStripMenuItem.Text = "PVP";
			this.pVPToolStripMenuItem.Click += new System.EventHandler(this.pVPToolStripMenuItem_Click);
			// 
			// todosToolStripMenuItem1
			// 
			this.todosToolStripMenuItem1.Name = "todosToolStripMenuItem1";
			this.todosToolStripMenuItem1.Size = new System.Drawing.Size(105, 22);
			this.todosToolStripMenuItem1.Text = "Todos";
			this.todosToolStripMenuItem1.Click += new System.EventHandler(this.todosToolStripMenuItem1_Click);
			// 
			// lbl_tit_und_inv
			// 
			this.lbl_tit_und_inv.AutoSize = true;
			this.lbl_tit_und_inv.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_tit_und_inv.Location = new System.Drawing.Point(12, 39);
			this.lbl_tit_und_inv.Name = "lbl_tit_und_inv";
			this.lbl_tit_und_inv.Size = new System.Drawing.Size(73, 13);
			this.lbl_tit_und_inv.TabIndex = 2;
			this.lbl_tit_und_inv.Text = "Proveedores:";
			// 
			// cmb_proveedores
			// 
			this.cmb_proveedores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmb_proveedores.FormattingEnabled = true;
			this.cmb_proveedores.Location = new System.Drawing.Point(16, 58);
			this.cmb_proveedores.Name = "cmb_proveedores";
			this.cmb_proveedores.Size = new System.Drawing.Size(311, 21);
			this.cmb_proveedores.TabIndex = 3;
			// 
			// dtp_fecha_fin
			// 
			this.dtp_fecha_fin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtp_fecha_fin.Location = new System.Drawing.Point(502, 58);
			this.dtp_fecha_fin.Name = "dtp_fecha_fin";
			this.dtp_fecha_fin.Size = new System.Drawing.Size(104, 22);
			this.dtp_fecha_fin.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(333, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Fecha procesado:";
			// 
			// dtp_fecha_ini
			// 
			this.dtp_fecha_ini.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtp_fecha_ini.Location = new System.Drawing.Point(376, 58);
			this.dtp_fecha_ini.Name = "dtp_fecha_ini";
			this.dtp_fecha_ini.Size = new System.Drawing.Size(104, 22);
			this.dtp_fecha_ini.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(333, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Entre";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(484, 61);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "y";
			// 
			// btn_buscar
			// 
			this.btn_buscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			this.btn_buscar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			this.btn_buscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_buscar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_buscar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_buscar.Location = new System.Drawing.Point(502, 86);
			this.btn_buscar.Name = "btn_buscar";
			this.btn_buscar.Size = new System.Drawing.Size(104, 33);
			this.btn_buscar.TabIndex = 24;
			this.btn_buscar.Text = "&BUSCAR";
			this.btn_buscar.UseVisualStyleBackColor = false;
			this.btn_buscar.Click += new System.EventHandler(this.btn_buscar_Click);
			// 
			// btn_cerrar
			// 
			this.btn_cerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_cerrar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_cerrar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_cerrar.Location = new System.Drawing.Point(502, 405);
			this.btn_cerrar.Name = "btn_cerrar";
			this.btn_cerrar.Size = new System.Drawing.Size(104, 33);
			this.btn_cerrar.TabIndex = 25;
			this.btn_cerrar.Text = "&CERRAR";
			this.btn_cerrar.UseVisualStyleBackColor = false;
			this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
			// 
			// dgv_documentos
			// 
			this.dgv_documentos.AllowUserToAddRows = false;
			this.dgv_documentos.AllowUserToDeleteRows = false;
			this.dgv_documentos.AllowUserToOrderColumns = true;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			this.dgv_documentos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dgv_documentos.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dgv_documentos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(1, 5, 1, 5);
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgv_documentos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgv_documentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_documentos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col1,
            this.Col2,
            this.Col3,
            this.Col4,
            this.Col5});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(217)))), ((int)(((byte)(241)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgv_documentos.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgv_documentos.EnableHeadersVisualStyles = false;
			this.dgv_documentos.Location = new System.Drawing.Point(16, 125);
			this.dgv_documentos.MultiSelect = false;
			this.dgv_documentos.Name = "dgv_documentos";
			this.dgv_documentos.ReadOnly = true;
			this.dgv_documentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgv_documentos.Size = new System.Drawing.Size(590, 274);
			this.dgv_documentos.TabIndex = 26;
			this.dgv_documentos.SelectionChanged += new System.EventHandler(this.dgv_documentos_SelectionChanged);
			// 
			// Col1
			// 
			this.Col1.HeaderText = "ACCIÓN";
			this.Col1.Name = "Col1";
			this.Col1.ReadOnly = true;
			// 
			// Col2
			// 
			this.Col2.HeaderText = "NRO. DOC.";
			this.Col2.Name = "Col2";
			this.Col2.ReadOnly = true;
			this.Col2.Width = 150;
			// 
			// Col3
			// 
			this.Col3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Col3.HeaderText = "NOMB. DOC.";
			this.Col3.Name = "Col3";
			this.Col3.ReadOnly = true;
			// 
			// Col4
			// 
			this.Col4.HeaderText = "COMP";
			this.Col4.Name = "Col4";
			this.Col4.ReadOnly = true;
			this.Col4.Visible = false;
			// 
			// Col5
			// 
			this.Col5.HeaderText = "PROV";
			this.Col5.Name = "Col5";
			this.Col5.ReadOnly = true;
			this.Col5.Visible = false;
			// 
			// pumToolStripMenuItem
			// 
			this.pumToolStripMenuItem.Name = "pumToolStripMenuItem";
			this.pumToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.pumToolStripMenuItem.Text = "Pum";
			this.pumToolStripMenuItem.Click += new System.EventHandler(this.pumToolStripMenuItem_Click);
			// 
			// FrmConectores
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(621, 450);
			this.Controls.Add(this.dgv_documentos);
			this.Controls.Add(this.btn_buscar);
			this.Controls.Add(this.btn_cerrar);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.dtp_fecha_ini);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dtp_fecha_fin);
			this.Controls.Add(this.lbl_tit_und_inv);
			this.Controls.Add(this.cmb_proveedores);
			this.Controls.Add(this._menuStrip);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this._menuStrip;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmConectores";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Conectores";
			this.Load += new System.EventHandler(this.FrmConectores_Load);
			this._menuStrip.ResumeLayout(false);
			this._menuStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgv_documentos)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem conectoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adiciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem descripciónTecnicaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem criterioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parametrosDePlaneaciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem códigoDeBarrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cotizaciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem precioDeVentaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portafoliosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem todosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cambioDePrecioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem costoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pVPToolStripMenuItem;
        private System.Windows.Forms.Label lbl_tit_und_inv;
        private System.Windows.Forms.ComboBox cmb_proveedores;
        private System.Windows.Forms.DateTimePicker dtp_fecha_fin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_fecha_ini;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_buscar;
        private System.Windows.Forms.Button btn_cerrar;
        private System.Windows.Forms.DataGridView dgv_documentos;
        private System.Windows.Forms.ToolStripMenuItem todosToolStripMenuItem1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col5;
		private System.Windows.Forms.ToolStripMenuItem pumToolStripMenuItem;
	}
}