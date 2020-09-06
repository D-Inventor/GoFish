using System;

namespace GoFish.Lib.Models
{
    public class Card : IEquatable<Card>
    {
        public string Collection { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }

        public bool Equals(Card other) =>
            Collection == other.Collection &&
            Index == other.Index;

        public override string ToString()
        {
            return $"{Collection}; {Index}; {Description}";
        }
    }
}
