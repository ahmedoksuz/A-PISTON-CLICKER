using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GPHive.Game;
using NaughtyAttributes;
using UnityEditor.Experimental;
using UnityEngine;

public class SlotSpawnAndManage : Singleton<SlotSpawnAndManage>
{
    [SerializeField] private GameObject slotReferanceObj;
    [HideInInspector] public Slot[,] _slots;
    [SerializeField] private Slot[] mainSlots;
    [SerializeField] private Transform slotSpawnRefPos;
    [SerializeField] private float spawnPointDistanceX, spawnPointDistanceZ;
    [SerializeField] private int lineCount, columnCount;

    [SerializeField] private Animator startPistonAnimator;
    [SerializeField] private Piston startPistonPiston;

    [SerializeField] private GameObject boostParticleReferance;
    private List<GameObject> boostParticles = new();
    [SerializeField] private List<MeshRenderer> boostObjects = new();
    public int connectingCount;

    private LockManager lockManager;
    private ObjectPooling objectPooling;
    private SpawnPiston spawnPiston;
    [SerializeField] private GameEvent pistonStuvationChange;

    private void Awake()
    {
        lockManager = LockManager.Instance;
        objectPooling = ObjectPooling.Instance;
        spawnPiston = SpawnPiston.Instance;
    }

    private void Start()
    {
        _slots = new Slot[lineCount, columnCount];
        SpawnSlots();
        _slots[0, 0].slotData.canWork = true;

        SetSlotRightLeftFrontAndBack();
        startPistonAnimator.enabled = true;
        lockManager.StartLockAreas();
        Invoke(nameof(CalculateAllWorking), .1f);
    }

    private void ParticleSpawn()
    {
        for (var i = 0; i < lineCount; i++)
        {
            var go = objectPooling.GetFromPool(boostParticleReferance);
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y,
                _slots[i, 0].transform.position.z);
            go.SetActive(true);
            boostParticles.Add(go);
        }

