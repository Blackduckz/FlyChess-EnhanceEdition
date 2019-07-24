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
    [HideInInspector] public int startOffset;               //开始位置偏移量
    [HideInInspector] public int maxGoBackDistance;


    private Player player;

    //根据最大移动距离，计算从哪个格子开始检查（即距离当前格子的偏移量）
    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
        if (player.extraPoint > 0)
        {
            maxMovement = 6 + player.extraPoint + manager.morePoint;
            startOffset = 1 + player.extraPoint + manager.morePoint;
        }
        else if (player.extraPoint < 0)
        {
            maxMovement = 6 + player.extraPoint - manager.morePoint;
            if (maxMovement > 0)
                startOffset = 1;
            //如果只能倒退，那么从倒退的最小距离开始计算
            else if (maxMovement < 0)
            {
                startOffset = maxMovement;
                maxGoBackDistance = Mathf.Abs(player.extraPoint - manager.morePoint) - 1;
            }
            //如果最大前进距离刚好为0，从身后1格开始计算
            else
            {
                startOffset = -1;
                maxGoBackDistance = Mathf.Abs(player.extraPoint - manager.morePoint) - 1;
            }

        }
        else
        {
            maxMovement = 6;
            startOffset = 1;
        }


        benefitFillter = maxMovement;

    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
