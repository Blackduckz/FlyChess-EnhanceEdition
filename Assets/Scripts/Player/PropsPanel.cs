using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropsPanel : MonoBehaviour
{
    //道具Tag与其对应数量Text词典，用于修改对应道具数量
    [HideInInspector] public Dictionary<string, Text> propsDic;


    private void Awake()
    {
        propsDic = new Dictionary<string, Text>();
        InitPropsDic();
        //Button[] buttons = GetComponentsInChildren<Button>();
        //foreach (Button btn in buttons)
        //    btn.onClick.AddListener(ClickPropBtn);
    }

    //初始化propsDic
    private void InitPropsDic()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            string propTag = child.tag;
            Text porpText = child.GetComponentInChildren<Text>();
            propsDic.Add(propTag, porpText);
        }
    }

    //更新道具数量
    public void UpdatePropAmount(string propTag,int amount)
    {
        Text propAmountText = propsDic[propTag];
        propAmountText.text = amount.ToString();
    }

    //public void ClickPropBtn()
    //{
    //    GameObject clickedBtn = EventSystem.current.currentSelectedGameObject;
    //    Text amountText = clickedBtn.GetComponentInChildren<Text>();
    //    int amount = Convert.ToInt32(amountText);

    //    if (amount > 0)
    //        ;
    //}
}
