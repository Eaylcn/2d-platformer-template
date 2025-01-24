public class PlayerCrouchWalkState : PlayerGroundedState
{
    public PlayerCrouchWalkState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

        player.SetVelocity(xInput * player.crouchWalkSpeed, rb.velocity.y);

        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.crouchState);
    }
}
