using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CanUse/Utility")]
public class UseWaitingProp : Action
{
    public GetSharedVariables gmTask;
    public GameObject prop;
    public string propName;
    [HideInInspector] public int placeIndex;

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
        GameObject propPanel = player.player_PropPanel;
        place = Utility.GetScriptInChild<PlacePropOnCell>(propPanel, propName);

        if (placeIndex <= 0)
            return TaskStatus.Failure;

        GameObject targetCell = manager.cellDic[placeIndex];
        place.StartPlaceProp(targetCell, prop);
        return TaskStatus.Success;
    }
}
