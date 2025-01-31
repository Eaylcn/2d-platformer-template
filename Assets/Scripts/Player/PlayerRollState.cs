public class PlayerRollState : PlayerGroundedState
{
    public PlayerRollState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.rollDuration;
        player.isRolling = true;
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

        player.SetVelocity(player.rollSpeed * player.rollDir, 0);

        if (stateTimer <= 0)
            stateMachine.ChangeState(player.idleState);
    }
}
