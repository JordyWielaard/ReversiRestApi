﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Model
{
    public class Coordinaten
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinaten(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
