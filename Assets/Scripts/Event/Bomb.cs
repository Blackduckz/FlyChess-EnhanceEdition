using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 炸弹脚本，当玩家投出对应点数爆炸，将玩家送回起点
/// </summary>
public class Bomb : MonoBehaviour
{
    //public static Bomb instant;         //单例，多次触发炸弹事件时防止重复生成
    [HideInInspector]public int triggerPoint;            //触发点数

    //public GameObject explosion;            //爆炸特效
    //public int duration;                //持续回合
    public Text pointText;

    //public int disactiveRound;         //失效轮数

    private void Awake()
    {
        //InitTriggerPoint();
        //if (instant == null)
        //{
        //    instant = this;
        //    GameManager.instant.eventAfterDice += TriggerBomb;
        //}
        //disactiveRound = GameManager.instant.round + 4 * duration;
        //GameManager.instant.eventAfterDice += TriggerBomb;
    }


    //生成触发点数
    public void InitTriggerPoint(int point)
    {
        //triggerPoint = Random.Range(1, 6);
        triggerPoint = point;
        pointText.text = triggerPoint.ToString();
    }

    //触发炸弹方法
    //private void TriggerBomb(int dicePoint)
    //{
    //    //到达失效轮数，销毁
    //    if (GameManager.instant.round == disactiveRound)
    //    {
    //        Destroy(gameObject);
    //        GameManager.instant.eventAfterDice -= TriggerBomb;
    //        return;
    //    }

    //    Player player = GameManager.instant.GetPlayer();
    //    if (dicePoint == triggerPoint)
    //    {
    //        //触发炸弹，取消移动，返回终点
    //        player.skipMove = true;
    //        player.ReturnToOrigin();
    //        GameManager.instant.eventAfterDice -= TriggerBomb;
    //        Instantiate(explosion, transform.position, Quaternion.identity);
    //        Destroy(gameObject);
    //    }
    //}
}
