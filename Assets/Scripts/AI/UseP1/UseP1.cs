using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("UseProp")]
public class UseP1 : Action
{
    public GetSharedVariables gmTask;
    private Player player;

    private GameManager manager;
    private EffectPassFunc passFunc;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        GameObject propPanel = player.player_PropPanel;
        passFunc = Utility.GetScriptInChild<EffectPassFunc>(propPanel, "EffectPassButton");
        passFunc.PassEffect();

        return TaskStatus.Success;
    }
}
