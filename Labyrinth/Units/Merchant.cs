using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinth
{
    public struct MerchantDialogue
    {
        public string Greeting { get; set; }
        public string IntroQuestion { get; set; }
        public string RepeatQuestion { get; set; }
        public string PriceError { get; set; }
        public string PartingMessage { get; set; }
    }

    public class Merchant : Unit
    {
        private const int MIN_ITEMS = 3;
        private const int MAX_ITEMS = 7;

        private Location _location;

        public MerchantDialogue Dialogue { get; private set; }
        public override Location Location
        {
            get => _location;
            set
            {
                if (value is null)
                    throw new ArgumentNullException($"Location cannot be null.");

                _location = value;

                if (_location.Merchant != this)
                    _location.Merchant = this;
            }
        }

        public Merchant() : base()
        {
            while (Items.Count < Utils.Random.Next(MIN_ITEMS, MAX_ITEMS))
            {
                Item item;

                do
                {
                    item = Item.RandomItem(Items);
                }
                while (item.ItemType == ItemType.Gold);

                Items.Add(item); // TODO: This might not have the desired behavior
            }

            PopulateDialogue();
        }

        public void Sell(ItemType itemType)
        {
            Items.Use(itemType);
        }

        public void Sell(Item item)
        {
            Items.Use(item.ItemType);

            // Need to update dialogue because item counts have changed
            PopulateDialogue();
        }

        protected void PopulateDialogue()
        {
            Dialogue = new MerchantDialogue()
            {
                Greeting = "\"You may find my wares useful in your quest.\"",
                IntroQuestion = "\"What would you like?\"",
                RepeatQuestion = "\"Anything else?\"",
                PartingMessage = "\"Farewell.\""
            };
        }
    }
}
