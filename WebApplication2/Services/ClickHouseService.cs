using ClickHouse.Client.ADO;
using ClickHouse.Client.ADO.Parameters;
using ClickHouse.Client.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using WebApplication2.Models;
using WebApplication2.Options;

namespace WebApplication2.Services
{

    public interface IClickHouseService
    {
        void TestConnection();

        Task InsertCustomer(Customer c);
        Task InsertOrder(Order o);
        Task InsertProduct(Product p);

        Task<List<Dictionary<string, object>>> GetAllCustomerslAsync();

        Task<List<Dictionary<string, object>>> GetAllOrderslAsync();

        Task<List<Dictionary<string, object>>> GetAllProductslAsync();

        // analytic
        Task<List<Dictionary<string, object>>> TotalSalesPerCustomer();

        Task<List<Dictionary<string, object>>> Top10MostProfitableProducts();

        Task<List<Dictionary<string, object>>> SalesPerDay();

        Task<List<Dictionary<string, object>>> SalesPerMonth();

        Task<List<Dictionary<string, object>>> Top20CustomerPerAverageSales();
        Task<List<Dictionary<string, object>>> TopSaleProductPerMonth();

    }
    public class ClickHouseService : IClickHouseService
    {

        private readonly ClickHouseOptions _clickHouseOptions;
        private readonly ClickHouseConnection _clickHouseConnection;

