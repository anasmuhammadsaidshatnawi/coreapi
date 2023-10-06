using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace API.Data
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IConfiguration _config;
        public RedisRepository(IConfiguration config)
        {
            _config = config;
        }
        public Task<CarMake> GetCarMakeByNameAsync(string carMakeName)
        {
            if (string.IsNullOrWhiteSpace(carMakeName)) return null;

            var redisConfigSection = _config.GetSection("RedisSettings");
            string redisKey = redisConfigSection["Key"];
            bool isRedisEnabled = bool.Parse(redisConfigSection["IsEnabled"]);
            ConnectionMultiplexer? redisConnection = null;
            bool isRedisConnected = false; ;

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
            {
                IDatabase redisDatabase = redisConnection.GetDatabase(int.Parse(redisConfigSection["DB"]));
                string value = redisDatabase.HashGet(redisKey, carMakeName.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(value))
                    return Task.FromResult(new CarMake(int.Parse(value), carMakeName.Trim().ToUpper()));
            }

            return Task.FromResult(new CarMake(-1, ""));
        }
    }
}
