using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/Utility")]
public class HasTargetCell : Conditional
{
    public GetSharedVariables gmTask;
    public UseP2 p2;
    public ThereAreItemOnCell item;

    public int area;
    public string cellName;

    private GameManager manager;
    private Player player;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        int startIndex = player.curCellIndex;
        int targetIndex = Utility.ThereIsTargetCell(startIndex, area, cellName, 1, false);
        if (targetIndex != -1)
        {
            p2.placeIndex = targetIndex;
            item.SetData(startIndex, 1, 1, 0);
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}
