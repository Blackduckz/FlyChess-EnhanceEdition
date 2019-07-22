using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 任意骰子，点击后出现6个不同点数骰子
/// </summary>

public class CheatDiceFunc : MonoBehaviour
{
    public GameObject presentPanel;         //骰子展示槽
    public Text extraPointText;             //额外点数Text

    private Button[] buttons;
    private Button clickedBtn;
    private Player player;

    private void Start()
    {
        buttons = presentPanel.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
            button.onClick.AddListener(DiscretionaryPoint);
    }

    //显示出隐藏的骰子
    public void ShowDices()
    {
        player = GameManager.instant.GetPlayer();
        if(player.props[gameObject.tag] > 0)
        {
            extraPointText.gameObject.SetActive(false);
            foreach (Button button in buttons)
                button.gameObject.SetActive(true);
        }
    }

    //获取当前点击的button，转换对应点数
    public void DiscretionaryPoint()
    {
        GameObject clickedBtnObj = EventSystem.current.currentSelectedGameObject;
        clickedBtn = clickedBtnObj.GetComponent<Button>();

        int pointText = Convert.ToInt32(clickedBtnObj.GetComponentInChildren<Text>().text);
        RunDiscretionaryPoint(pointText);
        clickedBtn.enabled = false;
    }

    //供AI调用接口
    public void DiscretionaryPoint(int point)
    {
        string btnName = "Button" + point;
        Transform clickedBtnTrsf = presentPanel. transform.Find(btnName);
        clickedBtn = clickedBtnTrsf.GetComponent<Button>();
        clickedBtn.enabled = false;

        int pointText = Convert.ToInt32(clickedBtn.GetComponentInChildren<Text>().text);
        RunDiscretionaryPoint(pointText);
    }

    //使玩家行走骰子对应点数
    private void RunDiscretionaryPoint(int pointText)
    {
        GameManager.instant.StartGameLoop(pointText);
        foreach (Button button in buttons)
        {
           if(button != clickedBtn)
                button.gameObject.SetActive(false);
        }

        //禁用当前道具，更新道具数量
        player.UseProp(gameObject.tag);

        //player.skipRound = true;
        GameManager.instant.clearAfterRound += DisactiveDices;
    }

    //隐藏骰子
    public void DisactiveDices()
    {
        clickedBtn.gameObject.SetActive(false);
        extraPointText.gameObject.SetActive(true);

        GameManager.instant.clearAfterRound -= DisactiveDices;
    }
}
