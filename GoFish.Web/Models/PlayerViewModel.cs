using System;
using System.Collections.Generic;

namespace GoFish.Web.Models
{
    public class PlayerViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int Cards { get; set; }
        public Dictionary<string, List<CardViewModel>> FinishedCollections { get; set; }
    }
}
