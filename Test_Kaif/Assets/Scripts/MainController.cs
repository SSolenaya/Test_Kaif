using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public static MainController Inst;

    public GlobalParamsSO globalParams;

    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }
}
