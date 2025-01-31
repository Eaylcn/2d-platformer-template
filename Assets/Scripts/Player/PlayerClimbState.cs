public class PlayerClimbState : PlayerState
{
    private bool isClimbingLadder;
    private bool isDowningLadder;

    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.isOnLadder = true;
        player.SetZeroVelocity();
        player.ColliderUpdate();
        player.rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = player.normalGravity;
        player.ClearModes();
        player.ColliderUpdate();
        isDowningLadder = false;
        isClimbingLadder = false;

        player.SetZeroVelocity();
        player.StartCoroutine("BusyFor", .15f);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.rb.velocity.x, yInput * player.climbSpeed);

        if (player.rb.velocity.y > 0)
        {
            isClimbingLadder = true;
            isDowningLadder = false;
        }
        else if (player.rb.velocity.y < 0)
        {
            isDowningLadder = true;
            isClimbingLadder = false;
        }
        else
        {
            isDowningLadder = false;
            isClimbingLadder = false;
        }


        if (player.IsGroundDetected() && isDowningLadder)
            stateMachine.ChangeState(player.idleState);

        if (!player.IsLadderTopDetected() && isClimbingLadder)
            stateMachine.ChangeState(player.ladderClimbState);

    }
}
