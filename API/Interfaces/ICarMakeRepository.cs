using API.Entities;

namespace API.Interfaces
{
    public interface ICarMakeRepository
    {
        Task<CarMake> GetCarMakeByNameAsync(string carMakeName); 
    }
}
