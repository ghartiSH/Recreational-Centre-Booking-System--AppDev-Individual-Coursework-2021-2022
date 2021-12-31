using System;
using System.Collections;
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
        XmlSerializer paidSerializer;
        List<CheckedoutVisitors> checkedoutVisitors;


        
        public Form1()
        {
            InitializeComponent();
            xmlSerializer = new XmlSerializer(typeof(List<Visitors>));
            visitors = new List<Visitors>();

            paidSerializer = new XmlSerializer(typeof(List<CheckedoutVisitors>));
            checkedoutVisitors = new List<CheckedoutVisitors>();
            
            LoadTicket();
            ///LoadXml();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/paidVisitors.xml", FileMode.Open, FileAccess.Read);
         
            checkedoutVisitors = (List<CheckedoutVisitors>)paidSerializer.Deserialize(file);

            dailyReportGrid.DataSource = checkedoutVisitors;
            file.Close();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            Visitors vs = new Visitors();
            FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/visitors.xml", FileMode.Create, FileAccess.Write);
            vs.fullname = fullNameTxt.Text;
            vs.email = emailTxt.Text;
            vs.phone = phoneTxt.Text;
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
            try{
                FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/visitors.xml", FileMode.Open, FileAccess.Read);
                visitors = (List<Visitors>)xmlSerializer.Deserialize(file);

                visitorListGrid.DataSource = visitors;
                file.Close();
            }
            catch
            {
                MessageBox.Show("No records Stored");
            }
        }

        private void LoadTicket()
        {
            try
            {

                DataTable dt = new DataTable();
                string[] lines = System.IO.File.ReadAllLines("C:/Users/bhara/source/repos/Coursework/ticket.csv");
                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');
                    foreach (string headerword in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerword));
                    }
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] dataWords = lines[i].Split(',');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];

                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    ticketGrid.DataSource = dt;
                }
            }
            catch
            {
                MessageBox.Show("Ticket File not Found");
            }
        }

        private void LoadXml()
        {
            try
            {
                FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/visitors.xml", FileMode.Open, FileAccess.Read);
                visitors = (List<Visitors>)xmlSerializer.Deserialize(file);

                visitorListGrid.DataSource = visitors;
                file.Close();
            }
            catch
            {
                MessageBox.Show("No records Stored");
            }
        }

        private void Find_Click(object sender, EventArgs e)
        {

            List<Visitors> matchList = FindVisitors();
            checkoutGrid.DataSource = matchList;
              
        }

        
        private void Calculate_Click(object sender, EventArgs e)
        {
            List<Visitors> matchList = FindVisitors();
            DataTable dt = GetTicket();
            ArrayList arr = new ArrayList();
            int totalBill=0;
            CheckedoutVisitors cv = new CheckedoutVisitors();

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {

                    arr.Add(row[column]);
                }
            }

            foreach (var visitor in matchList)
            {
                int outTime = int.Parse(outHHCmb.Text);
                int inTime = visitor.inDateTime.Hour;

                var result = (outTime - inTime);

                
                for (int i=0; i<arr.Count; i++)
                {

                    if (visitor.totalVisitors < 5)
                    {
                        if (arr[i].ToString() == visitor.category)
                        {
                            if (result == 1)
                            {
                                totalBill = visitor.totalVisitors * int.Parse(arr[i + 1].ToString());
                            }
                            else if (result == 2)
                            {
                                totalBill = visitor.totalVisitors * int.Parse(arr[i + 2].ToString());
                            }
                            else if (result == 3)
                            {
                                totalBill = visitor.totalVisitors * int.Parse(arr[i + 3].ToString());
                            }
                            else if (result == 4)
                            {
                                totalBill = visitor.totalVisitors * int.Parse(arr[i + 4].ToString());
                            }
                            else
                            {
                                totalBill = visitor.totalVisitors * int.Parse(arr[i + 5].ToString());
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (arr[i].ToString() == visitor.category)
                        {
                            if (result == 1)
                            {
                                totalBill = int.Parse(arr[i + 1].ToString());
                            }
                            else if (result == 2)
                            {
                                totalBill = int.Parse(arr[i + 2].ToString());
                            }
                            else if (result == 3)
                            {
                                totalBill = int.Parse(arr[i + 3].ToString());
                            }
                            else if (result == 4)
                            {
                                totalBill = int.Parse(arr[i + 4].ToString());
                            }
                            else
                            {
                                totalBill = int.Parse(arr[i + 5].ToString());
                            }
                            break;
                        }
                    }
                    
                }
                message.Text = "Hello " + visitor.fullname + " !!!... You've played for " + result + " hours.";

                cv.fullname = visitor.fullname;
                cv.email = visitor.fullname;
                cv.phone = visitor.phone;
                cv.category = visitor.category;
                cv.inDateTime = visitor.inDateTime;
                cv.totalVisitors = visitor.totalVisitors;
                cv.totalChildren = visitor.totalChildren;
                cv.outDateTime = DateTime.Parse(outHHCmb.Text + ":" + outMinCmb.Text);
                cv.paid = totalBill;

                checkedoutVisitors.Add(cv);


            }
            
            billTxt.Text = totalBill.ToString();
        }

        private List<Visitors> FindVisitors()
        {
            List<Visitors> matchList = new List<Visitors>();
            try
            {
                FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/visitors.xml", FileMode.Open, FileAccess.Read);
                visitors = (List<Visitors>)xmlSerializer.Deserialize(file);


                string phone = searchPhoneTxt.Text;

                foreach (var visitor in visitors)
                {
                    if (visitor.phone == phone)
                    {
                        matchList.Add(visitor);
                    }
                }
                file.Close();

            }
            catch
            {
                MessageBox.Show("No visitors added");
            }
            return matchList;

        }

        private DataTable GetTicket()
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines("C:/Users/bhara/source/repos/Coursework/ticket.csv");
            if (lines.Length > 0)
            {
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');
                foreach (string headerword in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerword));
                }
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] dataWords = lines[i].Split(',');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[columnIndex++];

                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private void Checkout_Click(object sender, EventArgs e)
        {

            FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/paidVisitors.xml", FileMode.Create, FileAccess.Write);

     
            paidSerializer.Serialize(file, checkedoutVisitors);
            file.Close();
            
        }


        private void VisitorsByCategory_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/paidVisitors.xml", FileMode.Open, FileAccess.Read);
            checkedoutVisitors = (List<CheckedoutVisitors>)paidSerializer.Deserialize(file);

            int childTotal = 0;
            int adultTotal = 0;
            int groupFiveTotal = 0;
            int groupTenTotal = 0;
            int groupFifteenTotal = 0;

            foreach (var cv in checkedoutVisitors)
            {
                switch (cv.category)
                {
                    case "Child":
                        childTotal = childTotal + 1;
                        break;
                    case "Adult":
                        adultTotal = adultTotal + 1;
                        break;
                    case "Group of 5":
                        groupFiveTotal = groupFiveTotal + 1;
                        break;
                    case "Group of 10":
                        groupTenTotal = groupTenTotal + 1;
                        break;
                    case "Group of 15":
                        groupFifteenTotal = groupFifteenTotal + 1;
                        break;
                }

            }
            List<DailyReport> dr = new List<DailyReport>();
            dr.Add(new DailyReport { category = "Child", noOfVisitors = childTotal });
            dr.Add(new DailyReport { category = "Adult", noOfVisitors = adultTotal });
            dr.Add(new DailyReport { category = "Group of 5", noOfVisitors = groupFiveTotal });
            dr.Add(new DailyReport { category = "Froup of 10", noOfVisitors = groupTenTotal });
            dr.Add(new DailyReport { category = "Group of 15", noOfVisitors = groupFifteenTotal });

            dailyReportGrid.DataSource = dr;
            file.Close();
        }
    }
}
