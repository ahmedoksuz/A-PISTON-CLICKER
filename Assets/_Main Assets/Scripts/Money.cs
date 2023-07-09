using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private float closingTime;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ClseMe), closingTime);
    }

    private void ClseMe()
    {
        gameObject.SetActive(false);
    }
}