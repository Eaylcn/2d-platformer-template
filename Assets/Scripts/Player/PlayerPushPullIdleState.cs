using UnityEngine;

public class PlayerPushPullIdleState : PlayerPushPullBaseState
{
    public PlayerPushPullIdleState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
        : base(_player, _stateMachine, animBoolName)
    { }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();

        player.isCrouching = true;
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

        // X input var -> push veya pull
        if (Mathf.Abs(xInput) > 0.01f)
        {
            bool forward = (xInput > 0 && player.facingDir == 1)
                        || (xInput < 0 && player.facingDir == -1);
            if (forward)
                stateMachine.ChangeState(player.pushState);
            else
                stateMachine.ChangeState(player.pullState);
        }
    }
}
