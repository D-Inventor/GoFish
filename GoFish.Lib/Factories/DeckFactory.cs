using System.Collections.Generic;
using System.Linq;

using GoFish.Lib.Models;
using GoFish.Lib.Providers;

namespace GoFish.Lib.Factories
{
    public class DeckFactory : IDeckFactory
    {
        private readonly IEnumerable<ICardCollectionProvider> _cardCollectionProviders;

        public DeckFactory(IEnumerable<ICardCollectionProvider> cardCollectionProviders)
        {
            _cardCollectionProviders = cardCollectionProviders;
        }

        public Deck Create(DeckSettings settings)
        {
            IEnumerable<Card> cards = _cardCollectionProviders
                .SelectMany(ccp => ccp.GetCards())
                .SelectMany(c => c.Value);

            return new Deck(cards);
        }
    }
}
