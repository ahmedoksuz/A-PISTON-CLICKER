using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUIController : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;

    public void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
    }
}