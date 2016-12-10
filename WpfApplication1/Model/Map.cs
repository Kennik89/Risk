using System.Collections.Generic;

namespace Model
{
    public class Map
    {
        public List<Shape> Countries { get; set; }
        public List<serialLine> Connections { get; set; }

        public Map()
        {
            Countries = new List<Shape>();
            Connections = new List<serialLine>();
        }
    }
}
