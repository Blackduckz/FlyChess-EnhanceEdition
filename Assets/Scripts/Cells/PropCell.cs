using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 高级道具格子类，获取高级道具
/// </summary>
public class PropCell : MonoBehaviour
{
    public List<int> weightList;                    //道具权重数组
    private List<string> propList ;         //随机道具容器

    private void Awake()
    {
        propList = new List<string>() { "CheatDice","Portal","TurnAround"};         
    }

    //外部接口，获取随机高级道具
    public void GetProp(Player player)
    {
        string randomProp = Utility.GetRandomPoint(propList, weightList);
        player.UpdatePropAmount(randomProp, 1);
    }
}
