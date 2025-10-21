using ClickHouse.Client.ADO;
using Microsoft.AspNetCore.Http.HttpResults;
<<<<<<< HEAD
using Microsoft.Extensions.Options;
using WebApplication2.Options;
=======
>>>>>>> d8755cb6b101e3725d3ccc7528080c79e3058c94

namespace WebApplication2.Services
{

    public interface IClickHouseService
    {
        void TestConnection();
    }
    public class ClickHouseService : IClickHouseService
    {

<<<<<<< HEAD
        private readonly ClickHouseOptions _clickHouseOptions;

        public ClickHouseService(IOptions<ClickHouseOptions> clickHouseOptions)
        {
            _clickHouseOptions = clickHouseOptions.Value;
            CreateTablesIfNotExist();
        }

=======
>>>>>>> d8755cb6b101e3725d3ccc7528080c79e3058c94
        public void TestConnection()
        {
            try
            {
<<<<<<< HEAD
                using (var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SHOW TABLES FROM my_database";
=======
                string connectionString = "Host=localhost;Port=8123;Database=my_databse;Username=admin;Password=admin";
                using (var connection = new ClickHouseConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM system.tables LIMIT 10";
>>>>>>> d8755cb6b101e3725d3ccc7528080c79e3058c94

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
<<<<<<< HEAD
                            Console.WriteLine(reader[0]);  
=======
                            Console.WriteLine(reader[0]);  // Display the first column
>>>>>>> d8755cb6b101e3725d3ccc7528080c79e3058c94
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);

            }
        }

<<<<<<< HEAD

        public void CreateTablesIfNotExist()
        {
            try
            {

                using (var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString))
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var command = connection.CreateCommand();
                    command.CommandText = "create table if not exists my_database.customers " +
                        "(customerId int," +
                        "name String," +
                        "registrationDate DateTime" +
                        ") engine = MergeTree" +
                        " primary key customerId ";

                    var reader = command.ExecuteReader();


                    command = connection.CreateCommand();
                    command.CommandText = "create table if not exists my_database.orders" +
                        "(orderId int," +
                        "customerId int," +
                        "productId String," +
                        "orderDate DateTime," +
                        "quanity int" +
                        ") engine = MergeTree" +
                        " primary key orderId";

                    reader = command.ExecuteReader();

                    command = connection.CreateCommand();
                    command.CommandText = "create table if not exists my_database.products" +
                        "(productId String," +
                        "name String," +
                        "category String," +
                        "price double" +
                        ") engine = MergeTree" +
                        " primary key productId"; ;

                    reader = command.ExecuteReader();

                   

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

=======
>>>>>>> d8755cb6b101e3725d3ccc7528080c79e3058c94
      
    }
}
