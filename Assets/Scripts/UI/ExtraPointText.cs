using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraPointText : MonoBehaviour
{
    private Text extraPointText;

    private void Start()
    {
        extraPointText = GetComponent<Text>();
    }

    public void ShowExtraPointText(int point)
    {
        string symbol ="";
        if (point > 0)
            symbol = "+";
        extraPointText.text = symbol + point.ToString();
    }

    public void ClearExtraPointText()
    {
        extraPointText.text = "";
    }
}
