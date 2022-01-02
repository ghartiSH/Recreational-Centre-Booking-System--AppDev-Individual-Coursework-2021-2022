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
            xmlSerializer = new XmlSerializer(typeof(List<Visitors>)); //serializer object for writing/reading visitors filestream
            visitors = new List<Visitors>(); //list for storing registered visitors

            paidSerializer = new XmlSerializer(typeof(List<CheckedoutVisitors>)); //serializer object for reading/writing checkedout visitors filestream
            checkedoutVisitors = new List<CheckedoutVisitors>(); //list for storing checked out visitors.
            
            LoadXml();  //loading visitors.xml file for preventing data overwrite
            LoadCheckedVisitorsXML(); //loading checked out visitors xml file for preventing data overwrite
        }

        

        private void Register_Click(object sender, EventArgs e) //registering new visitors
        {
            try
            {
                Visitors vs = new Visitors();
                FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/visitors.xml", FileMode.Create, FileAccess.Write);

                vs.fullname = fullNameTxt.Text;
                vs.email = emailTxt.Text;
                vs.phone = phoneTxt.Text;
                vs.category = categoryCmb.Text;
                vs.inDateTime = DateTime.Parse(mmCmb.Text + "-" + ddCmb.Text + "-" + yyCmb.Text + " " + hhCmb.Text + ":" + minCmb.Text); // storing date and time combinedly in mm-dd-yy hh:mm format
                vs.totalVisitors = int.Parse(totalVisitorsCmb.Text);
                vs.totalChildren = int.Parse(childrenCmb.Text);

                var isEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); //email validation regex
                var isValidEmail = isEmail.IsMatch(vs.email); //validating email

                var hasNumber = new Regex("^[0-9]+$"); //number only validation regex
                var isValidNumber = hasNumber.IsMatch(vs.phone);//validating number-only text phone number

                if (vs.fullname.Length > 2) //checking if the username is longer than two words
                {
                    nameError.Visible = false;
                    if (isValidEmail)
                    {
                        emailError.Visible = false;
                        if (isValidNumber)
                        {
                            phoneError.Visible = false;
                            if (vs.totalVisitors > 0) //checking if the no.of visitors is provided properly
                            {
                                visitorsError.Visible = false;
                                phoneError.Visible = false;
                                visitors.Add(vs);
                                xmlSerializer.Serialize(file, visitors);
                                MessageBox.Show("Visitor added Successfylly..!");
                            }
                            else
                            {
                                visitorsError.Visible = true; // displaying the error message

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
            finally //after the user is registered or not, clearing out all the textfields.
            {
                clear(); //method to clear all the textfields.
            }
        }

        

        private void ViewCurrentVisitors_Click(object sender, EventArgs e) //viewing registered visitors list
        {
            LoadXml(); //calling the method that displays the visitors list in grid view
        }

        /// <summary>
        /// additional method to read visitors list and display in grid automatically to prevent data overwriting
        /// </summary>
        private void LoadXml() 
        {
            List<Visitors> vis = new List<Visitors>();
            vis = ReadVisitors(); // calling readVisitors method which returns a list of visitors type

            visitorListGrid.DataSource = vis;
        }

        /// <summary>
        /// method to read checked out visitors list and display in grid automatically to prevent data over writing
        /// </summary>
        private void LoadCheckedVisitorsXML()
        {
            List<CheckedoutVisitors> chvis = new List<CheckedoutVisitors>();
            chvis = GetCheckedoutVisitors(); //calling method that returs a list of checkout visitors

            dailyReportGrid.DataSource = chvis;
        }
        

        private void Find_Click(object sender, EventArgs e) //button click for searching and finding a visitor for checking out
        {

            checkoutGrid.DataSource = FindVisitors(); //calling the method to find the visitor and displaying it in the data grid
              
        }

        
        private void Calculate_Click(object sender, EventArgs e) //calculating the bill for a visitor
        {
            try
            {
                List<Visitors> matchList = FindVisitors(); //storing the visitors details
                DataTable dt = GetTicket(); //getting ticket info
                ArrayList arr = new ArrayList(); //arraylist for storing ticket prices after accessing them from external csv
                int totalBill = 0; //total bill initialization
                CheckedoutVisitors cv = new CheckedoutVisitors(); //new object of checked out visitors for storing the vistor after he is checked out

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {

                        arr.Add(row[column]); //adding ticket prices to the arraylist
                    }
                }

                foreach (var visitor in matchList)
                {
                    int outTime = int.Parse(outHHCmb.Text); //taking out time from the combox (hour only)
                    int inTime = visitor.inDateTime.Hour; //taking visitors intime from the list item

                    var result = (outTime - inTime); //calculating the time (in hour he/she played)


                    for (int i = 0; i < arr.Count; i++) //looping thorughout the ticket list to find its exact rate
                    {

                        if (visitor.totalVisitors < 5) //checking if the visitor is in group or adult or child
                        {
                            if (arr[i].ToString() == visitor.category) // after he is not in group checking if he/she is adult or tha child type
                            {
                                if (result == 1) //checking the amount of time he played
                                {
                                    totalBill = visitor.totalVisitors * int.Parse(arr[i + 1].ToString()); //if he has played for one hour then the next column to the category column contains the price rate for 1 hour
                                }
                                else if (result == 2)
                                {
                                    totalBill = visitor.totalVisitors * int.Parse(arr[i + 2].ToString()); // 2 hours rate is 2 columns away from the category column
                                }
                                else if (result == 3)
                                {
                                    totalBill = visitor.totalVisitors * int.Parse(arr[i + 3].ToString());// 3 hours rate is 3 columns away from the category column
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
                            if (arr[i].ToString() == visitor.category) // if the visitor is in group no individual bill is calculated rather the rate for whole group is pre-defined
                            {
                                if (result == 1)
                                {
                                    totalBill = int.Parse(arr[i + 1].ToString()); // not multiplying but directly accessing the rate for groups
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
                    message.Text = "Hello " + visitor.fullname + " !!!... You've played for " + result + " hours."; //printing the message with visitors name and time he has played for

                    cv.fullname = visitor.fullname;
                    cv.email = visitor.fullname;
                    cv.phone = visitor.phone;
                    cv.category = visitor.category;
                    cv.inDateTime = visitor.inDateTime;
                    cv.totalVisitors = visitor.totalVisitors;
                    cv.totalChildren = visitor.totalChildren;
                    cv.outDateTime = DateTime.Parse(outHHCmb.Text + ":" + outMinCmb.Text);
                    cv.paid = totalBill;

                    checkedoutVisitors.Add(cv); //adding in checked out visitors list after checking out


                }

                billTxt.Text = totalBill.ToString(); //displaying the calculated bill in the textfield
            }
            catch
            {
                MessageBox.Show("Ticket Price Error!!"); //catch block message if string data is passed in the ticket price
            }
        }

        private void button3_Click(object sender, EventArgs e) //viewing checked out visitors button
        {
            dailyReportGrid.DataSource = GetCheckedoutVisitors(); //calling the method that returns checked out visitors list
        }

        private List<Visitors> FindVisitors() //method for searching the visitor
        {
            List<Visitors> matchList = new List<Visitors>();
            try
            {
                List<Visitors> vis = new List<Visitors>();
                vis = ReadVisitors();


                string phone = searchPhoneTxt.Text;

                foreach (var visitor in vis)
                {
                    if (visitor.phone == phone) // found if the phone number of the visitor matches with the inputted one
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

        

        private void Checkout_Click(object sender, EventArgs e) //checking out the visitor
        {
            var hasNumber = new Regex("^[0-9]+$"); //validiton for only entering number values in the bill paid textfield
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
            clear(); //clearing the texfields after the visitor is checked out
            
        }


        private void VisitorsByCategory_Click(object sender, EventArgs e) //categorizing the visitors by their respective categories
        {
            List<CheckedoutVisitors> vis = new List<CheckedoutVisitors>();

            vis = GetCheckedoutVisitors();

            int childTotal = 0; // this stores the amount of registrations done under that category
            int adultTotal = 0;
            int groupFiveTotal = 0;
            int groupTenTotal = 0;
            int groupFifteenTotal = 0;

            int childTotalVisitors = 0; //this stores total number of people individually who were present
            int adultTotalVisitors = 0;
            int groupFiveTotalVisitors = 0;
            int groupTenTotalVisitors = 0;
            int groupFifteenTotalVisitors = 0;
            foreach (var cv in vis)
            {
                switch (cv.category)
                {
                    case "Child":
                        childTotal = childTotal + 1; //adding the registration count
                        childTotalVisitors += cv.totalVisitors; //adding the total visitors
                        break;
                    case "Adult":
                        adultTotal = adultTotal + 1;
                        adultTotalVisitors += cv.totalVisitors;
                        break;
                    case "Group of 5":
                        groupFiveTotal = groupFiveTotal + 1;
                        groupFiveTotalVisitors += cv.totalVisitors;
                        break;
                    case "Group of 10":
                        groupTenTotal = groupTenTotal + 1;
                        groupTenTotalVisitors += cv.totalVisitors;
                        break;
                    case "Group of 15":
                        groupFifteenTotal = groupFifteenTotal + 1;
                        groupFifteenTotalVisitors += cv.totalVisitors;
                        break;
                }

            }
            List<DailyReport> dr = new List<DailyReport>(); //creating a new list of type DailyReport to temporarily store and display the daily stats
            dr.Add(new DailyReport { category = "Child", totalRegistrations = childTotal, noOfVisitors = childTotalVisitors }); // adding a new object to the list
            dr.Add(new DailyReport { category = "Adult", totalRegistrations = adultTotal, noOfVisitors =adultTotalVisitors});
            dr.Add(new DailyReport { category = "Group of 5", totalRegistrations = groupFiveTotal, noOfVisitors = groupFiveTotalVisitors });
            dr.Add(new DailyReport { category = "Froup of 10", totalRegistrations = groupTenTotal, noOfVisitors = groupTenTotalVisitors });
            dr.Add(new DailyReport { category = "Group of 15", totalRegistrations = groupFifteenTotal, noOfVisitors=groupFifteenTotalVisitors });

            dailyReportGrid.DataSource = dr; //displaying the created list in the datagridview
        }

        /// <summary>
        /// MEthod fro reading  checked out visitors file
        /// </summary>
        /// <returns>checked out visitors list</returns>
        public List<CheckedoutVisitors> GetCheckedoutVisitors() //method for reading checked out visitors file and returning its list
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
        /// <summary>
        /// Method for reading visitors xml file
        /// </summary>
        /// <returns>visitors list</returns>
        private List<Visitors> ReadVisitors()//method for reading visitors file and returning its list
        {
            try
            {
                FileStream file = new FileStream("C:/Users/bhara/source/repos/Coursework/visitors.xml", FileMode.Open, FileAccess.Read);
                visitors = (List<Visitors>)xmlSerializer.Deserialize(file);
                file.Close();
            }
            catch
            {
                MessageBox.Show("No records Stored.. \nvisitors.xml file not found");
            }
            return visitors;
        }
        /// <summary>
        /// Method for reading external csv file and returing it as a datatable
        /// </summary>
        /// <returns>dt which is a datatable containing ticket price rates </returns>

        private DataTable GetTicket() //method that reads the csv file and returns data in data table
        {
            DataTable dt = new DataTable();
            try
            {

                string[] lines = System.IO.File.ReadAllLines("C:/Users/bhara/source/repos/Coursework/ticket.csv");
                if (lines.Length > 0) //checking if the file is not empty
                {
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');  //array for storing the first line of the csv file as the header
                    foreach (string headerword in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerword));//assigning the first line as header (column name for the data grid view)
                    }
                    for (int i = 1; i < lines.Length; i++) //loop for reading all the rows of the file
                    {
                        string[] dataWords = lines[i].Split(','); // array for storing the data of each row
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
            return dt; //returning the datatable containing all the data of csv file

        }

       /// <summary>
       /// Method for creating a csv file
       /// </summary>
       /// <param name="path">it is the path where the file is created</param>
       /// <param name="dg">this is the datatgridview with data</param>

        private void CreateCSV(string path, DataGridView dg)
        {
            string csv = string.Empty;

            foreach(DataGridViewColumn coln in dg.Columns) //adding header 
            {
                    csv += coln.HeaderText + ',';
            }

            csv += "\r\n"; //new line

            foreach (DataGridViewRow row in dg.Rows)//adding rows
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell != null)
                    {
                        csv += cell.Value.ToString().Replace(",", ";") + ','; //adding data rows
                    }
                }

                csv += "\r\n";// new line

            }

            File.WriteAllText(path, csv);//writing the csv file to given path
        }

        private void GenerateDailyReport_Click(object sender, EventArgs e) //generating csv file for daily report
        {
            string path = "C:/Users/bhara/source/repos/Coursework/DailyReport.csv";

            CreateCSV(path, dailyReportGrid);
            MessageBox.Show("CSV report generated Successfully");
        }
        /// <summary>
        /// Bubble sort code for sorting arrays
        /// </summary>
        /// <param name="arr">unsorted array</param>
        /// <returns>sorted array</returns>
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

        private void SortByVisitors_Click(object sender, EventArgs e) //sorting visitors  by total number of visitors
        {
            List<WeeklyReport> list = GetWeeklyReport();//getting weekly report
            int[] array = new int[list.Count]; //creating an array that is the length of the list

            int i = 0;
            foreach(var item in list )//assigning the number of visitors in the array
            {
                array[i] = item.visitors;
                i++;
            }

            int[] sortedArray = bubbleSort(array); //bubble sorting

            List<WeeklyReport> sortedList = new List<WeeklyReport>();
            for (int j=sortedArray.Length-1; j >=0; j--) //arranging the data of list in ordered manner
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
            weeklyDataGrid.DataSource = sortedList; //displaying sorted list
        }

        private void ViewWeeklyVisitors_Click(object sender, EventArgs e) // viewing weekly report
        {
           
            weeklyDataGrid.DataSource = GetWeeklyReport(); 

        }
        /// <summary>
        /// method to calculate weekly report
        /// </summary>
        /// <returns>list of weekly report</returns>
        private List<WeeklyReport> GetWeeklyReport()
        {
            List<CheckedoutVisitors> vis = new List<CheckedoutVisitors>();
            vis = GetCheckedoutVisitors();

            int sundayVisitors = 0; //total visitors on that day
            int sundayIncome = 0; //total income on that day
            int sundayRegistrations=0; //total registrations on that day

            int mondayVisitors = 0;
            int mondayIncome = 0;
            int mondayRegistrations=0;

            int tuesdayVisitors = 0;
            int tuesdayIncome = 0;
            int tuesdayRegistrations=0;

            int wednesdayVisitors = 0;
            int wednesdayIncome = 0;
            int wednesdayRegistrations=0;

            int thursdayVisitors = 0;
            int thursdayIncome = 0;
            int thursdayRegistrations=0;

            int fridayIncome = 0;
            int fridayVisitors = 0;
            int fridayRegistrations = 0;

            int saturdayVisitors = 0;
            int saturdayIncome = 0;
            int saturdayRegistrations = 0;


            DateTime startDate = DateTime.Parse(weeklymmCmb.Text  + "-" + weeklyddCmb.Text + "-" + weeklyyyCmb.Text);
            var dayOnly = startDate.Date; //taking only date (not the time)
            foreach (var item in vis)
            {


                var visitorsDate = item.inDateTime.Date;

                if (dayOnly == visitorsDate) // checking if the day is sunday
                {
                    sundayVisitors = sundayVisitors + item.totalVisitors;
                    sundayIncome = sundayIncome + item.paid;
                    sundayRegistrations += 1;
                }
                else if (visitorsDate == (dayOnly.AddDays(1)))  //checking if the day is monday
                {
                    mondayVisitors = mondayVisitors + item.totalVisitors;
                    mondayIncome = mondayIncome + item.paid;
                    mondayRegistrations += 1;

                }
                else if (visitorsDate == (dayOnly.AddDays(2))) //checking if the day is tuesday
                {
                    tuesdayIncome = tuesdayIncome + item.paid;
                    tuesdayVisitors += item.totalVisitors;
                    tuesdayRegistrations += 1;
                }
                else if (visitorsDate == (dayOnly.AddDays(3))) //checking wednesday
                {
                    wednesdayIncome += item.paid;
                    wednesdayVisitors += item.totalVisitors;
                    wednesdayRegistrations += 1;

                }

                else if (visitorsDate == (dayOnly.AddDays(4)))// checking thursday
                {
                    thursdayIncome += item.paid;
                    thursdayVisitors += item.totalVisitors;
                    thursdayRegistrations += 1;


                }
                else if (visitorsDate == (dayOnly.AddDays(5)))//checking friday
                {
                    fridayVisitors += item.totalVisitors;
                    fridayIncome += item.paid;
                    fridayRegistrations += 1;

                }
                else if (visitorsDate == (dayOnly.AddDays(6)))// checking saturday
                {
                    saturdayVisitors += item.totalVisitors;
                    saturdayIncome += item.paid;
                    saturdayRegistrations += 1;

                }
            }
            List<WeeklyReport> wr = new List<WeeklyReport>();
            wr.Add(new WeeklyReport { date= dayOnly, day = "Sunday", visitors = sundayVisitors, income = sundayIncome, registrations=sundayRegistrations }); //adding data in the list as objeccts
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(1), day = "Monday", visitors = mondayVisitors, income = mondayIncome, registrations=mondayRegistrations });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(2), day = "Tuesday", visitors = tuesdayVisitors, income = tuesdayIncome, registrations=tuesdayRegistrations });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(3), day = "Wednesday", visitors = wednesdayVisitors, income = wednesdayIncome, registrations=wednesdayRegistrations });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(4), day = "Thursday", visitors = thursdayVisitors, income = thursdayIncome, registrations=thursdayRegistrations });
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(5), day = "Friday", visitors = fridayVisitors, income = fridayIncome , registrations=fridayRegistrations});
            wr.Add(new WeeklyReport { date= dayOnly.AddDays(6), day = "Saturday", visitors = saturdayVisitors, income = saturdayIncome, registrations=saturdayRegistrations });

            return wr; //returning the final list

        }

        private void SortByIncome_Click(object sender, EventArgs e) //sotring weekly report by income
        {
            List<WeeklyReport> list = GetWeeklyReport();
            int[] array = new int[list.Count];

            int i = 0;
            foreach (var item in list)
            {
                array[i] = item.income;
                i++;
            }

            int[] sortedArray = bubbleSort(array); //bubble sorting
            List<WeeklyReport> sortedList = new List<WeeklyReport>();
            for (int j = 0; j < sortedArray.Length; j++)
            {
                foreach (var ls in list) //arranging the data
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
            weeklyDataGrid.DataSource = sortedList;// displaying in gridlistview
        }

        /// <summary>
        /// method for displaying the checked out visitors date filtered by the given date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
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
        /// <summary>
        /// method for displaying all checked out visitors list in weekly report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
         
            weeklyDataGrid.DataSource = GetCheckedoutVisitors();
        }

        private void GenerateWeeklyReport_Click(object sender, EventArgs e) //generating weekly reprot csv
        {
            string path = "C:/Users/bhara/source/repos/Coursework/WeeklyReport.csv";

            CreateCSV(path, weeklyDataGrid);
            MessageBox.Show("CSV report generated Successfully");
        }

        private void GenerateGraph_Click(object sender, EventArgs e)//generating graph based on total number of visitors
        {
            Graph graphform = new Graph(); //object of next from
            List<WeeklyReport> list = GetWeeklyReport();

            graphform.LoadGraph(list); //method calling for graph
            graphform.Show(); //showing the form

        }

        private void loginButton_Click(object sender, EventArgs e)//method for admin login
        {
            string uname = usernameTxt.Text;
            string password = passwordTxt.Text;

            if (uname == "admin" && password =="admin") //username and password for admin
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

        private void ViewTicket_Click(object sender, EventArgs e) //viewing ticket in admin page
        {
           
            adminTicketGrid.DataSource = GetTicket();

            ticketMessage.Visible = true;

        }

        private void button3_Click_1(object sender, EventArgs e) //viewing ticket in home page
        {
            ticketGrid.DataSource = GetTicket();
        }



        private void searchVisitor_Click(object sender, EventArgs e)//filtering checked out visitors by date and category in daily repprt section
        {
            List<CheckedoutVisitors> vis = new List<CheckedoutVisitors>();
            vis = GetCheckedoutVisitors();


            DateTime checkDate = DateTime.Parse(dailymmCmb.Text + "-" + dailyddCmb.Text + "-" + dailyyyCmb.Text);
            string cat = reportCategory.Text;
            List<CheckedoutVisitors> matched = new List<CheckedoutVisitors>();
            foreach (var item in vis)
            {
                if (checkDate.Date ==item.inDateTime.Date) //checking date
                {
                    if (item.category == cat) //checking category
                    {
                        matched.Add(item);
                    }

                }
            }
            dailyReportGrid.DataSource = matched;

        }

        private void generateGraphByIncome_Click(object sender, EventArgs e) //generating graph based on total income in weekly report section
        {
            GraphbyIncome obj = new GraphbyIncome();
            List<WeeklyReport> list = GetWeeklyReport();

            obj.LoadGraph(list);
            obj.Show();

        }
        /// <summary>
        /// Method to clear text fields
        /// </summary>
        private void clear()
        {
            fullNameTxt.Text = "";
            searchPhoneTxt.Text = "";
            billTxt.Text = "";
            paidBillTxt.Text = "";
            fullNameTxt.Text = "";
            emailTxt.Text = "";
            phoneTxt.Text = "";
            categoryCmb.SelectedIndex = 0;
            totalVisitorsCmb.SelectedIndex = 0;
            childrenCmb.SelectedIndex = 0;
        }
    }
}
