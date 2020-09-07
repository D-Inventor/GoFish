using System;

using Game.Lib;

namespace GoFish.Web.Models.Events
{
    public class GameChange<TGameModel> : EventArgs
    {
        public GameChange(TGameModel game, Result result)
        {
            Game = game;
            Result = result;
        }

        public TGameModel Game { get; }
        public Result Result { get; }
    }
}
