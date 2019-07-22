using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskCategory("UseProp/Utility")]
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
        player = manager.GetPlayer();
        cells = manager.cellDic;
    }


    public override TaskStatus OnUpdate()
    {
        startIndex = Utility.GetVaildIndex(startIndex + offset, cells.Count);
        for (int i = 0; i < area; i++)
        {
            Player player = GetPlayer(startIndex);
            if (player != null && player.extraPoint > -1)
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
