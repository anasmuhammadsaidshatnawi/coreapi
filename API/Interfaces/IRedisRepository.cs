using API.Entities;

namespace API.Interfaces
{
    public interface IRedisRepository
    {
        Task<CarMake> GetCarMakeByNameAsync(string carMakeName);
    }
}
