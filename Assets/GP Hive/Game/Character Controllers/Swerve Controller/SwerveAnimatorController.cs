using System;
using System.Collections;
using System.Collections.Generic;
using GPHive.Core;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SwerveAnimatorController : MonoBehaviour
{
    private Animator animator;
    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int IdleHash = Animator.StringToHash("Idle");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        EventManager.LevelStarted += SetRun;
        EventManager.EnteredNextLevel += SetIdle;
        EventManager.LevelFailed += SetIdle;
        EventManager.LevelSuccessed += SetIdle;
    }

    private void SetRun()
    {
        animator.SetTrigger(RunHash);
    }

    private void SetIdle()
    {
        animator.SetTrigger(IdleHash);
    }
}