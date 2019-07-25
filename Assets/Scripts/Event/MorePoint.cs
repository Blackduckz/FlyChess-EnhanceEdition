using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 增多玩家获得的格子点数，不论正负
/// </summary>
public class MorePoint : MonoBehaviour
{
    public Text morePointText;                  //用于显示增多的点数
    public int durantion = 2;           //持续轮数
    public int morePoint = 2;           //额外点数

    private int disactiveRound;         //生效轮数

    void Awake()
    {
        //EventCell.RegisterEvent(GetMovePoint);
    }

    private void UpdateMorePointText(int point)
    {
        morePointText.text = "获得的格子点数+/-" + point;
    }

    //添加额外点数
    public void GetMovePoint()
    {
        disactiveRound = GameManager.instant.round + 4 * durantion + 1;
        int curMorePoint = GameManager.instant.morePoint;

        //如果当前没有额外点数，注册事件，并修改额外点数
        if (curMorePoint == 0)
        {
            GameManager.instant.disactiveEffect += ClearMovePoint;
            GameManager.instant.morePoint = morePoint;
            morePointText.gameObject.SetActive(true);
        }
          //否则，每次+1，直到额外点数为4
        else if(curMorePoint < 4)
            GameManager.instant.morePoint += 1;

        UpdateMorePointText(GameManager.instant.morePoint);
    }

    //到达持续时间后，清空额外点数
    public void ClearMovePoint()
    {
        if (GameManager.instant.round == disactiveRound)
        {
            GameManager.instant.morePoint = 0;
            GameManager.instant.disactiveEffect -= ClearMovePoint;
            morePointText.gameObject.SetActive(false);
        }
    }
}
