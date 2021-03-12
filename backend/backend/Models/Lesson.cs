using System;

namespace backend.Models
{
    public class Lesson
    {
        public string OrdinalNumber { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Groups { get; set; }
    }
}