using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brainpage
{
    class ActivityEvent
    {
        public int dur { get; set; } //The period of time we've been measuring before, in millseconds
        public int dst { get; set; } //The distance covered by the mouse
        public int mnum { get; set; } //The number of "mousemove" events
        public int keys { get; set; } //Number of key presses
        public int msclks { get; set; } //Number of mouse clicks
        public int scrll { get; set; } //Scroll "distance" of mouse wheel
        public string app { get; set; } //Identifier of currently active app
    }
}
