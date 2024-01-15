using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

public class JsonConverter : MonoBehaviour
{
//    public static string ConvertToJson(PlayerData playerData)
//    {
//        string jsonString = "";
//        string mainData = JsonUtility.ToJson(playerData);

//        ChipData[] startChip = new ChipData[1];
//        startChip[0] = playerData.StartChipData;
//        string startChipData = ToJson(startChip, true);

//        string chipsData = ToJson(playerData.ChipDatas, true);

//        jsonString = "{\"Items\":[" + mainData + "," + startChipData + "," + chipsData + "]}";
//        Debug.Log(jsonString);
//        return jsonString;
//    }

//    public static PlayerData ConvertFromJson(string jsonString)
//    {
//        Debug.Log(jsonString);
//        PlayerData playerData = new PlayerData();

//        string[] separators = new string[] { "[{", "},{", "}]" };
//        string[] subs = jsonString.Split(separators, StringSplitOptions.RemoveEmptyEntries);

//        playerData = JsonUtility.FromJson<PlayerData>("{" + subs[0] + "}");
//        playerData.StartChipData = JsonUtility.FromJson<ChipData>("{\"Items\"{:" + subs[1] + "}}");
//        playerData.ChipDatas = FromJson<ChipData>("{\"Items\":{" + subs[2] + "}}");

//        return playerData;
//    }

//    //Конвертация массивов
//    public static T[] FromJson<T>(string json)
//    {
//        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
//        return wrapper.Items;
//    }

//    public static string ToJson<T>(T[] array)
//    {
//        Wrapper<T> wrapper = new Wrapper<T>();
//        wrapper.Items = array;
//        return JsonUtility.ToJson(wrapper);
//    }

//    public static string ToJson<T>(T[] array, bool prettyPrint)
//    {
//        Wrapper<T> wrapper = new Wrapper<T>();
//        wrapper.Items = array;
//        return JsonUtility.ToJson(wrapper, prettyPrint);
//    }

//    [Serializable]
//    private class Wrapper<T>
//    {
//        public T[] Items;
//    }
}
