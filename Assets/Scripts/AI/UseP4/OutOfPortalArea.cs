using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/CanUseP4")]
public class OutOfPortalArea : Conditional
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
        if (player.curCellIndex > player.maxPortalIndex)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