        foreach (var particle in boostParticles) particle.SetActive(false);
    }

    private void SpawnSlots()
    {
        var Temp = SaveSystems.load();

        for (var i = 0; i < lineCount; i++)
        for (var j = 0; j < columnCount; j++)
        {
            var go = objectPooling.GetFromPool(slotReferanceObj);
            go.transform.position = slotSpawnRefPos.position +
                                    new Vector3(j * spawnPointDistanceX, 0, i * spawnPointDistanceZ);
            _slots[i, j] = go.GetComponent<Slot>();
            _slots[i, j].slotData.amISpawned = true;
            go.SetActive(true);

            if (Temp != null && Temp.GetLength(0) > 1)
                if (i < Temp.GetLength(0) && j < Temp.GetLength(1))
                {
                    _slots[i, j].slotData.working = Temp[i, j].slotData.working;
                    _slots[i, j].slotData.boost = Temp[i, j].slotData.boost;
                    _slots[i, j].slotData.canWork = Temp[i, j].slotData.canWork;
                    _slots[i, j].slotData._slotType = Temp[i, j].slotData._slotType;
                    _slots[i, j].slotData.pistonLevel = Temp[i, j].slotData.pistonLevel;


                    if (_slots[i, j].slotData._slotType == Slot.SlotType.fullSlot)
                        spawnPiston.NewPistonSpawn(_slots[i, j], _slots[i, j].slotData.pistonLevel);
                }
        }

        var tempSlots = SaveSystems.LoadManinSlots();

        for (var i = 0; i < mainSlots.Length; i++)
            if (tempSlots != null && tempSlots.Length > 0)
                if (i <= tempSlots.Length)
                {
                    mainSlots[i].slotData.working = tempSlots[i].slotData.working;
                    mainSlots[i].slotData.boost = tempSlots[i].slotData.boost;
                    mainSlots[i].slotData.canWork = tempSlots[i].slotData.canWork;
                    mainSlots[i].slotData._slotType = tempSlots[i].slotData._slotType;
                    mainSlots[i].slotData.pistonLevel = tempSlots[i].slotData.pistonLevel;
                    mainSlots[i].slotData.amISpawned = false;

                    if (mainSlots[i].slotData._slotType == Slot.SlotType.fullSlot)
                        spawnPiston.NewPistonSpawn(mainSlots[i], mainSlots[i].slotData.pistonLevel);
                }

        ParticleSpawn();
    }

    public List<Slot> GetSlotWithLineCount(int lineIndex, int _colmnCount)
    {
        if (_colmnCount > columnCount) _colmnCount = columnCount;

        List<Slot> ReturnList = new();
        for (var i = 0; i < _colmnCount; i++) ReturnList.Add(_slots[lineIndex, i]);

        return ReturnList;
    }

    private void SetSlotRightLeftFrontAndBack()
    {
        for (var i = 0; i < lineCount; i++)
        for (var j = 0; j < columnCount; j++)
        {
            var currentSlot = _slots[i, j];
            if (j > 0)
                currentSlot.leftSlot = _slots[i, j - 1];
            if (j < columnCount - 1)
                currentSlot.rightSlot = _slots[i, j + 1];
            if (i > 0)
                currentSlot.backSlot = _slots[i - 1, j];
            if (i < lineCount - 1)
                currentSlot.frontSlot = _slots[i + 1, j];
        }
    }

    private int counter;

    private void CheckSlot(bool firstSlot, Slot currentSlot)
    {
        if (firstSlot)
        {
            currentSlot.slotData.canWork = true;
            if (currentSlot.slotData._slotType == Slot.SlotType.fullSlot)
            {
                currentSlot.slotData.working = true;
                startPistonPiston.leftArmOpen();
                counter++;
            }
            else
            {
                currentSlot.slotData.working = false;
                startPistonPiston.ArmsClose();
            }
        }
        else
        {
            foreach (var slot in currentSlot._slotsAround)
                if (currentSlot.slotData._slotType == Slot.SlotType.fullSlot && slot.slotData.working)
                {
                    currentSlot.slotData.canWork = true;
                    break;
                }

            if (currentSlot.slotData._slotType == Slot.SlotType.fullSlot)
            {
                currentSlot.slotData.working = currentSlot.slotData.canWork;
                if (currentSlot.slotData.working) counter++;
            }
        }
    }

    public void CalculateAllWorking()
    {
        if (_slots != null)
        {
            //  first clear
            for (var i = 0; i < _slots.GetLength(0); i++)
            for (var j = 0; j < _slots.GetLength(1); j++)
            {
                var currentSlot = _slots[i, j];
                currentSlot.slotData.canWork = false;
                currentSlot.slotData.working = false;
                currentSlot.slotData.boost = false;
            }

            // working progges calculate
            for (var i = 0; i < _slots.GetLength(0); i++)
            {
                counter = 0;

                for (var j = 0; j < _slots.GetLength(1); j++)
                {
                    var currentSlot = _slots[i, j];
                    if (i == 0 && j == 0)
                        CheckSlot(true, currentSlot);
                    else
                        CheckSlot(false, currentSlot);
                }


                for (var k = 0; k < _slots.GetLength(1); k++)
                    if (counter == _slots.GetLength(1))
                    {
                        _slots[i, k].slotData.boost = true;
                        _slots[i, k].myPiston.boostLevel = i + 1;
                        if (k == 0 && i > 0)
                            if (!boostParticles[i].activeSelf)
                            {
                                boostParticles[i].SetActive(true);
                                boostObjects[i - 1].material.SetFloat("_albedo_sat", 0);
                            }
                    }
                    else
                    {
                        if (_slots[i, k].slotData._slotType == Slot.SlotType.fullSlot)
                            _slots[i, k].myPiston.boostLevel = 0;

                        if (k == 0 && i > 0)
                            if (boostParticles[i].activeSelf)
                            {
                                boostParticles[i].SetActive(false);
                                boostObjects[i - 1].material.SetFloat("_albedo_sat", -1);
                            }
                    }
            }

            // working progges calculate again
            int returnTime;
            if (lineCount > columnCount)
                returnTime = lineCount;
            else
                returnTime = columnCount;


            for (var q = 0; q < returnTime; q++)
            for (var i = 0; i < _slots.GetLength(0); i++)
            for (var j = 0; j < _slots.GetLength(1); j++)
            {
                var currentSlot = _slots[i, j];

                if (currentSlot.slotData.working)
                    foreach (var slot in currentSlot._slotsAround)
                        if (!slot.slotData.canWork)
                        {
                            slot.slotData.canWork = true;
                            if (slot.slotData._slotType == Slot.SlotType.fullSlot)
                                slot.slotData.working = slot.slotData.canWork;
                        }
            }

            connectingCount = 0;

            //animation calculate 
            for (var i = 0; i < _slots.GetLength(0); i++)
            for (var j = 0; j < _slots.GetLength(1); j++)
            {
                var currentSlot = _slots[i, j];
                if (currentSlot.slotData._slotType == Slot.SlotType.fullSlot)
                {
                    if (!currentSlot.slotData.working)
                    {
                        currentSlot.AnimatorClose(i, j);
                        TapAndSpeed.Instance.RemoveAnimator(currentSlot.myPiston.animator);
                    }
                    else
                    {
                        connectingCount++;
                        currentSlot.AnimatorPlay(startPistonAnimator, i, j);
                        TapAndSpeed.Instance.AddAnimator(currentSlot.myPiston.animator);
                    }


                    currentSlot.myPiston.ArmsClose();
                    if (currentSlot.frontSlot != null &&
                        currentSlot.frontSlot.slotData._slotType == Slot.SlotType.fullSlot)
                        currentSlot.myPiston.fronttArmOpen();

                    if (currentSlot.rightSlot != null &&
                        currentSlot.rightSlot.slotData._slotType == Slot.SlotType.fullSlot)
                        currentSlot.myPiston.leftArmOpen();
                }
            }

            SaveSystems.save(_slots);
            SaveMainSlot();
            pistonStuvationChange.Raise();
        }
    }

    public void SaveMainSlot()
    {
        SaveSystems.SaveManinSlots(mainSlots);
    }
}