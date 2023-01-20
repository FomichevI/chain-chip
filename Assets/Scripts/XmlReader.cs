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
        XmlNodeList levelNodeList = xml.SelectNodes("currentLevel");
        if (levelNodeList[0] != null)
            return true;
        else
            return false;
    }
    public bool NoAddsMode()
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNodeList levelNodeList = xml.SelectNodes("settings");
        if (levelNodeList[0].Attributes["noAddsOn"].Value == "1")
            return true;
        else
            return false;
    }
    public void LoadLevel(ref Column[] columnsContr)
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNodeList levelNodeList = xml.SelectNodes("currentLevel");
        XmlElement levelElem = (XmlElement)levelNodeList[0];
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
        {
            columnsContr[0].SetUp();
        }
        if (int.Parse(columnsNode.Attributes["second"].Value) == 1)
        {
            columnsContr[1].SetUp();
        }
        if (int.Parse(columnsNode.Attributes["third"].Value) == 1)
        {
            columnsContr[2].SetUp();
        }
        //загружаем счет
        XmlNode currentScoreNode = levelElem.SelectSingleNode("currentScore");
        ScoreController.S.SetScoreAndStage(int.Parse(currentScoreNode.Attributes["value"].Value));
        //загружаем заполнения скиллов
        XmlNode greenSkillNode = levelElem.SelectSingleNode("greenSkill");
        SkillsController.S.SetSkillFilling(int.Parse(greenSkillNode.Attributes["count"].Value), int.Parse(greenSkillNode.Attributes["filling"].Value), eChipColors.green);
        XmlNode redSkillNode = levelElem.SelectSingleNode("redSkill");
        SkillsController.S.SetSkillFilling(int.Parse(redSkillNode.Attributes["count"].Value), int.Parse(redSkillNode.Attributes["filling"].Value), eChipColors.red);
        XmlNode blueSkillNode = levelElem.SelectSingleNode("blueSkill");
        SkillsController.S.SetSkillFilling(int.Parse(blueSkillNode.Attributes["count"].Value), int.Parse(blueSkillNode.Attributes["filling"].Value), eChipColors.blue);
        XmlNode purpleSkillNode = levelElem.SelectSingleNode("purpleSkill");
        SkillsController.S.SetSkillFilling(int.Parse(purpleSkillNode.Attributes["count"].Value), int.Parse(purpleSkillNode.Attributes["filling"].Value), eChipColors.purple);
    }
    public int GetMaxScore()
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode scoreNode = xml.SelectSingleNode("maxScore");
        return int.Parse(scoreNode.Attributes["value"].Value);
    }
    public bool GetSoundsOn()
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNodeList nodeList = xml.SelectNodes("settings");
        if (int.Parse(nodeList[0].Attributes["soundsOn"].Value) == 1)
            return true;
        else
            return false;
    }
    public bool GetMusicOn()
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNodeList nodeList = xml.SelectNodes("settings");
        if (int.Parse(nodeList[0].Attributes["musicOn"].Value) == 1)
            return true;
        else
            return false;
    }
}
