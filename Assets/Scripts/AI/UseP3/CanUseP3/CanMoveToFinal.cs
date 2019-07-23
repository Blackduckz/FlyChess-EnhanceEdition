using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/CanUseP3")]
public class CanMoveToFinal : Conditional
{
    public GetMaxMovment getMax;
    public UseP3 p3;
    public ThereAreItemOnCell onCell;

    private GameManager manager;
    private Player player;
    private int startIndex;
    private int finalIndex;
    private int maxMovement;

    public override void OnAwake()
    {
        manager = getMax.manager;
        player = manager.GetPlayer();
        startIndex = player.curCellIndex;
        maxMovement = getMax.maxMovement;

        FinalCell finalCell = player.final.GetComponent<FinalCell>();
        finalIndex = finalCell.index;
    }

    //如果最大可移动距离足够达到终点，直接投6
    //或者离终点格只剩7步距离，此时优先走最大行动距离
    public override TaskStatus OnUpdate()
    {
        if (player.distanceFromFinal <= maxMovement ||
            player.distanceFromFinal - maxMovement <= 2)
        {
            p3.btnIndex = 6;
            onCell.SetData(startIndex, finalIndex - startIndex, 1, 0);
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
