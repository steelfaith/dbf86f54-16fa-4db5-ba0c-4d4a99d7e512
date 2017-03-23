using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class BurnResourceResponse
    {
        public Guid PlayerId { get; set; }
        public List<ElementalAffinity> CurrentResources { get; set; }
        public bool Success { get; set; }
    }
}
