namespace program.View
{
    partial class frmRegister
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLuongCB;
        private System.Windows.Forms.TextBox txtLuongCB;


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
            components = new System.ComponentModel.Container();
            lblFullName = new Label();
            lblEmail = new Label();
            lblUser = new Label();
            lblPass = new Label();
            txtFullName = new TextBox();
            txtEmail = new TextBox();
            txtUser = new TextBox();
            txtPass = new TextBox();
            btnRegister = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(16, 16);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(60, 20);
            lblFullName.TabIndex = 0;
            lblFullName.Text = "Họ và tên";
            // 
            // lblMaNV
            // 
            lblMaNV = new Label();
            lblMaNV.AutoSize = true;
            lblMaNV.Location = new Point(16, 56);
            lblMaNV.Name = "lblMaNV";
            lblMaNV.Size = new Size(56, 20);
            lblMaNV.TabIndex = 1;
            lblMaNV.Text = "Mã NV";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(16, 96);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(42, 20);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(16, 136);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(80, 20);
            lblUser.TabIndex = 2;
            lblUser.Text = "Tài khoản";
            // 
            // lblPass
            // 
            lblPass.AutoSize = true;
            lblPass.Location = new Point(16, 176);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(66, 20);
            lblPass.TabIndex = 3;
            lblPass.Text = "Mật khẩu";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(120, 13);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(240, 27);
            txtFullName.TabIndex = 4;
            // 
            // txtMaNV
            // 
            txtMaNV = new TextBox();
            txtMaNV.Location = new Point(120, 53);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.Size = new Size(240, 27);
            txtMaNV.TabIndex = 5;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(120, 93);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(240, 27);
            txtEmail.TabIndex = 6;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(120, 133);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(240, 27);
            txtUser.TabIndex = 7;
            // 
            // txtPass
            // 
            txtPass.Location = new Point(120, 173);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(240, 27);
            txtPass.TabIndex = 8;
            txtPass.UseSystemPasswordChar = true;
            // 
            // lblLuongCB
            // 
            lblLuongCB = new Label();
            lblLuongCB.AutoSize = true;
            lblLuongCB.Location = new Point(16, 213);
            lblLuongCB.Name = "lblLuongCB";
            lblLuongCB.Size = new Size(72, 20);
            lblLuongCB.TabIndex = 8;
            lblLuongCB.Text = "Lương CB";
            // 
            // txtLuongCB
            // 
            txtLuongCB = new TextBox();
            txtLuongCB.Location = new Point(120, 210);
            txtLuongCB.Name = "txtLuongCB";
            txtLuongCB.Size = new Size(240, 27);
            txtLuongCB.TabIndex = 9;
            // 
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(120, 293);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(110, 36);
            btnRegister.TabIndex = 8;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(250, 293);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(110, 36);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmRegister
            // 
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 360);
            Controls.Add(btnCancel);
            Controls.Add(btnRegister);
            Controls.Add(txtLuongCB);
            Controls.Add(lblLuongCB);
            Controls.Add(txtPass);
            Controls.Add(txtUser);
            Controls.Add(txtMaNV);
            Controls.Add(txtEmail);
            Controls.Add(txtFullName);
            Controls.Add(lblPass);
            Controls.Add(lblUser);
            Controls.Add(lblMaNV);
            Controls.Add(lblEmail);
            Controls.Add(lblFullName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmRegister";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tạo tài khoản";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}