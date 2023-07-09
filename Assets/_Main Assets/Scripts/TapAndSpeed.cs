using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines.Primitives;
using UnityEngine;

public class TapAndSpeed : Singleton<TapAndSpeed>
{
    [SerializeField] private List<Animator> _animators = new();
    [SerializeField] private float defaultSpeed, boostSpeed;
    [SerializeField] private CoefficientUpgrade speedUpgrade;
    [SerializeField] private float speedMultiplier;

    private float timer;

    private void Start()
    {
        SetAnimatorSpeed(defaultSpeed);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                SetAnimatorSpeed(boostSpeed);
                timer = 0;
            }
        }

        if (timer > .5f) SetAnimatorSpeed(defaultSpeed);

        timer += Time.deltaTime;
    }

    private void SetAnimatorSpeed(float speed)
    {
        if (speed > defaultSpeed)
            speed *= speedUpgrade.Level * (speedMultiplier * (boostSpeed / defaultSpeed)) + 1;
        else
            speed *= speedUpgrade.Level * speedMultiplier + 1;

        foreach (var animator in _animators) animator.speed = speed;
    }

    public void AddAnimator(Animator animator)
    {
        if (!_animators.Contains(animator)) _animators.Add(animator);
    }

    public void RemoveAnimator(Animator animator)
    {
        if (_animators.Contains(animator)) _animators.Remove(animator);
    }
}