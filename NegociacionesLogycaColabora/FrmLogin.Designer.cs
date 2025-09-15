namespace NegociacionesLogycaColabora
{
    partial class FrmLogin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
			this.txt_usuario = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txt_contra = new System.Windows.Forms.TextBox();
			this.btn_aceptar = new System.Windows.Forms.Button();
			this.btn_cerrar = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.chkAdmin = new System.Windows.Forms.CheckBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// txt_usuario
			// 
			this.txt_usuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txt_usuario.Location = new System.Drawing.Point(86, 81);
			this.txt_usuario.MaxLength = 15;
			this.txt_usuario.Name = "txt_usuario";
			this.txt_usuario.Size = new System.Drawing.Size(144, 22);
			this.txt_usuario.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 84);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Usuario:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Contraseña:";
			// 
			// txt_contra
			// 
			this.txt_contra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txt_contra.Location = new System.Drawing.Point(86, 109);
			this.txt_contra.MaxLength = 15;
			this.txt_contra.Name = "txt_contra";
			this.txt_contra.Size = new System.Drawing.Size(144, 22);
			this.txt_contra.TabIndex = 3;
			this.txt_contra.UseSystemPasswordChar = true;
			// 
			// btn_aceptar
			// 
			this.btn_aceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_aceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			this.btn_aceptar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
			this.btn_aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_aceptar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_aceptar.Location = new System.Drawing.Point(10, 142);
			this.btn_aceptar.Name = "btn_aceptar";
			this.btn_aceptar.Size = new System.Drawing.Size(108, 33);
			this.btn_aceptar.TabIndex = 4;
			this.btn_aceptar.Text = "&ACEPTAR";
			this.btn_aceptar.UseVisualStyleBackColor = false;
			this.btn_aceptar.Click += new System.EventHandler(this.btn_aceptar_Click);
			// 
			// btn_cerrar
			// 
			this.btn_cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_cerrar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_cerrar.Location = new System.Drawing.Point(122, 142);
			this.btn_cerrar.Name = "btn_cerrar";
			this.btn_cerrar.Size = new System.Drawing.Size(108, 33);
			this.btn_cerrar.TabIndex = 5;
			this.btn_cerrar.Text = "&CERRAR";
			this.btn_cerrar.UseVisualStyleBackColor = false;
			this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(240, 54);
			this.panel1.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.Black;
			this.label3.Location = new System.Drawing.Point(13, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 21);
			this.label3.TabIndex = 1;
			this.label3.Text = "Login";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(134, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(97, 44);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// chkAdmin
			// 
			this.chkAdmin.AutoSize = true;
			this.chkAdmin.Location = new System.Drawing.Point(12, 60);
			this.chkAdmin.Name = "chkAdmin";
			this.chkAdmin.Size = new System.Drawing.Size(199, 17);
			this.chkAdmin.TabIndex = 7;
			this.chkAdmin.Text = "Iniciar sesión como a&dministrador";
			this.chkAdmin.UseVisualStyleBackColor = true;
			this.chkAdmin.CheckedChanged += new System.EventHandler(this.chkAdmin_CheckedChanged);
			// 
			// FrmLogin
			// 
			this.AcceptButton = this.btn_aceptar;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.CancelButton = this.btn_cerrar;
			this.ClientSize = new System.Drawing.Size(240, 183);
			this.ControlBox = false;
			this.Controls.Add(this.chkAdmin);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btn_cerrar);
			this.Controls.Add(this.btn_aceptar);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txt_contra);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txt_usuario);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.Name = "FrmLogin";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Login";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLogin_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmLogin_KeyUp);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_usuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_contra;
        private System.Windows.Forms.Button btn_aceptar;
        private System.Windows.Forms.Button btn_cerrar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkAdmin;
    }
}