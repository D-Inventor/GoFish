using System;
using System.Collections.Generic;

namespace GoFish.Lib.Models
{
    public class Player : IEquatable<Player>
    {
        public Player(Guid guid, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or whitespace", nameof(username));
            }

            Username = username;
            Id = guid;
        }

        public Guid Id { get; }
        public string Username { get; }
        public ISet<Card> Cards { get; set; } = new HashSet<Card>();

        public IDictionary<string, ISet<Card>> FinishedCollections { get; set; } = new Dictionary<string, ISet<Card>>();

        public bool Equals(Player other)
        {
            return Id.Equals(other.Id);
        }
    }
}
