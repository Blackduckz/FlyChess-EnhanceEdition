using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBomb : MonoBehaviour
{
    public GameObject bombGobj;
    public GameObject bombPanel;
    public int duration;                //持续回合
    public GameObject explosion;            //爆炸特效

    private int disactiveRound;         //失效轮数
    private bool triggerBomb;
    private List<int> randomPoint;
    private List<GameObject> bombs;

    private void Awake()
    {
        triggerBomb = false;
        randomPoint = new List<int> { 1, 2, 3, 4, 5, 6 };
        bombs = new List<GameObject>();
        EventCell.RegisterEvent(CrateBomb);
    }

    //生成炸弹
    public void CrateBomb()
    {
        GameObject bombInsant;
        //多个炸弹失效时间以第一个为准
        if (bombs.Count < 1)
        {
            disactiveRound = GameManager.instant.round + 4 * duration;
            GameManager.instant.eventAfterDice += TriggerBomb;
        }
            
        //最多4个炸弹
        if (bombs.Count < 4)
        {
            bombInsant = Instantiate(bombGobj, bombPanel.transform);
            bombs.Add(bombInsant);
            Bomb bomb = bombInsant.GetComponent<Bomb>();

            //生成触发点数
            int random = Random.Range(0, randomPoint.Count);
            bomb.InitTriggerPoint(randomPoint[random]);
            randomPoint.RemoveAt(random);
            //bomb.disactiveRound = disactiveRound;


        }
        //if (Bomb.instant != bombInsant.GetComponent<Bomb>())
        //{
        //    Destroy(bombInsant);
        //    Bomb.instant.InitTriggerPoint();
        //}
    }

    //触发炸弹方法
    private void TriggerBomb(int dicePoint)
    {
        //到达失效轮数，销毁
        if (GameManager.instant.round == disactiveRound)
        {
            Destroy(gameObject);
            GameManager.instant.eventAfterDice -= TriggerBomb;
            return;
        }

        Player player = GameManager.instant.GetPlayer();
        foreach (var item in bombs)
        {
            Bomb bomb = item.GetComponent<Bomb>();
            if (dicePoint == bomb.triggerPoint)
            {
                //触发炸弹，取消移动，返回终点
                player.skipMove = true;
                player.ReturnToOrigin();
                GameManager.instant.eventAfterDice -= TriggerBomb;
                Instantiate(explosion, item.transform.position, Quaternion.identity);
                triggerBomb = true;
                break;
            }
        }

        if(triggerBomb)
        {
            DestroyAllBomb();
            randomPoint = new List<int> { 1, 2, 3, 4, 5, 6 };
            triggerBomb = false;
        }
    }


    //销毁所有炸弹
    private void DestroyAllBomb()
    {
        int count = bombs.Count;
        for (int i = 0; i < count; i++)
            Destroy(bombs[i].gameObject);

        bombs = new List<GameObject>();
    }

}
