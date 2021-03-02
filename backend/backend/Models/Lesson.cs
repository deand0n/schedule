using System;

namespace backend.Models
{
    public class Lesson
    {
        public int LessonNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}