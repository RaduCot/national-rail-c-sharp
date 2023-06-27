namespace NationalRail
{
    partial class Login
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.jPasswordField1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.jLabel_status = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.jTextField1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.jPasswordField1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.jLabel_status, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.jTextField1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(382, 253);
            this.tableLayoutPanel1.TabIndex = 4;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // jPasswordField1
            // 
            this.jPasswordField1.Dock = System.Windows.Forms.DockStyle.Top;
            this.jPasswordField1.Location = new System.Drawing.Point(10, 193);
            this.jPasswordField1.Margin = new System.Windows.Forms.Padding(10);
            this.jPasswordField1.Name = "jPasswordField1";
            this.jPasswordField1.PasswordChar = '*';
            this.jPasswordField1.Size = new System.Drawing.Size(362, 22);
            this.jPasswordField1.TabIndex = 5;
            this.jPasswordField1.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Image = global::NationalRail.Properties.Resources.national_rail;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(376, 70);
            this.label1.TabIndex = 0;
            // 
            // jLabel_status
            // 
            this.jLabel_status.AutoSize = true;
            this.jLabel_status.Dock = System.Windows.Forms.DockStyle.Top;
            this.jLabel_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jLabel_status.Location = new System.Drawing.Point(10, 80);
            this.jLabel_status.Margin = new System.Windows.Forms.Padding(10);
            this.jLabel_status.Name = "jLabel_status";
            this.jLabel_status.Size = new System.Drawing.Size(362, 17);
            this.jLabel_status.TabIndex = 1;
            this.jLabel_status.Text = "- Autentificare -";
            this.jLabel_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 107);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nume de utilizator:";
            // 
            // jTextField1
            // 
            this.jTextField1.Dock = System.Windows.Forms.DockStyle.Top;
            this.jTextField1.Location = new System.Drawing.Point(10, 134);
            this.jTextField1.Margin = new System.Windows.Forms.Padding(10);
            this.jTextField1.Name = "jTextField1";
            this.jTextField1.Size = new System.Drawing.Size(362, 22);
            this.jTextField1.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 166);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Parola:";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(91, 225);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.MinimumSize = new System.Drawing.Size(200, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 27);
            this.button1.TabIndex = 6;
            this.button1.Text = "Conectare";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 253);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(600, 300);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label jLabel_status;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox jTextField1;
        private System.Windows.Forms.TextBox jPasswordField1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}

