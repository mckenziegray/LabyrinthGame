using Labyrinth;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace LabyrinthTest
{
    class MerchantTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                Merchant merchant = new Merchant();

                Assert.IsNotNull(merchant.Items);
                Assert.IsTrue(merchant.Items.Count > 0);
                Assert.IsFalse(merchant.Items.Any(i => i.ItemType == ItemType.Gold));

                Assert.IsNotNull(merchant.Dialogue);
                foreach (PropertyInfo prop in merchant.GetType().GetProperties(BindingFlags.Public))
                {
                    Assert.IsNotNull(prop.GetValue(merchant));
                }
            }
        }
    }
}
