using System;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;


[Serializable]
public class Slot : MonoBehaviour
{
    [HideInInspector] [SerializeField] public SlotData slotData;
    [SerializeField] private GameEvent mergeEvent, slotEvent;

    public enum SlotType
    {
        nullSlot,
        fullSlot
    }

    [SerializeField] private Transform slotCenterPoint;
    [HideInInspector] public Piston myPiston;
    private bool awakeWithPiston;
    [HideInInspector] public Slot leftSlot, rightSlot, frontSlot, backSlot;
    [HideInInspector] public List<Slot> _slotsAround = new();
    [SerializeField] private ParticleSystem spawnPistonParticle, mergePistonParticle, setPistonParticle;

    public MeshRenderer MeshRenderer;
    public Collider Collider;

    //[SerializeField] private FloatVariable percentage;

    private void Start()
    {
        if (leftSlot != null)
            _slotsAround.Add(leftSlot);
        if (rightSlot != null)
            _slotsAround.Add(rightSlot);
        if (frontSlot != null)
            _slotsAround.Add(frontSlot);
        if (backSlot != null)
            _slotsAround.Add(backSlot);
    }


    public bool SlotIsNull()
    {
        if (slotData._slotType == SlotType.nullSlot)
            return true;
        else
            return false;
    }

    public void SetPiston(Piston piston, bool newSpawn)
    {
        myPiston = piston;
        if (!slotData.amISpawned) AnimatorClose();

        if (newSpawn)
            spawnPistonParticle.Play();

        myPiston.transform.position = slotCenterPoint.position;
        slotData._slotType = SlotType.fullSlot;
    }


    private void Merge(Piston piston)
    {
        mergeEvent.Raise();
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        myPiston.gameObject.SetActive(false);
        piston.gameObject.SetActive(false);
        SpawnPiston.Instance.NewPistonSpawn(this, piston.level + 1);
        mergePistonParticle.Play();
    }

    public bool Give(Piston piston)
    {
        if (slotData._slotType == SlotType.nullSlot)
        {
            if (PlayerPrefs.GetInt("Tutorial", 0) < 6)
            {
                if (SlotSpawnAndManage.Instance._slots[0, 0] == this)
                    slotEvent.Raise();
                else
                    return false;
            }

            SetPiston(piston, false);
            setPistonParticle.Play();
            return true;
        }
        else if (slotData._slotType == SlotType.fullSlot && piston.level == myPiston.level)
        {
            Merge(piston);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void JustPlacement(Piston piston)
    {
        if (slotData._slotType == SlotType.nullSlot)
        {
            var position = slotCenterPoint.transform.position;
            piston.transform.position = new Vector3(position.x, piston.transform.position.y, position.z);
        }
    }

    public void UnPlacement()
    {
        if (myPiston != null) AnimatorClose();
        myPiston = null;
        slotData._slotType = SlotType.nullSlot;
    }


    public void AnimatorPlay(Animator startPiston, int i, int j)
    {
        if (myPiston.animator.enabled)
            return;


        if (i == 0 && j == 0)
            PlayAnimationFromTime(myPiston.animator, LogAnimationPercentage(startPiston));
        else
            AnimationStartTimeCalculaterAndPlayer(startPiston, myPiston.animator, i * 50 + j * 50);

        myPiston.animator.enabled = true;
    }

    public void AnimatorClose(int i, int j)
    {
        if (slotData.amISpawned)
            PlayAnimationFromTime(myPiston.animator, i * 50 + j * 50);
        else
            myPiston.animator.Play("PistonAnim", 0, 0);

        myPiston.animator.Update(0.01f);
        myPiston.animator.enabled = false;
    }

    public void AnimatorClose()
    {
        myPiston.animator.Play("PistonAnim", 0, 0);

        myPiston.animator.Update(0.01f);
        myPiston.animator.enabled = false;
    }


    private void AnimationStartTimeCalculaterAndPlayer(Animator referanceAnimator, Animator changingAnimator,
        float percentageOfIncrease)
    {
        var refPercentage = (LogAnimationPercentage(referanceAnimator) + percentageOfIncrease) % 100f;
        PlayAnimationFromTime(changingAnimator, refPercentage);
    }


    private void PlayAnimationFromTime(Animator _animator, float percentage)
    {
        _animator.Play("PistonAnim", 0, percentage / 100f);
    }

    private float LogAnimationPercentage(Animator animator)
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var _normalizedTime = stateInfo.normalizedTime;
        _normalizedTime -= Mathf.Floor(_normalizedTime);
        return _normalizedTime * 100;
    }
}

[Serializable]
public class SlotData
{
    public Slot.SlotType _slotType;
    public int pistonLevel;
    public bool canWork, working = false, boost, amISpawned = false;
}