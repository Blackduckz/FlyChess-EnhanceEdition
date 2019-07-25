using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("UseProp/P2")]
public class UseP2 : Action
{
    public GetSharedVariables gmTask;
    public GameObject prop;
    [HideInInspector]public int placeIndex;

    private Player player;
    private GameManager manager;
    private PlacePropOnCell place;

    public override void OnAwake()
    {
        manager = gmTask.manager;
        player = gmTask.player;
    }

    public override TaskStatus OnUpdate()
    {
        GameObject propPanel = player.player_PropPanel;
        place = Utility.GetScriptInChild<PlacePropOnCell>(propPanel, "StopMoveButton");

        if (placeIndex <= 0 )
            return TaskStatus.Failure;

        GameObject targetCell = manager.cellDic[placeIndex];
        place.StartPlaceProp(targetCell, prop);
        return TaskStatus.Success;
    }
}
