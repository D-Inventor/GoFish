using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using GoFish.Lib.Models;

namespace GoFish.Lib.Providers
{
    public class JsonFileCardCollectionSource : IFileCardCollectionSource
    {
        public bool CanReadFile(string source)
            => Path.GetExtension(source).Equals(".json", StringComparison.InvariantCultureIgnoreCase);

        public IDictionary<string, ICollection<Card>> ReadFile(string source)
        {
            if (!CanReadFile(source))
                throw new InvalidOperationException("Source file is not json.");

            using (StreamReader sr = new StreamReader(source))
            {
                Dictionary<string, ICollection<Card>> result = JsonSerializer.Deserialize<Dictionary<string, ICollection<Card>>>(sr.ReadToEnd());
                foreach (KeyValuePair<string, ICollection<Card>> kvp in result)
                {
                    foreach (Card card in result[kvp.Key])
                    {
                        card.Collection = kvp.Key;
                    }
                }
                return result;
            }
        }
    }
}
