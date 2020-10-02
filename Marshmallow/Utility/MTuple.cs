﻿using System.Collections.Generic;
using System.Linq;

namespace Marshmallow.Utility
{
    public class MTuple
    {
        public MTuple(params object[] _items)
        {
            Items = _items.ToList();
        }

        public List<object> Items { get; }
    }
}
