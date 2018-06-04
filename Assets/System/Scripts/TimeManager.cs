using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PrefabAttribute("Singleton/TimeManager")]
public class TimeManager : Singleton<TimeManager>
{
    public event Action<float> onUpdate;
    public event Action onSecondUpdate;

    public event Action onFixedUpdate;
    public event Action onLateUpdate;

    float second = 0.0f;
    public static bool isPaused { get; private set; }

    // Use this for initialization
    void Start ()
    {

    }

    void Update ()
    {
        if (isPaused)
        {
            return;
        }

        if (onUpdate != null)
        {
            onUpdate(Time.deltaTime);
        }

        second += Time.deltaTime;
        if (second >= 1.0f)
        {
            second -= 1.0f;
            if (onSecondUpdate != null)
            {
                onSecondUpdate();
            }
        }
    }

    void FixedUpdate ()
    {
        if (isPaused)
        {
            return;
        }

        if (onFixedUpdate != null)
        {
            onFixedUpdate();
        }
    }

    void LateUpdate ()
    {
        if (isPaused)
        {
            return;
        }

        if (onLateUpdate != null)
        {
            onLateUpdate();
        }
    }

    public static void Pause (bool _isPaused)
    {
        isPaused = _isPaused;
    }


}
