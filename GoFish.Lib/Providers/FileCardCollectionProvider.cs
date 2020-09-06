using System;
using System.Collections.Generic;
using System.Linq;

using GoFish.Lib.Models;

namespace GoFish.Lib.Providers
{
    public class FileCardCollectionProvider : ICardCollectionProvider
    {
        private readonly string _source;
        private readonly IEnumerable<IFileCardCollectionSource> _cardCollectionSources;

        public FileCardCollectionProvider(string source, IEnumerable<IFileCardCollectionSource> cardCollectionSources)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException($"'{nameof(source)}' cannot be null or whitespace", nameof(source));
            }

            _source = source;
            _cardCollectionSources = cardCollectionSources;
        }

        public IReadOnlyDictionary<string, IReadOnlyCollection<Card>> GetCards()
        {
            Dictionary<string, IReadOnlyCollection<Card>> result = new Dictionary<string, IReadOnlyCollection<Card>>();

            IFileCardCollectionSource reader = _cardCollectionSources.FirstOrDefault(c => c.CanReadFile(_source));
            if (!(reader is null))
            {
                IDictionary<string, ICollection<Card>> cardCollections = reader.ReadFile(_source);

                foreach (KeyValuePair<string, ICollection<Card>> kvp in cardCollections)
                {
                    result.Add(kvp.Key, kvp.Value.ToList());
                }
            }

            return result;
        }
    }
}
