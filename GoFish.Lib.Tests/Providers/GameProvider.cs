using System;
using System.Collections.Generic;

using GoFish.Lib.Models;

namespace GoFish.Lib.Tests.Providers
{
    internal class GameProvider
    {
        public static GoFishGame Game => new GoFishGame
        {
            Deck = new Deck(new[]
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
            }),
            Players = new List<Player>
            {
                new Player(Guid.NewGuid(), "Test")
                {
                    Cards = new HashSet<Card>(new []
                    {
                        new Card
                        {
                            Collection = "C3",
                            Description = "D1",
                            Index = 1
                        },
                        new Card
                        {
                            Collection = "C3",
                            Description = "D2",
                            Index = 2
                        },
                        new Card
                        {
                            Collection = "C4",
                            Description = "D3",
                            Index = 2
                        }
                    })
                },
                new Player(Guid.NewGuid(), "Test")
                {
                    Cards = new HashSet<Card>(new []
                    {
                        new Card
                        {
                            Collection = "C4",
                            Description = "D1",
                            Index = 1
                        },
                        new Card
                        {
                            Collection = "C5",
                            Description = "D2",
                            Index = 2
                        },
                        new Card
                        {
                            Collection = "C4",
                            Description = "D3",
                            Index = 3
                        }
                    })
                },
                new Player(Guid.NewGuid(), "Test")
                {
                    Cards = new HashSet<Card>(new []
                    {
                        new Card
                        {
                            Collection = "C6",
                            Description = "D1",
                            Index = 1
                        },
                        new Card
                        {
                            Collection = "C7",
                            Description = "D2",
                            Index = 2
                        },
                        new Card
                        {
                            Collection = "C7",
                            Description = "D3",
                            Index = 1
                        }
                    })
                }
            },
            CurrentPlayerIndex = 0,
            RuleSet = new RuleSet
            {
                GiveCardBehaviour = GiveCardBehaviour.AllOfCollection,
                TurnBehaviour = TurnBehaviour.LastAskedPlayer,
                StartBehaviour = StartBehaviour.FirstPlayer,
                CardsOnStart = 2
            },
            GameState = GameState.Playing
        };
    }
}
