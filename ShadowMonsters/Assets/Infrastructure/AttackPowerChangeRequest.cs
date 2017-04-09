using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class AttackPowerChangeRequest
    {
        public Guid AttackInstanceId;
        public Guid AttackId { get; set; }
        public bool Up { get; set; }
    }
}
