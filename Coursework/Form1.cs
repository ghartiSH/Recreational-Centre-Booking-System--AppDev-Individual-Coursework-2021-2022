using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            
            LoadXml();
            LoadCheckedVisitorsXML();
        }

        

        private void Register_Click(object sender, EventArgs e)
        {
            try
            {
                Visitors vs = new Visitors();
                FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/visitors.xml", FileMode.Create, FileAccess.Write);

                vs.fullname = fullNameTxt.Text;
                vs.email = emailTxt.Text;
                vs.phone = phoneTxt.Text;
                vs.category = categoryCmb.Text;
                vs.inDateTime = DateTime.Parse(mmCmb.Text + "-" + ddCmb.Text + "-" + yyCmb.Text + " " + hhCmb.Text + ":" + minCmb.Text);
                vs.totalVisitors = int.Parse(totalVisitorsCmb.Text);
                vs.totalChildren = int.Parse(childrenCmb.Text);

                var isEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var isValidEmail = isEmail.IsMatch(vs.email);

                var hasNumber = new Regex("^[0-9]+$");
                var isValidNumber = hasNumber.IsMatch(vs.phone);

                if (vs.fullname.Length > 2)
                {
                    nameError.Visible = false;
                    if (isValidEmail)
                    {
                        emailError.Visible = false;
                        if (isValidNumber)
                        {
                            phoneError.Visible = false;
                            if (vs.totalVisitors > 0)
                            {
                                visitorsError.Visible = false;
                                phoneError.Visible = false;
                                visitors.Add(vs);
                                xmlSerializer.Serialize(file, visitors);
                                MessageBox.Show("Visitor added Successfylly..!");
                            }
                            else
                            {
                                visitorsError.Visible = true;

                            }
                        }
                        else
                        {
                            phoneError.Visible = true;
                        }
                    }
                    else
                    {
                        emailError.Visible = true;
                    }
                }
                else
                {
                    nameError.Visible = true;
                }
                file.Close();
            }
            catch
            {
                MessageBox.Show("File Error!!!...Delete the visitors.xml file and try again. \nCAUTION: Do not perform other actions in the file while wrriting in the file!!!");
            }
        }

        

        private void ViewCurrentVisitors_Click(object sender, EventArgs e)
        {
            List<Visitors> vis = new List<Visitors>();
            vis = ReadVisitors();

            visitorListGrid.DataSource = vis;
        }


        private void LoadXml()
        {
            List<Visitors> vis = new List<Visitors>();
            vis = ReadVisitors();

            visitorListGrid.DataSource = vis;
        }


        private void LoadCheckedVisitorsXML()
        {
            List<CheckedoutVisitors> chvis = new List<CheckedoutVisitors>();
            chvis = GetCheckedoutVisitors();

            dailyReportGrid.DataSource = chvis;
        }
        

        private void Find_Click(object sender, EventArgs e)
        {

            List<Visitors> matchList = FindVisitors();
            checkoutGrid.DataSource = matchList;
              
        }

        
        private void Calculate_Click(object sender, EventArgs e)
        {
            try
            {
                List<Visitors> matchList = FindVisitors();
                DataTable dt = GetTicket();
                ArrayList arr = new ArrayList();
                int totalBill = 0;
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


                    for (int i = 0; i < arr.Count; i++)
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
            catch
            {
                MessageBox.Show("Ticket Price Error!!");
            }
        }

        private void button3_Click(object sender, EventArgs e) //viewing checked out visitors button
        {
            List<CheckedoutVisitors> vis = new List<CheckedoutVisitors>();

            vis = GetCheckedoutVisitors();

            dailyReportGrid.DataSource = vis;
        }

        private List<Visitors> FindVisitors()
        {
            List<Visitors> matchList = new List<Visitors>();
            try
            {
                List<Visitors> vis = new List<Visitors>();
                vis = ReadVisitors();


                string phone = searchPhoneTxt.Text;

                foreach (var visitor in vis)
                {
                    if (visitor.phone == phone)
                    {
                        matchList.Add(visitor);
                    }
                }

            }
            catch
            {
                MessageBox.Show("No visitors added");
            }
            return matchList;

        }

        

        private void Checkout_Click(object sender, EventArgs e)
        {
            var hasNumber = new Regex("^[0-9]+$");
            var isValidNumber = hasNumber.IsMatch(paidBillTxt.Text);

            if (isValidNumber)
            {
                FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/paidVisitors.xml", FileMode.Create, FileAccess.Write);
                paidSerializer.Serialize(file, checkedoutVisitors);
                file.Close();
                MessageBox.Show("Visitor Checked out successfully..");
                billError.Visible = false;
            }
            else
            {
                billError.Visible = true;
            }
            
        }


        private void VisitorsByCategory_Click(object sender, EventArgs e)
        {
            List<CheckedoutVisitors> vis = new List<CheckedoutVisitors>();

            vis = GetCheckedoutVisitors();

            int childTotal = 0;
            int adultTotal = 0;
            int groupFiveTotal = 0;
            int groupTenTotal = 0;
            int groupFifteenTotal = 0;

            foreach (var cv in vis)
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
        }

        public List<CheckedoutVisitors> GetCheckedoutVisitors()
        {
            try
            {
                FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/paidVisitors.xml", FileMode.Open, FileAccess.Read);
                checkedoutVisitors = (List<CheckedoutVisitors>)paidSerializer.Deserialize(file);
                file.Close();
            }
            catch
            {
                MessageBox.Show("File not Found.. \npaidVisitors.xml file not found..");
            }
            return checkedoutVisitors;
        }

        private List<Visitors> ReadVisitors()
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
                MessageBox.Show("No records Stored.. \nvisitors.xml file not found");
            }
            return visitors;
        }

        private DataTable GetTicket()
        {
            DataTable dt = new DataTable();
            try
            {

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
            }

            catch
            {
                MessageBox.Show("Ticket File not found");
            }
            return dt;

        }

        private void clear()
        {
            fullNameTxt.Text = "";

        }

        private void CreateCSV(string path, DataGridView dg)
        {
            string csv = string.Empty;

            foreach(DataGridViewColumn coln in dg.Columns)
            {
                    csv += coln.HeaderText + ',';
            }

            csv += "\r\n";

            foreach (DataGridViewRow row in dg.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell != null)
                    {
                        csv += cell.Value.ToString().Replace(",", ";") + ',';
                    }
                }

                csv += "\r\n";

            }

            File.WriteAllText(path, csv);
        }

        private void GenerateDailyReport_Click(object sender, EventArgs e)
        {
            string path = "C:/Users/bhara/source/repos/Coursework/DailyReport.csv";

            CreateCSV(path, dailyReportGrid);
            MessageBox.Show("CSV report generated Successfully");
        }

        private int[] bubbleSort(int[] arr)
        {
            int[] array = arr;
            int temp;

            for (int i=0; i<array.Length-1; i++)
            {
                for (int j=0; j<array.Length-1; j++ )
                {
                    if (array[j] > array[j + 1])
                    {
                        temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                       
                }
            }
            return array;
        }

        private void SortByVisitors_Click(object sender, EventArgs e)
        {
            List<WeeklyReport> list = GetWeeklyReport();
            int[] array = new int[list.Count];

            int i = 0;
            foreach(var item in list )
            {
                array[i] = item.visitors;
                i++;
            }

            int[] sortedArray = bubbleSort(array);
            List<WeeklyReport> sortedList = new List<WeeklyReport>();
            for (int j=0; j<sortedArray.Length;j++ )
            {
                foreach(var ls in list)
                {
                    if (!sortedList.Contains(ls))
                    {
                        if (sortedArray[j] == ls.visitors)
                        {
                            sortedList.Add(ls);
                            break;
                        }
                    }
                }
            }
            weeklyDataGrid.DataSource = sortedList;
        }

        private void ViewWeeklyVisitors_Click(object sender, EventArgs e)
        {
           List <WeeklyReport> wrObj = new List <WeeklyReport>();

            wrObj = GetWeeklyReport();
            weeklyDataGrid.DataSource = wrObj;
           
        }

        private List<WeeklyReport> GetWeeklyReport()
        {
            List<CheckedoutVisitors> vis = new List<CheckedoutVisitors>();
            vis = GetCheckedoutVisitors();

            int sundayVisitors = 0;
            int sundayIncome = 0;
            int mondayVisitors = 0;
            int mondayIncome = 0;
            int tuesdayVisitors = 0;
            int tuesdayIncome = 0;
            int wednesdayVisitors = 0;
            int wednesdayIncome = 0;
            int thursdayVisitors = 0;
            int thursdayIncome = 0;
            int fridayIncome = 0;
            int fridayVisitors = 0;
            int saturdayVisitors = 0;
            int saturdayIncome = 0;


            DateTime startDate = DateTime.Parse(weeklymmCmb.Text  + "-" + weeklyddCmb.Text + "-" + weeklyyyCmb.Text);
            var dayOnly = startDate.Date;
            foreach (var item in vis)
            {


                var visitorsDate = item.inDateTime.Date;

                if (dayOnly == visitorsDate)
                {
                    sundayVisitors = sundayVisitors + 1;
                    sundayIncome = sundayIncome + item.paid;
                }
                else if (visitorsDate == (dayOnly.AddDays(1)))
                {
                    mondayVisitors = mondayVisitors + 1;
                    mondayIncome = mondayIncome + item.paid;

                }
                else if (visitorsDate == (dayOnly.AddDays(2))) 
                {
                    tuesdayIncome = tuesdayIncome + item.paid;
                    tuesdayVisitors += 1;
                }
                else if (visitorsDate == (dayOnly.AddDays(3))) 
                {
                    wednesdayIncome += item.paid;
                    wednesdayVisitors += 1;

                }

                else if (visitorsDate == (dayOnly.AddDays(4)))
                {
                    thursdayIncome += item.paid;
                    thursdayVisitors += 1;


                }
                else if (visitorsDate == (dayOnly.AddDays(5)))
                {
                    fridayVisitors += 1;
                    fridayIncome += item.paid;
                }
                else if (visitorsDate == (dayOnly.AddDays(6)))
                {
                    saturdayVisitors += 1;
                    saturdayIncome += item.paid;
                }
            }
            List<WeeklyReport> wr = new List<WeeklyReport>();
            wr.Add(new WeeklyReport { date= dayOnly, day = "Sunday", visitors = sundayVisitors, income = sundayIncome });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(1), day = "Monday", visitors = mondayVisitors, income = mondayIncome });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(2), day = "Tuesday", visitors = tuesdayVisitors, income = tuesdayIncome });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(3), day = "Wednesday", visitors = wednesdayVisitors, income = wednesdayIncome });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(4), day = "Thursday", visitors = thursdayVisitors, income = thursdayIncome });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(5), day = "Friday", visitors = fridayVisitors, income = fridayIncome });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(6), day = "Saturday", visitors = saturdayVisitors, income = saturdayIncome });

            return wr;

        }

        private void SortByIncome_Click(object sender, EventArgs e)
        {
            List<WeeklyReport> list = GetWeeklyReport();
            int[] array = new int[list.Count];

            int i = 0;
            foreach (var item in list)
            {
                array[i] = item.income;
                i++;
            }

            int[] sortedArray = bubbleSort(array);
            List<WeeklyReport> sortedList = new List<WeeklyReport>();
            for (int j = 0; j < sortedArray.Length; j++)
            {
                foreach (var ls in list)
                {
                    if (!sortedList.Contains(ls))
                    {
                        if (sortedArray[j] == ls.income)
                        {
                            sortedList.Add(ls);
                            break;
                        }
                    }
                }
            }
            weeklyDataGrid.DataSource = sortedList;
        }

        private void button1_Click(object sender, EventArgs e) //Daily Report generating button
        {
            List<CheckedoutVisitors> chv = GetCheckedoutVisitors();

            List<CheckedoutVisitors>  matched = new List<CheckedoutVisitors>();

            DateTime checkDate = DateTime.Parse(dailymmCmb.Text + "-" + dailyddCmb.Text + "-" + dailyyyCmb.Text);

            foreach (var item in chv)
            {
                if (checkDate.Date == item.inDateTime.Date)
                {
                    matched.Add(item);
                }
            }


            dailyReportGrid.DataSource = matched;

        }

        private void button2_Click(object sender, EventArgs e) // Wekly Report All visitors showing button
        {
            List<CheckedoutVisitors> vis = new List<CheckedoutVisitors>();

            vis = GetCheckedoutVisitors();

            weeklyDataGrid.DataSource = vis;
        }

        private void GenerateWeeklyReport_Click(object sender, EventArgs e)
        {
            string path = "C:/Users/bhara/source/repos/Coursework/WeeklyReport.csv";

            CreateCSV(path, weeklyDataGrid);
            MessageBox.Show("CSV report generated Successfully");
        }

        private void GenerateGraph_Click(object sender, EventArgs e)
        {
            Graph graphform = new Graph();
            List<WeeklyReport> list = GetWeeklyReport();

            graphform.LoadGraph(list);
            graphform.Show();

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string uname = usernameTxt.Text;
            string password = passwordTxt.Text;

            if (uname == "admin" && password =="admin")
            {
                ticketPanel.Visible = true;
            }
            else
            {
                MessageBox.Show("Not Authorized.");
            }
        }

        private void SetTicket_Click(object sender, EventArgs e)
        {

            //Build the CSV file data as a Comma separated string.
            string csv = "";

            //Add the Header row for CSV file.
            for (int i=0; i<6; i++)
            {
                if (i==5)
                {
                    csv += adminTicketGrid.Columns[i].HeaderText;
                }
                else
                {
                    csv += adminTicketGrid.Columns[i].HeaderText + ',';
                }
            }

            //Add new line.
            csv += "\r\n";

            //Adding the Rows
            foreach (DataGridViewRow row in adminTicketGrid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //Add the Data rows.
                    csv += cell.Value.ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            //Exporting to CSV.
            string folderPath = "C:/Users/bhara/source/repos/Coursework/";
            File.WriteAllText(folderPath + "ticket.csv", csv);
            MessageBox.Show("Ticket Edited Successfully..");

        }

        private void ViewTicket_Click(object sender, EventArgs e)
        {
            DataTable tb = new DataTable();
            tb = GetTicket();

            adminTicketGrid.DataSource = tb;

            ticketMessage.Visible = true;

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = GetTicket();
            ticketGrid.DataSource = dt;
        }



        private void searchVisitor_Click(object sender, EventArgs e)
        {
            List<CheckedoutVisitors> vis = new List<CheckedoutVisitors>();
            vis = GetCheckedoutVisitors();

            string cat = reportCategory.Text;
            List<CheckedoutVisitors> matched = new List<CheckedoutVisitors>();
            foreach (var item in vis)
            {
                if (item.category == cat)
                {
                    matched.Add(item);
                }
            }
            dailyReportGrid.DataSource = matched;

        }
    }
}
