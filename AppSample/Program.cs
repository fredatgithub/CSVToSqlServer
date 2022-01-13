using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace AppSample
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      ReadFile();
    }

    public static void ReadFile()
    {
      try
      {
        DataTable table = new DataTable();
        table = GetTable();

        // Create an instance of StreamReader to read from a file.
        // The using statement also closes the StreamReader.
        using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + @"\Countries.txt"))
        {
          string line;
          int i = 1;

          // Read and display lines from the file until 
          // the end of the file is reached. 
          while ((line = sr.ReadLine()) != null)
          {
            table.Rows.Add(i, line);
            Console.WriteLine(line);
            i++;
          }
        }
        //Insert datatable to sql Server
        Insert(table);
      }
      catch (Exception e)
      {
        // Let the user know what went wrong.
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
      }
      Console.ReadKey();
    }

    private static DataTable GetTable()
    {
      DataTable table = new DataTable();
      table.Columns.Add("idCountry", typeof(short));
      table.Columns.Add("name", typeof(string));
      return table;
    }

    private static void Insert(DataTable dtData)
    {
      SqlConnection con = new SqlConnection(@"Data Source=PCName\SQLSERVER;Initial Catalog=NetSamples;Integrated Security=True");
      SqlCommand cmd = new SqlCommand("InsertCountries", con);
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.AddWithValue("@dtCountry", dtData);
      cmd.Connection = con;
      try
      {
        con.Open();
        cmd.ExecuteNonQuery();
        Console.WriteLine("Records inserted successfully!");
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        con.Close();
        con.Dispose();
      }
    }
  }
}
