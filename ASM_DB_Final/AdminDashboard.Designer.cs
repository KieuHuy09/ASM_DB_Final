namespace ASM_DB_Final
{
    partial class AdminDashboard
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
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnAuthorsManagement = new System.Windows.Forms.Button();
            this.btnProductManagement = new System.Windows.Forms.Button();
            this.btnCustomerManagement = new System.Windows.Forms.Button();
            this.btnWarehouseDashboard = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnOrder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(633, 381);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click_1);
            // 
            // btnAuthorsManagement
            // 
            this.btnAuthorsManagement.Location = new System.Drawing.Point(153, 12);
            this.btnAuthorsManagement.Name = "btnAuthorsManagement";
            this.btnAuthorsManagement.Size = new System.Drawing.Size(134, 74);
            this.btnAuthorsManagement.TabIndex = 8;
            this.btnAuthorsManagement.Text = "Authors Management";
            this.btnAuthorsManagement.UseVisualStyleBackColor = true;
            this.btnAuthorsManagement.Click += new System.EventHandler(this.btnAuthorsManagement_Click);
            // 
            // btnProductManagement
            // 
            this.btnProductManagement.Location = new System.Drawing.Point(293, 12);
            this.btnProductManagement.Name = "btnProductManagement";
            this.btnProductManagement.Size = new System.Drawing.Size(135, 74);
            this.btnProductManagement.TabIndex = 7;
            this.btnProductManagement.Text = "Product Management";
            this.btnProductManagement.UseVisualStyleBackColor = true;
            this.btnProductManagement.Click += new System.EventHandler(this.btnProductManagement_Click_1);
            // 
            // btnCustomerManagement
            // 
            this.btnCustomerManagement.Location = new System.Drawing.Point(12, 12);
            this.btnCustomerManagement.Name = "btnCustomerManagement";
            this.btnCustomerManagement.Size = new System.Drawing.Size(135, 74);
            this.btnCustomerManagement.TabIndex = 6;
            this.btnCustomerManagement.Text = "Customer Management";
            this.btnCustomerManagement.UseVisualStyleBackColor = true;
            this.btnCustomerManagement.Click += new System.EventHandler(this.btnCustomerManagement_Click);
            // 
            // btnWarehouseDashboard
            // 
            this.btnWarehouseDashboard.Location = new System.Drawing.Point(434, 12);
            this.btnWarehouseDashboard.Name = "btnWarehouseDashboard";
            this.btnWarehouseDashboard.Size = new System.Drawing.Size(144, 74);
            this.btnWarehouseDashboard.TabIndex = 10;
            this.btnWarehouseDashboard.Text = "WarehouseDashboard";
            this.btnWarehouseDashboard.UseVisualStyleBackColor = true;
            this.btnWarehouseDashboard.Click += new System.EventHandler(this.btnWarehouseDashboard_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.Location = new System.Drawing.Point(584, 12);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(144, 77);
            this.btnDashboard.TabIndex = 11;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // btnOrder
            // 
            this.btnOrder.Location = new System.Drawing.Point(12, 102);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(135, 76);
            this.btnOrder.TabIndex = 12;
            this.btnOrder.Text = "Order";
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // AdminDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 445);
            this.Controls.Add(this.btnOrder);
            this.Controls.Add(this.btnDashboard);
            this.Controls.Add(this.btnWarehouseDashboard);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnAuthorsManagement);
            this.Controls.Add(this.btnProductManagement);
            this.Controls.Add(this.btnCustomerManagement);
            this.Name = "AdminDashboard";
            this.Text = "AdminDashboard";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnAuthorsManagement;
        private System.Windows.Forms.Button btnProductManagement;
        private System.Windows.Forms.Button btnCustomerManagement;
        private System.Windows.Forms.Button btnWarehouseDashboard;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnOrder;
    }
}