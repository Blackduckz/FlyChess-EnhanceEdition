using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/Utility")]
public class MayMoveBack : Conditional
{
    public GetSharedVariables gmTask;
    public int negativePoint;

    private GameManager manager;
    [HideInInspector]public Player player;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        if (player.extraPoint < negativePoint)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
