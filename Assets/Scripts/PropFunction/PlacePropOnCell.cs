using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 放置类道具生成方法
/// </summary>
public class PlacePropOnCell : MonoBehaviour
{
    public int maxAccebNum;             //允许放置的范围
    public GameObject propSprite;               //需要放置的道具
    public Color highLightColor;        //高亮颜色

    private Dictionary<int,Color> orignColor;         //格子的原本颜色
    //可用格子的容器
    private Dictionary<int, GameObject> targetCells;
    private Player player;
    private int curRound;           //点击道具时的轮数

    private void InitData()
    {
        curRound = GameManager.instant.round;
        targetCells = new Dictionary<int, GameObject>();
        orignColor = new Dictionary<int, Color>();
        player = GameManager.instant.GetPlayer();
    }

    //外部接口，供OnClick调用
    public void StartPlaceProp()
    {
        InitData();
        //在起点时不能使用，执行放置逻辑
        if (player.curCellIndex != player.originIndex && player.props[gameObject.tag] > 0)
            StartCoroutine(PlaceProp());
    }

    //外部接口，供AI调用
    public void StartPlaceProp(GameObject targetCell, GameObject prop)
    {
        InitData();
        StartCoroutine(PlaceProp(targetCell, prop));
    }

    //放置道具方法,供AI调用
    private IEnumerator PlaceProp(GameObject targetCell, GameObject prop)
    {
        GetAcibleCells();
        GetCellOrginColor();
        ChangeCellsColor();
        yield return StartCoroutine(ClickCellToPlaceProp(targetCell, prop));
        ResetCellsColor();
    }

    //放置道具方法
    private IEnumerator PlaceProp()
    {
        GetAcibleCells();
        GetCellOrginColor();
        ChangeCellsColor();
        yield return StartCoroutine(ClickCellToPlaceProp());
        ResetCellsColor();
    }

    //获得maxAccebNum范围内的格子
    private void GetAcibleCells()
    {
        int curCellIndex = player.curCellIndex;
        Dictionary<int,GameObject> cells = GameManager.instant.cellDic;
        int length = cells.Count;

        //获取前maxAccebNum个格子
        int startIndex = Utility.GetVaildIndex(curCellIndex - maxAccebNum,length);
        GetAccessibleCells(startIndex);
        //获取后maxAccebNum个格子
        int endIndex = Utility.GetVaildIndex(curCellIndex + 1, length);
        GetAccessibleCells(endIndex);
    }

    //获取前后maxAccebNum范围内格子
    private void GetAccessibleCells(int startIndex)
    {
        Dictionary<int, GameObject> cells = GameManager.instant.cellDic;
        int length = cells.Count;
        int layerMask = (1 << 11) | (1 << 12);

        for (int i = 0; i < maxAccebNum; i++)
        {
            //以格子子物体为圆心，检测格子上是否有玩家或者道具
            if (!Utility.HasItemOnCell(startIndex, layerMask)) 
                targetCells.Add(startIndex, cells[startIndex]);
            startIndex = Utility.GetVaildIndex(startIndex + 1, length);
        }
    }

    //获取格子原本color值
    private void GetCellOrginColor()
    {
        foreach (var item in targetCells)
        {
            SpriteRenderer renderer = item.Value.GetComponent<SpriteRenderer>();
            orignColor.Add(item.Key, renderer.color);
        }
    }

    //高亮格子颜色
    private void ChangeCellsColor()
    {
        foreach (var item in targetCells)
        {
            SpriteRenderer renderer = item.Value.GetComponent<SpriteRenderer>();
            renderer.color = highLightColor;
        }
    }

    //重置格子的颜色
    private void ResetCellsColor()
    {
        foreach (var item in targetCells)
        {
            SpriteRenderer renderer = item.Value.GetComponent<SpriteRenderer>();
            renderer.color = orignColor[item.Key];
        }
    }

    //生成道具
    private void InstantiateProp(Cell cell, GameObject targetCell, GameObject prop)
    {
        //点击位置为三角格，生成位置需要调整
        if (Utility.IsTypeOf<TriCell>(cell))
            InitCells.InstantiateSprite(targetCell, new Vector3(-0.3f, -0.3f), prop, Vector3.one);
        //非三角格
        else
        {
            Vector3 scaleFactor = (targetCell.transform.rotation.z != 0) ?
                new Vector3(0.55f, 0.5f, 0) : new Vector3(0.5f, 0.55f, 0);
            InitCells.InstantiateSprite(targetCell, prop, scaleFactor);
        }
    }


    //点击检测，在玩家点击右键或者点击有效格子前一直执行
    private IEnumerator ClickCellToPlaceProp ()
    {
        while(true)
        {
            //按下右键或者开始行走，退出
            if (Input.GetMouseButtonDown(1) )
                yield break;

            if (Input.GetMouseButtonDown(0) )
            {
                //做射线检测，判断玩家是否点击格子
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int layerMask = (1 << 8) | (1 << 9);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 100f, layerMask);

                if (hit.collider != null)
                {
                    //根据格子的tag获取对应脚本
                    GameObject hitedCell = hit.transform.gameObject;
                    Cell cell = Utility.GetCellScriptByTag(hitedCell);

                    //点击了有效范围内的格子，在格子上生成道具
                    if (targetCells.ContainsKey(cell.index))
                    {
                        InstantiateProp(cell, hitedCell, propSprite);
                        player.UseProp(gameObject.tag);
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }


    //AI调用接口
    private IEnumerator ClickCellToPlaceProp(GameObject targetCell, GameObject prop)
    {
        Cell cell = Utility.GetCellScriptByTag(targetCell);
        InstantiateProp(cell, targetCell, prop);

        yield return new WaitForSeconds(0.5f);
        //更新道具数量
        player.UseProp(gameObject.tag);
    }


}
