public class PlayerSlideState : PlayerGroundedState
{
    public PlayerSlideState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.slideDuration;
        player.isSliding = true;
        player.ColliderUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);

        player.ClearModes();
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.slideSpeed * player.slideDir, 0);

        if (stateTimer <= 0)
            stateMachine.ChangeState(player.idleState);
    }
}
