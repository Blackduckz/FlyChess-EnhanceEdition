using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Collections;

[TaskCategory("UseProp/Utility")]
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
        player = gmTask.player;
    }

    public override TaskStatus OnUpdate()
    {
        Dictionary<int, GameObject> cells = manager.cellDic;
        GameObject propPanel = player.player_PropPanel;
        int portalIndex = Utility.GetVaildIndex(player.curCellIndex + 3, cells.Count);
        int stopIndex = Utility.GetVaildIndex(player.curCellIndex + 1, cells.Count);

        StartCoroutine(PlaceProps(propPanel, portalIndex, stopIndex));

        return TaskStatus.Success;
    }

    private IEnumerator PlaceProps(GameObject propPanel, int portalIndex, int stopIndex)
    {
        //在前方3格放置传送门
        place = Utility.GetScriptInChild<PlacePropOnCell>(propPanel, "PortalButton");
        GameObject targetCell = manager.cellDic[portalIndex];
        place.StartPlaceProp(targetCell, portal);

        yield return new WaitForSeconds(0.5f);

        //在前方1格放置停止移动
        place = Utility.GetScriptInChild<PlacePropOnCell>(propPanel, "StopMoveButton");
        targetCell = manager.cellDic[stopIndex];
        place.StartPlaceProp(targetCell, stopMove);
    }
}
