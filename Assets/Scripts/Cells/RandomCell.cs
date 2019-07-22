using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 随机格，随机获取点数或者普通道具
/// </summary>

public class RandomCell : MonoBehaviour
{
    public int pointProbability = 30;            //点数概率
    public int propProbability = 70;             //道具概率
    public List<int> weightList;                    //点数权重数组

    private List<int> pointList;         //随机点数容器
    private List<string> propList;        //随机道具容器

    private void Awake()
    {
        pointList = new List<int>();
        propList = new List<string>(){ "EffectPass","StopMove"};

        //存储-6~+6点数，
        for (int i = -6; i < 0; i++)
            pointList.Add(i);
        for (int i = 1; i <= 6; i++)
            pointList.Add(i);
    }

    //外部接口，获取随机效果
    public string GetRandom(Player player)
    {
        string result;
        int random = Random.Range(1, 100);
        //获得点数
        if(random <= pointProbability)
        {
            int randomPoint = Utility.GetRandomValue(pointList,weightList);
            result = randomPoint.ToString();
            player.extraPoint = randomPoint;
            player.UpdateExtraPoint(randomPoint);
        }
        //获得普通道具
        else
        {
            int index = Random.Range(0, propList.Count);
            player.UpdatePropAmount(propList[index], 1);
            result = propList[index];
        }
        return result;
    }

}
