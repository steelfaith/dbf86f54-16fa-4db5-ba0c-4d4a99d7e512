﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class DataEventArgs<T> : EventArgs
    {
        public T Data { get; set; }
    }
}
