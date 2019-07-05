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

    //使玩家行走骰子对应点数
    public void DiscretionaryPoint()
    {
        //获取当前点击的button，转换对应点数
        GameObject clickedBtn = EventSystem.current.currentSelectedGameObject;
        int pointText = Convert.ToInt32(clickedBtn.GetComponentInChildren<Text>().text);
        GameManager.instant.StartGameLoop(pointText);

        //隐藏骰子
        extraPointText.gameObject.SetActive(true);
        foreach (Button button in buttons)
            button.gameObject.SetActive(false);

        player.skipRound = true;
        player.ChangeButtonState(false);
        player.UpdatePropAmount(gameObject.tag,-1);
    }

}
