using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class ButtonPressResolution
    {
        public Guid PlayerId { get; set; }
        public Guid AttackId { get; set; }  
        public float TimeOutSeconds { get; set; }
        /// <summary>
        /// casted spells rotate clockwise
        /// </summary>
        public bool Clockwise { get; set; }
    }
}
