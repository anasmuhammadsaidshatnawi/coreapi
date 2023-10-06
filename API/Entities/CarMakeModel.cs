namespace API.Entities
{
    public class CarMakeModel
    {
        public int Make_ID { get; set; }
        public string Make_Name { get; set; }
        public int Model_ID { get; set; }
        public string Model_Name { get; set; }
    }

    public class CarMakeModelResponse
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public string SearchCriteria { get; set; }
        public List<CarMakeModel> Results { get; set; }
    }
}
