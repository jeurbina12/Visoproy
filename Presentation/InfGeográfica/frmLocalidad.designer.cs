namespace Presentation.InfGeográfica
{
    partial class frmLocalidad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLocalidad));
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblId = new System.Windows.Forms.ToolStripLabel();
            this.txtId = new System.Windows.Forms.ToolStripTextBox();
            this.lblNombre = new System.Windows.Forms.ToolStripLabel();
            this.txtNombre = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chkOrden = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbVerMap = new System.Windows.Forms.ToolStripButton();
            this.tsbCadMostrar = new System.Windows.Forms.ToolStripButton();
            this.tsbXYObt = new System.Windows.Forms.ToolStripButton();
            this.tsbAreaObt = new System.Windows.Forms.ToolStripButton();
            this.tsbAtrObt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbGuardar = new System.Windows.Forms.ToolStripButton();
            this.grpCoordenadas = new System.Windows.Forms.GroupBox();
            this.btnCadDibujar = new System.Windows.Forms.Button();
            this.txtX = new System.Windows.Forms.TextBox();
            this.lblX = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.lblEscala = new System.Windows.Forms.Label();
            this.txtEscala = new System.Windows.Forms.TextBox();
            this.lblParroquia = new System.Windows.Forms.Label();
            this.txtParroquia = new System.Windows.Forms.TextBox();
            this.txtMunicipio = new System.Windows.Forms.TextBox();
            this.lblMunicipio = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblPc = new System.Windows.Forms.Label();
            this.lblFRegitro = new System.Windows.Forms.Label();
            this.txtArea = new System.Windows.Forms.TextBox();
            this.lblArea = new System.Windows.Forms.Label();
            this.lblClientes = new System.Windows.Forms.Label();
            this.txtClientes = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblObs = new System.Windows.Forms.ToolStripLabel();
            this.txtFecAct = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ToolStrip1.SuspendLayout();
            this.grpCoordenadas.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblId,
            this.txtId,
            this.lblNombre,
            this.txtNombre,
            this.toolStripSeparator1,
            this.chkOrden,
            this.toolStripSeparator2,
            this.tsbVerMap,
            this.tsbCadMostrar,
            this.tsbXYObt,
            this.tsbAreaObt,
            this.tsbAtrObt,
            this.toolStripSeparator3,
            this.tsbGuardar});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(518, 25);
            this.ToolStrip1.TabIndex = 86;
            // 
            // lblId
            // 
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(20, 22);
            this.lblId.Text = "Id:";
            // 
            // txtId
            // 
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(40, 25);
            // 
            // lblNombre
            // 
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(61, 22);
            this.lblNombre.Text = "Localidad:";
            // 
            // txtNombre
            // 
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(160, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // chkOrden
            // 
            this.chkOrden.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.chkOrden.Image = global::Presentation.Properties.Resources.shape_move_front;
            this.chkOrden.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkOrden.Name = "chkOrden";
            this.chkOrden.Size = new System.Drawing.Size(23, 22);
            this.chkOrden.ToolTipText = "Indica si el formulario se mantiene por encima de todos los formularios";
            this.chkOrden.Click += new System.EventHandler(this.chkOrden_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbVerMap
            // 
            this.tsbVerMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbVerMap.Image = ((System.Drawing.Image)(resources.GetObject("tsbVerMap.Image")));
            this.tsbVerMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbVerMap.Name = "tsbVerMap";
            this.tsbVerMap.Size = new System.Drawing.Size(23, 22);
            this.tsbVerMap.ToolTipText = "Ver en Plano MAP";
            this.tsbVerMap.Click += new System.EventHandler(this.tsbVerMap_Click);
            // 
            // tsbCadMostrar
            // 
            this.tsbCadMostrar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCadMostrar.Image = ((System.Drawing.Image)(resources.GetObject("tsbCadMostrar.Image")));
            this.tsbCadMostrar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCadMostrar.Name = "tsbCadMostrar";
            this.tsbCadMostrar.Size = new System.Drawing.Size(23, 22);
            this.tsbCadMostrar.Text = "Ver Plano";
            this.tsbCadMostrar.ToolTipText = "Ver en Plano CAD";
            this.tsbCadMostrar.Click += new System.EventHandler(this.tsbVerCAD_Click);
            // 
            // tsbXYObt
            // 
            this.tsbXYObt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbXYObt.Enabled = false;
            this.tsbXYObt.Image = ((System.Drawing.Image)(resources.GetObject("tsbXYObt.Image")));
            this.tsbXYObt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbXYObt.Name = "tsbXYObt";
            this.tsbXYObt.Size = new System.Drawing.Size(23, 22);
            this.tsbXYObt.ToolTipText = "Obtener Coordenadas X , Y desde plano AutoCAD";
            this.tsbXYObt.Click += new System.EventHandler(this.tsbObtenerXY_Click);
            // 
            // tsbAreaObt
            // 
            this.tsbAreaObt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAreaObt.Image = global::Presentation.Properties.Resources.map_m2;
            this.tsbAreaObt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAreaObt.Name = "tsbAreaObt";
            this.tsbAreaObt.Size = new System.Drawing.Size(23, 22);
            this.tsbAreaObt.Text = "Obtener Área";
            this.tsbAreaObt.Click += new System.EventHandler(this.tsbAreaObt_Click);
            // 
            // tsbAtrObt
            // 
            this.tsbAtrObt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAtrObt.Image = global::Presentation.Properties.Resources.application_edit;
            this.tsbAtrObt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAtrObt.Name = "tsbAtrObt";
            this.tsbAtrObt.Size = new System.Drawing.Size(23, 22);
            this.tsbAtrObt.Text = "Redefinir Bloque";
            this.tsbAtrObt.Click += new System.EventHandler(this.tsbAtrObt_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbGuardar
            // 
            this.tsbGuardar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbGuardar.Image = ((System.Drawing.Image)(resources.GetObject("tsbGuardar.Image")));
            this.tsbGuardar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGuardar.Name = "tsbGuardar";
            this.tsbGuardar.Size = new System.Drawing.Size(23, 22);
            this.tsbGuardar.Text = "Guardar";
            this.tsbGuardar.Click += new System.EventHandler(this.tsbGuardar_Click);
            // 
            // grpCoordenadas
            // 
            this.grpCoordenadas.Controls.Add(this.btnCadDibujar);
            this.grpCoordenadas.Controls.Add(this.txtX);
            this.grpCoordenadas.Controls.Add(this.lblX);
            this.grpCoordenadas.Controls.Add(this.lblY);
            this.grpCoordenadas.Controls.Add(this.txtY);
            this.grpCoordenadas.Location = new System.Drawing.Point(0, 131);
            this.grpCoordenadas.Name = "grpCoordenadas";
            this.grpCoordenadas.Size = new System.Drawing.Size(512, 77);
            this.grpCoordenadas.TabIndex = 89;
            this.grpCoordenadas.TabStop = false;
            this.grpCoordenadas.Text = "Coordenadas de Ubicación UTM (Zona 19 - Hemisferio Norte)";
            // 
            // btnCadDibujar
            // 
            this.btnCadDibujar.Location = new System.Drawing.Point(418, 19);
            this.btnCadDibujar.Name = "btnCadDibujar";
            this.btnCadDibujar.Size = new System.Drawing.Size(86, 50);
            this.btnCadDibujar.TabIndex = 12;
            this.btnCadDibujar.Text = "Dibujar en CAD";
            this.btnCadDibujar.UseVisualStyleBackColor = true;
            this.btnCadDibujar.Click += new System.EventHandler(this.btnDibujarCAD_Click);
            // 
            // txtX
            // 
            this.txtX.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtX.Location = new System.Drawing.Point(7, 50);
            this.txtX.Name = "txtX";
            this.txtX.ReadOnly = true;
            this.txtX.Size = new System.Drawing.Size(70, 13);
            this.txtX.TabIndex = 9;
            this.txtX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(11, 33);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(38, 13);
            this.lblX.TabIndex = 2;
            this.lblX.Text = "Este X";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(105, 32);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(43, 13);
            this.lblY.TabIndex = 2;
            this.lblY.Text = "Norte Y";
            // 
            // txtY
            // 
            this.txtY.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtY.Location = new System.Drawing.Point(98, 49);
            this.txtY.Name = "txtY";
            this.txtY.ReadOnly = true;
            this.txtY.Size = new System.Drawing.Size(70, 13);
            this.txtY.TabIndex = 9;
            this.txtY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEscala
            // 
            this.lblEscala.AutoSize = true;
            this.lblEscala.Location = new System.Drawing.Point(173, 76);
            this.lblEscala.Name = "lblEscala";
            this.lblEscala.Size = new System.Drawing.Size(39, 13);
            this.lblEscala.TabIndex = 103;
            this.lblEscala.Text = "Escala";
            // 
            // txtEscala
            // 
            this.txtEscala.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEscala.Location = new System.Drawing.Point(166, 93);
            this.txtEscala.Name = "txtEscala";
            this.txtEscala.Size = new System.Drawing.Size(63, 13);
            this.txtEscala.TabIndex = 102;
            this.txtEscala.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblParroquia
            // 
            this.lblParroquia.AutoSize = true;
            this.lblParroquia.Location = new System.Drawing.Point(4, 29);
            this.lblParroquia.Name = "lblParroquia";
            this.lblParroquia.Size = new System.Drawing.Size(54, 13);
            this.lblParroquia.TabIndex = 90;
            this.lblParroquia.Text = "Parroquía";
            // 
            // txtParroquia
            // 
            this.txtParroquia.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtParroquia.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtParroquia.Location = new System.Drawing.Point(4, 45);
            this.txtParroquia.Name = "txtParroquia";
            this.txtParroquia.ReadOnly = true;
            this.txtParroquia.Size = new System.Drawing.Size(130, 13);
            this.txtParroquia.TabIndex = 91;
            // 
            // txtMunicipio
            // 
            this.txtMunicipio.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtMunicipio.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMunicipio.Location = new System.Drawing.Point(140, 45);
            this.txtMunicipio.Name = "txtMunicipio";
            this.txtMunicipio.ReadOnly = true;
            this.txtMunicipio.Size = new System.Drawing.Size(130, 13);
            this.txtMunicipio.TabIndex = 93;
            // 
            // lblMunicipio
            // 
            this.lblMunicipio.AutoSize = true;
            this.lblMunicipio.Location = new System.Drawing.Point(149, 29);
            this.lblMunicipio.Name = "lblMunicipio";
            this.lblMunicipio.Size = new System.Drawing.Size(52, 13);
            this.lblMunicipio.TabIndex = 92;
            this.lblMunicipio.Text = "Municipio";
            // 
            // txtUsuario
            // 
            this.txtUsuario.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsuario.Location = new System.Drawing.Point(392, 45);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(120, 13);
            this.txtUsuario.TabIndex = 96;
            this.txtUsuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPc
            // 
            this.lblPc.AutoSize = true;
            this.lblPc.Location = new System.Drawing.Point(415, 29);
            this.lblPc.Name = "lblPc";
            this.lblPc.Size = new System.Drawing.Size(60, 13);
            this.lblPc.TabIndex = 97;
            this.lblPc.Text = "Usuario PC";
            // 
            // lblFRegitro
            // 
            this.lblFRegitro.AutoSize = true;
            this.lblFRegitro.Location = new System.Drawing.Point(283, 29);
            this.lblFRegitro.Name = "lblFRegitro";
            this.lblFRegitro.Size = new System.Drawing.Size(103, 13);
            this.lblFRegitro.TabIndex = 94;
            this.lblFRegitro.Text = "Fecha Actualización";
            // 
            // txtArea
            // 
            this.txtArea.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtArea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtArea.Location = new System.Drawing.Point(97, 93);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(63, 13);
            this.txtArea.TabIndex = 98;
            this.txtArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtArea.TextChanged += new System.EventHandler(this.txtArea_TextChanged);
            // 
            // lblArea
            // 
            this.lblArea.AutoSize = true;
            this.lblArea.Location = new System.Drawing.Point(103, 77);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(46, 13);
            this.lblArea.TabIndex = 99;
            this.lblArea.Text = "Área m2";
            // 
            // lblClientes
            // 
            this.lblClientes.AutoSize = true;
            this.lblClientes.Location = new System.Drawing.Point(12, 77);
            this.lblClientes.Name = "lblClientes";
            this.lblClientes.Size = new System.Drawing.Size(44, 13);
            this.lblClientes.TabIndex = 100;
            this.lblClientes.Text = "Clientes";
            // 
            // txtClientes
            // 
            this.txtClientes.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtClientes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtClientes.Location = new System.Drawing.Point(12, 93);
            this.txtClientes.Name = "txtClientes";
            this.txtClientes.ReadOnly = true;
            this.txtClientes.Size = new System.Drawing.Size(79, 13);
            this.txtClientes.TabIndex = 101;
            this.txtClientes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblObs});
            this.statusStrip1.Location = new System.Drawing.Point(0, 217);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(518, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 102;
            // 
            // lblObs
            // 
            this.lblObs.Name = "lblObs";
            this.lblObs.Size = new System.Drawing.Size(72, 20);
            this.lblObs.Text = "Notificación";
            // 
            // txtFecAct
            // 
            this.txtFecAct.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtFecAct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFecAct.Location = new System.Drawing.Point(276, 45);
            this.txtFecAct.Name = "txtFecAct";
            this.txtFecAct.ReadOnly = true;
            this.txtFecAct.Size = new System.Drawing.Size(110, 13);
            this.txtFecAct.TabIndex = 104;
            this.txtFecAct.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmLocalidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 239);
            this.Controls.Add(this.txtFecAct);
            this.Controls.Add(this.lblEscala);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtEscala);
            this.Controls.Add(this.txtClientes);
            this.Controls.Add(this.lblClientes);
            this.Controls.Add(this.txtArea);
            this.Controls.Add(this.lblArea);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblPc);
            this.Controls.Add(this.lblFRegitro);
            this.Controls.Add(this.txtMunicipio);
            this.Controls.Add(this.lblMunicipio);
            this.Controls.Add(this.txtParroquia);
            this.Controls.Add(this.lblParroquia);
            this.Controls.Add(this.grpCoordenadas);
            this.Controls.Add(this.ToolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLocalidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Datos de la Localidad";
            this.Load += new System.EventHandler(this.frmLocalidad_Load);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.grpCoordenadas.ResumeLayout(false);
            this.grpCoordenadas.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStrip ToolStrip1;
        private System.Windows.Forms.ToolStripLabel lblId;
        private System.Windows.Forms.ToolStripLabel lblNombre;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbGuardar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbVerMap;
        internal System.Windows.Forms.ToolStripButton tsbCadMostrar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.GroupBox grpCoordenadas;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblParroquia;
        private System.Windows.Forms.Label lblMunicipio;
        internal System.Windows.Forms.Label lblPc;
        internal System.Windows.Forms.Label lblFRegitro;
        private System.Windows.Forms.ToolStripButton tsbXYObt;
        internal System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.Button btnCadDibujar;
        private System.Windows.Forms.Label lblClientes;
        private System.Windows.Forms.ToolStripButton chkOrden;
        internal System.Windows.Forms.Label lblEscala;
        private System.Windows.Forms.TextBox txtEscala;
        private System.Windows.Forms.ToolStripTextBox txtNombre;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtParroquia;
        private System.Windows.Forms.TextBox txtMunicipio;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtArea;
        private System.Windows.Forms.TextBox txtClientes;
        private System.Windows.Forms.ToolStripTextBox txtId;
        private System.Windows.Forms.ToolStripLabel lblObs;
        private System.Windows.Forms.ToolStripButton tsbAreaObt;
        private System.Windows.Forms.ToolStripButton tsbAtrObt;
        private System.Windows.Forms.TextBox txtFecAct;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}