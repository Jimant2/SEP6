namespace SEP6.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string title { get; set; }
        public int year { get; set; }
        public Ratings rating { get; set; }
        public List<Directors> directors { get; set; }
        public List<Stars> stars { get; set; }       
        public int birth { get; set; }
        public string? Poster { get; set; }
        public string? Type { get; set; }

    }
}
