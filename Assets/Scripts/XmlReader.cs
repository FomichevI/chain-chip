using UnityEngine;
using System.Xml;

public class XmlReader : MonoBehaviour
{
    static public XmlReader S;
    private XmlDocument _saveX;
    private string _path;

    private void Awake()
    {
        S = this;
        if (Application.platform == RuntimePlatform.Android)
            _path = Application.persistentDataPath;
        else if (Application.platform == RuntimePlatform.WindowsEditor)
            _path = Application.dataPath;
    }

    public bool HasSave()
    {
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
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode settingsNode = xml.SelectSingleNode("settings");
        if (settingsNode.Attributes["noAddsOn"].Value == "1")
            return true;
        else
            return false;
    }
    public void LoadLevel(ref Column[] columnsContr)//загружаем уровень
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
    public int GetScore()//загружаем счет
    {
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
    public int GetMaxScore()
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode scoreNode = xml.SelectSingleNode("maxScore");
        return int.Parse(scoreNode.Attributes["value"].Value);
    }
    public int GetCurrentScore()
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode level = xml.SelectSingleNode("currentLevel");
        XmlNode currentScore = level.SelectSingleNode("currentScore");
        return int.Parse(currentScore.Attributes["value"].Value);
    }
    public bool GetSoundsOn()
    {
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
