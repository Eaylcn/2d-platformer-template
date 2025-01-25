using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected() && !player.isBusy)
            stateMachine.ChangeState(player.jumpState);

        if (Input.GetKeyDown(KeyCode.X) && player.IsGroundDetected() && !player.isBusy)
            stateMachine.ChangeState(player.deathState);

        if (Input.GetKeyDown(KeyCode.LeftControl) && player.IsGroundDetected() && !player.isCrouching)
            stateMachine.ChangeState(player.crouchState);
        else if (Input.GetKeyDown(KeyCode.LeftControl) && player.IsGroundDetected() && player.isCrouching && !player.IsCrouchTopFloorDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
