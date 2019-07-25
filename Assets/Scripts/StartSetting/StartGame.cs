using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject bgMusic;
    public Dropdown dropDown;

    public void LoadGameScene()
    {
        //设置AI数量
        int aiNum = Convert.ToInt32(dropDown.options[dropDown.value].text);
        GameManager.instant.aiNumber = 4 - aiNum;

        //加载游戏场景
        SceneManager.LoadScene("GameScene");
        DontDestroyOnLoad(bgMusic);
    }
}
