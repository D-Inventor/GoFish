namespace Game.Lib
{
    public interface IGameAction<TGameModel>
    {
        Result Validate(TGameModel game);
        Result Perform(ref TGameModel game);
    }
}
