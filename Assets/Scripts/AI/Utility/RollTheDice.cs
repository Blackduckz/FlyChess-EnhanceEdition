using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/Utility")]
public class RollTheDice : Action
{
    public GetSharedVariables gmTask;
    private GameManager manager;

    public override void OnAwake()
    {
        manager = gmTask.manager;
    }

    public override TaskStatus OnUpdate()
    {
        manager.StartGameLoop();
        return TaskStatus.Success;
    }
}
