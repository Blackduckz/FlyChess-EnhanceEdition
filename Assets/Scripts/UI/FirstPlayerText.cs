using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPlayerText : MonoBehaviour
{
    private Text firstPlayerText;

    void Start()
    {
        firstPlayerText = GetComponent<Text>();
    }

    public void UpdateFirstPlayerText(string playerText)
    {
        firstPlayerText.text = "第一名为：" + playerText;
    }
}
