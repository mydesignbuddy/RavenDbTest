﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDbTest
{
    public class Character
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CharacterClass Class { get; set; }
        public Race Race { get; set; }

        public List<Item> Inventory { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
    }

    public class Race
    {
        public string Name { get; set; }
    }

    public class CharacterClass
    {
        public string Name { get; set; }
    }
}
