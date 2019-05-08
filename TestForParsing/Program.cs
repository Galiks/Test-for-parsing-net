using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MySql.Data.MySqlClient;

namespace TestForParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            //AngleSharpParsing angleSharp = new AngleSharpParsing();
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //var angleSharpParsingListResult = angleSharp.Parsing();
            //watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds / 1000);
            //Console.WriteLine(angleSharpParsingListResult.Count);
            //foreach (var item in angleSharpParsingListResult)
            //{
            //    Console.WriteLine(item);
            //}
            RestSharpParsing restSharpParsing = new RestSharpParsing();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var list = restSharpParsing.Parsing();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds / 1000);
            Console.WriteLine(list.Count);
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        private static void ORMDatabaseUse()
        {
            using (ShopContext shopContext = new ShopContext())
            {
                shopContext.Database.EnsureDeleted();
                shopContext.Database.EnsureCreated();
                HtmlAgilityPackParsing htmlAgilityPackParsing = new HtmlAgilityPackParsing();              
                foreach (var item in htmlAgilityPackParsing.Parsing())
                {
                    shopContext.Shops.Add(item); 
                }
                shopContext.SaveChanges();
                var result = shopContext.Shops.ToList();
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }              
            }
        }

        private static void DirectDatabaseUse(AngleSharpParsing webClientParsing)
        {
            string connectionString = "server=localhost;user id=root;persistsecurityinfo=True;database=forvisualstudio;password=admin";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();
                string queryForDelete = "TRUNCATE forvisualstudio.shop";
                MySqlCommand command = new MySqlCommand(queryForDelete, connection);
                command.ExecuteNonQuery();

                command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "INSERTDATA";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                List<Shop> shops = webClientParsing.Parsing();
                stopwatch.Stop();
                Console.WriteLine((stopwatch.ElapsedMilliseconds / 1000.0).ToString());
                foreach (var item in shops)
                {
                    Console.WriteLine(item.ToString());

                    var name = new MySqlParameter("?shopname", MySqlDbType.VarChar)
                    {
                        Value = item.Name
                    };
                    var discount = new MySqlParameter("?discount", MySqlDbType.Double)
                    {
                        Value = item.Discount
                    };
                    var label = new MySqlParameter("?label", MySqlDbType.VarChar)
                    {
                        Value = item.Label
                    };
                    var image = new MySqlParameter("?image", MySqlDbType.VarChar)
                    {
                        Value = item.Image
                    };
                    var url = new MySqlParameter("?url", MySqlDbType.VarChar)
                    {
                        Value = item.URL
                    };
                    command.Parameters.AddRange(new MySqlParameter[] { name, discount, label, image, url });

                    command.ExecuteScalar();

                    command.Parameters.Clear();
                }
            }
        }
    }

}
