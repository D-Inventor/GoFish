using System.Collections.Generic;

using GoFish.Lib.Factories;
using GoFish.Lib.Models;

namespace GoFish.Lib.Tests.Fakes
{
    internal class FakeDeckFactory : IDeckFactory
    {
        private readonly Deck returnValue;

        public FakeDeckFactory(IEnumerable<Card> returnValue)
        {
            this.returnValue = new Deck(returnValue);
        }

        public Deck Create(DeckSettings settings)
        {
            return returnValue;
        }
    }
}
