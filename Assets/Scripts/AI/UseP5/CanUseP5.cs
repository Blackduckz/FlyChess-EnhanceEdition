using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/CanUseP5")]
public class CanUseP5 : Conditional
{
    public GetSharedVariables gmTask;

    private GameManager manager;
    private Player player;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = gmTask.player;
    }

    public override TaskStatus OnUpdate()
    {
        Player firstPlayer = manager.GetFirstPlayer();

        //如果有道具，第一名不是自己，
        bool conditon1 = player.props["TurnAround"] > 0 && firstPlayer != player;
        //第一名与终点距离小于27，且其未被被转向，则使用转向道具
        bool conditon2 =  firstPlayer.distanceFromFinal < 27 &&!firstPlayer.isTurnAround;

        if (conditon1 &&conditon2)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
