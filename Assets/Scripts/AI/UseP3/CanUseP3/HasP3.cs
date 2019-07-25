using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/CanUseP3")]
public class HasP3 : Conditional
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
        if(player.props["CheatDice"] > 0)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}

