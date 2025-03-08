using ClickHouse.Client.ADO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication2.Services
{

    public interface IClickHouseService
    {
        void TestConnection();
    }
    public class ClickHouseService : IClickHouseService
    {

        public void TestConnection()
        {
            try
            {
                string connectionString = "Host=localhost;Port=8123;Database=my_databse;Username=admin;Password=admin";
                using (var connection = new ClickHouseConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM system.tables LIMIT 10";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader[0]);  // Display the first column
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);

            }
        }

      
    }
}
