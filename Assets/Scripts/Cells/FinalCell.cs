using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 终点格脚本，检测玩家是否正确到达终点格
/// </summary>
public class FinalCell : Cell
{
    public int entreDir;            //正确进入终点的方向
    public GameObject playerPlane;           //对应的玩家
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        if (collision.gameObject == playerPlane)
        {
            //如果倒退进入终点格，停止移动
            //修改playerScript.backToFinal = true 
            if (player.isReverse)
            {
                player.stopMove = true;
                player.backToFinal = true;
            }
            //判断是否正常到达终点，
            else if (player.moveDir == entreDir && !player.backToFinal 
                && !player.passFinal )
                GameManager.instant.GameOver();
            else
            {
                //判断是否反向越过终点
                if (player.isMove)
                {
                    //反向越过
                    if (!player.passFinal)
                        player.passFinal = true;
                    //反向越过之后正向经过
                    else
                        player.passFinal = false;
                }
            }
        }

    }
}
