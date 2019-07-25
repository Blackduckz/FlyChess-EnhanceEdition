using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("UseProp/P5")]
public class UseP5 : Action
{
    public GetSharedVariables gmTask;

    private Player player;
    private GameManager manager;
    private TurnAroundFunc turnAround;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = gmTask.player;
    }

    public override TaskStatus OnUpdate()
    {
        GameObject propPanel = player.player_PropPanel;
        turnAround = Utility.GetScriptInChild<TurnAroundFunc>(propPanel, "TurnAroundButton");
        turnAround.TurnAround();

        return TaskStatus.Success;
    }
}
