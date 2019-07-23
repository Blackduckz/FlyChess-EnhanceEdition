using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/CanUseP2")]
public class HasPointCell : Conditional
{
    public GetSharedVariables gmTask;
    public UseP2 p2;
    public ThereAreItemOnCell onCell;

    public int point;
    public int area;
    public int stride;

    private GameManager manager;
    private Player player;
    private int startIndex;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    //寻找符合目标点数的格子
    public override TaskStatus OnUpdate()
    {
        startIndex = player.curCellIndex;
        List<NormalCell> cells = Utility.GetTargetCells<NormalCell>(startIndex, area, stride);
        if (cells.Count == 0)
            return TaskStatus.Failure;

        foreach (NormalCell cell in cells)
        {
            if (Compare(point, cell.extraPoint))
            {
                onCell.SetData(cell.index, area, stride, 1);
                p2.placeIndex = cell.index;
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }

    //根据传入的过滤值比较额外点数要大于还是小于
    private bool Compare(int para1,int para2)
    {
        if (para1 > 0)
            return para2 > para1;
        else
            return para2 < para1;
    }
}
