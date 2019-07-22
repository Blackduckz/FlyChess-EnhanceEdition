using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/Utility")]
public class ThereAreItemOnCell : Conditional
{
    public GetSharedVariables gmTask;
    public int[] layerMasks;
    public int area;
    public int stride;
    public int offset;

    private GameManager manager;
    private Player player;
    private int targetIndex;
    private int layerMask;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        int count = manager.cellDic.Count;
        targetIndex = Utility.GetVaildIndex(targetIndex + offset, count);
        getLayerMask();

        for (int i = 0; i < area; i++)
        {
            if (Utility.HasItemOnCell(targetIndex, layerMask))
                return TaskStatus.Failure;
            targetIndex = Utility.GetVaildIndex(targetIndex + stride, manager.cellDic.Count);
        }

        return TaskStatus.Success;
    }

    public void SetData(int targetIndex, int area,int stride, int offset)
    {
        this.targetIndex = targetIndex;
        this.area = area;
        this.stride = stride;
        this.offset = offset;
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
