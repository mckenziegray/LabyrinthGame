using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinth
{
    /// <summary>
    /// <see cref="EventArgs"/> for the <see cref="Player.StatsIncreased"/> event
    /// </summary>
    class StatsIncreasedEventArgs : EventArgs
    {
        public List<Tuple<string, int>> StatsIncreased;
    }
}
