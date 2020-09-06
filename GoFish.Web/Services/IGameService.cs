using System;
using System.Collections.Generic;

using Game.Lib;

using GoFish.Lib.Models;
using GoFish.Web.Models;

namespace GoFish.Web.Services
{
    public interface IGameService
    {
        Result Create();
        GoFishGame Get();
        Result Join(string username);
        Result Start();
        Result Pass(Guid id);
        Result Give(IEnumerable<CardViewModel> cards);
    }
}