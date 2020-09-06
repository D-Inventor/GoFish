using System.Collections;
using System.Collections.Generic;
using System.Linq;

using GoFish.Lib.Providers;

namespace GoFish.Lib.Models
{
    public class Deck : IEnumerable<Card>
    {
        private readonly List<Card> _stack;

        public Deck(IEnumerable<Card> cards)
        {
            _stack = cards.ToList();
        }

        public void InsertAt(int index, Card card)
        {
            _stack.Insert(index, card);
        }

        public bool TryDraw(out Card card)
        {
            if (_stack.Count == 0)
            {
                card = null;
                return false;
            }

            Card result = _stack[0];
            _stack.RemoveAt(0);
            card = result;
            return true;
        }

        public void Shuffle(IRandomProvider randomProvider = null)
        {
            randomProvider = randomProvider ?? RandomProvider.GetInstance();
            _stack.Sort((c1, c2) => randomProvider.Next(-100, 100));
        }

        public int Count => _stack.Count;

        public IEnumerator<Card> GetEnumerator()
        {
            return ((IEnumerable<Card>)_stack).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_stack).GetEnumerator();
        }
    }
}
