using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("UseProp/P3")]
public class UseP3 : Action
{
    public GetSharedVariables gmTask;
    [HideInInspector] public int btnIndex;

    private Player player;
    private GameManager manager;
    private CheatDiceFunc cheatDice;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = manager.GetPlayer();
    }

    public override TaskStatus OnUpdate()
    {
        if (btnIndex == 0)
            return TaskStatus.Failure;

        GameObject propPanel = player.player_PropPanel;
        cheatDice = Utility.GetScriptInChild<CheatDiceFunc>(propPanel, "CheatDiceButton");
        cheatDice.ShowDices();
        cheatDice.DiscretionaryPoint(btnIndex);
        return TaskStatus.Success;
    }
}
