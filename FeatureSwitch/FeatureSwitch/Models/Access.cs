using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureSwitch.Models
{
    public partial class Access
    {
        public bool canAccess { get; set; }
        public string Error { get; set; }
    }
}
