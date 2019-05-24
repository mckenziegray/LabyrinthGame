using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinth
{
    public struct MerchantDialogue
    {
        public string Greeting;
        public string IntroQuestion;
        public string RepeatQuestion;
        public string PriceError;
        public string Items;
        public string PartingMessage;
    }

    class Merchant : Unit
    {
        private const int MIN_ITEMS = 3;
        private const int MAX_ITEMS = 7;

        private Location location;
        private bool isMale;

        public MerchantDialogue Dialogue { get; private set; }
        public override Location Location
        {
            get { return location; }
            set
            {
                location = value;
                
                if (location.Merchant != this)
                    location.Merchant = this;
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
                while (item.ItemType != ItemType.Gold);

                Items.Add(item); // TODO: This might not have the desired behavior
            }

            PopulateDialogue();
        }

        public void Sell(Item item)
        {
            Items.Use(item.ItemType);
        }

        public void PopulateDialogue()
        {
            string itemStr = "";
            foreach (Item item in Items)
            {
                itemStr += $"\t";
                if (item.Stackable)
                    itemStr += $"{item.Count} ";

                if (item is Armor)
                    itemStr += $"{(item as Armor).ArmorType} ";
                else if (item is Weapon)
                    itemStr += $"{(item as Weapon).WeaponType} ";
                else if (item is Shield)
                    itemStr += $"{(item as Shield).ShieldType} ";

                itemStr += $"{item.ItemType}\t{item.Value}\n";
            }

            foreach (string action in Enum.GetNames(typeof(MerchantAction)))
            {
                itemStr += $"\t{action}";
            }

            this.Dialogue = new MerchantDialogue()
            {
                Greeting = "\"You may find my wares useful in your quest.\"",
                IntroQuestion = "\"What would you like?\"",
                RepeatQuestion = "\"Anything else?\"",
                Items = itemStr,
                PartingMessage = "\"Farewell.\""
            };
        }
    }
}
