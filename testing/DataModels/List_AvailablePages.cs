using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing.DataModels
{
    // Used for switching sidebars...
    public enum EPageList
    {
        // No sidebar, Default(N/A), Pages/Sidebar_Home, TextureCombine
        None = 0,
        Default = 1,
        CombineTexture = 2,
        PlayVideo = 3,
        HomePage = 4,
        //
        Sidebar = 5,
        TrafficAnalysis = 6,
        Settings = 7,
        YOLOSample = 8
    }
}
