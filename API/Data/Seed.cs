using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using API.Entities;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedMakes(DataContext context, IConfiguration config)
        {
            try
            {
                //context.CarMakes.ExecuteDelete();
                if (await context.CarMakes.AnyAsync()) return;

                var redisConfigSection = config.GetSection("RedisSettings");
                string filePath = config.GetValue<string>("CSVFilePath");
                string redisKey = redisConfigSection["Key"];
                bool isRedisEnabled = bool.Parse(redisConfigSection["IsEnabled"]);
                ConnectionMultiplexer? redisConnection = null;
                IDatabase? redisDatabase = null;
                bool isRedisConnected = false;

                if (isRedisEnabled) 
                {
                    try
                    {
                        redisConnection = ConnectionMultiplexer.Connect(redisConfigSection["Connection"]);
                        isRedisConnected = redisConnection.IsConnected;
                    }
                    catch (Exception) { }                   
                }
               
                if (isRedisConnected)
                     redisDatabase = redisConnection.GetDatabase(int.Parse(redisConfigSection["DB"]));

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = csv.GetRecords<CSVCareMake>().OrderBy(e => e.make_id).ToList();

                    foreach (var record in records)
                    {
                        context.CarMakes.Add(new CarMake(record.make_id, record.make_name));
                        if (isRedisConnected)
                            redisDatabase.HashSet(redisKey, record.make_name, record.make_id);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
