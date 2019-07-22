using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 效果传递道具功能
/// </summary>
public class EffectPassFunc : MonoBehaviour
{
    public GameObject pointTextObj;

    private AudioSource audioSource;
    private Player playerA;
    private Player playerB;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartPassEffect()
    {
        StartCoroutine(PassEffect());
    }

    //效果传递
    public IEnumerator PassEffect()
    {
        //获取玩家引用
        playerA = GameManager.instant.GetPlayer();
        playerB = GameManager.instant.GetPlayer(1);

        if (playerA.props[gameObject.tag] > 0 && playerA.extraPoint != 0)
        {
            audioSource.Play();
            yield return StartCoroutine(PassExtraPointText());

            //传递额外点数A->B
            playerB.extraPoint += playerA.extraPoint;
            playerB.extraPointText.ShowExtraPointText(playerB.extraPoint);

            //清空A的额外点数
            playerA.extraPoint = 0;

            //更新道具数量
            playerA.UseProp(gameObject.tag);
        }
    }

    //将额外点数文字飞到下家框中
    private IEnumerator PassExtraPointText()
    {
        //生成一个Text，并设置Parent
        GameObject pointText = Instantiate(pointTextObj, transform.position, Quaternion.identity);
        pointText.transform.SetParent(playerA.extraPointText.transform.parent);

        //将PlayerA的extraPointText赋给Text
        Text text = pointText.GetComponent<Text>();
        text.text = playerA.extraPointText.pointText.text;

        //清空A的extraPointText
        playerA.extraPointText.ClearExtraPointText();

        //直接获取的position即为屏幕坐标，不需要转换
        Transform pointTextTrsf = pointText.transform;
        Vector3 targetPos = playerB.extraPointText.transform.position;

        float sqrRemainingDistance = (pointTextTrsf.position - targetPos).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            //使用MoveTowards进行平滑移动
            Vector3 newPosition = Vector3.MoveTowards(pointTextTrsf.position, targetPos, 12f);
            pointTextTrsf.position = newPosition;
            sqrRemainingDistance = (newPosition - targetPos).sqrMagnitude;
            yield return null;
        }
        //飞行完成，销毁
        Destroy(pointText);
    }
}
