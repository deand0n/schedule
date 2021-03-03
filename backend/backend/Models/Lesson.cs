using System;

namespace backend.Models
{
    public class Lesson
    {
        public int LessonNumber { get; set; }
        // Time[0] - start time
        // Time[1] - end time
        public string[] Time { get; set; } = new string[2]; 
        public string Type { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public string Location { get; set; }
    }
}