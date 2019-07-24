using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/Utility")]
public class GetSharedVariables : Action
{
    public SharedGameObject gameManager;
    [HideInInspector]public GameManager manager;

    public override void OnAwake()
    {
        manager = ((GameObject)gameManager.GetValue()).GetComponent<GameManager>();
    }

    public override TaskStatus OnUpdate()
    {
        if (manager != null)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
