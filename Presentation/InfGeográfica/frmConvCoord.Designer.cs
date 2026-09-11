namespace Presentation.InfGeográfica
{
    partial class frmConvCoord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConvCoord));
            this.grpTipo = new System.Windows.Forms.GroupBox();
            this.rbnUtmDg = new System.Windows.Forms.RadioButton();
            this.rbnDgUtm = new System.Windows.Forms.RadioButton();
            this.lblDatos = new System.Windows.Forms.Label();
            this.txtDatos = new System.Windows.Forms.TextBox();
            this.lblResultados = new System.Windows.Forms.Label();
            this.txtResultados = new System.Windows.Forms.TextBox();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnCopiar = new System.Windows.Forms.Button();
            this.grpTipo.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTipo
            // 
            this.grpTipo.Controls.Add(this.rbnUtmDg);
            this.grpTipo.Controls.Add(this.rbnDgUtm);
            this.grpTipo.Location = new System.Drawing.Point(12, 12);
            this.grpTipo.Name = "grpTipo";
            this.grpTipo.Size = new System.Drawing.Size(122, 78);
            this.grpTipo.TabIndex = 0;
            this.grpTipo.TabStop = false;
            this.grpTipo.Text = "Tipo de Conversión";
            // 
            // rbnUtmDg
            // 
            this.rbnUtmDg.AutoSize = true;
            this.rbnUtmDg.Location = new System.Drawing.Point(17, 48);
            this.rbnUtmDg.Name = "rbnUtmDg";
            this.rbnUtmDg.Size = new System.Drawing.Size(86, 17);
            this.rbnUtmDg.TabIndex = 1;
            this.rbnUtmDg.Text = "UTM  - > DG";
            this.rbnUtmDg.UseVisualStyleBackColor = true;
            // 
            // rbnDgUtm
            // 
            this.rbnDgUtm.AutoSize = true;
            this.rbnDgUtm.Checked = true;
            this.rbnDgUtm.Location = new System.Drawing.Point(17, 25);
            this.rbnDgUtm.Name = "rbnDgUtm";
            this.rbnDgUtm.Size = new System.Drawing.Size(83, 17);
            this.rbnDgUtm.TabIndex = 0;
            this.rbnDgUtm.TabStop = true;
            this.rbnDgUtm.Text = "DG - > UTM";
            this.rbnDgUtm.UseVisualStyleBackColor = true;
            // 
            // lblDatos
            // 
            this.lblDatos.AutoSize = true;
            this.lblDatos.Location = new System.Drawing.Point(154, 26);
            this.lblDatos.Name = "lblDatos";
            this.lblDatos.Size = new System.Drawing.Size(157, 13);
            this.lblDatos.TabIndex = 1;
            this.lblDatos.Text = "Ingrese Datos de Coordenadas:";
            // 
            // txtDatos
            // 
            this.txtDatos.Location = new System.Drawing.Point(157, 42);
            this.txtDatos.Name = "txtDatos";
            this.txtDatos.Size = new System.Drawing.Size(325, 20);
            this.txtDatos.TabIndex = 2;
            // 
            // lblResultados
            // 
            this.lblResultados.AutoSize = true;
            this.lblResultados.Location = new System.Drawing.Point(154, 77);
            this.lblResultados.Name = "lblResultados";
            this.lblResultados.Size = new System.Drawing.Size(132, 13);
            this.lblResultados.TabIndex = 3;
            this.lblResultados.Text = "Coordenadas Convertidas:";
            // 
            // txtResultados
            // 
            this.txtResultados.BackColor = System.Drawing.SystemColors.Info;
            this.txtResultados.Location = new System.Drawing.Point(157, 103);
            this.txtResultados.Name = "txtResultados";
            this.txtResultados.Size = new System.Drawing.Size(325, 20);
            this.txtResultados.TabIndex = 4;
            // 
            // btnAplicar
            // 
            this.btnAplicar.Location = new System.Drawing.Point(51, 143);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(133, 42);
            this.btnAplicar.TabIndex = 5;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(202, 143);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(133, 42);
            this.btnCerrar.TabIndex = 6;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            // 
            // btnCopiar
            // 
            this.btnCopiar.Location = new System.Drawing.Point(341, 143);
            this.btnCopiar.Name = "btnCopiar";
            this.btnCopiar.Size = new System.Drawing.Size(133, 42);
            this.btnCopiar.TabIndex = 7;
            this.btnCopiar.Text = "Copiar";
            this.btnCopiar.UseVisualStyleBackColor = true;
            this.btnCopiar.Click += new System.EventHandler(this.btnCopiar_Click);
            // 
            // frmConvCoord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 217);
            this.Controls.Add(this.btnCopiar);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.txtResultados);
            this.Controls.Add(this.lblResultados);
            this.Controls.Add(this.txtDatos);
            this.Controls.Add(this.lblDatos);
            this.Controls.Add(this.grpTipo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConvCoord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conversor de Coordenadas";
            this.grpTipo.ResumeLayout(false);
            this.grpTipo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTipo;
        private System.Windows.Forms.RadioButton rbnUtmDg;
        private System.Windows.Forms.RadioButton rbnDgUtm;
        private System.Windows.Forms.Label lblDatos;
        private System.Windows.Forms.TextBox txtDatos;
        private System.Windows.Forms.Label lblResultados;
        private System.Windows.Forms.TextBox txtResultados;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnCopiar;
    }
}