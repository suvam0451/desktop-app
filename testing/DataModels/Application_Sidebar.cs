using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing.DataModels
{
    // Used for switching sidebars...
    public enum Application_Sidebar {
        // No sidebar, Default(N/A), Pages/Sidebar_Home, TextureCombine
        None = 0,
        Default = 1,
        HomePage = 2,
        TextureCombiner = 3
    }

    public enum Application_Workload
    {
        None = 0,
        Default = 1
    }
}
