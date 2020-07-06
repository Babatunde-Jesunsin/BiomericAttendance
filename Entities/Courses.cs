using System;
using System.Collections.Generic;
using System.Text;

namespace Prunedge_User_Administration_Library.Entities
{
    public class Courses
    {
        public int id { get; set; }
        public string CourseTitle { get; set; }
        public string CourseCode { get; set; }
        public int Duration { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        public string lecturerId { get; set; }

    }
}
