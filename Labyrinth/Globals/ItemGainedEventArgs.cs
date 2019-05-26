using System;

namespace Labyrinth
{
    /// <summary>
    /// <see cref="EventArgs"/> for the <see cref="Player.ItemGained"/> event
    /// </summary>
    public class ItemGainedEventArgs : EventArgs
    {
        public Item Item;
        public bool ItemKept;
        public bool FirstItem;
    }
}
