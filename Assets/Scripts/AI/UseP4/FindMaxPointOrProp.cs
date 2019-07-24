using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;

[TaskCategory("CanUse/CanUseP4")]
public class FindMaxPointOrProp : Conditional
{
    public GetSharedVariables gmTask;
    public ThereAreItemOnCell onCell;
    public UseWaitingProp useProp;

    private Player player;
    private GameManager manager;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    //查找身后三格内的最大点数格(至少大于+2)，如果没有，则查找是否有非道具格
    //都找不到，则返回Failure
    public override TaskStatus OnUpdate()
    {
        Dictionary<int, GameObject> cellDic = manager.cellDic;
        List<NormalCell> normalCells = new List<NormalCell>();
        int noPointCellIndex = 0;
        int startIndex = Utility.GetVaildIndex(player.curCellIndex - 1, cellDic.Count);

        for (int i = 0; i < 3; i++)
        {
            NormalCell normalCell = cellDic[startIndex].GetComponent<NormalCell>();
            if (normalCell != null && normalCell.extraPoint > 2)
                normalCells.Add(normalCell);
            else if (normalCell == null)
                noPointCellIndex = startIndex;

            startIndex = Utility.GetVaildIndex(startIndex  - 1, cellDic.Count);
        }

        if (normalCells.Count > 0)
        {
            normalCells = normalCells.OrderByDescending(n => n.extraPoint).ToList();
            onCell.SetData(normalCells[0].index, 1, 1, 0);
            useProp.placeIndex = normalCells[0].index;
        }
        else if (noPointCellIndex != 0)
        {
            onCell.SetData(noPointCellIndex, 1, 1, 0);
            useProp.placeIndex = noPointCellIndex;
        }
        else
            return TaskStatus.Failure;

        return TaskStatus.Success;
    }
}
