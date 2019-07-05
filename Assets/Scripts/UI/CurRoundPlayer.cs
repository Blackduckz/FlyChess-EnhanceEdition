using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurRoundPlayer : MonoBehaviour
{
    private Text playerText;

    private void Awake()
    {
        playerText = GetComponent<Text>();
    }

    public void UpdateText(string text)
    {
        playerText.text = "当前玩家回合："+ text;
    }
}
