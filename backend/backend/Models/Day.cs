using System.Collections.Generic;

namespace backend.Models
{
    public class Day
    {
        public string Date { get; set; }
        public string DayOfTheWeek { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}