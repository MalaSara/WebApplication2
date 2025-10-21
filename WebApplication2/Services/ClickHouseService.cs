using ClickHouse.Client.ADO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using WebApplication2.Options;

namespace WebApplication2.Services
{

    public interface IClickHouseService
    {
        void TestConnection();
    }
    public class ClickHouseService : IClickHouseService
    {

        private readonly ClickHouseOptions _clickHouseOptions;

        public ClickHouseService(IOptions<ClickHouseOptions> clickHouseOptions)
        {
            _clickHouseOptions = clickHouseOptions.Value;
            CreateTablesIfNotExist();
        }


        public void TestConnection()
        {
            try
            {

                using (var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SHOW TABLES FROM my_database";

                //string connectionString = "Host=localhost;Port=8123;Database=my_databse;Username=admin;Password=admin";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader[0]);
                        }
                        
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);

            }
        }


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
      
    }
}
