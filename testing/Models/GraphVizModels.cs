using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing.Models
{
    public enum GraphViz_Engine { 
        Dot = 0
    }
    public class GraphViz_Settings : testing.BaseViewModel
    {
        public GraphViz_Engine Engine { get; set; } = GraphViz_Engine.Dot;
        public bool AllowOverlap { get; set; } = false;
        public int FontSize { get; set; } = 14;

        public GraphViz_Settings() {}
    }
}
