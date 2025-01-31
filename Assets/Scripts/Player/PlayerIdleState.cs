public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
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

        if (xInput == player.facingDir && player.IsWallDetected())
            return;

        if (xInput != 0 && !player.isBusy && !player.isWalking)
            stateMachine.ChangeState(player.runState);
        else if (xInput != 0 && !player.isBusy && player.isWalking)
            stateMachine.ChangeState(player.walkState);

        if (player.IsCrouchTopFloorDetected())
            stateMachine.ChangeState(player.crouchState);

    }
}
