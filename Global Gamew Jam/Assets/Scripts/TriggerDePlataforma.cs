using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDePlataforma : MonoBehaviour
{
    protected GameObject plataforma;

    protected void Awake()
    {
        plataforma = transform.parent.gameObject;
    }
}
