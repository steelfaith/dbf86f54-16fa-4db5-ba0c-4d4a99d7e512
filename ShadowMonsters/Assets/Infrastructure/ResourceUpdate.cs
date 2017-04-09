using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class ResourceUpdate
    {
        public Guid Id { get; set; }
        public List<ElementalAffinity> Resources { get; set; }
    }
}
