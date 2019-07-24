using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/Utility")]
public class HasWaittingProp : Conditional
{
    public GetSharedVariables gmTask;
    public string propName;

    private GameManager manager;
    private Player player;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        if (player.props[propName] > 0 && player.curCellIndex != player.originIndex)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }

}
