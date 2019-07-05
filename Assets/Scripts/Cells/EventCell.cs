using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件格脚本，随机抽取事件执行
/// </summary>
public class EventCell : MonoBehaviour
{
    public int duration;            //效果持续时间
    public delegate void CellEvent();
    static List<CellEvent> eventList;

    private void Awake()
    {
        eventList = new List<CellEvent>();
    }

    public static void RegisterEvent(CellEvent cellEvent)
    {
        eventList.Add(cellEvent);
    }

    //执行随机抽取的事件
    public void ExecuteEvent()
    {
        int index = Random.Range(0, eventList.Count);
        eventList[index]();
    }
}
