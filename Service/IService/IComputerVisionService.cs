﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface  IComputerVisionService
    {
        Task<string> ExtractTextFromImageAsync(string imagePath);
    }
}
