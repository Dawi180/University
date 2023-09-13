﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Interfaces;

namespace University.Models
{
    public class CourseExam : ICourseExam
    {
        public long CourseExamId { get; set; }

        public long ExamId { get; set; }
        public virtual Exam? Exam { get; set; }

        public long CourseId { get; set; }
        public virtual Course? Course { get; set; }
    }
}
