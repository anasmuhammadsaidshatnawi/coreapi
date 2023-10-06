using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CarMakeRepository : ICarMakeRepository
    {
        private readonly DataContext _context;

        public CarMakeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CarMake> GetCarMakeByNameAsync(string carMakeName)
        {
            if (string.IsNullOrWhiteSpace(carMakeName)) return null;
            return await _context.CarMakes.FirstOrDefaultAsync(e => e.Name == carMakeName.Trim().ToUpper());
        }
    }
}
