public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.ClearModes();
        player.ColliderUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.landState);

        if (xInput != 0)
            player.SetVelocity(player.runSpeed * .8f * xInput, rb.velocity.y);
    }
}
