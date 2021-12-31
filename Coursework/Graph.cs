using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Coursework
{
    public partial class Graph : Form
    {
        public Graph()
        {
            InitializeComponent();
        }

        public void LoadGraph(List<WeeklyReport> weeklyReports)
        {
            if (weeklyReports != null)
            {
                BarChart.DataSource = weeklyReports;
                BarChart.Series["visitors"].ChartType = SeriesChartType.Column;

                BarChart.Series["visitors"].XValueMember = "day";
                BarChart.Series["visitors"].YValueMembers = "visitors";

                BarChart.Titles.Add("No of visitors per Day in the selected Week");
            }
            else
            {
                MessageBox.Show("List is empty");
            }
        }
    }
}
