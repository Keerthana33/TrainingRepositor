using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Speech.Synthesis;

namespace DatabaseApp
{
    class Program
    {
        static string conString = @"Server=INCHCMPC08657;Database = Northwind;trusted_connection = true;";
       public static SpeechSynthesizer speaker = new SpeechSynthesizer();
        static void Main(string[] args)
        {
           
            Console.WriteLine("-----------------Welcome to Database-----------------");
            speaker.Speak("Welcome to Database");
            while (true)
            {
                string[] menuOption = { "1. Display Data", "2. Insert Data", "3. Update Data", "4. Delete Data", "5. Display Count" };
                foreach (var item in menuOption)
                {
                    Console.WriteLine(item);
                }
                speaker.Speak("Please Enter your Choice");
                int choice = GetInt("Enter your Choice");
                switch (choice)
                {
                    case 1:
                        DisplayData();
                       
                        break;
                    case 2:
                        InsertData();
                       
                        speaker.Speak("Content Inserted Succesfully");
                        break;
                    case 3:
                        UpdateData();
                       
                        speaker.Speak("Content Updated Succesfully");
                        break;
                    case 4:
                        DeleteData();
                        
                        speaker.Speak("Content Deleted Succesfully");
                        break;
                    case 5:
                        DisplayCount();
                       
                        break;
                    default:
                        Console.WriteLine("Invalid Entry!!!!!");
                        break;
                }
            }


            Console.ReadLine();


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
                    speaker.Speak($"The row count is {count}");
                }
            }
        }

        private static void DeleteData()
        {
            Console.WriteLine("Enter the LastName to be deleted ");
            string Last = Console.ReadLine();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                string delateString = @"DELETE FROM Employees WHERE LastName=@LastName";
                using (SqlCommand cmd = new SqlCommand(delateString, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                 
                    cmd.Parameters.AddWithValue("@LastName", Last);


                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateData()
        {
            Console.WriteLine("Enter the FirstName to be updated");
            string First = Console.ReadLine();
            Console.WriteLine("Enter the LastName");
            string Last = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                string updateString = @"UPDATE Employees SET FirstName =@FirstName WHERE LastName=@LastName";
                using (SqlCommand cmd = new SqlCommand(updateString, con))
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@FirstName", First);
                    cmd.Parameters.AddWithValue("@LastName", Last);


                    cmd.ExecuteNonQuery();
                }
            }


        }

        private static void InsertData()
        {
            Console.WriteLine("Enter the FirstName");
            string First = Console.ReadLine();
            Console.WriteLine("Enter the LastName");
            string Last= Console.ReadLine();

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                string insertString = @"INSERT INTO Employees (FirstName, LastName) VALUES (@FirstName,@LastName) ";
                using (SqlCommand cmd = new SqlCommand(insertString, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@FirstName",First);
                    cmd.Parameters.AddWithValue("@LastName", Last);
                 
                 
                    cmd.ExecuteNonQuery();
                }
            }

        }

        private static void DisplayData()
        {
            

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string selectAllString = @"SELECT * FROM EMPLOYEES";
                using (SqlCommand cmd = new SqlCommand(selectAllString, con))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Console.WriteLine($"FirstName: {rdr["FirstName"]}  | LastName: {rdr["LastName"]}");
                    }
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
