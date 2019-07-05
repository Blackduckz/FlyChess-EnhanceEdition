using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 所有格子的父类，记录格子ID和移动朝向
/// 并提供一个注册方法
/// </summary>
public class Cell : MonoBehaviour
{
    public int index;           //格子ID
    public int moveDir;     //移动朝向

     public void Awake()
    {
        GameManager.instant.Register(index, gameObject);
    }

}
