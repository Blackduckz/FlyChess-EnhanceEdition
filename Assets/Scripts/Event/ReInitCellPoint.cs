using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReInitCellPoint : MonoBehaviour
{
    public Text ReInitText;

    private void Awake()
    {
        //EventCell.RegisterEvent(ReInitCellPiont);
    }

    public void ReInitCellPiont()
    {
        ReInitText.gameObject.SetActive(true);
        List<NormalCell> tempCellList = new List<NormalCell>(InitNormalCells.cells);

        foreach (var item in InitNormalCells.pointSpritesDic)
        {
            //随机取格子
            int cell_index = Random.Range(0, tempCellList.Count);

            //分配图片和对应的点数
            Transform child = item.Key.transform;
            int childPoint = item.Value;

            Transform parent = tempCellList[cell_index].transform;
            tempCellList[cell_index].extraPoint = childPoint;
            child.parent = parent;
            child.position = parent.position;
            child.rotation = Quaternion.identity;
            child.localScale = Vector3.one;

            tempCellList.RemoveAt(cell_index);
        }

        Invoke("SetTextActive", 0.5f);
    }

    private void SetTextActive()
    {
        ReInitText.gameObject.SetActive(false);
    }
}
