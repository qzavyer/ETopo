using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETopo
{
    public enum CgnType
    {
        Stone = 1
    }
    class Cgn
    {
        public string Name { get; set; }
        public Point Point { get; set; }
        public CgnType Type { get; set; }
    }
}
