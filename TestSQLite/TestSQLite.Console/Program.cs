using System;
using System.Data.SQLite;

namespace TestSQLite.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string connectionString = @"URI=file:.\TestSQLiteDatabase.db";

            string query = "SELECT SQLITE_VERSION()";

            using var con = new SQLiteConnection(connectionString);
            con.Open();

            using var cmd = new SQLiteCommand(query, con);
            string version = cmd.ExecuteScalar().ToString();
            con.Close();

            Console.WriteLine($"SQLite version: {version}");

            var x = 0;
            var showcount = 0;
            //--------------------------------------------
            while (4 > x++)
            {
                con.Open();

                //cmd.CommandText = "DROP TABLE IF EXISTS TestTable";
                //cmd.ExecuteNonQuery();

                //cmd.CommandText = @"CREATE TABLE TestTable(Id VARCHAR(36) PRIMARY KEY, Json TEXT)";
                //cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS TestTable(Id VARCHAR(36) PRIMARY KEY, Json TEXT)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO TestTable(Id, Json) VALUES(@Id, @Json)";
                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@Json", cmd.GetHashCode().ToString());
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                con.Close();
                //--------------------------------------------
                con.Open();
                //cmd.CommandText = "SELECT * FROM TestTable LIMIT 5";
                cmd.CommandText = "SELECT * FROM TestTable";
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine($"{(showcount++).ToString().PadLeft(4, '0')} > {rdr.GetGuid(0)} - {rdr.GetString(1)}");
                }
                con.Close();
            }

            Console.WriteLine($"Regiters={showcount}");

            Console.ReadKey();
        }
    }
}
