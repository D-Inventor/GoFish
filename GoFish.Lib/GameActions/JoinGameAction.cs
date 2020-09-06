using System;
using System.Linq;

using Game.Lib;

using GoFish.Lib.Models;

namespace GoFish.Lib.GameActions
{
    public class JoinGameAction : GoFishGameActionBase
    {
        private readonly Guid _guid;
        private readonly string _username;

        public JoinGameAction(Guid guid, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or whitespace", nameof(username));
            }

            _guid = guid;
            _username = username;
        }

        protected override Result Apply(ref GoFishGame game)
        {
            ResultBuilder resultBuilder = new ResultBuilder();

            game.Players.Add(new Player(_guid, _username));
            resultBuilder.AddFeedback($"{_username} joined the game.");

            return resultBuilder.Build();
        }

        public override Result Validate(GoFishGame game)
        {
            if (game is null)
                throw new ArgumentNullException(nameof(game));

            ResultBuilder resultBuilder = new ResultBuilder();

            if (game.GameState != GameState.Waiting) resultBuilder.AddFeedback("Games can only be joined if they are in the 'waiting' state.", false);

            if (game.Players.Any(p => p.Id.Equals(_guid))) resultBuilder.AddFeedback($"Games can only be joined if the player is not already part of them.", false);

            return resultBuilder.Build();
        }

        public override string ToString()
        {
            return "Join game";
        }
    }
}
