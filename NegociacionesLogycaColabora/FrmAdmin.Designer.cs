namespace NegociacionesLogycaColabora
{
    partial class FrmAdmin
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAdmin));
			this.btn_cerrar = new System.Windows.Forms.Button();
			this._imageList = new System.Windows.Forms.ImageList(this.components);
			this.btn_act = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btn_proveedores = new System.Windows.Forms.Button();
			this.btn_compradores = new System.Windows.Forms.Button();
			this.btn_usuarios = new System.Windows.Forms.Button();
			this.btn_unidades_med = new System.Windows.Forms.Button();
			this.btn_mot_no_aceptacion = new System.Windows.Forms.Button();
			this.btn_cat_logyca = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// btn_cerrar
			// 
			this.btn_cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
			this.btn_cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_cerrar.ForeColor = System.Drawing.SystemColors.Window;
			this.btn_cerrar.Location = new System.Drawing.Point(240, 238);
			this.btn_cerrar.Name = "btn_cerrar";
			this.btn_cerrar.Size = new System.Drawing.Size(136, 38);
			this.btn_cerrar.TabIndex = 7;
			this.btn_cerrar.Text = "&CERRAR";
			this.btn_cerrar.UseVisualStyleBackColor = false;
			this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
			// 
			// _imageList
			// 
			this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
			this._imageList.TransparentColor = System.Drawing.Color.Transparent;
			this._imageList.Images.SetKeyName(0, "carpenter-ruler.png");
			this._imageList.Images.SetKeyName(1, "man-user.png");
			this._imageList.Images.SetKeyName(2, "shopping-cart.png");
			this._imageList.Images.SetKeyName(3, "factory-stock-house.png");
			this._imageList.Images.SetKeyName(4, "check-box-empty.png");
			this._imageList.Images.SetKeyName(5, "item-connections.png");
			// 
			// btn_act
			// 
			this.btn_act.BackColor = System.Drawing.Color.White;
			this.btn_act.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_act.ImageList = this._imageList;
			this.btn_act.Location = new System.Drawing.Point(12, 232);
			this.btn_act.Name = "btn_act";
			this.btn_act.Size = new System.Drawing.Size(180, 50);
			this.btn_act.TabIndex = 4;
			this.btn_act.Text = "    ACTIVIDADES";
			this.btn_act.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btn_act.UseVisualStyleBackColor = false;
			this.btn_act.Visible = false;
			this.btn_act.Click += new System.EventHandler(this.btn_act_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(388, 54);
			this.panel1.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(8, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(135, 25);
			this.label2.TabIndex = 0;
			this.label2.Text = "Administrador";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(279, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(101, 39);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// btn_proveedores
			// 
			this.btn_proveedores.BackColor = System.Drawing.Color.White;
			this.btn_proveedores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_proveedores.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_proveedores.ImageIndex = 3;
			this.btn_proveedores.ImageList = this._imageList;
			this.btn_proveedores.Location = new System.Drawing.Point(11, 174);
			this.btn_proveedores.Name = "btn_proveedores";
			this.btn_proveedores.Size = new System.Drawing.Size(180, 50);
			this.btn_proveedores.TabIndex = 5;
			this.btn_proveedores.Text = "            PROVEEDORES";
			this.btn_proveedores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_proveedores.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btn_proveedores.UseVisualStyleBackColor = false;
			this.btn_proveedores.Click += new System.EventHandler(this.btn_proveedores_Click);
			// 
			// btn_compradores
			// 
			this.btn_compradores.BackColor = System.Drawing.Color.White;
			this.btn_compradores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_compradores.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_compradores.ImageIndex = 2;
			this.btn_compradores.ImageList = this._imageList;
			this.btn_compradores.Location = new System.Drawing.Point(199, 118);
			this.btn_compradores.Name = "btn_compradores";
			this.btn_compradores.Size = new System.Drawing.Size(180, 50);
			this.btn_compradores.TabIndex = 4;
			this.btn_compradores.Text = "          COMPRADORES";
			this.btn_compradores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_compradores.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btn_compradores.UseVisualStyleBackColor = false;
			this.btn_compradores.Click += new System.EventHandler(this.btn_compradores_Click);
			// 
			// btn_usuarios
			// 
			this.btn_usuarios.BackColor = System.Drawing.Color.White;
			this.btn_usuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_usuarios.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_usuarios.ImageIndex = 1;
			this.btn_usuarios.ImageList = this._imageList;
			this.btn_usuarios.Location = new System.Drawing.Point(12, 118);
			this.btn_usuarios.Name = "btn_usuarios";
			this.btn_usuarios.Size = new System.Drawing.Size(180, 50);
			this.btn_usuarios.TabIndex = 3;
			this.btn_usuarios.Text = "              USUARIOS";
			this.btn_usuarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_usuarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btn_usuarios.UseVisualStyleBackColor = false;
			this.btn_usuarios.Click += new System.EventHandler(this.btn_usuarios_Click);
			// 
			// btn_unidades_med
			// 
			this.btn_unidades_med.BackColor = System.Drawing.Color.White;
			this.btn_unidades_med.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_unidades_med.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_unidades_med.ImageIndex = 0;
			this.btn_unidades_med.ImageList = this._imageList;
			this.btn_unidades_med.Location = new System.Drawing.Point(198, 62);
			this.btn_unidades_med.Name = "btn_unidades_med";
			this.btn_unidades_med.Size = new System.Drawing.Size(180, 50);
			this.btn_unidades_med.TabIndex = 2;
			this.btn_unidades_med.Text = "    UNIDADES DE MEDIDA";
			this.btn_unidades_med.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_unidades_med.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btn_unidades_med.UseVisualStyleBackColor = false;
			this.btn_unidades_med.Click += new System.EventHandler(this.btn_unidades_med_Click);
			// 
			// btn_mot_no_aceptacion
			// 
			this.btn_mot_no_aceptacion.BackColor = System.Drawing.Color.White;
			this.btn_mot_no_aceptacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_mot_no_aceptacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_mot_no_aceptacion.ImageIndex = 4;
			this.btn_mot_no_aceptacion.ImageList = this._imageList;
			this.btn_mot_no_aceptacion.Location = new System.Drawing.Point(198, 174);
			this.btn_mot_no_aceptacion.Name = "btn_mot_no_aceptacion";
			this.btn_mot_no_aceptacion.Size = new System.Drawing.Size(180, 50);
			this.btn_mot_no_aceptacion.TabIndex = 6;
			this.btn_mot_no_aceptacion.Text = "    MOTIVOS NO ACEPTACIÓN";
			this.btn_mot_no_aceptacion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btn_mot_no_aceptacion.UseVisualStyleBackColor = false;
			this.btn_mot_no_aceptacion.Click += new System.EventHandler(this.btn_mot_no_aceptacion_Click);
			// 
			// btn_cat_logyca
			// 
			this.btn_cat_logyca.BackColor = System.Drawing.Color.White;
			this.btn_cat_logyca.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_cat_logyca.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_cat_logyca.ImageIndex = 5;
			this.btn_cat_logyca.ImageList = this._imageList;
			this.btn_cat_logyca.Location = new System.Drawing.Point(11, 62);
			this.btn_cat_logyca.Name = "btn_cat_logyca";
			this.btn_cat_logyca.Size = new System.Drawing.Size(180, 50);
			this.btn_cat_logyca.TabIndex = 1;
			this.btn_cat_logyca.Text = "       CATEGORÍA LOGYCA";
			this.btn_cat_logyca.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btn_cat_logyca.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btn_cat_logyca.UseVisualStyleBackColor = false;
			this.btn_cat_logyca.Click += new System.EventHandler(this.btn_cat_logyca_Click);
			// 
			// FrmAdmin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(388, 293);
			this.ControlBox = false;
			this.Controls.Add(this.btn_cat_logyca);
			this.Controls.Add(this.btn_mot_no_aceptacion);
			this.Controls.Add(this.btn_proveedores);
			this.Controls.Add(this.btn_cerrar);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btn_act);
			this.Controls.Add(this.btn_compradores);
			this.Controls.Add(this.btn_usuarios);
			this.Controls.Add(this.btn_unidades_med);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmAdmin";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Administrador";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_cerrar;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.Button btn_unidades_med;
        private System.Windows.Forms.Button btn_usuarios;
        private System.Windows.Forms.Button btn_compradores;
        private System.Windows.Forms.Button btn_act;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_proveedores;
        private System.Windows.Forms.Button btn_mot_no_aceptacion;
        private System.Windows.Forms.Button btn_cat_logyca;
    }
}