using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using GPHive.Game;
using NaughtyAttributes;
using UnityEngine;

public class PathPump : MonoBehaviour
{
    private Vector3 startPosition;

    private SplineComputer splineComputer;
    private SplineMesh splineMesh;
    private bool moving;
    public bool Filling { get; private set; }
    [SerializeField] private PumpConfig pumpConfig;
    private SplinePoint firstPoint;
    private SplinePoint secondPoint;
    private Coroutine idlePumpCoroutine;
    [SerializeField] private GameObject moneyPoolReferance;
    [SerializeField] private Transform moneySpawnRefPoint;


    private void Awake()
    {
        splineComputer = GetComponentInParent<SplineComputer>();
        splineMesh = GetComponentInParent<SplineMesh>();
    }

    private void Start()
    {
        startPosition = transform.localPosition;
        firstPoint = splineComputer.GetPoint(splineComputer.pointCount - 1);
        secondPoint = splineComputer.GetPoint(splineComputer.pointCount - 2);
        splineComputer.RebuildImmediate();
    }

    private void SpawnAndThrowMoney()
    {
        for (var i = 0; i < SlotSpawnAndManage.Instance.connectingCount; i++)
        {
            var go = ObjectPooling.Instance.GetFromPool(moneyPoolReferance);
            go.transform.position = moneySpawnRefPoint.position;
            go.SetActive(true);
        }
    }

    public void Pump()
    {
        var _key = new SizeModifier.SizeKey(0, 0)
        {
            centerStart = .5f,
            centerEnd = .5f,
            size = pumpConfig.PumpSize,
            interpolation = pumpConfig.PumpCurve
        };

        splineMesh.sizeModifier.AddKey(_key);
        StartCoroutine(CO_Pump(_key));
    }

    private IEnumerator CO_Pump(SplineSampleModifier.Key key)
    {
        while (key.end < pumpConfig.PumpDistance)
        {
            key.end += Time.deltaTime * pumpConfig.PumpSpeed;
            splineMesh.Rebuild();
            yield return null;
        }

        while (key.end < 1)
        {
            key.end += Time.deltaTime * pumpConfig.PumpSpeed;
            key.start += Time.deltaTime * pumpConfig.PumpSpeed;
            splineMesh.Rebuild();
            yield return null;
        }

        while (key.start < 1)
        {
            key.start += Time.deltaTime * pumpConfig.PumpSpeed;
            splineMesh.Rebuild();
            yield return null;
        }

        splineMesh.sizeModifier.RemoveKey(0);
        splineMesh.Rebuild();
        SpawnAndThrowMoney();
    }
}