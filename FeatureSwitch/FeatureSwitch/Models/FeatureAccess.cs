using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureSwitch.Models
{
    public partial class FeatureAccess
    {
        public int Id { get; set; }

        public string FeatureName { get; set; }

        public string Email { get; set; }

        public bool? Enable { get; set; }
    }
}
