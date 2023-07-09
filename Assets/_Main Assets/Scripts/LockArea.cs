using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GPHive.Game;
using TMPro;
using UnityEngine;

public class LockArea : MonoBehaviour
{
    [HideInInspector] public string referanceIndex;
    [HideInInspector] public List<Slot> Slots = new();
    [HideInInspector] public int price;
    [SerializeField] private TextMeshProUGUI priceText;
    private PlayerEconomy _playerEconomy;

    private void Awake()
    {
        _playerEconomy = PlayerEconomy.Instance;
    }

    private void LockZone()
    {
        for (var i = 0; i < Slots.Count; i++)
        {
            Slots[i].Collider.enabled = false;
            Slots[i].MeshRenderer.enabled = false;
        }
    }

    public void UnlockZone()
    {
        if (_playerEconomy.GetMoney() >= price)
        {
            _playerEconomy.SpendMoney(price);
            PlayerPrefs.SetInt(referanceIndex, 1);
            for (var i = 0; i < Slots.Count; i++)
            {
                Slots[i].Collider.enabled = true;
                Slots[i].MeshRenderer.enabled = true;
            }

            gameObject.SetActive(false);
        }
    }

    public void CalculatePos(Transform lockCanvas)
    {
        transform.parent = lockCanvas.transform;

        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var minZ = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;
        var maxZ = float.MinValue;

        // objelerin pozisyonlarını kontrol et
        foreach (var slot in Slots)
        {
            minX = Mathf.Min(minX, slot.transform.position.x);
            minY = Mathf.Min(minY, slot.transform.position.y);
            minZ = Mathf.Min(minZ, slot.transform.position.z);
            maxX = Mathf.Max(maxX, slot.transform.position.x);
            maxY = Mathf.Max(maxY, slot.transform.position.y);
            maxZ = Mathf.Max(maxZ, slot.transform.position.z);
        }

        // resmin pozisyonunu hesapla
        var imagePos = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);
        transform.position = imagePos;

        var width = 200 * Slots.Count;
        width -= 100;
        var height = 200;
        transform.localScale = Vector3.one;
        var rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);
        priceText.text = "$"+_playerEconomy.ConvertToKBM(price);
        LockZone();
    }
}