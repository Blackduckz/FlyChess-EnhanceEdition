using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/CanUseP2")]
public class HasP2 : Conditional
{
    public GetSharedVariables gmTask;

    private GameManager manager;
    private Player player;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        if (player.props["StopMove"] > 0 && player.curCellIndex != player.originIndex)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}

