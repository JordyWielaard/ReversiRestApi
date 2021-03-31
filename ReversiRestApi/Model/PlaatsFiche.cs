using System;
namespace ReversiRestApi.Model
{
    public struct PlaatsFiche
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string SpelerToken { get; set; }
        public string SpelToken { get; set; }
        public bool Pas { get; set; }

    }
}