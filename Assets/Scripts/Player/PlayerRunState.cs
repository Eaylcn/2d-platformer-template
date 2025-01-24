public class PlayerRunState : PlayerGroundedState
{
    public PlayerRunState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.runSpeed, rb.velocity.y);

        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
