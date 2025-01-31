using UnityEngine;

public class PlayerPullState : PlayerPushPullBaseState
{
    public PlayerPullState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
        : base(_player, _stateMachine, animBoolName)
    { }

    public override void Enter()
    {
        base.Enter();

        player.isCrouching = false;
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        // X input yok => pushPullIdle
        if (Mathf.Abs(xInput) < 0.01f)
        {
            stateMachine.ChangeState(player.pushPullIdleState);
            return;
        }

        // Ayn� y�ne bast�ysa => push
        bool forward = (xInput > 0 && player.facingDir == 1)
                    || (xInput < 0 && player.facingDir == -1);
        if (forward)
        {
            stateMachine.ChangeState(player.pushState);
            return;
        }

        // Pull velocity
        player.SetVelocityNoFlip(xInput * player.pullSpeed, player.rb.velocity.y);
    }
}
