using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortStep
{
    public Action Action;
    public float duration;

    public SortStep(Action action, float duration)
    {
        Action = action;
        this.duration = duration;
    }
}
