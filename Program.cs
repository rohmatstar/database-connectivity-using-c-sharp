using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Transactions;

public class Program
{
    public static void Main(string[] args)
    {
        /*GetAllRegions();*/
        /*InsertRegion("Antartica");*/
        /*GetRegion(1);*/
        /*UpdateRegion(1, "Antartica");*/
        /*DeleteRegion(15);*/

        /*GetAllCountries();*/
        /*InsertCountry("11", "Jamaika", 2);*/
        /*GetCountry("1");*/
        /*UpdateCountry("11", "Honduras", 2);*/
        /*DeleteCountry("11");*/

        /*GetAllDepartments();*/
        /*GetAllEmployees();*/
        /*GetAllJobs();*/
        /*GetAllLocations();*/
        GetAllHistories();

        Console.ReadLine();
    }

    // CONNECTION
    static SqlConnection Connect()
    {
        string connectionString;
        connectionString = "Data Source=AHN-YU-JIN;Database=db_hr_mcc;Integrated Security=True;Connect Timeout=30;";

        SqlConnection connect;
        connect = new SqlConnection(connectionString);
        return connect;
    }

    // CLASS OBJECT
    private class Region
    {
        public int id { get; set; }
        public string name { get; set; }

    }

    private class Country
    {
        public string id { get; set; }
        public string name { get; set; }
        public int region { get; set; }

    }
    private class Departments
    {
        public int id { get; set; }
        public string name { get; set; }
        public int loc { get; set; }
        public int manager { get; set; }

    }

