﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnAroundFunc : MonoBehaviour
{
    public int duration;            //持续回合数
    private int resumeRound;            //恢复朝向轮数
    private Player firstPlayer;
    private List<Player> playerRank;

    //外部接口，供OnClick调用
    public void TurnAround()
    {
        playerRank = new List<Player>();
        TurnAroundTheFirstPlayer();
    }

    //使第一名调转方向duration个回合
    public void TurnAroundTheFirstPlayer()
    {
        Player curPlayer = GameManager.instant.GetPlayer();
        if(curPlayer.props[gameObject.tag] > 0)
        {
            //游戏开局时不能使用
            firstPlayer = GameManager.instant.GetPlayer();
            if (firstPlayer.turn == GameManager.instant.round + 1)
                return;

            firstPlayer = FindFirstPlayer();
            //如果当前第一名已经被转向，不能再次将其转向
            if (firstPlayer.isTurnAround)
                return;
            else
                firstPlayer.isTurnAround = true;

            //计算恢复原向的轮数

            //计算与目标的轮数差值
            int targetPlayerTurn = firstPlayer.turn;
            int curPlayerTurn = GameManager.instant.playerTurn + 1;
            int gap = targetPlayerTurn - curPlayerTurn;
            //计算目标执行时的轮数
            int targetRound = GameManager.instant.round + gap;
            //恢复朝向轮数即为目标执行轮数+4 * duration + 1
            resumeRound = targetRound + 4 * duration + 1;

            //改变朝向
            firstPlayer.ReverseDir(false);
            firstPlayer.ChangeOrientation(firstPlayer.moveDir);
            firstPlayer.CheckNeedRotate();

            //将恢复事件注册到GameManager中
            GameManager.instant.disactiveEffect += ResumeOrientation;

            //关闭道具互动状态，更新数量

            curPlayer.ChangeButtonState(false);
            curPlayer.UpdatePropAmount("TurnAround", -1);
            curPlayer.skipRound = true;
        }
        
    }

    //找到排名第一的玩家
    private Player FindFirstPlayer()
    {
        //对玩家数组进行升序排序
        GameObject[] players = GameManager.instant.players;
        foreach (GameObject item in players)
        {
            Player player = item.GetComponent<Player>();
            playerRank.Add(player);
        }

        playerRank.Sort((x, y) => x.CompareTo(y));
        return playerRank[0];
    }

    //恢复朝向
    public void ResumeOrientation()
    {
        if (GameManager.instant.round == resumeRound)
        {
            firstPlayer.isTurnAround = false;
            firstPlayer.ReverseDir(false);
            firstPlayer.ChangeOrientation(firstPlayer.moveDir);
            GameManager.instant.disactiveEffect -= ResumeOrientation;
        }
    }
}
