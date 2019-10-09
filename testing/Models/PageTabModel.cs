using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing.DataModels;

namespace testing.Models
{
    public class PageTabModel : testing.BaseViewModel
    {
        public bool IsSelected { get; set; } = false;
        public String Header { get; set; } = "MyHeader";
        // public String Content { get; set; } = "MyContent";
        public EPageList Content { get; set; } = EPageList.None;

        public PageTabModel(String _Header, EPageList _Content, bool _IsSelected = false)
        {
            Header = _Header;
            Content = _Content;
            IsSelected = _IsSelected;
        }
    }
}
