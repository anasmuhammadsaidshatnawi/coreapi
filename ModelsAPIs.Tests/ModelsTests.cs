using API.Controllers;
using API.Interfaces;
using AutoMapper;
using FakeItEasy;

namespace ModelsAPIs.Tests
{
    public class ModelsTests
    {
        private readonly ICarMakeRepository _repository;
        private readonly IRedisRepository _redisRepository;
        private readonly IMapper _mapper;
        public ModelsTests()
        {
            _repository = A.Fake<ICarMakeRepository>();
            _redisRepository = A.Fake<IRedisRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void ModelsController_Get_OK()
        {
            var carMakes = A.Fake<IEnumerable<string>>();
            A.CallTo(() => _mapper.Map<IEnumerable<string>>(carMakes)).Returns(carMakes);
            var controller = new ModelsController(_repository, _redisRepository);

            var results = controller.GetAsync(2015, "honda").Result;

            Assert.NotNull(results);
        }
    }
}