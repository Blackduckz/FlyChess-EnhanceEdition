using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;

[TaskCategory("CanUse/CanUseP3")]
public class CanGetMorePoint : Conditional
{
    public GetMaxMovment getMax;
    public UseP3 p3;
    public ThereAreItemOnCell onCell;

    private GameManager manager;
    private Player player;
    private int maxMovement;
    private int stride;
    private int benefitFillter;
    private int startOffset;

    private List<NormalCell> normalCells;
    private Dictionary<NormalCell, int> morePointCells;

    public override void OnAwake()
    {
        manager = getMax.manager;
        player = manager.GetPlayer();
    }

    //检查是否有能提供足够收益的点数格
    public override TaskStatus OnUpdate()
    {
        int count = manager.cellDic.Count;
        maxMovement = getMax.maxMovement;
        stride = getMax.stride;
        benefitFillter = getMax.benefitFillter;
        startOffset = getMax.startOffset;

        int startIndex =  Utility.GetVaildIndex(player.curCellIndex + startOffset, count);
        if (maxMovement > 0)
        {
            if (player.extraPoint > 0)
                normalCells = Utility.GetTargetCells<NormalCell>(startIndex, maxMovement - startOffset + 1, stride);
            else
                normalCells = Utility.GetTargetCells<NormalCell>(startIndex, maxMovement, stride); 
        }

        else if (maxMovement < 0)
        {
            int maxGoBackIndex = Utility.GetVaildIndex(startIndex - startOffset - getMax.maxGoBackDistance, count);
            int goBackDistance = Utility.GetVaildIndex(startIndex - maxGoBackIndex + 1, count);
            normalCells = Utility.GetTargetCells<NormalCell>(startIndex, goBackDistance, stride);
        }
        else
            normalCells = Utility.GetTargetCells<NormalCell>(startIndex, getMax.maxGoBackDistance, stride);

        //没有符合条件的格子
        if (normalCells.Count == 0)
             return TaskStatus.Failure;

        //存在足够收益的格子
        if (Evaluate())
        {
            foreach (var item in morePointCells)
            {
                int targetIndex = item.Key.index;

                //目标格子在前方
                if (maxMovement > 0)
                {
                    int dianstance = targetIndex - startIndex + 1;
                    p3.btnIndex = dianstance;
                    startIndex = Utility.GetVaildIndex(startIndex + 1, count);
                    onCell.SetData(startIndex, dianstance, 1, 0);
                }
                   
                //只能倒退情况
                else
                {
                    int goBackDis = player.extraPoint - manager.morePoint;
                    p3.btnIndex = targetIndex - goBackDis - (startIndex - startOffset);
                    onCell.SetData(targetIndex, startIndex - startOffset - targetIndex, 1, 0);
                }
                
                return TaskStatus.Success;
            }
        }

         return TaskStatus.Failure;
    }

    //评估函数，找出是否有能提供足够收益的正点数格
    private bool Evaluate()
    {
        //剔除负数点数格
        for (int i = 0; i < normalCells.Count; i++)
        {
            if (normalCells[i].extraPoint < 0)
            {
                normalCells.RemoveAt(i);
                i--;
            }
        }
        //计算每个正数格能带来的收益
        //收益定义为走到该格子之后，下一步能移动到的最近格子与行走之前格子距离的差值
        morePointCells = new Dictionary<NormalCell, int>();
        foreach (NormalCell cell in normalCells)
        {
            int distance = cell.index - player.curCellIndex;
            int benifit = 1 + cell.extraPoint + distance;
            if (benifit > benefitFillter)
                morePointCells.Add(cell, benifit);
        }

        //如果有满足条件的正数格，按照收益排序
        if (morePointCells.Count > 0)
        {
            morePointCells = morePointCells.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, o => o.Value);
            return true;
        }

        return false;
    }
}
