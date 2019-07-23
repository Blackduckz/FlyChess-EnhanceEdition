using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/CanUseP3")]
public class CanMoveToPropCell : Conditional
{
    public GetMaxMovment getMax;
    public UseP3 p3;
    public ThereAreItemOnCell onCell;

    private GameManager manager;
    private Player player;
    private int maxMovement;
    private int stride;
    private int startOffset;

    public override void OnAwake()
    {
        manager = getMax.manager;
        player = manager.GetPlayer();
    }


    public override TaskStatus OnUpdate()
    {
        startOffset = getMax.startOffset;
        maxMovement = getMax.maxMovement;
        stride = getMax.stride;

        int startIndex = Utility.GetVaildIndex(player.curCellIndex + startOffset, manager.cellDic.Count);

        int targetIndex = Utility.ThereIsTargetCell(startIndex, maxMovement, "PropCell", stride, false);
        if(targetIndex == -1)
            return TaskStatus.Failure;

        int cellIndex = targetIndex - (startIndex - startOffset);
        if (cellIndex > 0)
        {
            p3.btnIndex = cellIndex;
            onCell.SetData(startIndex, cellIndex, 1, 0);
        }
            

        //只能倒退情况
        else
        {
            int goBackDis = player.extraPoint - manager.morePoint;
            p3.btnIndex = targetIndex - goBackDis - (startIndex - startOffset);
            onCell.SetData(targetIndex, startIndex - targetIndex, 1, 0);
        }
        return TaskStatus.Success;
    }
}
