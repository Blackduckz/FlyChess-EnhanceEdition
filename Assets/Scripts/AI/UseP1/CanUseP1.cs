using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse")] 
public class CanUseP1 : Conditional
{
    public GetSharedVariables gmTask;

    private GameManager manager;
    private Player player;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        //如果身上有负点数，使用效果传递
        if (player.props["EffectPass"] > 0 && player.extraPoint < 0)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
