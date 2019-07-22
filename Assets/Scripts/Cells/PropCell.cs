using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 高级道具格子类，获取高级道具
/// </summary>
public class PropCell : MonoBehaviour
{
    public GameObject[] powfulProps;          //高级道具
    public List<int> weightList;                    //道具权重数组
    private List<string> propList ;         //随机道具容器
    public static Dictionary<string, GameObject> propSpriteDic;         //存储道具名字对应的图像

    private void Awake()
    {
        propList = new List<string>() { "CheatDice","Portal","TurnAround"};
        propSpriteDic = new Dictionary<string, GameObject>();

        foreach (GameObject item in powfulProps)
            propSpriteDic.Add(item.tag, item);
    }

    //外部接口，获取随机高级道具
    public string GetProp(Player player)
    {
        string randomProp = Utility.GetRandomValue(propList, weightList);
        player.UpdatePropAmount(randomProp, 1);
        return randomProp;
    }
}
