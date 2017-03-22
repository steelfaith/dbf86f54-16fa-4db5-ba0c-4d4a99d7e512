using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class AddResourceRequest
    {
        public ElementalAffinity Affinity { get; set; }
        public Guid PlayerId { get; set; }
    }
}
