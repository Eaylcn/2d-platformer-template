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

        if (player.IsLadderBottomDetected() && Input.GetKeyDown(KeyCode.S) && !player.isBusy)
            stateMachine.ChangeState(player.ladderDownState);
        else if (player.IsLadderTopDetected() && Input.GetKeyDown(KeyCode.W) && !player.isBusy)
            stateMachine.ChangeState(player.climbState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected() && !player.isBusy)
            stateMachine.ChangeState(player.jumpState);

        if (Input.GetKeyDown(KeyCode.X) && player.IsGroundDetected() && !player.isBusy)
            stateMachine.ChangeState(player.deathState);

        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.isCrouching)
            stateMachine.ChangeState(player.punchAttack);

        if (Input.GetKeyDown(KeyCode.LeftControl) && player.IsGroundDetected() && !player.isCrouching)
            stateMachine.ChangeState(player.crouchState);
        else if (Input.GetKeyDown(KeyCode.LeftControl) && player.IsGroundDetected() && player.isCrouching && !player.IsCrouchTopFloorDetected())
            stateMachine.ChangeState(player.idleState);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (player.isCrouching && player.IsCrouchTopFloorDetected())
            {
                stateMachine.ChangeState(player.crouchState);
                return;
            }

            PushPullObject ppo = player.CheckForPushPullObject();
            if (ppo != null)
            {
                stateMachine.ChangeState(player.pushPullIdleState);
                return;
            }
        }

        if (!player.isWalking)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !player.IsWallDetected() && xInput == 0)
            {
                player.rollDir = player.facingDir;

                stateMachine.ChangeState(player.rollState);
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift) && !player.IsWallDetected() && xInput != 0)
            {

                player.slideDir = Input.GetAxisRaw("Horizontal");

                stateMachine.ChangeState(player.slideState);
            }
        }
    }
}
