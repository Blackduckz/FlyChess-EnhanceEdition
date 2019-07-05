using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初始化格子父类，提供一个GameObject数组
/// </summary>
public class InitCells : MonoBehaviour
{
    public GameObject[] sprites;

    //动态生成格子内的Sprite
    public static GameObject InstantiateSprite(GameObject cell, GameObject sprite, Vector3 scaleFactor)
    {
        GameObject gobj  = Instantiate(sprite, cell.transform);
        gobj.transform.rotation = Quaternion.identity;
        gobj.transform.position = cell.transform.position;
        gobj.transform.localScale = scaleFactor;
        return gobj;
    }

    //动态生成格子内的Sprite（供三角格使用）
    public static void InstantiateSprite(GameObject cell, Vector3 pos,GameObject sprite, Vector3 scaleFactor)
    {
        GameObject gobj = Instantiate(sprite, cell.transform);
        gobj.transform.parent = cell.transform;
        gobj.transform.rotation = Quaternion.identity;
        gobj.transform.localPosition = pos;
        gobj.transform.localScale = scaleFactor;
    }
}
