using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 停止移动道具脚本，当玩家进入时停止移动
/// </summary>
public class StopMoveFunc : WaitingProp
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        int playerRound = GameManager.instant.GetPlayerRound(player.turn);

        player.stopMove = true;
        Destroy(gameObject);
    }
}
