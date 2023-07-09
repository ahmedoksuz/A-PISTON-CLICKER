using System.Collections;
using System.Collections.Generic;
using GPHive.Game;
using UnityEngine;

public class LockManager : Singleton<LockManager>
{
    [SerializeField] private Transform lockCnavas;
    [SerializeField] private GameObject lockAreaPrefebReferance;
    [SerializeField] private List<LockIndex> _lockIndexes = new();

    public void StartLockAreas()
    {
        for (var i = 0; i < _lockIndexes.Count; i++) SpawnLockArea(_lockIndexes[i]);
    }

    private void SpawnLockArea(LockIndex lockIndex)
    {
        if (PlayerPrefs.GetInt(lockIndex.referanceIndex.ToString(), 0) == 0)
        {
            var go = ObjectPooling.Instance.GetFromPool(lockAreaPrefebReferance);
            var lockArea = go.GetComponent<LockArea>();
            lockArea.referanceIndex = lockIndex.referanceIndex.ToString();
            lockArea.price = lockIndex.price;
            lockArea.Slots =
                SlotSpawnAndManage.Instance.GetSlotWithLineCount(lockIndex.lineIndex, lockIndex.colmnCount);

            lockArea.CalculatePos(lockCnavas);
            lockArea.CalculatePos(lockCnavas);
            go.SetActive(true);
        }
    }
}

[System.Serializable]
public class LockIndex
{
    public int referanceIndex, lineIndex, colmnCount, price;
}