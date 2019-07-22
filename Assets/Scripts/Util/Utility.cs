﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 通用方法类
/// </summary>
public class Utility : MonoBehaviour
{
    private static Dictionary<int, GameObject> cells;
    private void Start()
    {
        cells = GameManager.instant.cellDic;
    }


    //返回数组范围内的索引
    public static int GetVaildIndex(int index,int length)
    {
        if (index <= 0)
            index += length;
        else if (index > length)
            index %= length;
        return index;
    }

    //通过Tag获取对应的Cell脚本
    public static Cell GetCellScriptByTag(GameObject targetCell)
    {
        Cell cell = null;
        if (targetCell.tag == "NormalCells")
            cell = targetCell.GetComponent<NormalCell>();
        else if (targetCell.tag == "StopMove" || targetCell.tag == "EffectPass")
            cell = targetCell.GetComponent<Cell>();
        else if(targetCell.tag == "FinalCell")
            cell = targetCell.GetComponent<FinalCell>();
        else
            cell = targetCell.GetComponent<TriCell>();
        return cell;
    }

    //获取距离当前格子stride的格子脚本
    public static Cell GetCellWithCurrentIndex(int curCellIndex, int stride)
    {
        int length = cells.Count;
        int index = GetVaildIndex(curCellIndex + stride, length);
        Cell targetCell = GetCellScriptByTag(cells[index]);
        return targetCell;
    }

    //确定Cell的类别
    public static bool IsTypeOf<T>(Cell cell)
    {
        return cell.GetType() == typeof(T);
    }

    //判断后一个为三角格，前一个为普通格情况
    public static bool IsSpcTri(int curCellIndex)
    {
        Cell preCell = GetCellWithCurrentIndex(curCellIndex, -1);
        Cell nextCell = GetCellWithCurrentIndex(curCellIndex, +1);
        bool isTric = IsTypeOf<TriCell>(preCell);
        bool isNormal = IsTypeOf<NormalCell>(nextCell) || IsTypeOf<Cell>(nextCell);
        return isTric && isNormal;
    }

    //根据权重获取随机值
    public static T GetRandomValue<T>(List<T> itemList,List<int> weightlist)
    {
        //计算权重总和
        int totalWeights = 0;
        for (int i = 0; i < weightlist.Count; i++)
            totalWeights += weightlist[i] + 1;  //权重+1，防止为0情况。

        //随机赋值权重
        System.Random ran = new System.Random(GetRandomSeed());  //GetRandomSeed()防止快速频繁调用导致随机一样的问题 
        List<KeyValuePair<int, int>> wlist = new List<KeyValuePair<int, int>>();    //第一个int为list下标索引、第二个int为权重排序值

        for (int i = 0; i < weightlist.Count; i++)
        {
            int w = weightlist[i] + 1 + ran.Next(0, totalWeights);   // （权重+1） + 从0到（总权重-1）的随机数
            wlist.Add(new KeyValuePair<int, int>(i, w));
        }
        //排序
        wlist.Sort(
          delegate (KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2)
          {
              return kvp2.Value - kvp1.Value;
          });
        //随机法则
        return itemList[wlist[0].Key];
    }

    //获取随机种子
    private static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    //从子物体中获取Script
    public static T GetScriptInChild<T>(GameObject gameObject, string name)
    {
        Transform child = gameObject.GetComponent<Transform>().Find(name);
        return child.GetComponent<T>();
    }

    //查找范围内是否含有（不含有）目标格子，有的话返回格子的Index，否则返回-1
    public static int ThereIsTargetCell(int startIndex, int area, string cellName, int stride, bool filter)
    {
        Dictionary<int, GameObject> cells = GameManager.instant.cellDic;
        int length = cells.Count;

        if (!filter)
        {
            for (int i = 0; i < area; i++)
            {
                startIndex = GetVaildIndex(startIndex + stride, length);
                if (cells[startIndex].tag == cellName)
                    return startIndex;
            }
        }
        else
        {
            for (int i = 0; i < area; i++)
            {
                startIndex = GetVaildIndex(startIndex + stride, length);
                if (cells[startIndex].tag != cellName)
                    return startIndex;
            }
        }

        return -1;
    }

    //返回符合条件的格子脚本List
    public static List<T> GetTargetCells<T>(int startIndex, int area, int stride)
        where T : Cell
    {
        Dictionary<int, GameObject> cells = GameManager.instant.cellDic;
        int length = cells.Count;
        List<T> targetCells = new List<T>();

        for (int i = 0; i < area; i++)
        {
            startIndex = GetVaildIndex(startIndex + stride, length);
            T cellScript = cells[startIndex].GetComponent<T>();
            if (cellScript != null)
                targetCells.Add(cellScript);
        }
        return targetCells;
    }

    //检测格子上是否有某类物体
    public static bool HasItemOnCell(int startIndex, int layerMask)
    {
        GameObject cell = cells[startIndex];
        Transform cellChild = cell.transform.GetChild(0);
        Collider2D c2d = Physics2D.OverlapCircle(cellChild.position, 0.1f, layerMask);

        if (c2d == null)
            return false;

        return true;
    }
}
