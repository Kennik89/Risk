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
        public List<Line> connections { get; set; }

    }
}
