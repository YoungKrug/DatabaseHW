using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace DataBaseTesting
{
    public class DBConnection
    {
        private DBConnection()
        {
        }

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                MySqlConnectionStringBuilder connStringBuilder = new MySqlConnectionStringBuilder();
                connStringBuilder.Server = "localhost";
                connStringBuilder.UserID = "root";
                connStringBuilder.Password = "123456";
                connStringBuilder.Database = databaseName;              
                connStringBuilder.Port = 3306;
                //string connstring = string.Format("Server=localhost; database={0}; UID=root; password=123456", databaseName);
                connection = new MySqlConnection(connStringBuilder.ToString());
                try
                {
                    connection.Open();
                }
                catch
                {
                    Console.Write("Cannot connect");
                    return false;
                }
            }

            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }

    class Program
    {
        //IT WORKS
        static void Main(string[] args)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "studentgrades";
            if (dbCon.IsConnect())
            {
                //int choosenNum = 0;
                //suppose col0 and col1 are defined as VARCHAR in the DB
                Console.Write("Enter the a class number and we will return the ones that are greater: ");
                string text =  Console.ReadLine();
                Console.Write("\n");
                string query = "Select c.num,c.classID, c.prefix from Class c where c.num >" + text;
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string someStringFromColumnZero = reader.GetString(i);
                        if (i != reader.FieldCount - 1)
                            Console.Write(someStringFromColumnZero + ",");
                        else
                            Console.Write(someStringFromColumnZero);
                    }
                    Console.WriteLine("\n");
                }
            }
                dbCon.Close();
            Console.Write("Press any button to continue...");
            Console.ReadKey();
        }
    }
}

