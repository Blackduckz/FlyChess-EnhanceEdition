using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/CanUseP2")]
public class HasNoNegativeCell : Conditional
{
    public GetSharedVariables gmTask;
    public UseP2 p2;
    public ThereAreItemOnCell onCell;
    public int area;
    public int stride;

    private GameManager manager;
    private Player player;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }


    public override TaskStatus OnUpdate()
    {
        Dictionary<int, GameObject> cellDic = manager.cellDic;
        int startIndex = Utility.GetVaildIndex(player.curCellIndex + stride, cellDic.Count);

        //查找范围内中第一个非负数格
        for (int i = 0; i < area; i++)
        {
            NormalCell normalCell = cellDic[startIndex].GetComponent<NormalCell>();
            if (normalCell == null || normalCell.extraPoint > 0) 
            {
                p2.placeIndex = startIndex;
                onCell.SetData(startIndex, 1, -1);
                return TaskStatus.Success;
            }
            startIndex = Utility.GetVaildIndex(startIndex + stride, cellDic.Count);
        }

        //无法放置，使检测脚本从自身开始，保证返回failure
        onCell.area = 1;
        return TaskStatus.Failure;
    }
}
