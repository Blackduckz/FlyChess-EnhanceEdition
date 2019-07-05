using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBomb : MonoBehaviour
{
    public GameObject bomb;

    private void Awake()
    {
        EventCell.RegisterEvent(CrateBomb);
    }

    //生成炸弹
    public void CrateBomb()
    {
        GameObject bombInsant =  Instantiate(bomb, new Vector3(0f, 1.5f, 0f), Quaternion.identity);
        if (Bomb.instant != bombInsant.GetComponent<Bomb>())
        {
            Destroy(bombInsant);
            Bomb.instant.InitTriggerPoint();
        }
    }

}
