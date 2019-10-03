using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing.Models
{
    public class SidebarModel : testing.BaseViewModel
    {
        public String ImageLocation { get; set; }
        public String DisplayName { get; set; } = "DisplayName N/A";

        public SidebarModel(String Name) {
            DisplayName = Name;
        }
    }

    public class StringItemModel : testing.BaseViewModel {

        public String Whatever { get; set; } = "kawaii";

        public StringItemModel(String Name)
        {
            Whatever = Name;
        }
        public override string ToString()
        {
            return "Uncover you";
        }
    }
}
