using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;

[TaskCategory("CanUse/CanUseP3")]
public class NeedRollMaxPoint : Conditional
{
    public GetMaxMovment getMax;
    public UseP3 p3;
    public ThereAreItemOnCell onCell;

    private GameManager manager;
    private Player player;
    private int maxMovement;
    private int startOffset;

    public override void OnAwake()
    {
        manager = getMax.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        maxMovement = getMax.maxMovement;
        int startIndex = player.curCellIndex;
        Dictionary<int, GameObject> cellDic = manager.cellDic;
        int cellIndex = Utility.GetVaildIndex(startIndex + maxMovement, cellDic.Count);

        if (cellDic[cellIndex].tag != "NormalCells")
        {
            p3.btnIndex = 6;
            onCell.SetData(startIndex, maxMovement, 1, 1);
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}
