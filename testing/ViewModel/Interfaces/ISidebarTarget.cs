using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing.ViewModels
{
    public interface ISidebarTarget
    {
        String ConsoleText { get; set; }
        
        int MaximumPages { get; set; }
        int currentPage { get; set; }
        void ChangePage(int In);
        void NextPage();
    }
}