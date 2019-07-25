using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("CanUse/Utility")]
public class GetTargetPlayer : Conditional
{
    public GetSharedVariables gmTask;
    public int area;
    public int stride;
    public int offset;

    private GameManager manager;
    private Dictionary<int, GameObject> cells;
    private Player player;
    private int startIndex;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = gmTask.player;
        cells = manager.cellDic;
    }

    //该玩家身上没有小于-1的点数，且离终点距离小于27，且领先当前玩家
    public override TaskStatus OnUpdate()
    {
        startIndex = Utility.GetVaildIndex(player.curCellIndex + offset, cells.Count);
        for (int i = 0; i < area; i++)
        {
            Player targetPlayer = GetPlayer(startIndex);
            if (targetPlayer != null && targetPlayer.extraPoint > -1 && targetPlayer.distanceFromFinal < 27 &&
                targetPlayer.distanceFromFinal < player.distanceFromFinal)
                return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    private Player GetPlayer(int cellIndex)
    {
        GameObject cell = cells[cellIndex];
        Transform cellChild = cell.transform.GetChild(0);
        Collider2D c2d = Physics2D.OverlapCircle(cellChild.position, 0.1f, 1 << 11);
        return c2d.GetComponent<Player>();
    }
}
