using System.Collections.Generic;

using GoFish.Lib.Models;

namespace GoFish.Lib.Providers
{
    public interface ICardCollectionProvider
    {
        IReadOnlyDictionary<string, IReadOnlyCollection<Card>> GetCards();
    }
}
