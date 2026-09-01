namespace Presentation
{
    partial class frmLoginEditar
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblCI = new System.Windows.Forms.Label();
            this.txtCI = new System.Windows.Forms.MaskedTextBox();
            this.txtCargo = new System.Windows.Forms.TextBox();
            this.lblCargo = new System.Windows.Forms.Label();
            this.txtTelef = new System.Windows.Forms.MaskedTextBox();
            this.lblTelef = new System.Windows.Forms.Label();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.lblMail = new System.Windows.Forms.Label();
            this.cbxDpto = new System.Windows.Forms.ComboBox();
            this.lblDpto = new System.Windows.Forms.Label();
            this.cbxGrupo = new System.Windows.Forms.ComboBox();
            this.lblGrupo = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cbxEstado = new System.Windows.Forms.ComboBox();
            this.grpObs = new System.Windows.Forms.GroupBox();
            this.txtObs = new System.Windows.Forms.RichTextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblId = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.grpObs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.BackColor = System.Drawing.Color.Black;
            this.lblTitulo.Font = new System.Drawing.Font("Century Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.DimGray;
            this.lblTitulo.Location = new System.Drawing.Point(112, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(290, 25);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Datos del Perfil de Usuario";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.Silver;
            this.lblUsuario.Location = new System.Drawing.Point(166, 57);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(54, 17);
            this.lblUsuario.TabIndex = 2;
            this.lblUsuario.Text = "Usuario";
            // 
            // txtUser
            // 
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUser.Location = new System.Drawing.Point(165, 81);
            this.txtUser.Name = "txtUser";
            this.txtUser.ReadOnly = true;
            this.txtUser.Size = new System.Drawing.Size(103, 23);
            this.txtUser.TabIndex = 1;
            // 
            // txtNombre
            // 
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(274, 81);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(209, 23);
            this.txtNombre.TabIndex = 1;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.BackColor = System.Drawing.Color.Transparent;
            this.lblNombre.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.ForeColor = System.Drawing.Color.Silver;
            this.lblNombre.Location = new System.Drawing.Point(273, 57);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(128, 17);
            this.lblNombre.TabIndex = 4;
            this.lblNombre.Text = "Nombre y Apellido";
            // 
            // lblCI
            // 
            this.lblCI.AutoSize = true;
            this.lblCI.BackColor = System.Drawing.Color.Transparent;
            this.lblCI.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.lblCI.ForeColor = System.Drawing.Color.Silver;
            this.lblCI.Location = new System.Drawing.Point(14, 137);
            this.lblCI.Name = "lblCI";
            this.lblCI.Size = new System.Drawing.Size(60, 17);
            this.lblCI.TabIndex = 6;
            this.lblCI.Text = "Cédula:";
            // 
            // txtCI
            // 
            this.txtCI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCI.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.txtCI.Location = new System.Drawing.Point(14, 156);
            this.txtCI.Mask = "99.999.999";
            this.txtCI.Name = "txtCI";
            this.txtCI.ReadOnly = true;
            this.txtCI.Size = new System.Drawing.Size(59, 23);
            this.txtCI.TabIndex = 2;
            this.txtCI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCI.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePrompt;
            // 
            // txtCargo
            // 
            this.txtCargo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCargo.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.txtCargo.Location = new System.Drawing.Point(15, 255);
            this.txtCargo.Name = "txtCargo";
            this.txtCargo.ReadOnly = true;
            this.txtCargo.Size = new System.Drawing.Size(217, 23);
            this.txtCargo.TabIndex = 5;
            // 
            // lblCargo
            // 
            this.lblCargo.AutoSize = true;
            this.lblCargo.BackColor = System.Drawing.Color.Transparent;
            this.lblCargo.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.lblCargo.ForeColor = System.Drawing.Color.Silver;
            this.lblCargo.Location = new System.Drawing.Point(14, 236);
            this.lblCargo.Name = "lblCargo";
            this.lblCargo.Size = new System.Drawing.Size(172, 17);
            this.lblCargo.TabIndex = 8;
            this.lblCargo.Text = "Cargo y Función Laborar:";
            // 
            // txtTelef
            // 
            this.txtTelef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelef.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.txtTelef.Location = new System.Drawing.Point(79, 156);
            this.txtTelef.Mask = "0000-0000000/0000-0000000";
            this.txtTelef.Name = "txtTelef";
            this.txtTelef.ReadOnly = true;
            this.txtTelef.Size = new System.Drawing.Size(153, 23);
            this.txtTelef.TabIndex = 3;
            this.txtTelef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTelef.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lblTelef
            // 
            this.lblTelef.AutoSize = true;
            this.lblTelef.BackColor = System.Drawing.Color.Transparent;
            this.lblTelef.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.lblTelef.ForeColor = System.Drawing.Color.Silver;
            this.lblTelef.Location = new System.Drawing.Point(77, 138);
            this.lblTelef.Name = "lblTelef";
            this.lblTelef.Size = new System.Drawing.Size(67, 17);
            this.lblTelef.TabIndex = 11;
            this.lblTelef.Text = "Teléfonos";
            // 
            // txtMail
            // 
            this.txtMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMail.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.txtMail.Location = new System.Drawing.Point(16, 203);
            this.txtMail.Name = "txtMail";
            this.txtMail.ReadOnly = true;
            this.txtMail.Size = new System.Drawing.Size(216, 23);
            this.txtMail.TabIndex = 4;
            this.txtMail.Text = "@corpoelec.gob.ve";
            this.txtMail.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblMail
            // 
            this.lblMail.AutoSize = true;
            this.lblMail.BackColor = System.Drawing.Color.Transparent;
            this.lblMail.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.lblMail.ForeColor = System.Drawing.Color.Silver;
            this.lblMail.Location = new System.Drawing.Point(14, 184);
            this.lblMail.Name = "lblMail";
            this.lblMail.Size = new System.Drawing.Size(53, 17);
            this.lblMail.TabIndex = 13;
            this.lblMail.Text = "Correo";
            // 
            // cbxDpto
            // 
            this.cbxDpto.Enabled = false;
            this.cbxDpto.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.cbxDpto.Location = new System.Drawing.Point(266, 156);
            this.cbxDpto.Name = "cbxDpto";
            this.cbxDpto.Size = new System.Drawing.Size(217, 25);
            this.cbxDpto.TabIndex = 6;
            this.cbxDpto.SelectedIndexChanged += new System.EventHandler(this.cbxDpto_SelectedIndexChanged);
            // 
            // lblDpto
            // 
            this.lblDpto.AutoSize = true;
            this.lblDpto.BackColor = System.Drawing.Color.Transparent;
            this.lblDpto.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.lblDpto.ForeColor = System.Drawing.Color.Silver;
            this.lblDpto.Location = new System.Drawing.Point(263, 137);
            this.lblDpto.Name = "lblDpto";
            this.lblDpto.Size = new System.Drawing.Size(105, 17);
            this.lblDpto.TabIndex = 22;
            this.lblDpto.Text = "Departamento";
            // 
            // cbxGrupo
            // 
            this.cbxGrupo.Enabled = false;
            this.cbxGrupo.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.cbxGrupo.Location = new System.Drawing.Point(266, 203);
            this.cbxGrupo.Name = "cbxGrupo";
            this.cbxGrupo.Size = new System.Drawing.Size(217, 25);
            this.cbxGrupo.TabIndex = 7;
            // 
            // lblGrupo
            // 
            this.lblGrupo.AutoSize = true;
            this.lblGrupo.BackColor = System.Drawing.Color.Black;
            this.lblGrupo.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.lblGrupo.ForeColor = System.Drawing.Color.Silver;
            this.lblGrupo.Location = new System.Drawing.Point(263, 184);
            this.lblGrupo.Name = "lblGrupo";
            this.lblGrupo.Size = new System.Drawing.Size(122, 17);
            this.lblGrupo.TabIndex = 24;
            this.lblGrupo.Text = "Grupo de Trabajo";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.BackColor = System.Drawing.Color.Black;
            this.lblEstado.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.lblEstado.ForeColor = System.Drawing.Color.Silver;
            this.lblEstado.Location = new System.Drawing.Point(263, 236);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(52, 17);
            this.lblEstado.TabIndex = 25;
            this.lblEstado.Text = "Estado";
            // 
            // cbxEstado
            // 
            this.cbxEstado.Enabled = false;
            this.cbxEstado.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.cbxEstado.Location = new System.Drawing.Point(266, 255);
            this.cbxEstado.Name = "cbxEstado";
            this.cbxEstado.Size = new System.Drawing.Size(217, 25);
            this.cbxEstado.TabIndex = 8;
            // 
            // grpObs
            // 
            this.grpObs.Controls.Add(this.txtObs);
            this.grpObs.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.grpObs.ForeColor = System.Drawing.Color.Silver;
            this.grpObs.Location = new System.Drawing.Point(14, 282);
            this.grpObs.Name = "grpObs";
            this.grpObs.Size = new System.Drawing.Size(469, 79);
            this.grpObs.TabIndex = 30;
            this.grpObs.TabStop = false;
            this.grpObs.Text = "Observación / Requerimientos";
            // 
            // txtObs
            // 
            this.txtObs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtObs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObs.Location = new System.Drawing.Point(3, 19);
            this.txtObs.MaxLength = 100;
            this.txtObs.Name = "txtObs";
            this.txtObs.ReadOnly = true;
            this.txtObs.Size = new System.Drawing.Size(463, 57);
            this.txtObs.TabIndex = 0;
            this.txtObs.Text = "";
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardar.Location = new System.Drawing.Point(289, 375);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(155, 37);
            this.btnGuardar.TabIndex = 9;
            this.btnGuardar.Text = "Editar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btnCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(51, 375);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(155, 37);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtId
            // 
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtId.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(116, 81);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(43, 23);
            this.txtId.TabIndex = 33;
            this.txtId.TextChanged += new System.EventHandler(this.txtId_TextChanged);
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.BackColor = System.Drawing.Color.Transparent;
            this.lblId.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblId.ForeColor = System.Drawing.Color.Silver;
            this.lblId.Location = new System.Drawing.Point(113, 57);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(23, 17);
            this.lblId.TabIndex = 32;
            this.lblId.Text = "N°";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Presentation.Properties.Resources.UsuarioEditar;
            this.pictureBox2.Location = new System.Drawing.Point(20, 46);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(53, 49);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // frmLoginEditar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(503, 427);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.cbxEstado);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblCI);
            this.Controls.Add(this.lblGrupo);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.cbxGrupo);
            this.Controls.Add(this.lblCargo);
            this.Controls.Add(this.lblDpto);
            this.Controls.Add(this.cbxDpto);
            this.Controls.Add(this.txtCI);
            this.Controls.Add(this.txtCargo);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.grpObs);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.txtTelef);
            this.Controls.Add(this.txtMail);
            this.Controls.Add(this.lblMail);
            this.Controls.Add(this.lblTelef);
            this.Name = "frmLoginEditar";
            this.Text = "Editar Perfil de Usuario";
            this.TransparencyKey = System.Drawing.Color.White;
            this.Load += new System.EventHandler(this.FormUserProfile_Load);
            this.grpObs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblCI;
        private System.Windows.Forms.MaskedTextBox txtCI;
        private System.Windows.Forms.TextBox txtCargo;
        private System.Windows.Forms.Label lblCargo;
        private System.Windows.Forms.MaskedTextBox txtTelef;
        private System.Windows.Forms.Label lblTelef;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Label lblMail;
        private System.Windows.Forms.ComboBox cbxDpto;
        private System.Windows.Forms.Label lblDpto;
        private System.Windows.Forms.ComboBox cbxGrupo;
        private System.Windows.Forms.Label lblGrupo;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cbxEstado;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox grpObs;
        private System.Windows.Forms.RichTextBox txtObs;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Button btnCancelar;
    }
}