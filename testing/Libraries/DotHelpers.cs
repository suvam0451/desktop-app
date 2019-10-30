using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace testing.Libraries
{
    class DotHelpers
    {
        static public bool BuildConnectivityGraph(Dictionary<int, List<int>> _In) {
            clsGraph lazy = new clsGraph(_In);
            // lazy.BFS(4);
            lazy.BuildConnectivity();
            lazy.WriteFile("any");
            lazy.RunDOT();
            MessageBox.Show("finished generating image...");
            return true;
        }
    }
}
