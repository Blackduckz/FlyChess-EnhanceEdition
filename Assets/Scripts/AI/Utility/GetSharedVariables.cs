using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/Utility")]
public class GetSharedVariables : Action
{
    private int playerTurn;
    [HideInInspector]public GameManager manager;
    [HideInInspector] public Player player;

    public override void OnAwake()
    {
        manager = GameManager.instant;
        playerTurn = (int)GlobalVariables.Instance.GetVariable("PlayerTurn").GetValue();
        player = manager.GetSpecifiedPlayer(playerTurn);
    }

    public override TaskStatus OnUpdate()
    {
        
    
        if (manager != null)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
