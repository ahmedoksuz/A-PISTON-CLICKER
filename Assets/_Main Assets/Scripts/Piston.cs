using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class Piston : MonoBehaviour
{
    [HideInInspector] public int level, boostLevel;
    public float baseStartValue, baseEarnMultiplier, ıncomeEarnMultiplier;
    public Animator animator;
    public CoefficientUpgrade IncomeUpgrade;

    private void Awake()
    {
        animator.enabled = false;
    }

    private void OnEnable()
    {
        ArmsClose();
    }

    [SerializeField] private GameObject fronttArm, rightArm;

    public void ArmsClose()
    {
        fronttArm.SetActive(false);
        rightArm.SetActive(false);
    }

    public void leftArmOpen()
    {
        rightArm.SetActive(true);
    }

    public void fronttArmOpen()
    {
        fronttArm.SetActive(true);
    }
}