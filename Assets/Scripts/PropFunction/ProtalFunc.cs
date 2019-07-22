using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传送门道具脚本，当玩家进入时传送到除终点外的随机格子
/// </summary>
public class ProtalFunc : WaitingProp
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        
        //防止多次触发
        if(!player.stopMove)
            StartCoroutine(WaitForPortal(false));
    }

    //等待传送完成后，销毁自身
    private IEnumerator WaitForPortal(bool needDice)
    {
        yield return StartCoroutine(player.Portal(needDice));
        //player.skipRound = true;
        Destroy(gameObject);
    }
}
