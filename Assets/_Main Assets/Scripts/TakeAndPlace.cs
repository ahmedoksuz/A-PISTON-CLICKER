using MoreMountains.NiceVibrations;
using UnityEngine;


public class TakeAndPlace : Singleton<TakeAndPlace>
{
    private bool isTaked = false;
    private Piston dragingPiston;
    private Slot placedSlot, takenSlot;
    private Camera _camera;
    private Status _status;

    private SlotSpawnAndManage _slotSpawnAndManage;

    private void Awake()
    {
        _slotSpawnAndManage = SlotSpawnAndManage.Instance;
    }

    public enum Status
    {
        noting,
        taeken,
        placed
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private Touch touch;
    private Ray ray;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            ray = Camera.main.ScreenPointToRay(touch.position);

            Take();
            MoveAndCancel();
            Place();

            if (touch.phase == TouchPhase.Ended && dragingPiston != null) CancelMove();
            //TouchEnd();
        }
    }

    private void Take()
    {
        if (_status == Status.noting)
            if (Physics.Raycast(ray, out var hit, 3000))
                if (hit.transform.gameObject.CompareTag("Slot"))
                {
                    var go = hit.transform.gameObject;
                    takenSlot = go.GetComponent<Slot>();
                    if (takenSlot.slotData._slotType == Slot.SlotType.fullSlot)
                    {
                        dragingPiston = takenSlot.myPiston;
                        _status = Status.taeken;
                        takenSlot.myPiston.ArmsClose();
                        takenSlot.UnPlacement();
                        _slotSpawnAndManage.CalculateAllWorking();
                        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                    }
                }
    }

    private void MoveAndCancel()
    {
        if (_status is Status.taeken or Status.placed && touch.phase != TouchPhase.Ended)
        {
            dragingPiston.transform.position = _camera.transform.position + ray.direction * 15.5f;
            dragingPiston.transform.position = new Vector3( dragingPiston.transform.position.x,.8f, dragingPiston.transform.position.z);
        }
    }

    private void CancelMove()
    {
        takenSlot.SetPiston(dragingPiston, false);
        dragingPiston = null;
        _status = Status.noting;
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        _slotSpawnAndManage.CalculateAllWorking();
    }

    private void Place()
    {
        if (_status is Status.taeken or Status.placed)
        {
            if (Physics.Raycast(ray, out var hit, 3000) && hit.transform.gameObject.CompareTag("Slot"))
                if (hit.transform.gameObject.CompareTag("Slot"))
                {
                    var slot = hit.transform.gameObject;
                    placedSlot = slot.GetComponent<Slot>();
                    placedSlot.JustPlacement(dragingPiston);
                    _status = Status.placed;
                    TouchEnd();
                    return;
                }

            if (placedSlot != null)
            {
                placedSlot = null;
                _status = Status.taeken;
            }
        }
    }

    private void TouchEnd()
    {
        if (touch.phase == TouchPhase.Ended && dragingPiston != null)
        {
            if (takenSlot == placedSlot)
            {
                CancelMove();
                return;
            }

            if (!placedSlot.Give(dragingPiston))
            {
                CancelMove();
            }
            else
            {
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                dragingPiston = null;
                _status = Status.noting;
                takenSlot.UnPlacement();
                placedSlot = null;
                takenSlot = null;
                _slotSpawnAndManage.CalculateAllWorking();
            }
        }
    }
}