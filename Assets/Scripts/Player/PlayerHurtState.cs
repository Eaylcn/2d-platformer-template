public class PlayerHurtState : PlayerState
{
    public PlayerHurtState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }
    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            player.hurtCount++;

            if (player.hurtCount >= 5)
                stateMachine.ChangeState(player.deathState);
            else
                stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
