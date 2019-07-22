using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("UseProp/P2")]
public class UseBothP2AndP4 : Action
{
    public GetSharedVariables gmTask;
    public GameObject stopMove;
    public GameObject portal;

    private Player player;
    private GameManager manager;
    private PlacePropOnCell place;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        
        return TaskStatus.Success;
    }
}
