using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class ReviveRequest
    {
        public Guid PlaneswalkerId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
