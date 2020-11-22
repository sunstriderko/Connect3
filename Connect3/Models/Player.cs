using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Animation;

namespace Connect3.Models
{
    public class Player
    {
        public string Name { get; set; }

        public double WinCount { get; set; } = 0;

        public Player(string aName)
        {
            Name = aName;
        }

    }
}
