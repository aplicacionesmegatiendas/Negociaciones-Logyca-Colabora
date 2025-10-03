namespace NegociacionesLogycaColabora
{
    partial class FrmCompradores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCompradores));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_gln = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txt_nombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_usuario = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_usuarios = new System.Windows.Forms.Button();
            this.btn_cerrar = new System.Windows.Forms.Button();
            this.btn_guardar = new System.Windows.Forms.Button();
            this.btn_eliminar = new System.Windows.Forms.Button();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgv_compradores = new System.Windows.Forms.DataGridView();
            this.Col01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_compradores)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Gln:";
            // 
            // txt_gln
            // 
            this.txt_gln.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_gln.Location = new System.Drawing.Point(70, 66);
            this.txt_gln.MaxLength = 20;
            this.txt_gln.Name = "txt_gln";
            this.txt_gln.Size = new System.Drawing.Size(206, 22);
            this.txt_gln.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(837, 54);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "Compradores";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(737, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(97, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // txt_nombre
            // 
            this.txt_nombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_nombre.Location = new System.Drawing.Point(70, 94);
            this.txt_nombre.MaxLength = 50;
            this.txt_nombre.Name = "txt_nombre";
            this.txt_nombre.Size = new System.Drawing.Size(507, 22);
            this.txt_nombre.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nombre:";
            // 
            // txt_usuario
            // 
            this.txt_usuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_usuario.Location = new System.Drawing.Point(70, 153);
            this.txt_usuario.Name = "txt_usuario";
            this.txt_usuario.ReadOnly = true;
            this.txt_usuario.Size = new System.Drawing.Size(177, 22);
            this.txt_usuario.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Usuario:";
            // 
            // btn_usuarios
            // 
            this.btn_usuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_usuarios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_usuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_usuarios.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_usuarios.Location = new System.Drawing.Point(250, 153);
            this.btn_usuarios.Name = "btn_usuarios";
            this.btn_usuarios.Size = new System.Drawing.Size(26, 23);
            this.btn_usuarios.TabIndex = 9;
            this.btn_usuarios.Text = "...";
            this.btn_usuarios.UseVisualStyleBackColor = false;
            this.btn_usuarios.Click += new System.EventHandler(this.btn_usuarios_Click);
            // 
            // btn_cerrar
            // 
            this.btn_cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
            this.btn_cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
            this.btn_cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cerrar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cerrar.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_cerrar.Location = new System.Drawing.Point(718, 559);
            this.btn_cerrar.Name = "btn_cerrar";
            this.btn_cerrar.Size = new System.Drawing.Size(108, 33);
            this.btn_cerrar.TabIndex = 13;
            this.btn_cerrar.Text = "CERRA&R";
            this.btn_cerrar.UseVisualStyleBackColor = false;
            this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
            // 
            // btn_guardar
            // 
            this.btn_guardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_guardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_guardar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_guardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_guardar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_guardar.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_guardar.Location = new System.Drawing.Point(477, 559);
            this.btn_guardar.Name = "btn_guardar";
            this.btn_guardar.Size = new System.Drawing.Size(108, 33);
            this.btn_guardar.TabIndex = 11;
            this.btn_guardar.Text = "&GUARDAR";
            this.btn_guardar.UseVisualStyleBackColor = false;
            this.btn_guardar.Click += new System.EventHandler(this.btn_guardar_Click);
            // 
            // btn_eliminar
            // 
            this.btn_eliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_eliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
            this.btn_eliminar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
            this.btn_eliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_eliminar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_eliminar.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_eliminar.Location = new System.Drawing.Point(591, 559);
            this.btn_eliminar.Name = "btn_eliminar";
            this.btn_eliminar.Size = new System.Drawing.Size(108, 33);
            this.btn_eliminar.TabIndex = 12;
            this.btn_eliminar.Text = "&ELIMINAR";
            this.btn_eliminar.UseVisualStyleBackColor = false;
            this.btn_eliminar.Click += new System.EventHandler(this.btn_eliminar_Click);
            // 
            // txt_email
            // 
            this.txt_email.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_email.Location = new System.Drawing.Point(70, 123);
            this.txt_email.MaxLength = 50;
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(507, 22);
            this.txt_email.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Email:";
            // 
            // dgv_compradores
            // 
            this.dgv_compradores.AllowUserToAddRows = false;
            this.dgv_compradores.AllowUserToDeleteRows = false;
            this.dgv_compradores.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dgv_compradores.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_compradores.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv_compradores.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(1, 5, 1, 5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_compradores.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_compradores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_compradores.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col01,
            this.Col02,
            this.Col03,
            this.Col04,
            this.Col05});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(217)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_compradores.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_compradores.EnableHeadersVisualStyles = false;
            this.dgv_compradores.Location = new System.Drawing.Point(16, 193);
            this.dgv_compradores.Name = "dgv_compradores";
            this.dgv_compradores.ReadOnly = true;
            this.dgv_compradores.Size = new System.Drawing.Size(811, 352);
            this.dgv_compradores.TabIndex = 10;
            this.dgv_compradores.SelectionChanged += new System.EventHandler(this.dgv_compradores_SelectionChanged);
            // 
            // Col01
            // 
            this.Col01.HeaderText = "ID";
            this.Col01.Name = "Col01";
            this.Col01.ReadOnly = true;
            this.Col01.Visible = false;
            // 
            // Col02
            // 
            this.Col02.HeaderText = "GLN";
            this.Col02.Name = "Col02";
            this.Col02.ReadOnly = true;
            // 
            // Col03
            // 
            this.Col03.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col03.HeaderText = "NOMBRE";
            this.Col03.Name = "Col03";
            this.Col03.ReadOnly = true;
            // 
            // Col04
            // 
            this.Col04.HeaderText = "EMAIL";
            this.Col04.Name = "Col04";
            this.Col04.ReadOnly = true;
            this.Col04.Width = 250;
            // 
            // Col05
            // 
            this.Col05.HeaderText = "USUARIO";
            this.Col05.Name = "Col05";
            this.Col05.ReadOnly = true;
            // 
            // FrmCompradores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(837, 603);
            this.Controls.Add(this.dgv_compradores);
            this.Controls.Add(this.txt_email);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_eliminar);
            this.Controls.Add(this.btn_cerrar);
            this.Controls.Add(this.btn_guardar);
            this.Controls.Add(this.btn_usuarios);
            this.Controls.Add(this.txt_usuario);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_nombre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txt_gln);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCompradores";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compradores";
            this.Load += new System.EventHandler(this.FrmCompradores_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_compradores)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_gln;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txt_nombre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_usuario;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_usuarios;
        private System.Windows.Forms.Button btn_cerrar;
        private System.Windows.Forms.Button btn_guardar;
        private System.Windows.Forms.Button btn_eliminar;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgv_compradores;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col01;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col02;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col03;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col04;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col05;
    }
}