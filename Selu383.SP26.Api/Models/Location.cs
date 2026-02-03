namespace Selu383.SP26.Api.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;        // required, max 120

        public string Address { get; set; } = string.Empty;     // required

        public int TableCount { get; set; }                     // >= 1
    }
}