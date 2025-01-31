using UnityEngine;

public class PlayerPushPullBaseState : PlayerGroundedState
{
    protected PushPullObject currentObj;
    protected DistanceJoint2D currentJoint;

    private bool anchorInitialized = false; // Joint bir kez mi kuruldu?

    public PlayerPushPullBaseState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
        : base(_player, _stateMachine, animBoolName)
    { }

    public override void Enter()
    {
        base.Enter();

        player.ClearModes();

        if (!anchorInitialized)
        {
            currentObj = player.CheckForPushPullObject();

            if (currentObj != null)
            {
                currentObj.SetInteracting(true);
                currentJoint = currentObj.gameObject.AddComponent<DistanceJoint2D>();
                currentJoint.autoConfigureDistance = false;
                currentJoint.distance = (currentObj.transform.localScale.x * 0.5f) + 0.75f;
                currentJoint.connectedBody = player.rb;
            }
            anchorInitialized = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        anchorInitialized = false;

        if (currentJoint != null)
        {
            GameObject.Destroy(currentJoint);
            currentJoint = null;
        }

        if (currentObj != null)
        {
            currentObj.SetInteracting(false);
            currentObj = null;
        }
    }

    public override void Update()
    {
        base.Update();

        if (currentObj == null)
        {
            stateMachine.ChangeState(player.idleState);
            Debug.Log("PlayerPushPullBaseState: Current object is null.");
            return;
        }

        if (!currentObj.IsGroundDetected())
        {
            currentObj.rb.velocity = new Vector2(player.facingDir * (player.pushSpeed * 2.5f), currentObj.rb.velocity.y);
            stateMachine.ChangeState(player.idleState);
            player.SetZeroVelocity();
            player.StartCoroutine("BusyFor", 1f);

            Debug.Log("PlayerPushPullBaseState: Current object is not detected on the ground.");
            return;
        }
    }
}
