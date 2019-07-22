using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraPointText : MonoBehaviour
{
    public Text pointText;

    private void Start()
    {
        pointText = GetComponent<Text>();
    }

    public void ShowExtraPointText(int point)
    {
        string symbol ="";
        if (point > 0)
            symbol = "+";
        pointText.text = symbol + point.ToString();
    }

    public void ClearExtraPointText()
    {
        pointText.text = "";
    }
}
