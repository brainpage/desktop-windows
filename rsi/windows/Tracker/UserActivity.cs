using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker
{
    [Serializable]
    class UserActivity
    {
        public int Duration { get;  set; }

        public string name { get;  set; }
    }
}
