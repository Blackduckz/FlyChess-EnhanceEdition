using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("UseProp/P4")]
public class TryPlaceP4OnCell : Action
{
    public GetSharedVariables gmTask;
    public ThereAreItemOnCell onCell;
    public UseWaitingProp useProp;
    public int stride;

    private Player player;
    private GameManager manager;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        int targetIndex = player.curCellIndex + stride;
        onCell.SetData(targetIndex, 1, stride,0);
        useProp.placeIndex = targetIndex;
        return TaskStatus.Success;
    }
}
