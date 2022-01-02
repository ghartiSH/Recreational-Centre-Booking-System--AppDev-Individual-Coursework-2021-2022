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
    public partial class GraphbyIncome : Form
    {
 

        public GraphbyIncome()
        {
            InitializeComponent();
        }

        public void LoadGraph(List<WeeklyReport> weeklyReports)
        {
            if (weeklyReports != null)
            {
                PieChart.DataSource = weeklyReports;
                PieChart.Series["income"].ChartType = SeriesChartType.Line;

                PieChart.Series["income"].XValueMember = "day";
                PieChart.Series["income"].YValueMembers = "income";

                PieChart.Titles.Add("total income per Day in the selected Week");
            }
            else
            {
                MessageBox.Show("List is empty");
            }
        }

    }
}
