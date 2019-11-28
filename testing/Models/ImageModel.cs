using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing.ViewModels;

namespace testing.Models 
{
    public class ImageObject : BaseViewModel
    {
        public string location { get; set; }
        public string displayName { get; set; } = "DisplayName N/A";

        public override string ToString()
        {
            return displayName;
        }
    }
}