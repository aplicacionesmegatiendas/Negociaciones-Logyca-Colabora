namespace NegociacionesLogycaColabora
{
    partial class FrmArchivosPendientes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmArchivosPendientes));
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lvArchivos = new System.Windows.Forms.ListView();
            this._imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.btn_mover = new System.Windows.Forms.Button();
            this.cmb_sucursal = new System.Windows.Forms.ComboBox();
            this.lbl_tit_suc = new System.Windows.Forms.Label();
            this.txt_cantidad = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_tipo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_fecha = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_doc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_razon_soc = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_proveedor = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_accion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_procesar = new System.Windows.Forms.Button();
            this.btn_cerrar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_gln = new System.Windows.Forms.TextBox();
            this.txt_comprador = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_accion = new System.Windows.Forms.Label();
            this.lbl_version = new System.Windows.Forms.Label();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this.conectoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crearConectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultarProcesadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultaCambioDePrecioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_cambioPrecio = new System.Windows.Forms.Button();
            this.btn_adicion = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlInfo.SuspendLayout();
            this._menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList.Images.SetKeyName(0, "plus.png");
            this._imageList.Images.SetKeyName(1, "retiro.png");
            this._imageList.Images.SetKeyName(2, "activacion.png");
            this._imageList.Images.SetKeyName(3, "inactivacion.png");
            this._imageList.Images.SetKeyName(4, "icon.png");
            this._imageList.Images.SetKeyName(5, "add.png");
            this._imageList.Images.SetKeyName(6, "price.png");
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1297, 54);
            this.panel1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(13, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "Archivos Pendientes";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1188, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(97, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lvArchivos
            // 
            this.lvArchivos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvArchivos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lvArchivos.HideSelection = false;
            this.lvArchivos.LargeImageList = this._imageList2;
            this.lvArchivos.Location = new System.Drawing.Point(173, 155);
            this.lvArchivos.MultiSelect = false;
            this.lvArchivos.Name = "lvArchivos";
            this.lvArchivos.Size = new System.Drawing.Size(646, 400);
            this.lvArchivos.SmallImageList = this._imageList2;
            this.lvArchivos.TabIndex = 10;
            this.lvArchivos.UseCompatibleStateImageBehavior = false;
            this.lvArchivos.SelectedIndexChanged += new System.EventHandler(this.lvArchivos_SelectedIndexChanged);
            // 
            // _imageList2
            // 
            this._imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList2.ImageStream")));
            this._imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList2.Images.SetKeyName(0, "price-tag.png");
            this._imageList2.Images.SetKeyName(1, "price-tag (1).png");
            this._imageList2.Images.SetKeyName(2, "tag (1).png");
            // 
            // pnlInfo
            // 
            this.pnlInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfo.Controls.Add(this.btn_mover);
            this.pnlInfo.Controls.Add(this.cmb_sucursal);
            this.pnlInfo.Controls.Add(this.lbl_tit_suc);
            this.pnlInfo.Controls.Add(this.txt_cantidad);
            this.pnlInfo.Controls.Add(this.label12);
            this.pnlInfo.Controls.Add(this.label11);
            this.pnlInfo.Controls.Add(this.label10);
            this.pnlInfo.Controls.Add(this.txt_tipo);
            this.pnlInfo.Controls.Add(this.label9);
            this.pnlInfo.Controls.Add(this.txt_fecha);
            this.pnlInfo.Controls.Add(this.label8);
            this.pnlInfo.Controls.Add(this.txt_doc);
            this.pnlInfo.Controls.Add(this.label7);
            this.pnlInfo.Controls.Add(this.txt_razon_soc);
            this.pnlInfo.Controls.Add(this.label6);
            this.pnlInfo.Controls.Add(this.txt_proveedor);
            this.pnlInfo.Controls.Add(this.label5);
            this.pnlInfo.Controls.Add(this.txt_accion);
            this.pnlInfo.Controls.Add(this.label4);
            this.pnlInfo.Location = new System.Drawing.Point(825, 155);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(458, 400);
            this.pnlInfo.TabIndex = 11;
            // 
            // btn_mover
            // 
            this.btn_mover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_mover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
            this.btn_mover.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_mover.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
            this.btn_mover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_mover.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_mover.Location = new System.Drawing.Point(335, 351);
            this.btn_mover.Name = "btn_mover";
            this.btn_mover.Size = new System.Drawing.Size(109, 33);
            this.btn_mover.TabIndex = 18;
            this.btn_mover.Text = "&MOVER ARCHIVO";
            this.btn_mover.UseVisualStyleBackColor = false;
            this.btn_mover.Click += new System.EventHandler(this.btn_mover_Click);
            // 
            // cmb_sucursal
            // 
            this.cmb_sucursal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sucursal.FormattingEnabled = true;
            this.cmb_sucursal.Location = new System.Drawing.Point(127, 97);
            this.cmb_sucursal.Name = "cmb_sucursal";
            this.cmb_sucursal.Size = new System.Drawing.Size(98, 21);
            this.cmb_sucursal.TabIndex = 6;
            // 
            // lbl_tit_suc
            // 
            this.lbl_tit_suc.AutoSize = true;
            this.lbl_tit_suc.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_tit_suc.Location = new System.Drawing.Point(17, 105);
            this.lbl_tit_suc.Name = "lbl_tit_suc";
            this.lbl_tit_suc.Size = new System.Drawing.Size(53, 13);
            this.lbl_tit_suc.TabIndex = 5;
            this.lbl_tit_suc.Text = "Sucursal:";
            // 
            // txt_cantidad
            // 
            this.txt_cantidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_cantidad.Location = new System.Drawing.Point(126, 294);
            this.txt_cantidad.Name = "txt_cantidad";
            this.txt_cantidad.ReadOnly = true;
            this.txt_cantidad.Size = new System.Drawing.Size(103, 22);
            this.txt_cantidad.TabIndex = 17;
            this.txt_cantidad.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 299);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Cantidad:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(16, 166);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 15);
            this.label11.TabIndex = 7;
            this.label11.Text = "Datos archivo";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 15);
            this.label10.TabIndex = 0;
            this.label10.Text = "Proveedor";
            // 
            // txt_tipo
            // 
            this.txt_tipo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_tipo.Location = new System.Drawing.Point(126, 238);
            this.txt_tipo.Name = "txt_tipo";
            this.txt_tipo.ReadOnly = true;
            this.txt_tipo.Size = new System.Drawing.Size(231, 22);
            this.txt_tipo.TabIndex = 13;
            this.txt_tipo.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 243);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Tipo Doc.:";
            // 
            // txt_fecha
            // 
            this.txt_fecha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_fecha.Location = new System.Drawing.Point(126, 210);
            this.txt_fecha.Name = "txt_fecha";
            this.txt_fecha.ReadOnly = true;
            this.txt_fecha.Size = new System.Drawing.Size(231, 22);
            this.txt_fecha.TabIndex = 11;
            this.txt_fecha.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 215);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Fecha elaboración:";
            // 
            // txt_doc
            // 
            this.txt_doc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_doc.Location = new System.Drawing.Point(126, 182);
            this.txt_doc.Name = "txt_doc";
            this.txt_doc.ReadOnly = true;
            this.txt_doc.Size = new System.Drawing.Size(231, 22);
            this.txt_doc.TabIndex = 9;
            this.txt_doc.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 187);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Nro. Doc.:";
            // 
            // txt_razon_soc
            // 
            this.txt_razon_soc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_razon_soc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_razon_soc.Location = new System.Drawing.Point(126, 69);
            this.txt_razon_soc.Name = "txt_razon_soc";
            this.txt_razon_soc.ReadOnly = true;
            this.txt_razon_soc.Size = new System.Drawing.Size(318, 22);
            this.txt_razon_soc.TabIndex = 4;
            this.txt_razon_soc.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Razón social:";
            // 
            // txt_proveedor
            // 
            this.txt_proveedor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_proveedor.Location = new System.Drawing.Point(126, 41);
            this.txt_proveedor.Name = "txt_proveedor";
            this.txt_proveedor.ReadOnly = true;
            this.txt_proveedor.Size = new System.Drawing.Size(231, 22);
            this.txt_proveedor.TabIndex = 2;
            this.txt_proveedor.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Nit:";
            // 
            // txt_accion
            // 
            this.txt_accion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_accion.Location = new System.Drawing.Point(126, 266);
            this.txt_accion.Name = "txt_accion";
            this.txt_accion.ReadOnly = true;
            this.txt_accion.Size = new System.Drawing.Size(231, 22);
            this.txt_accion.TabIndex = 15;
            this.txt_accion.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 271);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Acción:";
            // 
            // btn_procesar
            // 
            this.btn_procesar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_procesar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_procesar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_procesar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_procesar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_procesar.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_procesar.Location = new System.Drawing.Point(17, 480);
            this.btn_procesar.Name = "btn_procesar";
            this.btn_procesar.Size = new System.Drawing.Size(144, 33);
            this.btn_procesar.TabIndex = 12;
            this.btn_procesar.Text = "&PROCESAR";
            this.btn_procesar.UseVisualStyleBackColor = false;
            this.btn_procesar.Click += new System.EventHandler(this.btn_procesar_Click);
            // 
            // btn_cerrar
            // 
            this.btn_cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_cerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
            this.btn_cerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(6)))), ((int)(((byte)(19)))));
            this.btn_cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cerrar.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_cerrar.Location = new System.Drawing.Point(17, 519);
            this.btn_cerrar.Name = "btn_cerrar";
            this.btn_cerrar.Size = new System.Drawing.Size(144, 33);
            this.btn_cerrar.TabIndex = 13;
            this.btn_cerrar.Text = "&CERRAR";
            this.btn_cerrar.UseVisualStyleBackColor = false;
            this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(170, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Comprador:";
            // 
            // txt_gln
            // 
            this.txt_gln.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_gln.Location = new System.Drawing.Point(255, 108);
            this.txt_gln.Name = "txt_gln";
            this.txt_gln.ReadOnly = true;
            this.txt_gln.Size = new System.Drawing.Size(263, 22);
            this.txt_gln.TabIndex = 4;
            this.txt_gln.TabStop = false;
            // 
            // txt_comprador
            // 
            this.txt_comprador.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_comprador.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_comprador.Location = new System.Drawing.Point(526, 108);
            this.txt_comprador.Name = "txt_comprador";
            this.txt_comprador.ReadOnly = true;
            this.txt_comprador.Size = new System.Drawing.Size(756, 22);
            this.txt_comprador.TabIndex = 5;
            this.txt_comprador.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(58, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Acciones";
            // 
            // lbl_accion
            // 
            this.lbl_accion.AutoSize = true;
            this.lbl_accion.Location = new System.Drawing.Point(171, 139);
            this.lbl_accion.Name = "lbl_accion";
            this.lbl_accion.Size = new System.Drawing.Size(16, 13);
            this.lbl_accion.TabIndex = 9;
            this.lbl_accion.Text = "...";
            // 
            // lbl_version
            // 
            this.lbl_version.AutoSize = true;
            this.lbl_version.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_version.Location = new System.Drawing.Point(18, 109);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(17, 17);
            this.lbl_version.TabIndex = 2;
            this.lbl_version.Text = "...";
            // 
            // _menuStrip
            // 
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conectoresToolStripMenuItem,
            this.consultasToolStripMenuItem});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(1297, 24);
            this._menuStrip.TabIndex = 0;
            this._menuStrip.Text = "menuStrip1";
            // 
            // conectoresToolStripMenuItem
            // 
            this.conectoresToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crearConectorToolStripMenuItem});
            this.conectoresToolStripMenuItem.Name = "conectoresToolStripMenuItem";
            this.conectoresToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.conectoresToolStripMenuItem.Text = "Conectores";
            // 
            // crearConectorToolStripMenuItem
            // 
            this.crearConectorToolStripMenuItem.Name = "crearConectorToolStripMenuItem";
            this.crearConectorToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.crearConectorToolStripMenuItem.Text = "Crear conector...";
            this.crearConectorToolStripMenuItem.Click += new System.EventHandler(this.crearConectorToolStripMenuItem_Click);
            // 
            // consultasToolStripMenuItem
            // 
            this.consultasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consultarProcesadosToolStripMenuItem,
            this.consultaCambioDePrecioToolStripMenuItem});
            this.consultasToolStripMenuItem.Name = "consultasToolStripMenuItem";
            this.consultasToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.consultasToolStripMenuItem.Text = "Consultas";
            // 
            // consultarProcesadosToolStripMenuItem
            // 
            this.consultarProcesadosToolStripMenuItem.Name = "consultarProcesadosToolStripMenuItem";
            this.consultarProcesadosToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.consultarProcesadosToolStripMenuItem.Text = "Consultar procesados...";
            this.consultarProcesadosToolStripMenuItem.Click += new System.EventHandler(this.consultarProcesadosToolStripMenuItem_Click);
            // 
            // consultaCambioDePrecioToolStripMenuItem
            // 
            this.consultaCambioDePrecioToolStripMenuItem.Name = "consultaCambioDePrecioToolStripMenuItem";
            this.consultaCambioDePrecioToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.consultaCambioDePrecioToolStripMenuItem.Text = "Consulta Cambio de Precio...";
            this.consultaCambioDePrecioToolStripMenuItem.Click += new System.EventHandler(this.consultaCambioDePrecioToolStripMenuItem_Click);
            // 
            // btn_cambioPrecio
            // 
            this.btn_cambioPrecio.BackColor = System.Drawing.Color.White;
            this.btn_cambioPrecio.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_cambioPrecio.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_cambioPrecio.FlatAppearance.BorderSize = 2;
            this.btn_cambioPrecio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cambioPrecio.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cambioPrecio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_cambioPrecio.ImageKey = "price.png";
            this.btn_cambioPrecio.ImageList = this._imageList;
            this.btn_cambioPrecio.Location = new System.Drawing.Point(12, 235);
            this.btn_cambioPrecio.Name = "btn_cambioPrecio";
            this.btn_cambioPrecio.Size = new System.Drawing.Size(144, 50);
            this.btn_cambioPrecio.TabIndex = 8;
            this.btn_cambioPrecio.Text = "    CAMBIO DE PRECIO";
            this.btn_cambioPrecio.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_cambioPrecio.UseVisualStyleBackColor = false;
            this.btn_cambioPrecio.Click += new System.EventHandler(this.btn_cambioPrecio_Click);
            // 
            // btn_adicion
            // 
            this.btn_adicion.BackColor = System.Drawing.Color.White;
            this.btn_adicion.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_adicion.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_adicion.FlatAppearance.BorderSize = 2;
            this.btn_adicion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_adicion.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_adicion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(65)))), ((int)(((byte)(148)))));
            this.btn_adicion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_adicion.ImageKey = "add.png";
            this.btn_adicion.ImageList = this._imageList;
            this.btn_adicion.Location = new System.Drawing.Point(12, 179);
            this.btn_adicion.Name = "btn_adicion";
            this.btn_adicion.Size = new System.Drawing.Size(144, 50);
            this.btn_adicion.TabIndex = 7;
            this.btn_adicion.Text = "    ADICIÓN";
            this.btn_adicion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_adicion.UseVisualStyleBackColor = false;
            this.btn_adicion.Click += new System.EventHandler(this.btn_adicion_Click);
            // 
            // FrmArchivosPendientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1297, 572);
            this.Controls.Add(this.lbl_version);
            this.Controls.Add(this.lbl_accion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_procesar);
            this.Controls.Add(this.txt_comprador);
            this.Controls.Add(this.txt_gln);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_cerrar);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.lvArchivos);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_cambioPrecio);
            this.Controls.Add(this.btn_adicion);
            this.Controls.Add(this._menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this._menuStrip;
            this.Name = "FrmArchivosPendientes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Negociaciones Logyca Colabora";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmArchivosPendientes_FormClosed);
            this.Load += new System.EventHandler(this.FrmArchivosPendientes_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_adicion;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.Button btn_cambioPrecio;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView lvArchivos;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Button btn_cerrar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_gln;
        private System.Windows.Forms.TextBox txt_comprador;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_accion;
        private System.Windows.Forms.TextBox txt_accion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_tipo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_fecha;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_doc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_razon_soc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_proveedor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_procesar;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ImageList _imageList2;
        private System.Windows.Forms.TextBox txt_cantidad;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbl_version;
        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem conectoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crearConectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultarProcesadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultaCambioDePrecioToolStripMenuItem;
        private System.Windows.Forms.ComboBox cmb_sucursal;
        private System.Windows.Forms.Label lbl_tit_suc;
        private System.Windows.Forms.Button btn_mover;
    }
}