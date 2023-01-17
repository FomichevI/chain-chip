using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public class XMLSaver : MonoBehaviour
{
    public static XMLSaver S;
    private XmlDocument saveX;
    private string path;

    void Awake()
    {
        S = this;
        if (Application.platform == RuntimePlatform.Android)
            path = Application.persistentDataPath;
        else if (Application.platform == RuntimePlatform.WindowsEditor)
            path = Application.dataPath;
        if (!File.Exists(path + "/SaveXML.xml"))
        {
            saveX = new XmlDocument();

            XmlElement xmlElem = saveX.CreateElement("xml");
            XmlElement maxScoreElem = saveX.CreateElement("maxScore");
            XmlAttribute valueAtt = saveX.CreateAttribute("value");
            XmlText valueText = saveX.CreateTextNode("0");
            valueAtt.AppendChild(valueText);
            maxScoreElem.Attributes.Append(valueAtt);

            XmlElement settingsElem = saveX.CreateElement("settings");
            XmlAttribute musicOnAtt = saveX.CreateAttribute("musicOn");
            XmlText musicOnText = saveX.CreateTextNode("1");
            musicOnAtt.AppendChild(musicOnText);
            settingsElem.Attributes.Append(musicOnAtt);

            XmlAttribute soundsOnAtt = saveX.CreateAttribute("soundsOn");
            XmlText soundsOnText = saveX.CreateTextNode("1");
            soundsOnAtt.AppendChild(soundsOnText);
            settingsElem.Attributes.Append(soundsOnAtt);

            XmlAttribute noAddsOnAtt = saveX.CreateAttribute("noAddsOn");
            XmlText noAddsOnText = saveX.CreateTextNode("0");
            noAddsOnAtt.AppendChild(noAddsOnText);
            settingsElem.Attributes.Append(noAddsOnAtt);

            saveX.AppendChild(xmlElem);
            xmlElem.AppendChild(maxScoreElem);
            xmlElem.AppendChild(settingsElem);

            saveX.Save(path + "/SaveXML.xml");
            Debug.Log("SaveXML is created!");
        }
        //else
        //{
        //    File.Delete(Application.persistentDataPath + "/SaveXML.xml");
        //    Debug.Log("del");
        //}
    }
    public void SaveTable(List<GameObject> chipsOnTable, GameObject currentChip, bool[] columnsUp)
    {
        saveX = new XmlDocument();
        saveX.Load(path + "/SaveXML.xml");
        //удаляем старое сохранение уровня
        XmlNode xml = saveX.SelectSingleNode("xml");
        XmlNodeList levelNodeList = xml.SelectNodes("currentLevel");
        if(levelNodeList[0] != null)
            saveX.DocumentElement.RemoveChild(levelNodeList[0]);

        //создаем новое сохранение
        XmlElement levelElem = saveX.CreateElement("currentLevel");
        for (int i = 0; i < chipsOnTable.Count; i++)
        {
            XmlElement chipElem = saveX.CreateElement("chip");
            //позиция фишки
            XmlAttribute posXAtt = saveX.CreateAttribute("posX");
            XmlText posXText = saveX.CreateTextNode(chipsOnTable[i].transform.position.x.ToString());
            posXAtt.AppendChild(posXText);
            chipElem.Attributes.Append(posXAtt);
            XmlAttribute posYAtt = saveX.CreateAttribute("posY");
            XmlText posYText = saveX.CreateTextNode(chipsOnTable[i].transform.position.y.ToString());
            posYAtt.AppendChild(posYText);
            chipElem.Attributes.Append(posYAtt);
            XmlAttribute posZAtt = saveX.CreateAttribute("posZ");
            XmlText posZText = saveX.CreateTextNode(chipsOnTable[i].transform.position.z.ToString());
            posZAtt.AppendChild(posZText);
            chipElem.Attributes.Append(posZAtt);
            //значение и цвет фишки
            ChipController chip = chipsOnTable[i].GetComponent<ChipController>();
            XmlAttribute valueAtt = saveX.CreateAttribute("value");
            XmlText valueText = saveX.CreateTextNode(chip.chipValue.ToString());
            valueAtt.AppendChild(valueText);
            chipElem.Attributes.Append(valueAtt);
            XmlAttribute colorAtt = saveX.CreateAttribute("color");
            XmlText colorText;
            if (chip.chipColor == eChipColors.green)
                colorText = saveX.CreateTextNode("green");
            else if (chip.chipColor == eChipColors.red)
                colorText = saveX.CreateTextNode("red");
            else if (chip.chipColor == eChipColors.blue)
                colorText = saveX.CreateTextNode("blue");
            else
                colorText = saveX.CreateTextNode("purple");

            colorAtt.AppendChild(colorText);
            chipElem.Attributes.Append(colorAtt);
            levelElem.AppendChild(chipElem);
        }
        //создаем текущую активную фишку
        XmlElement currentСhipElem = saveX.CreateElement("currentChip");

        XmlAttribute isSkillAtt = saveX.CreateAttribute("isSkill");
        XmlText isSkillText;
        if(currentChip.GetComponent<SkillChip>())
        {
            isSkillText = saveX.CreateTextNode(1.ToString());
            SkillChip sc = currentChip.GetComponent<SkillChip>();
            XmlAttribute colorCurAtt = saveX.CreateAttribute("color");
            XmlText colorCurText;
            if (sc.color == eChipColors.green)
                colorCurText = saveX.CreateTextNode("green");
            else if (sc.color == eChipColors.red)
                colorCurText = saveX.CreateTextNode("red");
            else if (sc.color == eChipColors.blue)
                colorCurText = saveX.CreateTextNode("blue");
            else
                colorCurText = saveX.CreateTextNode("purple");
            colorCurAtt.AppendChild(colorCurText);
            currentСhipElem.Attributes.Append(colorCurAtt);
        }
        else //тогда обычная фишка
        {
            isSkillText = saveX.CreateTextNode(0.ToString());
            ChipController curChip = currentChip.GetComponent<ChipController>();
            XmlAttribute valueCurAtt = saveX.CreateAttribute("value");
            XmlText valueCurText = saveX.CreateTextNode(curChip.chipValue.ToString());
            valueCurAtt.AppendChild(valueCurText);
            currentСhipElem.Attributes.Append(valueCurAtt);
            XmlAttribute colorCurAtt = saveX.CreateAttribute("color");
            XmlText colorCurText;
            if (curChip.chipColor == eChipColors.green)
                colorCurText = saveX.CreateTextNode("green");
            else if (curChip.chipColor == eChipColors.red)
                colorCurText = saveX.CreateTextNode("red");
            else if (curChip.chipColor == eChipColors.blue)
                colorCurText = saveX.CreateTextNode("blue");
            else
                colorCurText = saveX.CreateTextNode("purple");
            colorCurAtt.AppendChild(colorCurText);
            currentСhipElem.Attributes.Append(colorCurAtt);
        }
        isSkillAtt.AppendChild(isSkillText);
        currentСhipElem.Attributes.Append(isSkillAtt);
        levelElem.AppendChild(currentСhipElem);

        //создаем список столбов (1-активный, 0-неактивный)
        XmlElement columnsElem = saveX.CreateElement("columns");
        XmlAttribute firstAtt = saveX.CreateAttribute("first");
        XmlAttribute secondAtt = saveX.CreateAttribute("second");
        XmlAttribute thirdAtt = saveX.CreateAttribute("third");
        XmlText firstUpText;
        if (columnsUp[0])
            firstUpText = saveX.CreateTextNode(1.ToString());
        else
            firstUpText = saveX.CreateTextNode(0.ToString());
        XmlText secondUpText;
        if (columnsUp[1])
            secondUpText = saveX.CreateTextNode(1.ToString());
        else
            secondUpText = saveX.CreateTextNode(0.ToString());
        XmlText thirdUpText;
        if (columnsUp[2])
            thirdUpText = saveX.CreateTextNode(1.ToString());
        else
            thirdUpText = saveX.CreateTextNode(0.ToString());

        firstAtt.AppendChild(firstUpText);
        columnsElem.Attributes.Append(firstAtt);
        secondAtt.AppendChild(secondUpText);
        columnsElem.Attributes.Append(secondAtt);
        thirdAtt.AppendChild(thirdUpText);
        columnsElem.Attributes.Append(thirdAtt);
        levelElem.AppendChild(columnsElem);

        //создаем текущий счет 
        XmlElement currentScoreElem = saveX.CreateElement("currentScore");
        XmlAttribute currentScoreAtt = saveX.CreateAttribute("value");
        XmlText currentScoreValueText = saveX.CreateTextNode(ScoreController.S.GetScore().ToString());
        currentScoreAtt.AppendChild(currentScoreValueText);
        currentScoreElem.Attributes.Append(currentScoreAtt);
        levelElem.AppendChild(currentScoreElem);

        //создаем текущее количество и заполнение навыков
        XmlElement greenSkillElem = saveX.CreateElement("greenSkill");
        XmlAttribute greenSkillCountAtt = saveX.CreateAttribute("count");
        XmlAttribute greenSkillFillingAtt = saveX.CreateAttribute("filling");
        XmlText greenSkillCountText = saveX.CreateTextNode(SkillsController.S.GetCount(eChipColors.green).ToString());
        XmlText greenSkillFillingText = saveX.CreateTextNode(SkillsController.S.GetFilling(eChipColors.green).ToString());
        greenSkillCountAtt.AppendChild(greenSkillCountText);
        greenSkillElem.Attributes.Append(greenSkillCountAtt);
        greenSkillFillingAtt.AppendChild(greenSkillFillingText);
        greenSkillElem.Attributes.Append(greenSkillFillingAtt);
        levelElem.AppendChild(greenSkillElem);

        XmlElement redSkillElem = saveX.CreateElement("redSkill");
        XmlAttribute redSkillCountAtt = saveX.CreateAttribute("count");
        XmlAttribute redSkillFillingAtt = saveX.CreateAttribute("filling");
        XmlText redSkillCountText = saveX.CreateTextNode(SkillsController.S.GetCount(eChipColors.red).ToString());
        XmlText redSkillFillingText = saveX.CreateTextNode(SkillsController.S.GetFilling(eChipColors.red).ToString());
        redSkillCountAtt.AppendChild(redSkillCountText);
        redSkillElem.Attributes.Append(redSkillCountAtt);
        redSkillFillingAtt.AppendChild(redSkillFillingText);
        redSkillElem.Attributes.Append(redSkillFillingAtt);
        levelElem.AppendChild(redSkillElem);

        XmlElement blueSkillElem = saveX.CreateElement("blueSkill");
        XmlAttribute blueSkillCountAtt = saveX.CreateAttribute("count");
        XmlAttribute blueSkillFillingAtt = saveX.CreateAttribute("filling");
        XmlText blueSkillCountText = saveX.CreateTextNode(SkillsController.S.GetCount(eChipColors.blue).ToString());
        XmlText blueSkillFillingText = saveX.CreateTextNode(SkillsController.S.GetFilling(eChipColors.blue).ToString());
        blueSkillCountAtt.AppendChild(blueSkillCountText);
        blueSkillElem.Attributes.Append(blueSkillCountAtt);
        blueSkillFillingAtt.AppendChild(blueSkillFillingText);
        blueSkillElem.Attributes.Append(blueSkillFillingAtt);
        levelElem.AppendChild(blueSkillElem);

        XmlElement purpleSkillElem = saveX.CreateElement("purpleSkill");
        XmlAttribute purpleSkillCountAtt = saveX.CreateAttribute("count");
        XmlAttribute purpleSkillFillingAtt = saveX.CreateAttribute("filling");
        XmlText purpleSkillCountText = saveX.CreateTextNode(SkillsController.S.GetCount(eChipColors.purple).ToString());
        XmlText purpleSkillFillingText = saveX.CreateTextNode(SkillsController.S.GetFilling(eChipColors.purple).ToString());
        purpleSkillCountAtt.AppendChild(purpleSkillCountText);
        purpleSkillElem.Attributes.Append(purpleSkillCountAtt);
        purpleSkillFillingAtt.AppendChild(purpleSkillFillingText);
        purpleSkillElem.Attributes.Append(purpleSkillFillingAtt);
        levelElem.AppendChild(purpleSkillElem);

        xml.AppendChild(levelElem);
        //сохраняем все изменения
        saveX.Save(path + "/SaveXML.xml");
        //Debug.Log("Level is saved!");
    }
    public void UnnullTable()
    {
        saveX = new XmlDocument();
        saveX.Load(path + "/SaveXML.xml");
        //удаляем старое сохранение уровня
        XmlNode xml = saveX.SelectSingleNode("xml");
        XmlNodeList levelNodeList = xml.SelectNodes("currentLevel");      
        if(levelNodeList[0] != null)
            saveX.DocumentElement.RemoveChild(levelNodeList[0]);
        saveX.Save(path + "/SaveXML.xml");
    }
    public void SetSounds(int isOn)
    {
        saveX = new XmlDocument();
        saveX.Load(path + "/SaveXML.xml");
        XmlNode xml = saveX.SelectSingleNode("xml");
        XmlNodeList nodeList = xml.SelectNodes("settings");
        nodeList[0].Attributes["soundsOn"].Value = isOn.ToString();
        saveX.Save(path + "/SaveXML.xml");
    }
    public void SetMusic(int isOn)
    {
        saveX = new XmlDocument();
        saveX.Load(path + "/SaveXML.xml");
        XmlNode xml = saveX.SelectSingleNode("xml");
        XmlNodeList nodeList = xml.SelectNodes("settings");
        nodeList[0].Attributes["musicOn"].Value = isOn.ToString();
        saveX.Save(path + "/SaveXML.xml");
    }
    public void SetMaxScore(int score)
    {
        saveX = new XmlDocument();
        saveX.Load(path + "/SaveXML.xml");
        XmlNode xml = saveX.SelectSingleNode("xml");
        XmlNodeList nodeList = xml.SelectNodes("maxScore");
        nodeList[0].Attributes["value"].Value = score.ToString();
        saveX.Save(path + "/SaveXML.xml");
    }
    public void SwitchOffAds()
    {
        saveX = new XmlDocument();
        saveX.Load(path + "/SaveXML.xml");
        XmlNode xml = saveX.SelectSingleNode("xml");
        XmlNodeList nodeList = xml.SelectNodes("settings");
        nodeList[0].Attributes["noAddsOn"].Value = 1.ToString();
        saveX.Save(path + "/SaveXML.xml");
    }
    
    //public void SetLanguade(string lang)
    //{
    //    saveX = new XmlDocument();
    //    saveX.Load(Application.persistentDataPath + "/SaveXML.xml");
    //    XmlNodeList nodeList = saveX.SelectNodes("save");
    //    nodeList[0].Attributes["language"].Value = lang;
    //    saveX.Save(Application.persistentDataPath + "/SaveXML.xml");
    //}
    //public string GetLanguage()
    //{
    //    saveX = new XmlDocument();
    //    saveX.Load(Application.persistentDataPath + "/SaveXML.xml");
    //    XmlNodeList nodeList = saveX.SelectNodes("save");
    //    return nodeList[0].Attributes["language"].Value;

    //}
}
