using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 用于企图移动到某个特定位置时检测该格子是否有玩家或者道具
/// 要求前置条件调用SetData设置待检测格子信息
/// 如果没有修改，根据手动设置值默认返回Success或Failure
/// area =0 返回success; area =1 ,stride = 0 返回failure
/// </summary>

[TaskCategory("CanUse/Utility")]
public class ThereAreItemOnCell : Conditional
{
    public GetSharedVariables gmTask;
    public int[] layerMasks;
    public int area;
    public int stride;

    private GameManager manager;
    private Player player;
    private int targetIndex;
    private int layerMask;
    private int count;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
        count = manager.cellDic.Count;
        targetIndex = Utility.GetVaildIndex(player.curCellIndex + stride, count);
    }

    public override TaskStatus OnUpdate()
    {
       
        getLayerMask();

        for (int i = 0; i < area; i++)
        {
            if (Utility.HasItemOnCell(targetIndex, layerMask))
                return TaskStatus.Failure;
            targetIndex = Utility.GetVaildIndex(targetIndex + stride, count);
        }

        return TaskStatus.Success;
    }

    public void SetData(int targetIndex, int area,int stride)
    {
        this.targetIndex = targetIndex;
        this.area = area;
        this.stride = stride;
    }

    private void getLayerMask()
    {
        layerMask = 0;
        for (int i = 0; i < layerMasks.Length; i++)
        {
            int temp = 1 << layerMasks[i];
            layerMask |= temp;
        }
    }

}
