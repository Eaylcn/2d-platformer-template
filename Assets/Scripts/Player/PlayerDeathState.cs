using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.StartCoroutine(ReloadSceneAfterAnimation());
    }
    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator ReloadSceneAfterAnimation()
    {
        yield return null;
        AnimatorStateInfo stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
