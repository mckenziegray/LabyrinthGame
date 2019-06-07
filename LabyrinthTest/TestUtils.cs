using Labyrinth;

namespace LabyrinthTest
{
    static class TestUtils
    {
        public static int MAX_VALUE = 10000; // NOTE: Don't use int.MaxValue

        public static Item RandomStackableItem()
        {
            Item stackableItem;
            do
            {
                stackableItem = Item.RandomItem();
            }
            while (!stackableItem.Stackable);

            return stackableItem;
        }

        public static Item RandomNonStackableItem()
        {
            Item nonStackableItem;
            do
            {
                nonStackableItem = Item.RandomItem();
            }
            while (nonStackableItem.Stackable);

            return nonStackableItem;
        }
    }
}
