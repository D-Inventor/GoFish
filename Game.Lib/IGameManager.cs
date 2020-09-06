namespace Game.Lib
{
    public interface IGameManager<TGameModel>
    {
        Result PerformAction(IGameAction<TGameModel> gameAction, ref TGameModel game);
    }
}