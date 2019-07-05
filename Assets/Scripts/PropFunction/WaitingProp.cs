using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 放置类道具父类
/// </summary>

public class WaitingProp : MonoBehaviour
{
    protected int curCellIndex;            //道具所在格子id
    protected Player player;
    private void Awake()
    {
        Transform parent = transform.parent;
        Cell cell = Utility.GetCellScriptByTag(parent.gameObject);
        curCellIndex = cell.index;
    }

}
