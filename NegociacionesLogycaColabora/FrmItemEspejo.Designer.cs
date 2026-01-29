namespace NegociacionesLogycaColabora
{
    partial class FrmItemEspejo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmItemEspejo));
            this.btn_aceptar = new System.Windows.Forms.Button();
            this.btn_buscar = new System.Windows.Forms.Button();
            this.txt_descripcion_barra = new System.Windows.Forms.TextBox();
            this.dgv_items = new System.Windows.Forms.DataGridView();
            this.Col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rdb_descripcion = new System.Windows.Forms.RadioButton();
            this.rdb_barra = new System.Windows.Forms.RadioButton();
            this.chk_imp_inv = new System.Windows.Forms.CheckBox();
            this.chk_criterios_clasificacion = new System.Windows.Forms.CheckBox();
            this.chk_parametros_planeacion = new System.Windows.Forms.CheckBox();
            this.chk_listas_precio = new System.Windows.Forms.CheckBox();
            this.chk_portafolio_barra = new System.Windows.Forms.CheckBox();
            this.chk_descripcion_tecnica = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_items)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_aceptar
            // 
            this.btn_aceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_aceptar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_aceptar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_aceptar.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_aceptar.Location = new System.Drawing.Point(734, 359);
            this.btn_aceptar.Name = "btn_aceptar";
            this.btn_aceptar.Size = new System.Drawing.Size(95, 33);
            this.btn_aceptar.TabIndex = 11;
            this.btn_aceptar.Text = "&ACEPTAR";
            this.btn_aceptar.UseVisualStyleBackColor = false;
            this.btn_aceptar.Click += new System.EventHandler(this.btn_aceptar_Click);
            // 
            // btn_buscar
            // 
            this.btn_buscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_buscar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_buscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_buscar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_buscar.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_buscar.Location = new System.Drawing.Point(576, 12);
            this.btn_buscar.Name = "btn_buscar";
            this.btn_buscar.Size = new System.Drawing.Size(95, 33);
            this.btn_buscar.TabIndex = 3;
            this.btn_buscar.Text = "&BUSCAR";
            this.btn_buscar.UseVisualStyleBackColor = false;
            this.btn_buscar.Click += new System.EventHandler(this.btn_buscar_Click);
            // 
            // txt_descripcion_barra
            // 
            this.txt_descripcion_barra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_descripcion_barra.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_descripcion_barra.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_descripcion_barra.Location = new System.Drawing.Point(279, 21);
            this.txt_descripcion_barra.MaxLength = 20;
            this.txt_descripcion_barra.Name = "txt_descripcion_barra";
            this.txt_descripcion_barra.Size = new System.Drawing.Size(291, 22);
            this.txt_descripcion_barra.TabIndex = 2;
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
            this.Col4,
            this.Col2,
            this.Col3,
            this.Col5,
            this.Col6,
            this.Col7});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(217)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_items.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_items.EnableHeadersVisualStyles = false;
            this.dgv_items.Location = new System.Drawing.Point(12, 51);
            this.dgv_items.MultiSelect = false;
            this.dgv_items.Name = "dgv_items";
            this.dgv_items.ReadOnly = true;
            this.dgv_items.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_items.Size = new System.Drawing.Size(659, 341);
            this.dgv_items.TabIndex = 4;
            // 
            // Col1
            // 
            this.Col1.HeaderText = "ID";
            this.Col1.Name = "Col1";
            this.Col1.ReadOnly = true;
            // 
            // Col4
            // 
            this.Col4.HeaderText = "REFERENCIA";
            this.Col4.Name = "Col4";
            this.Col4.ReadOnly = true;
            // 
            // Col2
            // 
            this.Col2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col2.HeaderText = "DESCRIPCIÓN";
            this.Col2.Name = "Col2";
            this.Col2.ReadOnly = true;
            // 
            // Col3
            // 
            this.Col3.HeaderText = "DESCCORTA";
            this.Col3.Name = "Col3";
            this.Col3.ReadOnly = true;
            this.Col3.Visible = false;
            // 
            // Col5
            // 
            this.Col5.HeaderText = "IND_COMP";
            this.Col5.Name = "Col5";
            this.Col5.ReadOnly = true;
            this.Col5.Visible = false;
            // 
            // Col6
            // 
            this.Col6.HeaderText = "IND_VTA";
            this.Col6.Name = "Col6";
            this.Col6.ReadOnly = true;
            this.Col6.Visible = false;
            // 
            // Col7
            // 
            this.Col7.HeaderText = "IND_MANUF";
            this.Col7.Name = "Col7";
            this.Col7.ReadOnly = true;
            this.Col7.Visible = false;
            // 
            // rdb_descripcion
            // 
            this.rdb_descripcion.AutoSize = true;
            this.rdb_descripcion.Checked = true;
            this.rdb_descripcion.Location = new System.Drawing.Point(130, 24);
            this.rdb_descripcion.Name = "rdb_descripcion";
            this.rdb_descripcion.Size = new System.Drawing.Size(85, 17);
            this.rdb_descripcion.TabIndex = 0;
            this.rdb_descripcion.TabStop = true;
            this.rdb_descripcion.Text = "Descripción";
            this.rdb_descripcion.UseVisualStyleBackColor = true;
            // 
            // rdb_barra
            // 
            this.rdb_barra.AutoSize = true;
            this.rdb_barra.Location = new System.Drawing.Point(222, 24);
            this.rdb_barra.Name = "rdb_barra";
            this.rdb_barra.Size = new System.Drawing.Size(52, 17);
            this.rdb_barra.TabIndex = 1;
            this.rdb_barra.Text = "Barra";
            this.rdb_barra.UseVisualStyleBackColor = true;
            // 
            // chk_imp_inv
            // 
            this.chk_imp_inv.Checked = true;
            this.chk_imp_inv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_imp_inv.Location = new System.Drawing.Point(681, 66);
            this.chk_imp_inv.Name = "chk_imp_inv";
            this.chk_imp_inv.Size = new System.Drawing.Size(161, 31);
            this.chk_imp_inv.TabIndex = 5;
            this.chk_imp_inv.Text = "Grupo impositivo y Tipo de inventario";
            this.chk_imp_inv.UseVisualStyleBackColor = true;
            // 
            // chk_criterios_clasificacion
            // 
            this.chk_criterios_clasificacion.AutoSize = true;
            this.chk_criterios_clasificacion.Checked = true;
            this.chk_criterios_clasificacion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_criterios_clasificacion.Location = new System.Drawing.Point(681, 106);
            this.chk_criterios_clasificacion.Name = "chk_criterios_clasificacion";
            this.chk_criterios_clasificacion.Size = new System.Drawing.Size(147, 17);
            this.chk_criterios_clasificacion.TabIndex = 6;
            this.chk_criterios_clasificacion.Text = "Criterios de clasificacón";
            this.chk_criterios_clasificacion.UseVisualStyleBackColor = true;
            // 
            // chk_parametros_planeacion
            // 
            this.chk_parametros_planeacion.AutoSize = true;
            this.chk_parametros_planeacion.Checked = true;
            this.chk_parametros_planeacion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_parametros_planeacion.Location = new System.Drawing.Point(681, 132);
            this.chk_parametros_planeacion.Name = "chk_parametros_planeacion";
            this.chk_parametros_planeacion.Size = new System.Drawing.Size(159, 17);
            this.chk_parametros_planeacion.TabIndex = 7;
            this.chk_parametros_planeacion.Text = "Parámetros de planeación";
            this.chk_parametros_planeacion.UseVisualStyleBackColor = true;
            // 
            // chk_listas_precio
            // 
            this.chk_listas_precio.AutoSize = true;
            this.chk_listas_precio.Checked = true;
            this.chk_listas_precio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_listas_precio.Location = new System.Drawing.Point(681, 158);
            this.chk_listas_precio.Name = "chk_listas_precio";
            this.chk_listas_precio.Size = new System.Drawing.Size(105, 17);
            this.chk_listas_precio.TabIndex = 8;
            this.chk_listas_precio.Text = "Listas de precio";
            this.chk_listas_precio.UseVisualStyleBackColor = true;
            // 
            // chk_portafolio_barra
            // 
            this.chk_portafolio_barra.AutoSize = true;
            this.chk_portafolio_barra.Checked = true;
            this.chk_portafolio_barra.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_portafolio_barra.Location = new System.Drawing.Point(681, 184);
            this.chk_portafolio_barra.Name = "chk_portafolio_barra";
            this.chk_portafolio_barra.Size = new System.Drawing.Size(115, 17);
            this.chk_portafolio_barra.TabIndex = 9;
            this.chk_portafolio_barra.Text = "Portafolio y Barra";
            this.chk_portafolio_barra.UseVisualStyleBackColor = true;
            // 
            // chk_descripcion_tecnica
            // 
            this.chk_descripcion_tecnica.AutoSize = true;
            this.chk_descripcion_tecnica.Checked = true;
            this.chk_descripcion_tecnica.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_descripcion_tecnica.Location = new System.Drawing.Point(681, 210);
            this.chk_descripcion_tecnica.Name = "chk_descripcion_tecnica";
            this.chk_descripcion_tecnica.Size = new System.Drawing.Size(125, 17);
            this.chk_descripcion_tecnica.TabIndex = 10;
            this.chk_descripcion_tecnica.Text = "Descripción tecnica";
            this.chk_descripcion_tecnica.UseVisualStyleBackColor = true;
            // 
            // FrmItemEspejo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(841, 411);
            this.Controls.Add(this.chk_descripcion_tecnica);
            this.Controls.Add(this.chk_portafolio_barra);
            this.Controls.Add(this.chk_listas_precio);
            this.Controls.Add(this.chk_parametros_planeacion);
            this.Controls.Add(this.chk_criterios_clasificacion);
            this.Controls.Add(this.chk_imp_inv);
            this.Controls.Add(this.rdb_barra);
            this.Controls.Add(this.rdb_descripcion);
            this.Controls.Add(this.dgv_items);
            this.Controls.Add(this.btn_aceptar);
            this.Controls.Add(this.btn_buscar);
            this.Controls.Add(this.txt_descripcion_barra);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmItemEspejo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Item Espejo";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_items)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_aceptar;
        private System.Windows.Forms.Button btn_buscar;
        private System.Windows.Forms.TextBox txt_descripcion_barra;
        private System.Windows.Forms.DataGridView dgv_items;
        private System.Windows.Forms.RadioButton rdb_descripcion;
        private System.Windows.Forms.RadioButton rdb_barra;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col7;
        private System.Windows.Forms.CheckBox chk_imp_inv;
        private System.Windows.Forms.CheckBox chk_criterios_clasificacion;
        private System.Windows.Forms.CheckBox chk_parametros_planeacion;
        private System.Windows.Forms.CheckBox chk_listas_precio;
        private System.Windows.Forms.CheckBox chk_portafolio_barra;
        private System.Windows.Forms.CheckBox chk_descripcion_tecnica;
    }
}