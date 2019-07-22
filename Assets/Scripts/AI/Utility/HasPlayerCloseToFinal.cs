using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/Utility")]
public class HasPlayerCloseToFinal : Conditional
{
    public GetSharedVariables gmTask;

    private GameManager manager;

    public override void OnAwake()
    {
        manager = gmTask.manager;
    }

    //查找是否有玩家离终点距离小于27且领先自己
    public override TaskStatus OnUpdate()
    {
        if (manager.HasPlayerCloseToFinal())
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
