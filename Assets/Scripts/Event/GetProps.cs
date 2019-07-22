using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全员随机获取一个道具
/// </summary>
public class GetProps : MonoBehaviour
{
    public GameObject[] propsGobj;
    private List<Player> players;


    void Awake()
    {
        players = new List<Player>();
        //EventCell.RegisterEvent(GetProp);

        GameObject[] players_obj = GameManager.instant.players;
        foreach (var item in players_obj)
            players.Add(item.GetComponent<Player>());

    }


    //全员随机获取一个道具
    public void GetProp()
    {
        foreach (Player item in players)
        {
            Dictionary<string, int> props = item.props;
            int random = Random.Range(0, propsGobj.Length);
            item.UpdatePropAmount(propsGobj[random].tag, 1);
            StartCoroutine(item.GetCellEffect(propsGobj[random]));
        }
    }
}
