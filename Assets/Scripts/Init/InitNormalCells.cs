using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初始化普通格子，分配点数
/// </summary>
public class InitNormalCells : InitCells
{

    public static List<NormalCell> cells;                                               //保存所有点数格引用
    public List<int> extraPoints;                 //保存额外点数

    //记录点数图像对应的点数
    public static Dictionary<GameObject, int> pointSprites;


    private void Awake()
    {
        cells = new List<NormalCell>(GetComponentsInChildren<NormalCell>());
        pointSprites = new Dictionary<GameObject, int>();
        InitCellsPoint();
    }

    //随机分配格子的点数效果
    private void InitCellsPoint()
    {
        List<NormalCell> tempCellList = new List<NormalCell>(cells);
        List<int> tempPointList = new List<int>(extraPoints);
        for (int i = 0; i < 20; i++)
        {
            //随机取格子
            int cell_index = Random.Range(0, tempCellList.Count);
            //随机分配点数
            int point_index = Random.Range(0, tempPointList.Count);
            tempCellList[cell_index].extraPoint = tempPointList[point_index];
            tempPointList.RemoveAt(point_index);

            //根据点数分配对应的sprite
            int sprite_index = 0;
            if (tempCellList[cell_index].extraPoint < 0)
                sprite_index = tempCellList[cell_index].extraPoint + 6;
            else
                sprite_index = tempCellList[cell_index].extraPoint + 5;
            //动态生成sprite
            GameObject pointSpirte =  InstantiateSprite(tempCellList[cell_index].gameObject, sprites[sprite_index],Vector3.one);
            pointSprites.Add(pointSpirte, tempCellList[cell_index].extraPoint);

            tempCellList.RemoveAt(cell_index);
        }
    }


}
