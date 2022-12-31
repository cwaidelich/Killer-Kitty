using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EManager : MonoBehaviour
{
    #region Singelton
    public static EManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion




    public event Action onPlayerDead;
    public void TonPlayerDead()
    {
        if (onPlayerDead != null)
            onPlayerDead.Invoke();
    }
}
