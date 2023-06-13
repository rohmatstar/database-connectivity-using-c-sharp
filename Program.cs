using System.Data;
using System.Data.SqlClient;
using System.Transactions;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Enter New Region: ");
        string region_name = Console.ReadLine();
        bool isInsertSuccess = InsertRegion(region_name);
        if (isInsertSuccess)
        {
            List<Region> regions = GetAllRegions();
            foreach (Region region in regions)
            {
                Console.WriteLine("ID: " + region.id + ", Name: " + region.name);
            }
        }
        else
        {
            Console.WriteLine("Eror when Insert");
        }
        Console.ReadLine();
    }

    private class Region
    {
        public int id { get; set; }
        public string name { get; set; }

    }

    static SqlConnection Connect()
    {
        string connectionString;
        connectionString = "Data Source=AHN-YU-JIN;Database=db_hr_mcc;Integrated Security=True;Connect Timeout=30;";

        SqlConnection connect;
        connect = new SqlConnection(connectionString);
        return connect;
    }
    static List<Region> GetAllRegions()
    {
        SqlConnection connect = Connect();
        var region = new List<Region>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_m_regions";

            connect.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var reg = new Region();
                    reg.id = reader.GetInt32(0);
                    reg.name = reader.GetString(1);

                    region.Add(reg);
                }
            }
            else
            {
                Console.WriteLine("No Data Found!");
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }
        connect.Close();
        return region;
    }

    public static bool InsertRegion(string name)
    {
        int result = 0;

        SqlConnection connect = Connect();
        connect.Open();

        SqlTransaction transaction = connect.BeginTransaction();
        try
        {

            SqlCommand command = new SqlCommand();
            command.Connection = connect;
            command.CommandText = "INSERT INTO tb_m_regions (name) VALUES(@region_name)";
            command.Transaction = transaction;

            //SqlParameter PName = new SqlParameter("@region_name", name);
            //command.Parameters.Add(PName);

            SqlParameter pName = new SqlParameter();
            pName.ParameterName = "@region_name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(pName);

            result = command.ExecuteNonQuery();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            try
            {
                transaction.Rollback();
            }
            catch (Exception rollback)
            {
                Console.WriteLine(rollback.Message);
            }
        }
        connect.Close();
        return result > 0;
    }
}