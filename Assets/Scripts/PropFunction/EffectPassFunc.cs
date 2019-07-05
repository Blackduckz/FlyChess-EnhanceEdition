using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 效果传递道具功能
/// </summary>
public class EffectPassFunc : MonoBehaviour
{
    //效果传递
    public void PassEffect()
    {
        //获取玩家引用
        Player playerA = GameManager.instant.GetPlayer();
        Player playerB = GameManager.instant.GetPlayer(1);

        if (playerA.props[gameObject.tag] > 0 && playerA.extraPoint != 0)
        {
            //传递额外点数A->B
            playerB.extraPoint += playerA.extraPoint;
            playerB.extraPointText.ShowExtraPointText(playerB.extraPoint);

            //清空A的额外点数
            playerA.extraPoint = 0;
            playerA.extraPointText.ClearExtraPointText();

            //更新道具数量
            playerA.UpdatePropAmount(gameObject.tag, -1);

            Player player = GameManager.instant.GetPlayer();
            player.ChangeButtonState(false);
        }
    }
}
