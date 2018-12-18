using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;

namespace CrystalReportWindowsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var crystalReport = new CrystalReport1();
            var datatable = GetData();
            crystalReport.SetDataSource(datatable);
            this.crystalReportViewer1.ReportSource = crystalReport;
            this.crystalReportViewer1.RefreshReport();

        }

        private DataSet1 GetData()
        {
            string constr = @"Data Source=C229\SQLEXPRESS2014;Initial Catalog=Test;Persist Security Info=True;User ID=sa;Password=admin123!@#";
            var reportDataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"select e.EmployeeID as EmployeeID, e.Name as EmployeeName, r.Name as RoleName, d.Name as DepartmentName, e.Address as Address, c.City as City, e.Pincode as Pincode from Employee e
                Inner Join City c on e.CityID = c.CityID
                Inner Join Department d on d.DepartmentID = e.DepartmentID
                Inner Join Role r on r.RoleID = e.RoleID";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        //sda.Fill(reportDataTable);
                        //return reportDataTable;
                        using (DataSet1 dsCustomers = new DataSet1())
                        {
                            sda.Fill(dsCustomers, "DataTable1");
                            return dsCustomers;
                        }
                    }
                }
            }
        }
    }
}
