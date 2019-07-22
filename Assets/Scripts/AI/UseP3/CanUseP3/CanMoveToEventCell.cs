using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/CanUseP3")]
public class CanMoveToEventCell : Conditional
{
    public GetMaxMovment getMax;
    public UseP3 p3;
    public ThereAreItemOnCell onCell;

    private GameManager manager;
    private Player player;
    private int maxMovement;

    public override void OnAwake()
    {
        manager = getMax.manager;
        player = manager.GetPlayer();
        maxMovement = getMax.maxMovement;
    }

    //检查是否需要移动到事件格
    public override TaskStatus OnUpdate()
    {
        //如果有与终点距离小于27且领先自己的玩家，移动到事件格
        if (!manager.HasPlayerCloseToFinal())
            return TaskStatus.Failure;

        int startIndex = player.curCellIndex;
        int targetIndex = Utility.ThereIsTargetCell(startIndex, maxMovement, "EventCell", 1, false);
        if (targetIndex == -1)
            return TaskStatus.Failure;

        p3.btnIndex = targetIndex - startIndex;
        onCell.SetData(startIndex, p3.btnIndex, 1, 0);
        return TaskStatus.Success;

    }
}
