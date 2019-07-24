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
        int count = manager.cellDic.Count;

        int startIndex = Utility.GetVaildIndex(player.curCellIndex + startOffset, count);
        int targetIndex;

        if (maxMovement > 0)
        {
            int distanceFromTarget;
            //能前进，且有最小前进距离
            if (player.extraPoint > 0)
                targetIndex = Utility.ThereIsTargetCell(startIndex, maxMovement - startOffset + 1, "PropCell", stride, false);
            else
                targetIndex = Utility.ThereIsTargetCell(startIndex, maxMovement, "PropCell", stride, false);

            distanceFromTarget = targetIndex - startIndex + 1;

            if (targetIndex == -1)
                return TaskStatus.Failure;

            p3.btnIndex = distanceFromTarget;
            onCell.SetData(startIndex - startOffset, distanceFromTarget, 1, 1);
        }
        //如果只能倒退，且倒退距离不为0，考虑倒退的最近格到最远格之间是否有符合条件的格子
        else
        {
            int maxGoBackIndex = Utility.GetVaildIndex(startIndex - startOffset - getMax.maxGoBackDistance, count);
            int goBackDistance = Utility.GetVaildIndex(startIndex - maxGoBackIndex + 1, count);

            if(maxMovement != 0)
                targetIndex = Utility.ThereIsTargetCell(startIndex, goBackDistance, "PropCell", stride, false);
            else
                targetIndex = Utility.ThereIsTargetCell(startIndex, getMax.maxGoBackDistance, "PropCell", stride, false);

            if (targetIndex == -1)
                return TaskStatus.Failure;

            int goBackDis = player.extraPoint - manager.morePoint;
            p3.btnIndex = targetIndex - goBackDis - (startIndex - startOffset);
            onCell.SetData(targetIndex, startIndex - startOffset - targetIndex, 1, 0);
        }
      
        return TaskStatus.Success;
    }
}
