using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 三角格子脚本，用于处理旋转
/// </summary>

public class TriCell : Cell
{
    public int clockwideDir;           //顺时针旋转方向
    public int antiClockwideDir;   //顺时针旋转方向

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        Transform playerTrf = collision.gameObject.GetComponent<Transform>();
        GetRotation(player,playerTrf);
    }

    //根据进入方向获取旋转
    public void GetRotation(Player player,Transform playerTrf)
    {
        if (player.moveDir == clockwideDir || player.moveDir == antiClockwideDir)
        {
            player.needRotate = true;
            //顺时针旋转
            if (player.moveDir == clockwideDir)
            {
                player.targetRotation = Quaternion.Euler(0f, 0f, -90f) * playerTrf.rotation;
                player.isClockWide = true;
            }
            //逆时针旋转
            else if (player.moveDir == antiClockwideDir)
            {
                player.targetRotation = Quaternion.Euler(0f, 0f, 90f) * playerTrf.rotation;
                player.isClockWide = false;
            }
        }
        else
            player.needRotate = false;
    }
}
