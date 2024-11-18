using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger: MonoBehaviour
{
    public abstract void Activate();
    public abstract void DeActivate();
}
