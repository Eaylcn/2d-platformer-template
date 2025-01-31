public class PlayerLadderDownState : PlayerState
{
    private bool moveFinished;

    public PlayerLadderDownState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.isOnLadder = true;
        player.SetZeroVelocity();
        player.ColliderUpdate();
        player.rb.gravityScale = 0;
        moveFinished = false;
    }

    public override void Exit()
    {
        base.Exit();

        player.ClearModes();
        player.ColliderUpdate();
        player.rb.gravityScale = player.normalGravity;
    }

    public override void Update()
    {
        base.Update();

        if (!moveFinished)
        {
            player.SetVelocity(player.rb.velocity.x, -5f);
            moveFinished = true;
        }

        if (moveFinished)
        {
            if (player.IsLadderTopDetected() && triggerCalled)
                stateMachine.ChangeState(player.climbState);
        }
    }
}
