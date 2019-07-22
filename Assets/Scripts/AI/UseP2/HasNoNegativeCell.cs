using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/CanUseP2")]
public class HasNoNegativeCell : Conditional
{
    public GetSharedVariables gmTask;
    public UseP2 p2;
    //public CanPlaceProp canPlace;
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
        int startIndex = player.curCellIndex;
        Dictionary<int, GameObject> cellDic = manager.cellDic;

        //查找范围内中第一个非负数格
        for (int i = 0; i < 3; i++)
        {
            NormalCell normalCell = cellDic[startIndex].GetComponent<NormalCell>();
            if (normalCell == null || normalCell.extraPoint > 0)
            {
                p2.placeIndex = startIndex;
                //canPlace.SetData(1, startIndex, -1, 0);
                return TaskStatus.Success;
            }
            startIndex = Utility.GetVaildIndex(startIndex + stride, cellDic.Count);
        }

        return TaskStatus.Failure;
    }
}
