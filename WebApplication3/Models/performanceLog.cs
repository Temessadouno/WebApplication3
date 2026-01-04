namespace WebApplication3.Models;

public class performanceLog
{
        public int Id { get; set; }
        public string Page { get; set; }
        public string Methode { get; set; }
        public long TempsMs { get; set; }
        public DateTime Date { get; set; }
    }

