using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SaveSystems : Singleton<SaveSystems>
{
    public static void save(Slot[,] slots)
    {
        PlayerPrefs.SetInt("i", slots.GetLength(0));
        PlayerPrefs.SetInt("j", slots.GetLength(1));
        
        var slotData = new SlotData[slots.GetLength(0) * slots.GetLength(1)];
        var index = 0;
        for (var i = 0; i < slots.GetLength(0); i++)
        for (var j = 0; j < slots.GetLength(1); j++)
        {
            if (slots[i, j].slotData._slotType == Slot.SlotType.fullSlot)
                slots[i, j].slotData.pistonLevel = slots[i, j].myPiston.level;

            slotData[index] = slots[i, j].slotData;
            index++;
        }

        var json = JsonConvert.SerializeObject(slotData);
        PlayerPrefs.SetString("JsonString", json);
    }

    public static Slot[,] load()
    {
        var json = PlayerPrefs.GetString("JsonString", "null");
        if (json == "null") return null;

        var rowSize = PlayerPrefs.GetInt("i");
        var colSize = PlayerPrefs.GetInt("j");

        var slotData = JsonConvert.DeserializeObject<SlotData[]>(json);

        var slots = new Slot[rowSize, colSize];
        var index = 0;
        for (var i = 0; i < slots.GetLength(0); i++)
        for (var j = 0; j < slots.GetLength(1); j++)
        {
            slots[i, j] = new Slot();
            slots[i, j].slotData = slotData[index];
            index++;
        }

        return slots;
    }

    public static void SaveManinSlots(Slot[] slots)
    {
        var slotData = new SlotData[slots.Length];
        var index = 0;

        for (var j = 0; j < slots.Length; j++)
        {
            if (slots[j].slotData._slotType == Slot.SlotType.fullSlot)
                slots[j].slotData.pistonLevel = slots[j].myPiston.level;

            slotData[index] = slots[j].slotData;
            index++;
        }

        var json = JsonConvert.SerializeObject(slotData);
        PlayerPrefs.SetString("JsonStringForMain", json);
    }

    public static Slot[] LoadManinSlots()
    {
        var json = PlayerPrefs.GetString("JsonStringForMain", "null");
        if (json == "null") return null;

        var slotData = JsonConvert.DeserializeObject<SlotData[]>(json);
        var slots = new Slot[slotData.Length];
        var index = 0;
        for (var j = 0; j < slots.Length; j++)
        {
            slots[j] = new Slot();
            slots[j].slotData = slotData[index];
            index++;
        }

        return slots;
    }
}