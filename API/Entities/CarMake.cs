namespace API.Entities
{
    public class CarMake
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CarMake(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public bool IsValidMake
        {
            get { return Id > 0; }
        }
    }
}
