using UnityEngine;

public class PlayerPushState : PlayerPushPullBaseState
{
    public PlayerPushState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
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

        // Ters y�ne bast�ysa => pull
        bool forward = (xInput > 0 && player.facingDir == 1)
                    || (xInput < 0 && player.facingDir == -1);
        if (!forward)
        {
            stateMachine.ChangeState(player.pullState);
            return;
        }

        // Karakterin horizontal velocity:
        // "NoFlip" diyorsan sprite d�nmez, pull animasyonu arkada kal�r
        player.SetVelocityNoFlip(xInput * player.pushSpeed, player.rb.velocity.y);

        // Duvara dayan�rsa, Unity fizik objeyi ve karakteri durdurabilir
        // Yukar�da velocity'yi kontrol edip animasyonu idle'a �ekebilirsin
    }
}
