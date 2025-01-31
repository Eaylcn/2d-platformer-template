public class PlayerLadderClimbState : PlayerState
{
    public PlayerLadderClimbState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.isOnLadder = true;
        player.SetZeroVelocity();
        player.ColliderUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = player.normalGravity;
        player.ClearModes();
        player.ColliderUpdate();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.rb.velocity.x, 1.5f);

        if (player.IsGroundDetected() && triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }
}
