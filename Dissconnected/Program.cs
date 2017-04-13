using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;


namespace ConsoleApplication3
{
    class Program
    {
        static string conString;
        static void Main(string[] args)
        {
          
            SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
            Console.WriteLine("Enter the Server name");
            build.DataSource = Console.ReadLine();
            Console.WriteLine("Enter the table name");
            build.InitialCatalog= Console.ReadLine();
            Console.WriteLine("Enter the condition for Trusted connection that is true or false");
            build.IntegratedSecurity = bool.Parse(Console.ReadLine());
            conString = build.ConnectionString;
         

            Console.WriteLine("-----------------Welcome to Database-----------------");
            while (true)
            {
                string[] menuOption = { "1. Display Data", "2. Insert Data", "3. Update Data", "4. Delete Data", "5. Display Count" };
                foreach (var item in menuOption)
                {
                    Console.WriteLine(item);
                }
                int choice = GetInt("Enter your Choice");
                switch (choice)
                {
                    case 1:
                        displayData();

                        break;
                    case 2:
                        InsertData();

                        break;
                    case 3:
                        UpdateData();

                        break;
                    case 4:
                        deleteData();

                        break;
                    case 5:
                        DisplayCount();

                        break;
                    default:
                        Console.WriteLine("Invalid Entry!!!!!");
                        break;
                }
                Console.ReadLine();
            }
        }

        private static void DisplayCount()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string selectAllString = @"SELECT COUNT(*) FROM EMPLOYEES";
                using (SqlCommand cmd = new SqlCommand(selectAllString, con))
                {
                    int count = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"Total no. of records = {count}");
                }
            }
        }

        private static void displayData()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(conString, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Console.WriteLine(dr["FirstName"]);
                    }
                }
            }
        }

        private static void UpdateData()
        {
            Console.WriteLine("Enter the id to be updated");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the FirstName to be updated");
            string First = Console.ReadLine();
            Console.WriteLine("Enter the LastName");
            string Last = Console.ReadLine();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(conString, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    SqlCommandBuilder scb = new SqlCommandBuilder(da);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if ((int)dr["EmployeeID"] == id)
                            dr["FirstName"] = First;
                        dr["LastName"] = Last;
                    }
                    da.Update(ds);
                }
            }
        }

        private static void InsertData()
        {

            Console.WriteLine("Enter the EmployeeID to be inserted ");
            string lname = Console.ReadLine();
            Console.WriteLine("Enter the firstname to be inserted ");
            string fname = Console.ReadLine();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(conString, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@FirstName", fname);
                    cmd.Parameters.AddWithValue("@LastName", lname);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    SqlCommandBuilder scb = new SqlCommandBuilder(da);
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["FirstName"] = fname;
                    dr["LastName"] = lname;
                    DataTable dt = ds.Tables[0];
                    dt.Rows.Add(dr);
                    da.Update(ds);
                }
            }
        }

        private static void deleteData()
        {
            Console.WriteLine("Enter the EmployeeID to be deleted ");
            int id = int.Parse(Console.ReadLine());
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(conString, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@EmployeeID", id);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    SqlCommandBuilder scb = new SqlCommandBuilder(da);
                    DataRow dr = ds.Tables[0].NewRow();
                    foreach (DataRow dr1 in ds.Tables[0].Rows)
                    {
                        if ((int)dr1["EmployeeID"] == id)
                            dr.Delete();
                    }
                    da.Update(ds);
                }
            }
        }

        private static int GetInt(string message)
        {
            int val = 0;
            while (true)
            {
                Console.WriteLine(message);
                if (int.TryParse(Console.ReadLine(), out val))
                    break;

                Console.WriteLine("Error! Try again");
            }
            return val;

        }
    }

}
