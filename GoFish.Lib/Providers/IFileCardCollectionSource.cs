using System.Collections.Generic;

using GoFish.Lib.Models;

namespace GoFish.Lib.Providers
{
    public interface IFileCardCollectionSource
    {
        bool CanReadFile(string source);
        IDictionary<string, ICollection<Card>> ReadFile(string source);
    }
}
