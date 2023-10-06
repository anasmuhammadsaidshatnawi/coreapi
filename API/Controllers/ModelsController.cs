using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly ICarMakeRepository _repository;
        private readonly IRedisRepository _redisRepository;

        public ModelsController(ICarMakeRepository repository, IRedisRepository redisRepository)
        {
            _repository = repository;
            _redisRepository = redisRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync(int modelyear, string make) {
            CarMake carMake = new(-1, "");

            carMake = await _redisRepository.GetCarMakeByNameAsync(make);
            if (carMake == null || !carMake.IsValidMake)
                carMake = await _repository.GetCarMakeByNameAsync(make);
            if (carMake == null || !carMake.IsValidMake) { return BadRequest($"{make} is incorrect"); }
            var url = $"https://vpic.nhtsa.dot.gov/api/vehicles/GetModelsForMakeIdYear/makeId/{carMake.Id}" +
                $"/modelyear/{modelyear}?format=json";
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.Content != null) 
            {
                CarMakeModelResponse res = JsonConvert.DeserializeObject<CarMakeModelResponse>(response.Content);
                if (res.Results.Any())
                    return Ok(res.Results.Select(m => m.Model_Name));
            }

            return NoContent();
        }   
    }
}