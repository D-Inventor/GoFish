using System.Collections.Generic;

using GoFish.Lib.Models;

namespace GoFish.Lib.Tests.Providers
{
    internal static class CardProvider
    {

        public static List<Card> Cards = new List<Card>()
        {
            new Card
            {
                Collection = "C1",
                Description = "D1",
                Index = 1
            },
            new Card
            {
                Collection = "C2",
                Description = "D2",
                Index = 2
            },
            new Card
            {
                Collection = "C2",
                Description = "D3",
                Index = 1
            }
        };

        public static ISet<Card> HashSetCards = new HashSet<Card>(Cards);

        public static string CardJson =
            "[" +
            "   {" +
            "       \"Collection\":\"C1\"," +
            "       \"Index\":1," +
            "       \"Description\":\"D1\"" +
            "   }" +
            "]";

        public static Card CardObject = new Card
        {
            Collection = "C1",
            Index = 1,
            Description = "D1"
        };
    }
}