    private class Employees
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime hiredate { get; set; }
        public int salary { get; set; }
        public decimal comission { get; set; }
        public int manager { get; set; }
        public string job { get; set; }
        public int department{ get; set; }
    }
    private class Jobs
    {
        public string id { get; set; }
        public string title { get; set; }
        public int min { get; set; }
        public int max { get; set; }

    }

    private class Locations
    {
        public int id { get; set; }
        public string street { get; set; }
        public string postal { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country{ get; set; }
    }

    private class Histories
    {
        public DateTime startdate { get; set; }
        public int emp_id { get; set; }
        public DateTime enddate { get; set; }
        public int dep_id { get; set; }
        public string job_id { get; set; }
    }

    // METHOD

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

                    Console.WriteLine("ID: " + reg.id + ", Name: " + reg.name);
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
    static List<Region> GetRegion(int id)
    {
        SqlConnection connect = Connect();
        connect.Open();

        var region = new List<Region>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_m_regions WHERE id = @region_id";

            SqlParameter id_param = new SqlParameter();
            id_param.ParameterName = "@region_id";
            id_param.Value = id;
            id_param.SqlDbType = SqlDbType.Int;

            command.Parameters.Add(id_param);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var reg = new Region();
                    reg.id = reader.GetInt32(0);
                    reg.name = reader.GetString(1);

                    Console.WriteLine("ID: " + reg.id + ", Name: " + reg.name);
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
    public static void InsertRegion(string name)
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
        if(result > 0)
        {
            Console.WriteLine("Inserted!");
        }
        else
        {
            Console.WriteLine("Failed when Insert!");
        }
    }
    public static void UpdateRegion(int id, string name)
    {
        int result = 0;

        SqlConnection connect = Connect();
        connect.Open();

        SqlTransaction transaction = connect.BeginTransaction();
        try
        {

            SqlCommand command = new SqlCommand();
            command.Connection = connect;
            command.CommandText = "UPDATE tb_m_regions SET name = @region_name WHERE id = @region_id";
            command.Transaction = transaction;

            SqlParameter param_id = new SqlParameter();
            param_id.ParameterName = "@region_id";
            param_id.Value = id;
            param_id.SqlDbType = SqlDbType.Int;

            command.Parameters.Add(param_id);

            SqlParameter param_name = new SqlParameter();
            param_name.ParameterName = "@region_name";
            param_name.Value = name;
            param_name.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(param_name);

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
        if (result > 0)
        {
            Console.WriteLine("Updated!");
        }
        else
        {
            Console.WriteLine("Failed when Update!");
        }
    }
    public static void DeleteRegion(int id)
    {
        int result = 0;

        SqlConnection connect = Connect();
        connect.Open();

        SqlTransaction transaction = connect.BeginTransaction();
        try
        {

            SqlCommand command = new SqlCommand();
            command.Connection = connect;
            command.CommandText = "DELETE tb_m_regions WHERE id = @region_id";
            command.Transaction = transaction;

            SqlParameter param_id = new SqlParameter();
            param_id.ParameterName = "@region_id";
            param_id.Value = id;
            param_id.SqlDbType = SqlDbType.Int;

            command.Parameters.Add(param_id);

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
        if (result > 0)
        {
            Console.WriteLine("Deleted!");
        }
        else
        {
            Console.WriteLine("Failed when Delete!");
        }
    }


    static List<Country> GetAllCountries()
    {
        SqlConnection connect = Connect();
        var country = new List<Country>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_m_countries ORDER BY id";

            connect.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var con = new Country();
                    con.id = reader.GetString(0);
                    con.name = reader.GetString(1);
                    con.region = reader.GetInt32(2);

                    Console.WriteLine("ID: " + con.id + ", Name: " + con.name + ", Region: " + con.region);
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
        return country;
    }
    static List<Country> GetCountry(string id)
    {
        SqlConnection connect = Connect();
        connect.Open();

        var country = new List<Country>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_m_countries WHERE id = @country_id";

            SqlParameter id_param = new SqlParameter();
            id_param.ParameterName = "@country_id";
            id_param.Value = id;
            id_param.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(id_param);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var con = new Country();
                    con.id = reader.GetString(0);
                    con.name = reader.GetString(1);
                    con.region = reader.GetInt32(2);

                    Console.WriteLine("ID: " + con.id + ", Name: " + con.name + ", Region: " + con.region);
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
        return country;
    }
    public static void InsertCountry(string id, string name, int region_id)
    {
        int result = 0;

        SqlConnection connect = Connect();
        connect.Open();

        SqlTransaction transaction = connect.BeginTransaction();
        try
        {

            SqlCommand command = new SqlCommand();
            command.Connection = connect;
            command.CommandText = "INSERT INTO tb_m_countries (id, name, region_id) VALUES(@id, @country_name, @region_id)";
            command.Transaction = transaction;

            SqlParameter param_id = new SqlParameter();
            param_id.ParameterName = "@id";
            param_id.Value = id;
            param_id.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(param_id);

            SqlParameter param_name = new SqlParameter();
            param_name.ParameterName = "@country_name";
            param_name.Value = name;
            param_name.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(param_name);

            SqlParameter param_region_id = new SqlParameter();
            param_region_id.ParameterName = "@region_id";
            param_region_id.Value = region_id;
            param_region_id.SqlDbType = SqlDbType.Int;

            command.Parameters.Add(param_region_id);

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
        if (result > 0)
        {
            Console.WriteLine("Inserted!");
        }
        else
        {
            Console.WriteLine("Failed when Insert!");
        }
    }
    public static void UpdateCountry(string id, string name, int region_id)
    {
        int result = 0;

        SqlConnection connect = Connect();
        connect.Open();

        SqlTransaction transaction = connect.BeginTransaction();
        try
        {

            SqlCommand command = new SqlCommand();
            command.Connection = connect;
            command.CommandText = "UPDATE tb_m_countries SET name = @country_name, region_id = @region_id WHERE id = @country_id";
            command.Transaction = transaction;

            SqlParameter param_id = new SqlParameter();
            param_id.ParameterName = "@country_id";
            param_id.Value = id;
            param_id.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(param_id);

            SqlParameter param_name = new SqlParameter();
            param_name.ParameterName = "@country_name";
            param_name.Value = name;
            param_name.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(param_name);

            SqlParameter param_region_id = new SqlParameter();
            param_region_id.ParameterName = "@region_id";
            param_region_id.Value = region_id;
            param_region_id.SqlDbType = SqlDbType.Int;

            command.Parameters.Add(param_region_id);

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
        if (result > 0)
        {
            Console.WriteLine("Updated!");
        }
        else
        {
            Console.WriteLine("Failed when Update!");
        }
    }
    public static void DeleteCountry(string id)
    {
        int result = 0;

        SqlConnection connect = Connect();
        connect.Open();

        SqlTransaction transaction = connect.BeginTransaction();
        try
        {

            SqlCommand command = new SqlCommand();
            command.Connection = connect;
            command.CommandText = "DELETE tb_m_countries WHERE id = @country_id";
            command.Transaction = transaction;

            SqlParameter param_id = new SqlParameter();
            param_id.ParameterName = "@country_id";
            param_id.Value = id;
            param_id.SqlDbType = SqlDbType.VarChar;

            command.Parameters.Add(param_id);

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
        if (result > 0)
        {
            Console.WriteLine("Deleted!");
        }
        else
        {
            Console.WriteLine("Failed when Delete!");
        }
    }

    static List<Country> GetAllDepartments()
    {
        SqlConnection connect = Connect();
        var country = new List<Country>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_m_departments";

            connect.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var con = new Departments();
                    con.id = reader.GetInt32(0);
                    con.name = reader.GetString(1);
                    con.loc = reader.GetInt32(2);
                    con.manager = reader.GetInt32(3);

                    Console.WriteLine("ID: " + con.id + ", Name: " + con.name + ", Location: " + con.loc + ", Manager: " + con.manager);
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
        return country;
    }

    static List<Employees> GetAllEmployees()
    {
        SqlConnection connect = Connect();
        var employees = new List<Employees>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_m_employees";

            connect.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var data = new Employees();
                    data.id = reader.GetInt32(0);
                    data.first_name = reader.GetString(1);
                    data.last_name = reader.GetString(2);
                    data.email = reader.GetString(3);
                    data.phone = reader.GetString(4);
                    data.hiredate = reader.GetDateTime(5);
                    data.salary = reader.GetInt32(6);
                    data.comission = reader.GetDecimal(7);
                    data.manager = reader.GetInt32(8);
                    data.job = reader.GetString(9);
                    data.department = reader.GetInt32(10);

                    Console.WriteLine("ID: " + data.id + "\nName: " + data.first_name + " " + data.last_name + "\nEmail: " + data.email + "\nPhone: " + data.phone + "\nHired: " + data.hiredate + "\nSalary: " + data.salary + "\nComission: " + data.comission + "\nManager: " + data.manager + "\nJob: " + data.job + "\nDepartment: " + data.department + "\n");
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
        return employees;
    }

    static List<Jobs> GetAllJobs()
    {
        SqlConnection connect = Connect();
        var jobs = new List<Jobs>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_m_jobs";

            connect.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var data = new Jobs();
                    data.id = reader.GetString(0);
                    data.title = reader.GetString(1);
                    data.min = reader.GetInt32(2);
                    data.max = reader.GetInt32(3);

                    Console.WriteLine("ID: " + data.id + ", Title: " + data.title + ", Min Salary: " + data.min + ", Max Salary: " + data.max);
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
        return jobs;
    }

    static List<Locations> GetAllLocations()
    {
        SqlConnection connect = Connect();
        var jobs = new List<Locations>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_m_locations";

            connect.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var data = new Locations();
                    data.id = reader.GetInt32(0);
                    data.street = reader.GetString(1);
                    data.postal = reader.GetString(2);
                    data.city = reader.GetString(3);
                    data.state = reader.GetString(4);
                    data.country = reader.GetString(5);

                    Console.WriteLine("ID: " + data.id + "\nStreet: " + data.street + "\nPostal: " + data.postal + "\nCity: " + data.city + "\nState: " + data.state + "\nCountry: " + data.country + "\n");
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
        return jobs;
    }

    static List<Histories> GetAllHistories()
    {
        SqlConnection connect = Connect();
        var history = new List<Histories>();
        try
        {
            //Command
            SqlCommand command = connect.CreateCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM tb_tr_histories";

            connect.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var data = new Histories();
                    data.startdate = reader.GetDateTime(0);
                    data.emp_id = reader.GetInt32(1);
                    data.enddate = reader.GetDateTime(2);
                    data.dep_id = reader.GetInt32(3);
                    data.job_id = reader.GetString(4);

                    Console.WriteLine("Start Date: " + data.startdate + "\nEmployee ID: " + data.emp_id + "\nEnd Date: " + data.enddate + "\nDepartment ID: " + data.dep_id + "\n Job ID: " + data.job_id + "\n");
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
        return history;
    }
}