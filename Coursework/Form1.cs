using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Coursework
{
    public partial class Form1 : Form

    {
        XmlSerializer xmlSerializer;
        List<Visitors> visitors;
        public Form1()
        {
            InitializeComponent();
            xmlSerializer = new XmlSerializer(typeof(List<Visitors>));
            visitors = new List<Visitors>();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Register_Click(object sender, EventArgs e)
        {
            Visitors vs = new Visitors();
            FileStream file = new FileStream("./visitors.xml", FileMode.Create, FileAccess.Write);
            vs.fullname = fullNameTxt.Text;
            vs.email = emailTxt.Text;
            vs.phone = phone.Text;
            vs.category = categoryCmb.Text;
            vs.inDateTime = DateTime.Parse(ddCmb.Text + "-" + mmCmb.Text + "-" + yyCmb.Text + " " + hhCmb.Text + ":" + minCmb.Text);
            vs.totalVisitors = int.Parse(totalVisitorsCmb.Text);
            vs.totalChildren = int.Parse(totalVisitorsCmb.Text);

            visitors.Add(vs);

            xmlSerializer.Serialize(file, visitors);
            file.Close();
        }

        private void clear()
        {
            fullNameTxt.Text = "";
            
        }

        private void ViewCurrentVisitors_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("./visitors.xml", FileMode.Open, FileAccess.Read);
            visitors = (List<Visitors>)xmlSerializer.Deserialize(file);

            visitorListGrid.DataSource = visitors;
            file.Close();
        }
    }
}
