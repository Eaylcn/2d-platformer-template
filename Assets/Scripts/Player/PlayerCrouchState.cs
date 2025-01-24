public class PlayerCrouchState : PlayerGroundedState
{
    public PlayerCrouchState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        player.isCrouching = true;
    }
    public override void Update()
    {
        base.Update();

        if (xInput == player.facingDir && player.IsWallDetected())
            return;

        if (xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.crouchWalkState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
