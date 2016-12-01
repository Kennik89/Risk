using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Model
{
    public class Map
    {
        public List<Shape> countries { get; set; }
        public List<serialLine> connections { get; set; }

        public Map()
        {
            countries = new List<Shape>();
            connections = new List<serialLine>();
        }
    }
}
