using UnityEngine;
using System.Xml;
using System;
using System.Collections.Generic;

public class XmlReader : MonoBehaviour
{
    static public XmlReader S;
    private XmlDocument _saveX;
    private string _path;

    [SerializeField] private bool _useYandexData = false;

    private void Awake()
    {
        S = this;
        //if (Application.platform == RuntimePlatform.Android)
        //    _path = Application.persistentDataPath;
        //else if (Application.platform == RuntimePlatform.WindowsEditor)
        //    _path = Application.dataPath;
        _path = Application.persistentDataPath;
    }
    public bool HasSave()
    {
        if (_useYandexData)
            return !((DataManager.S.CurrentDataHandler.CurrentPlayerData.ChipDatas == null &&
                DataManager.S.CurrentDataHandler.CurrentPlayerData.CurrentScore == 0 &&
                DataManager.S.CurrentDataHandler.CurrentPlayerData.MaxScore == 0)||
                DataManager.S.CurrentDataHandler.CurrentPlayerData.ChipDatas.Count == 0);


        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode levelNode = xml.SelectSingleNode("currentLevel");
        if (levelNode != null)
            return true;
        else
            return false;
    }
    public bool NoAddsMode()
    {
        return false;
        //_saveX = new XmlDocument();
        //_saveX.Load(_path + "/SaveXML.xml");
        //XmlNode xml = _saveX.SelectSingleNode("xml");
        //XmlNode settingsNode = xml.SelectSingleNode("settings");
        //if (settingsNode.Attributes["noAddsOn"].Value == "1")
        //    return true;
        //else
        //    return false;
    }
    public void LoadLevel(ref Column[] columnsContr)//загружаем уровень
    {
        Debug.Log("LoadLevel...");
        if (_useYandexData)
        {
            //загружаем все фишки на столе
            foreach (ChipData c in DataManager.S.CurrentDataHandler.CurrentPlayerData.ChipDatas)
            {
                GameManager.S.AddChip(c.Position, c.Value, c.Color);
            }

            //загружаем фишку в стартовой точке
            if (!DataManager.S.CurrentDataHandler.CurrentPlayerData.StartChipIsSkill)
                GameManager.S.SetStartChip(DataManager.S.CurrentDataHandler.CurrentPlayerData.StartChipData.Value, DataManager.S.CurrentDataHandler.CurrentPlayerData.StartChipData.Color);
            else
                GameManager.S.SetSkillChip(DataManager.S.CurrentDataHandler.CurrentPlayerData.StartChipData.Color);

            //загружаем столбы    
            for (int i = 0; i < 3; i++)
            {
                if (DataManager.S.CurrentDataHandler.CurrentPlayerData.Columns[i] == 1)
                    columnsContr[i].SetUp();
            }
        }
        else
        {
            _saveX = new XmlDocument();
            _saveX.Load(_path + "/SaveXML.xml");
            XmlNode xml = _saveX.SelectSingleNode("xml");
            XmlNode levelNode = xml.SelectSingleNode("currentLevel");
            XmlElement levelElem = (XmlElement)levelNode;
            XmlNodeList chipsOnTableNodeList = levelElem.SelectNodes("chip");

            System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ",";
            //загружаем все фишки на столе
            for (int i = 0; i < chipsOnTableNodeList.Count; i++)
            {
                Vector3 pos = new Vector3(float.Parse(chipsOnTableNodeList[i].Attributes["posX"].Value, System.Globalization.NumberStyles.Any, ci),
                  float.Parse(chipsOnTableNodeList[i].Attributes["posY"].Value, System.Globalization.NumberStyles.Any, ci),
                  float.Parse(chipsOnTableNodeList[i].Attributes["posZ"].Value, System.Globalization.NumberStyles.Any, ci));
                int value = int.Parse(chipsOnTableNodeList[i].Attributes["value"].Value);
                eChipColors color;
                string col = chipsOnTableNodeList[i].Attributes["color"].Value;
                if (col == "green")
                    color = eChipColors.green;
                else if (col == "red")
                    color = eChipColors.red;
                else if (col == "blue")
                    color = eChipColors.blue;
                else
                    color = eChipColors.purple;
                GameManager.S.AddChip(pos, value, color);
            }
            //загружаем фишку в стартовой точке
            XmlNode currentChipNode = levelElem.SelectSingleNode("currentChip");
            string curCol = currentChipNode.Attributes["color"].Value;
            eChipColors currentColor;
            if (curCol == "green")
                currentColor = eChipColors.green;
            else if (curCol == "red")
                currentColor = eChipColors.red;
            else if (curCol == "blue")
                currentColor = eChipColors.blue;
            else
                currentColor = eChipColors.purple;
            if (int.Parse(currentChipNode.Attributes["isSkill"].Value) == 0)
            {
                int currentValue = int.Parse(currentChipNode.Attributes["value"].Value);
                GameManager.S.SetStartChip(currentValue, currentColor);
            }
            else
            {
                GameManager.S.SetSkillChip(currentColor);
            }
            //загружаем столбы
            XmlNode columnsNode = levelElem.SelectSingleNode("columns");
            if (int.Parse(columnsNode.Attributes["first"].Value) == 1)
                columnsContr[0].SetUp();

            if (int.Parse(columnsNode.Attributes["second"].Value) == 1)
                columnsContr[1].SetUp();

            if (int.Parse(columnsNode.Attributes["third"].Value) == 1)
                columnsContr[2].SetUp();
        }
    }
    public int GetScore()//загружаем счет
    {
        if (_useYandexData)        
            return DataManager.S.CurrentDataHandler.CurrentPlayerData.CurrentScore;        

        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode levelNode = xml.SelectSingleNode("currentLevel");
        if (levelNode != null)
        {
            XmlElement levelElem = (XmlElement)levelNode;
            XmlNode currentScoreNode = levelElem.SelectSingleNode("currentScore");
            if (currentScoreNode != null)
                return int.Parse(currentScoreNode.Attributes["value"].Value);
            else
                return 0;
        }
        else
        {
            return 0;
        }
    }
    public void GetSkillsFilling(ref int[,] countAndFilling, ref eChipColors[] colors)//загружаем заполнения скиллов
    {
        if (_useYandexData)
        {
            countAndFilling[0, 0] = DataManager.S.CurrentDataHandler.CurrentPlayerData.GreenSkillCount;
            countAndFilling[1, 0] = (int)DataManager.S.CurrentDataHandler.CurrentPlayerData.GreenSkillFilling;
            colors[0] = eChipColors.green;

            countAndFilling[0, 1] = DataManager.S.CurrentDataHandler.CurrentPlayerData.RedSkillCount;
            countAndFilling[1, 1] = (int)DataManager.S.CurrentDataHandler.CurrentPlayerData.RedSkillFilling;
            colors[1] = eChipColors.red;

            countAndFilling[0, 2] = DataManager.S.CurrentDataHandler.CurrentPlayerData.BlueSkillCount;
            countAndFilling[1, 2] = (int)DataManager.S.CurrentDataHandler.CurrentPlayerData.BlueSkillFilling;
            colors[2] = eChipColors.blue;

            countAndFilling[0, 3] = DataManager.S.CurrentDataHandler.CurrentPlayerData.PurpleSkillCount;
            countAndFilling[1, 3] = (int)DataManager.S.CurrentDataHandler.CurrentPlayerData.PurpleSkillFilling;
            colors[3] = eChipColors.purple;
        }
        else
        {
            _saveX = new XmlDocument();
            _saveX.Load(_path + "/SaveXML.xml");
            XmlNode xml = _saveX.SelectSingleNode("xml");
            XmlNode levelNode = xml.SelectSingleNode("currentLevel");
            if (levelNode != null)
            {
                XmlElement levelElem = (XmlElement)levelNode;
                XmlNode greenSkillNode = levelElem.SelectSingleNode("greenSkill");
                if (greenSkillNode != null)
                {
                    countAndFilling[0, 0] = int.Parse(greenSkillNode.Attributes["count"].Value);
                    countAndFilling[1, 0] = int.Parse(greenSkillNode.Attributes["filling"].Value);
                    colors[0] = eChipColors.green;
                    XmlNode redSkillNode = levelElem.SelectSingleNode("redSkill");
                    countAndFilling[0, 1] = int.Parse(redSkillNode.Attributes["count"].Value);
                    countAndFilling[1, 1] = int.Parse(redSkillNode.Attributes["filling"].Value);
                    colors[1] = eChipColors.red;
                    XmlNode blueSkillNode = levelElem.SelectSingleNode("blueSkill");
                    countAndFilling[0, 2] = int.Parse(blueSkillNode.Attributes["count"].Value);
                    countAndFilling[1, 2] = int.Parse(blueSkillNode.Attributes["filling"].Value);
                    colors[2] = eChipColors.blue;
                    XmlNode purpleSkillNode = levelElem.SelectSingleNode("purpleSkill");
                    countAndFilling[0, 3] = int.Parse(purpleSkillNode.Attributes["count"].Value);
                    countAndFilling[1, 3] = int.Parse(purpleSkillNode.Attributes["filling"].Value);
                    colors[3] = eChipColors.purple;
                }
            }
        }
    }
    public int GetMaxScore()
    {
        if (_useYandexData)        
            return DataManager.S.CurrentDataHandler.CurrentPlayerData.MaxScore;        

        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode scoreNode = xml.SelectSingleNode("maxScore");
        return int.Parse(scoreNode.Attributes["value"].Value);
    }
    public int GetCurrentScore()
    {
        if (_useYandexData)
            return DataManager.S.CurrentDataHandler.CurrentPlayerData.CurrentScore;

        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode level = xml.SelectSingleNode("currentLevel");
        XmlNode currentScore = level.SelectSingleNode("currentScore");
        return int.Parse(currentScore.Attributes["value"].Value);
    }
    public bool GetSoundsOn()
    {
        if (_useYandexData)
            return DataManager.S.CurrentDataHandler.CurrentPlayerData.VolumeOn;

        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode node = xml.SelectSingleNode("settings");
        if (int.Parse(node.Attributes["soundsOn"].Value) == 1)
            return true;
        else
            return false;
    }
    public bool GetMusicOn()
    {
        if (_useYandexData)
            return DataManager.S.CurrentDataHandler.CurrentPlayerData.MusicOn;

        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode node = xml.SelectSingleNode("settings");
        if (int.Parse(node.Attributes["musicOn"].Value) == 1)
            return true;
        else
            return false;
    }
}


[Serializable]
public class PlayerData
{
    public int CurrentScore = 0;
    public int MaxScore = 0;
    public bool VolumeOn = true;
    public bool MusicOn = true;

    public int GreenSkillCount = 0;
    public float GreenSkillFilling = 0;
    public int RedSkillCount = 0;
    public float RedSkillFilling = 0;
    public int PurpleSkillCount = 0;
    public float PurpleSkillFilling = 0;
    public int BlueSkillCount = 0;
    public float BlueSkillFilling = 0;

    public int[] Columns = new int[3];
    public List<ChipData> ChipDatas = null;
    public ChipData StartChipData;
    public bool StartChipIsSkill = false;
}

[Serializable]
public class ChipData
{
    public Vector3 Position;
    public int Value;
    public eChipColors Color;
}
