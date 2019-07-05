﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DelayMethod
{
    public static Coroutine DelayToDo(this MonoBehaviour mono, float delayTime, Action action, bool ignoreTimeScale = false)
    {
        Coroutine coroutine = null;
        if (ignoreTimeScale)
        {
            coroutine = mono.StartCoroutine(DelayIgnoreTimeToDo(delayTime, action));
        }
        else
        {
            coroutine = mono.StartCoroutine(DelayToInvokeDo(delayTime, action));

        }
        return coroutine;
    }

    public static IEnumerator DelayToInvokeDo(float delaySeconds, Action action)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    public static IEnumerator DelayIgnoreTimeToDo(float delaySeconds, Action action)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + delaySeconds)
        {
            yield return null;
        }
        action();
    }

    public static bool IsNullOrEntry(this string str)
    {
        return String.IsNullOrEmpty(str);
    }
}