        public ClickHouseService(IOptions<ClickHouseOptions> clickHouseOptions)
        {
            _clickHouseOptions = clickHouseOptions.Value;
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


                    CreateTablesIfNotExist();

                    using var reader = command.ExecuteReader();
                    Console.WriteLine("Tables in database 'my_database':");
                    while (reader.Read())
                    {
                        Console.WriteLine("- " + reader.GetString(0));

                    }

                    connection.Close();
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
                        "description String," +
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

        #region Insert

        public async Task InsertCustomer(Customer c)
        {
            string sql = @"
            INSERT INTO my_database.customers (customerid, name, registartionDate)
            VALUES (@id, @name, @email)
            ";

            using var cmd = _clickHouseConnection.CreateCommand();
            cmd.CommandText = sql;
        
            cmd.AddParameter("customerId", c.customerId);
            cmd.AddParameter("name", c.name);
            cmd.AddParameter("customerId", c.registrationDate);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task InsertOrder(Order o)
        {
            string sql = @"
            INSERT INTO my_database.orders (orderId, customerId, productId, orderDate, quanity)
            VALUES (@orderId, @customerId, @productId, @orderDate, @quanity)";

            using var cmd = _clickHouseConnection.CreateCommand();
            cmd.CommandText = sql;

            cmd.AddParameter("orderId", o.orderId);
            cmd.AddParameter("customerId", o.customerId);
            cmd.AddParameter("productId", o.productId);
            cmd.AddParameter("orderDate", o.orderDate);
            cmd.AddParameter("quanity", o.quanity);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task InsertProduct(Product p)
        {
            string sql = @"
            INSERT INTO my_database.products (productId, description, category, price)
            VALUES (@productId, @description, @category, @price";

            using var cmd = _clickHouseConnection.CreateCommand();
            cmd.CommandText = sql;

            cmd.AddParameter("productId", p.productId);
            cmd.AddParameter("description", p.description);
            cmd.AddParameter("category", p.category);
            cmd.AddParameter("price", p.price);

            await cmd.ExecuteNonQueryAsync();
        }

        #endregion


        #region GetAll

        public async Task<List<Dictionary<string,object>>> GetAllCustomerslAsync()
        {
            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM my_database.customers ORDER BY customerId";

            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        
        }


        public async Task<List<Dictionary<string, object>>> GetAllOrderslAsync()
        {
            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM my_database.orders ORDER BY orderId";

            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        }


        public async Task<List<Dictionary<string, object>>> GetAllProductslAsync()
        {
            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM my_database.products ORDER BY productId";

            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        }

        #endregion

        #region Analytic

        public async Task<List<Dictionary<string, object>>> TotalSalesPerCustomer()
        {
            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT
                c.customerId,
                SUM(p.price) AS total_spent,
                COUNT(*) AS order_count,
                AVG(p.price) AS avg_order_value
                FROM my_database.products AS p
                LEFT JOIN my_database.orders AS o ON o.productId = p.productId
                LEFT JOIN my_database.customers AS c ON c.customerId = o.customerId
                GROUP BY c.customerId
                ORDER BY total_spent DESC
                ";

            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        }

        public async Task<List<Dictionary<string, object>>> Top10MostProfitableProducts()
        {

            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
            SELECT
                p.productId,
                p.description,
                p.category,
                SUM(o.quanity) AS total_sold_units,
                SUM(o.quanity * p.price) AS total_revenue
            FROM my_database.products AS p
            LEFT JOIN my_database.orders AS o ON o.productId = p.productId
            GROUP BY p.productId, p.description, p.category
            ORDER BY total_revenue DESC
            LIMIT 10
            ";

            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        }

        public async Task<List<Dictionary<string, object>>> SalesPerDay()
        {
            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand(); 

            cmd.CommandText = @"
            SELECT
                toDate(o.orderDate) AS day,
                SUM(o.quanity * p.price) AS daily_revenue,
                COUNT(DISTINCT o.orderId) AS orders_count
            FROM my_database.orders AS o
            LEFT JOIN my_database.products AS p ON p.productId = o.productId
            GROUP BY day
            ORDER BY day
            ";


            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        }

        public async Task<List<Dictionary<string, object>>> SalesPerMonth()
        {
            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
            SELECT
            toStartOfMonth(o.orderDate) AS month,
            SUM(o.quanity * p.price) AS monthly_revenue,
            COUNT(DISTINCT o.orderId) AS monthly_orders
            FROM my_database.orders AS o
            LEFT JOIN my_database.products AS p ON p.productId = o.productId
            GROUP BY month
            ORDER BY month
            ";


            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        }

        public async Task<List<Dictionary<string, object>>> Top20CustomerPerAverageSales()
        {
            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
            SELECT
                c.customerId,
                c.name,
                AVG(p.price * o.quanity) AS avg_order_value
            FROM my_database.customers AS c
            LEFT JOIN my_database.orders AS o ON o.customerId = c.customerId
            LEFT JOIN my_database.products AS p ON p.productId = o.productId
            GROUP BY c.customerId, c.name
            ORDER BY avg_order_value DESC
            LIMIT 20
            ";


            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        }

        public async Task<List<Dictionary<string, object>>> TopSaleProductPerMonth()
        {
            using var connection = new ClickHouseConnection(_clickHouseOptions.ConnectionString);
            using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
            SELECT
                c.customerId,
                c.name,
                p.productId,
                p.description AS product_description,
                SUM(o.quanity) AS total_quanity
            FROM my_database.customers AS c
            LEFT JOIN my_database.orders AS o ON o.customerId = c.customerId
            LEFT JOIN my_database.products AS p ON p.productId = o.productId
            GROUP BY c.customerId, c.name, p.productId, p.description
            ORDER BY c.customerId, total_quanity DESC
            ";

            using var reader = await cmd.ExecuteReaderAsync();

            return await GetRowsForQueryAsync(reader);
        }

        #endregion


        private  static async Task<List<Dictionary<string, object>>> GetRowsForQueryAsync(DbDataReader reader)
        {
            var dt = new DataTable();


            for (int i = 0; i < reader.FieldCount; i++)
            {
                dt.Columns.Add(reader.GetName(i), typeof(object));
            }

            // Read rows manually
            while (await reader.ReadAsync())
            {
                var row = dt.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader.IsDBNull(i) ? null : reader.GetValue(i); // safe for nulls
                }
                dt.Rows.Add(row);
            }

            var rows = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                var dict = new Dictionary<string, object>(); 
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = dr[col];
                }
                rows.Add(dict);
            }

            return rows;
        }

    }
}
