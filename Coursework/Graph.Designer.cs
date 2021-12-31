
namespace Coursework
{
    partial class Graph
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.BarChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.BarChart)).BeginInit();
            this.SuspendLayout();
            // 
            // BarChart
            // 
            chartArea1.Name = "ChartArea1";
            this.BarChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.BarChart.Legends.Add(legend1);
            this.BarChart.Location = new System.Drawing.Point(12, 12);
            this.BarChart.Name = "BarChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "visitors";
            this.BarChart.Series.Add(series1);
            this.BarChart.Size = new System.Drawing.Size(849, 420);
            this.BarChart.TabIndex = 0;
            this.BarChart.Text = "Chart";
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 460);
            this.Controls.Add(this.BarChart);
            this.Name = "Graph";
            this.Text = "Graph";
            ((System.ComponentModel.ISupportInitialize)(this.BarChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart BarChart;
    }
}