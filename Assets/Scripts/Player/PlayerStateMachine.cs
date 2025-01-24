public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState _startingState)
    {
        currentState = _startingState;
        _startingState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
