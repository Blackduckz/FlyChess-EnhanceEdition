using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("UseProp/P3")]
public class GetMaxMovment : Action
{
    public GetSharedVariables gmTask;
    [HideInInspector] public GameManager manager;

    public int stride;            //移动方向
    [HideInInspector] public int benefitFillter;        //收益过滤值
    [HideInInspector] public int maxMovement;           //最大可移动步数
    [HideInInspector] public int startOffset = 0;               //开始位置偏移量


    private Player player;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
        if (player.extraPoint >= 0)
            maxMovement = 6 + player.extraPoint + manager.morePoint;
        else
            maxMovement = 6 + player.extraPoint - manager.morePoint;
        benefitFillter = maxMovement;

    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
