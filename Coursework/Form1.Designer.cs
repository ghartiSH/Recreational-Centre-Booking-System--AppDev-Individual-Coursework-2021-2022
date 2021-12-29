
namespace Coursework
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Home = new System.Windows.Forms.TabPage();
            this.VisitorsList = new System.Windows.Forms.TabPage();
            this.CheckoutVisitor = new System.Windows.Forms.TabPage();
            this.DailyReport = new System.Windows.Forms.TabPage();
            this.WeeklyReport = new System.Windows.Forms.TabPage();
            this.AdminPanel = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Home);
            this.tabControl1.Controls.Add(this.VisitorsList);
            this.tabControl1.Controls.Add(this.CheckoutVisitor);
            this.tabControl1.Controls.Add(this.DailyReport);
            this.tabControl1.Controls.Add(this.WeeklyReport);
            this.tabControl1.Controls.Add(this.AdminPanel);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(986, 550);
            this.tabControl1.TabIndex = 0;
            // 
            // Home
            // 
            this.Home.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Home.Location = new System.Drawing.Point(4, 22);
            this.Home.Name = "Home";
            this.Home.Padding = new System.Windows.Forms.Padding(3);
            this.Home.Size = new System.Drawing.Size(978, 524);
            this.Home.TabIndex = 0;
            this.Home.Text = "Home";
            this.Home.UseVisualStyleBackColor = true;
            // 
            // VisitorsList
            // 
            this.VisitorsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VisitorsList.Location = new System.Drawing.Point(4, 22);
            this.VisitorsList.Name = "VisitorsList";
            this.VisitorsList.Padding = new System.Windows.Forms.Padding(3);
            this.VisitorsList.Size = new System.Drawing.Size(978, 524);
            this.VisitorsList.TabIndex = 1;
            this.VisitorsList.Text = "Visitors List";
            this.VisitorsList.UseVisualStyleBackColor = true;
            // 
            // CheckoutVisitor
            // 
            this.CheckoutVisitor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CheckoutVisitor.Location = new System.Drawing.Point(4, 22);
            this.CheckoutVisitor.Name = "CheckoutVisitor";
            this.CheckoutVisitor.Size = new System.Drawing.Size(978, 524);
            this.CheckoutVisitor.TabIndex = 2;
            this.CheckoutVisitor.Text = "Checkout Visitor";
            this.CheckoutVisitor.UseVisualStyleBackColor = true;
            // 
            // DailyReport
            // 
            this.DailyReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DailyReport.Location = new System.Drawing.Point(4, 22);
            this.DailyReport.Name = "DailyReport";
            this.DailyReport.Size = new System.Drawing.Size(978, 524);
            this.DailyReport.TabIndex = 3;
            this.DailyReport.Text = "Daily Report";
            this.DailyReport.UseVisualStyleBackColor = true;
            // 
            // WeeklyReport
            // 
            this.WeeklyReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WeeklyReport.Location = new System.Drawing.Point(4, 22);
            this.WeeklyReport.Name = "WeeklyReport";
            this.WeeklyReport.Size = new System.Drawing.Size(978, 524);
            this.WeeklyReport.TabIndex = 4;
            this.WeeklyReport.Text = "Weekly Report";
            this.WeeklyReport.UseVisualStyleBackColor = true;
            // 
            // AdminPanel
            // 
            this.AdminPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AdminPanel.Location = new System.Drawing.Point(4, 22);
            this.AdminPanel.Name = "AdminPanel";
            this.AdminPanel.Size = new System.Drawing.Size(978, 524);
            this.AdminPanel.TabIndex = 5;
            this.AdminPanel.Text = "Admin Panel";
            this.AdminPanel.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 549);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Home;
        private System.Windows.Forms.TabPage VisitorsList;
        private System.Windows.Forms.TabPage CheckoutVisitor;
        private System.Windows.Forms.TabPage DailyReport;
        private System.Windows.Forms.TabPage WeeklyReport;
        private System.Windows.Forms.TabPage AdminPanel;
    }
}

