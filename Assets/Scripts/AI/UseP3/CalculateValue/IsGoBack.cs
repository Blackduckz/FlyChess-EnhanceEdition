using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/CanUseP3")]
public class IsGoBack : Conditional
{
    public GetMaxMovment getMax;
    public UseP3 p3;

    private GameManager manager;
    private Player player;
    private int maxMovement;

    public override void OnAwake()
    {
        manager = getMax.manager;
        player = manager.GetPlayer();
        maxMovement = getMax.maxMovement;
    }

    //检查是否只能倒退移动
    public override TaskStatus OnUpdate()
    {
        if (maxMovement > 0)
            return TaskStatus.Failure;


        getMax.stride = -1;
        return TaskStatus.Success;
    }
}
