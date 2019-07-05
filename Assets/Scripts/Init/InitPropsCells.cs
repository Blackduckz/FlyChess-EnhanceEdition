using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 初始化道具格
/// </summary>
public class InitPropsCells : InitCells
{
    public GameObject[] propsCells;     //道具格父物体，每种道具用单独一类父物体管理
    public string[] propTag;            //道具标签

    void Awake()
    {
        InitCellProps();
    }

    //初始化道具格
    private void InitCellProps()
    {
        for (int i = 0; i < propsCells.Length; i++)
        {
            //获取父物体
            Transform propTrf = propsCells[i].transform;
            //每个格子生成道具
            for (int j = 0; j < propTrf.childCount; j++)
            {
                Transform propCellTrsf = propTrf.GetChild(j);
                PropCell propCell = propCellTrsf.GetComponent<PropCell>();
                propCellTrsf.tag = propTag[i];

                Vector3 scaleFactor = (propCellTrsf.rotation.z != 0) ? 
                    new Vector3(0.55f, 0.5f, 0) : new Vector3(0.5f, 0.55f, 0);
                InstantiateSprite(propCellTrsf.gameObject, sprites[i], scaleFactor);
            }
        }
    }

}
