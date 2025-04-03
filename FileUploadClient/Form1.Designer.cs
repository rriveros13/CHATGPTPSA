namespace FileUploadClient
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTranferidos = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblUltimaTrans = new System.Windows.Forms.Label();
            this.lblUltimaTransfDetail = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.chElimTranExito = new System.Windows.Forms.CheckBox();
            this.chElimDuplicados = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTransfiriendoDetalle = new System.Windows.Forms.Label();
            this.lblTransfiriendo = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aplicaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transferirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blUsuario = new System.Windows.Forms.Label();
            this.lblContraseña = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtContraseña = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(144, 107);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(369, 22);
            this.txtPath.TabIndex = 8;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(656, 40);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(113, 84);
            this.btnUpload.TabIndex = 6;
            this.btnUpload.Text = "TRANSFERIR";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(519, 107);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(30, 23);
            this.btnSelectFolder.TabIndex = 9;
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(144, 145);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(369, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "CARPETA:";
            // 
            // lblTranferidos
            // 
            this.lblTranferidos.Location = new System.Drawing.Point(144, 209);
            this.lblTranferidos.Name = "lblTranferidos";
            this.lblTranferidos.Size = new System.Drawing.Size(369, 67);
            this.lblTranferidos.TabIndex = 5;
            this.lblTranferidos.Text = "Transferidos";
            this.lblTranferidos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Enabled = false;
            this.btnCancelar.Location = new System.Drawing.Point(656, 145);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(113, 40);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblUltimaTrans
            // 
            this.lblUltimaTrans.AutoSize = true;
            this.lblUltimaTrans.Location = new System.Drawing.Point(76, 290);
            this.lblUltimaTrans.Name = "lblUltimaTrans";
            this.lblUltimaTrans.Size = new System.Drawing.Size(146, 17);
            this.lblUltimaTrans.TabIndex = 7;
            this.lblUltimaTrans.Text = "ESTADO ULT. TRAN.";
            // 
            // lblUltimaTransfDetail
            // 
            this.lblUltimaTransfDetail.AutoSize = true;
            this.lblUltimaTransfDetail.Location = new System.Drawing.Point(225, 290);
            this.lblUltimaTransfDetail.Name = "lblUltimaTransfDetail";
            this.lblUltimaTransfDetail.Size = new System.Drawing.Size(13, 17);
            this.lblUltimaTransfDetail.TabIndex = 8;
            this.lblUltimaTransfDetail.Text = "-";
            // 
            // btnSalir
            // 
            this.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSalir.Enabled = false;
            this.btnSalir.Location = new System.Drawing.Point(656, 212);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(113, 40);
            this.btnSalir.TabIndex = 913;
            this.btnSalir.Text = "SALIR";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // chElimTranExito
            // 
            this.chElimTranExito.AutoSize = true;
            this.chElimTranExito.Checked = true;
            this.chElimTranExito.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chElimTranExito.Enabled = false;
            this.chElimTranExito.Location = new System.Drawing.Point(396, 70);
            this.chElimTranExito.Name = "chElimTranExito";
            this.chElimTranExito.Size = new System.Drawing.Size(228, 21);
            this.chElimTranExito.TabIndex = 5;
            this.chElimTranExito.Text = "Elim. Archivos Transf. con Éxito";
            this.chElimTranExito.UseVisualStyleBackColor = true;
            // 
            // chElimDuplicados
            // 
            this.chElimDuplicados.AutoSize = true;
            this.chElimDuplicados.Enabled = false;
            this.chElimDuplicados.Location = new System.Drawing.Point(396, 39);
            this.chElimDuplicados.Name = "chElimDuplicados";
            this.chElimDuplicados.Size = new System.Drawing.Size(192, 21);
            this.chElimDuplicados.TabIndex = 4;
            this.chElimDuplicados.Text = "Elim. Archivos Duplicados";
            this.chElimDuplicados.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "PROGRESO:";
            // 
            // lblTransfiriendoDetalle
            // 
            this.lblTransfiriendoDetalle.AutoSize = true;
            this.lblTransfiriendoDetalle.Location = new System.Drawing.Point(144, 189);
            this.lblTransfiriendoDetalle.Name = "lblTransfiriendoDetalle";
            this.lblTransfiriendoDetalle.Size = new System.Drawing.Size(13, 17);
            this.lblTransfiriendoDetalle.TabIndex = 13;
            this.lblTransfiriendoDetalle.Text = "-";
            // 
            // lblTransfiriendo
            // 
            this.lblTransfiriendo.AutoSize = true;
            this.lblTransfiriendo.Location = new System.Drawing.Point(15, 189);
            this.lblTransfiriendo.Name = "lblTransfiriendo";
            this.lblTransfiriendo.Size = new System.Drawing.Size(123, 17);
            this.lblTransfiriendo.TabIndex = 12;
            this.lblTransfiriendo.Text = "TRANSFIRIENDO:";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "ITTI Transferencia Minimizado";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ITTI Transferencia de Archivos";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aplicaciónToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(815, 28);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aplicaciónToolStripMenuItem
            // 
            this.aplicaciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transferirToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.aplicaciónToolStripMenuItem.Name = "aplicaciónToolStripMenuItem";
            this.aplicaciónToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.aplicaciónToolStripMenuItem.Text = "Aplicación";
            // 
            // transferirToolStripMenuItem
            // 
            this.transferirToolStripMenuItem.Name = "transferirToolStripMenuItem";
            this.transferirToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.transferirToolStripMenuItem.Text = "Transferir";
            this.transferirToolStripMenuItem.Click += new System.EventHandler(this.transferirToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // blUsuario
            // 
            this.blUsuario.AutoSize = true;
            this.blUsuario.Location = new System.Drawing.Point(62, 43);
            this.blUsuario.Name = "blUsuario";
            this.blUsuario.Size = new System.Drawing.Size(61, 17);
            this.blUsuario.TabIndex = 0;
            this.blUsuario.Text = "Usuario:";
            // 
            // lblContraseña
            // 
            this.lblContraseña.AutoSize = true;
            this.lblContraseña.Location = new System.Drawing.Point(38, 70);
            this.lblContraseña.Name = "lblContraseña";
            this.lblContraseña.Size = new System.Drawing.Size(85, 17);
            this.lblContraseña.TabIndex = 2;
            this.lblContraseña.Text = "Contraseña:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(144, 37);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(221, 22);
            this.txtUsuario.TabIndex = 1;
            // 
            // txtContraseña
            // 
            this.txtContraseña.Location = new System.Drawing.Point(144, 65);
            this.txtContraseña.Name = "txtContraseña";
            this.txtContraseña.PasswordChar = '*';
            this.txtContraseña.Size = new System.Drawing.Size(221, 22);
            this.txtContraseña.TabIndex = 3;
            // 
            // Form1
            // 
            this.AcceptButton = this.btnUpload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(815, 405);
            this.Controls.Add(this.txtContraseña);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblContraseña);
            this.Controls.Add(this.blUsuario);
            this.Controls.Add(this.lblTransfiriendo);
            this.Controls.Add(this.lblTransfiriendoDetalle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chElimDuplicados);
            this.Controls.Add(this.chElimTranExito);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblUltimaTransfDetail);
            this.Controls.Add(this.lblUltimaTrans);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lblTranferidos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ITTI - Transferencia de Archivos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTranferidos;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblUltimaTrans;
        private System.Windows.Forms.Label lblUltimaTransfDetail;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.CheckBox chElimTranExito;
        private System.Windows.Forms.CheckBox chElimDuplicados;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTransfiriendoDetalle;
        private System.Windows.Forms.Label lblTransfiriendo;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aplicaciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transferirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.Label blUsuario;
        private System.Windows.Forms.Label lblContraseña;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtContraseña;
    }
}

