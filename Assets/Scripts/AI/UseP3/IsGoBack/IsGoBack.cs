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

        //如果只能倒退，考虑后3格的情况
        getMax.startOffset = maxMovement;
        getMax.maxMovement = 3;
        getMax.stride = -1;

        //如果只能倒退，求出最大倒退距离
        //if (maxMovement <= 0)
        //    maxMovement = Mathf.Abs(maxMovement - 6 ) - 1;
        //List<NormalCell> normalCells = Utility.GetTargetCells<NormalCell>(player.curCellIndex, maxMovement, -1);
        return TaskStatus.Success;
    }
}
