﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MajorDTO
{
    public class MajorReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
