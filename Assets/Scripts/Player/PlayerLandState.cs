public class PlayerLandState : PlayerState
{
    public PlayerLandState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        xInput = 0;
    }
    public override void Update()
    {
        base.Update();


        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);
    }
}
