using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public class XMLSaver : MonoBehaviour
{
    public static XMLSaver S;
    private XmlDocument _saveX;
    private string _path;

    void Awake()
    {
        S = this;
        if (Application.platform == RuntimePlatform.Android)
            _path = Application.persistentDataPath;
        else if (Application.platform == RuntimePlatform.WindowsEditor)
            _path = Application.dataPath;
        if (!File.Exists(_path + "/SaveXML.xml"))
        {
            _saveX = new XmlDocument();

            XmlElement xmlElem = _saveX.CreateElement("xml");
            XmlElement maxScoreElem = _saveX.CreateElement("maxScore");
            XmlAttribute valueAtt = _saveX.CreateAttribute("value");
            XmlText valueText = _saveX.CreateTextNode("0");
            valueAtt.AppendChild(valueText);
            maxScoreElem.Attributes.Append(valueAtt);

            XmlElement settingsElem = _saveX.CreateElement("settings");
            XmlAttribute musicOnAtt = _saveX.CreateAttribute("musicOn");
            XmlText musicOnText = _saveX.CreateTextNode("1");
            musicOnAtt.AppendChild(musicOnText);
            settingsElem.Attributes.Append(musicOnAtt);

            XmlAttribute soundsOnAtt = _saveX.CreateAttribute("soundsOn");
            XmlText soundsOnText = _saveX.CreateTextNode("1");
            soundsOnAtt.AppendChild(soundsOnText);
            settingsElem.Attributes.Append(soundsOnAtt);

            XmlAttribute noAddsOnAtt = _saveX.CreateAttribute("noAddsOn");
            XmlText noAddsOnText = _saveX.CreateTextNode("0");
            noAddsOnAtt.AppendChild(noAddsOnText);
            settingsElem.Attributes.Append(noAddsOnAtt);

            _saveX.AppendChild(xmlElem);
            xmlElem.AppendChild(maxScoreElem);
            xmlElem.AppendChild(settingsElem);

            _saveX.Save(_path + "/SaveXML.xml");
            Debug.Log("SaveXML is created!");
        }
    }
    public void SaveTable(List<GameObject> chipsOnTable, GameObject currentChip, bool[] columnsUp)
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        //удаляем старое сохранение уровня
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode level = xml.SelectSingleNode("currentLevel");
        if (level != null)
        {
            XmlNodeList chipElem = level.SelectNodes("chip");
            if (chipElem[0] != null)
                foreach (XmlNode node in chipElem)
                    level.RemoveChild(node);
            XmlNode currentСhip = level.SelectSingleNode("currentChip");
            if (currentСhip != null)
                level.RemoveChild(currentСhip);
            XmlNode columns = level.SelectSingleNode("columns");
            if (columns != null)
                level.RemoveChild(columns);
        }
        else
        {
            XmlElement levelElem = _saveX.CreateElement("currentLevel");
            xml.AppendChild(levelElem);
            level = levelElem;
        }
        //создаем новое сохранение
        for (int i = 0; i < chipsOnTable.Count; i++)
        {
            XmlElement chipElem = _saveX.CreateElement("chip");
            //позиция фишки
            XmlAttribute posXAtt = _saveX.CreateAttribute("posX");
            XmlText posXText = _saveX.CreateTextNode(chipsOnTable[i].transform.position.x.ToString());
            posXAtt.AppendChild(posXText);
            chipElem.Attributes.Append(posXAtt);
            XmlAttribute posYAtt = _saveX.CreateAttribute("posY");
            XmlText posYText = _saveX.CreateTextNode(chipsOnTable[i].transform.position.y.ToString());
            posYAtt.AppendChild(posYText);
            chipElem.Attributes.Append(posYAtt);
            XmlAttribute posZAtt = _saveX.CreateAttribute("posZ");
            XmlText posZText = _saveX.CreateTextNode(chipsOnTable[i].transform.position.z.ToString());
            posZAtt.AppendChild(posZText);
            chipElem.Attributes.Append(posZAtt);
            //значение и цвет фишки
            Chip chip = chipsOnTable[i].GetComponent<Chip>();
            XmlAttribute valueAtt = _saveX.CreateAttribute("value");
            XmlText valueText = _saveX.CreateTextNode(chip.СhipValue.ToString());
            valueAtt.AppendChild(valueText);
            chipElem.Attributes.Append(valueAtt);
            XmlAttribute colorAtt = _saveX.CreateAttribute("color");
            XmlText colorText;
            if (chip.СhipColor == eChipColors.green)
                colorText = _saveX.CreateTextNode("green");
            else if (chip.СhipColor == eChipColors.red)
                colorText = _saveX.CreateTextNode("red");
            else if (chip.СhipColor == eChipColors.blue)
                colorText = _saveX.CreateTextNode("blue");
            else
                colorText = _saveX.CreateTextNode("purple");

            colorAtt.AppendChild(colorText);
            chipElem.Attributes.Append(colorAtt);
            level.AppendChild(chipElem);
        }
        //создаем текущую активную фишку
        XmlElement currentСhipElem = _saveX.CreateElement("currentChip");

        XmlAttribute isSkillAtt = _saveX.CreateAttribute("isSkill");
        XmlText isSkillText;
        if (currentChip.GetComponent<SkillChip>())
        {
            isSkillText = _saveX.CreateTextNode(1.ToString());
            SkillChip sc = currentChip.GetComponent<SkillChip>();
            XmlAttribute colorCurAtt = _saveX.CreateAttribute("color");
            XmlText colorCurText;
            if (sc.Color == eChipColors.green)
                colorCurText = _saveX.CreateTextNode("green");
            else if (sc.Color == eChipColors.red)
                colorCurText = _saveX.CreateTextNode("red");
            else if (sc.Color == eChipColors.blue)
                colorCurText = _saveX.CreateTextNode("blue");
            else
                colorCurText = _saveX.CreateTextNode("purple");
            colorCurAtt.AppendChild(colorCurText);
            currentСhipElem.Attributes.Append(colorCurAtt);
        }
        else //тогда обычная фишка
        {
            isSkillText = _saveX.CreateTextNode(0.ToString());
            Chip curChip = currentChip.GetComponent<Chip>();
            XmlAttribute valueCurAtt = _saveX.CreateAttribute("value");
            XmlText valueCurText = _saveX.CreateTextNode(curChip.СhipValue.ToString());
            valueCurAtt.AppendChild(valueCurText);
            currentСhipElem.Attributes.Append(valueCurAtt);
            XmlAttribute colorCurAtt = _saveX.CreateAttribute("color");
            XmlText colorCurText;
            if (curChip.СhipColor == eChipColors.green)
                colorCurText = _saveX.CreateTextNode("green");
            else if (curChip.СhipColor == eChipColors.red)
                colorCurText = _saveX.CreateTextNode("red");
            else if (curChip.СhipColor == eChipColors.blue)
                colorCurText = _saveX.CreateTextNode("blue");
            else
                colorCurText = _saveX.CreateTextNode("purple");
            colorCurAtt.AppendChild(colorCurText);
            currentСhipElem.Attributes.Append(colorCurAtt);
        }
        isSkillAtt.AppendChild(isSkillText);
        currentСhipElem.Attributes.Append(isSkillAtt);
        level.AppendChild(currentСhipElem);

        //создаем список столбов (1-активный, 0-неактивный)
        XmlElement columnsElem = _saveX.CreateElement("columns");
        XmlAttribute firstAtt = _saveX.CreateAttribute("first");
        XmlAttribute secondAtt = _saveX.CreateAttribute("second");
        XmlAttribute thirdAtt = _saveX.CreateAttribute("third");
        XmlText firstUpText;
        if (columnsUp[0])
            firstUpText = _saveX.CreateTextNode(1.ToString());
        else
            firstUpText = _saveX.CreateTextNode(0.ToString());
        XmlText secondUpText;
        if (columnsUp[1])
            secondUpText = _saveX.CreateTextNode(1.ToString());
        else
            secondUpText = _saveX.CreateTextNode(0.ToString());
        XmlText thirdUpText;
        if (columnsUp[2])
            thirdUpText = _saveX.CreateTextNode(1.ToString());
        else
            thirdUpText = _saveX.CreateTextNode(0.ToString());

        firstAtt.AppendChild(firstUpText);
        columnsElem.Attributes.Append(firstAtt);
        secondAtt.AppendChild(secondUpText);
        columnsElem.Attributes.Append(secondAtt);
        thirdAtt.AppendChild(thirdUpText);
        columnsElem.Attributes.Append(thirdAtt);
        level.AppendChild(columnsElem);

        //сохраняем все изменения
        _saveX.Save(_path + "/SaveXML.xml");
    }

    public void SaveScore(int score)
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        //удаляем старое сохранение уровня
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode level = xml.SelectSingleNode("currentLevel");
        if (level != null)
        {
            XmlNode currentScore = level.SelectSingleNode("currentScore");
            if (currentScore != null)
                level.RemoveChild(currentScore);
        }
        else
        {
            XmlElement levelElem = _saveX.CreateElement("currentLevel");
            xml.AppendChild(levelElem);
            level = levelElem;
        }

        //создаем текущий счет 
        XmlElement currentScoreElem = _saveX.CreateElement("currentScore");
        XmlAttribute currentScoreAtt = _saveX.CreateAttribute("value");
        XmlText currentScoreValueText = _saveX.CreateTextNode(score.ToString());
        currentScoreAtt.AppendChild(currentScoreValueText);
        currentScoreElem.Attributes.Append(currentScoreAtt);
        level.AppendChild(currentScoreElem);

        //сохраняем все изменения
        _saveX.Save(_path + "/SaveXML.xml");
    }

    public void SaveSkillsFilling(float cristalFilling, float fireFilling, float frostFilling, float LightningFilling,
        int cristalCount, int fireCount, int frostCount, int lightningCount)
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        //удаляем старое сохранение уровня
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode level = xml.SelectSingleNode("currentLevel");
        if (level != null)
        {
            XmlNode greenSkill = level.SelectSingleNode("greenSkill");
            if (greenSkill != null)
                level.RemoveChild(greenSkill);
            XmlNode redSkill = level.SelectSingleNode("redSkill");
            if (redSkill != null)
                level.RemoveChild(redSkill);
            XmlNode blueSkill = level.SelectSingleNode("blueSkill");
            if (blueSkill != null)
                level.RemoveChild(blueSkill);
            XmlNode purpleSkill = level.SelectSingleNode("purpleSkill");
            if (purpleSkill != null)
                level.RemoveChild(purpleSkill);
        }
        else
        {
            XmlElement levelElem = _saveX.CreateElement("currentLevel");
            xml.AppendChild(levelElem);
            level = levelElem;
        }

        //создаем текущее количество и заполнение навыков
        XmlElement greenSkillElem = _saveX.CreateElement("greenSkill");
        XmlAttribute greenSkillCountAtt = _saveX.CreateAttribute("count");
        XmlAttribute greenSkillFillingAtt = _saveX.CreateAttribute("filling");
        XmlText greenSkillCountText = _saveX.CreateTextNode(cristalCount.ToString());
        XmlText greenSkillFillingText = _saveX.CreateTextNode(cristalFilling.ToString());
        greenSkillCountAtt.AppendChild(greenSkillCountText);
        greenSkillElem.Attributes.Append(greenSkillCountAtt);
        greenSkillFillingAtt.AppendChild(greenSkillFillingText);
        greenSkillElem.Attributes.Append(greenSkillFillingAtt);
        level.AppendChild(greenSkillElem);

        XmlElement redSkillElem = _saveX.CreateElement("redSkill");
        XmlAttribute redSkillCountAtt = _saveX.CreateAttribute("count");
        XmlAttribute redSkillFillingAtt = _saveX.CreateAttribute("filling");
        XmlText redSkillCountText = _saveX.CreateTextNode(fireCount.ToString());
        XmlText redSkillFillingText = _saveX.CreateTextNode(fireFilling.ToString());
        redSkillCountAtt.AppendChild(redSkillCountText);
        redSkillElem.Attributes.Append(redSkillCountAtt);
        redSkillFillingAtt.AppendChild(redSkillFillingText);
        redSkillElem.Attributes.Append(redSkillFillingAtt);
        level.AppendChild(redSkillElem);

        XmlElement blueSkillElem = _saveX.CreateElement("blueSkill");
        XmlAttribute blueSkillCountAtt = _saveX.CreateAttribute("count");
        XmlAttribute blueSkillFillingAtt = _saveX.CreateAttribute("filling");
        XmlText blueSkillCountText = _saveX.CreateTextNode(frostCount.ToString());
        XmlText blueSkillFillingText = _saveX.CreateTextNode(frostFilling.ToString());
        blueSkillCountAtt.AppendChild(blueSkillCountText);
        blueSkillElem.Attributes.Append(blueSkillCountAtt);
        blueSkillFillingAtt.AppendChild(blueSkillFillingText);
        blueSkillElem.Attributes.Append(blueSkillFillingAtt);
        level.AppendChild(blueSkillElem);

        XmlElement purpleSkillElem = _saveX.CreateElement("purpleSkill");
        XmlAttribute purpleSkillCountAtt = _saveX.CreateAttribute("count");
        XmlAttribute purpleSkillFillingAtt = _saveX.CreateAttribute("filling");
        XmlText purpleSkillCountText = _saveX.CreateTextNode(lightningCount.ToString());
        XmlText purpleSkillFillingText = _saveX.CreateTextNode(LightningFilling.ToString());
        purpleSkillCountAtt.AppendChild(purpleSkillCountText);
        purpleSkillElem.Attributes.Append(purpleSkillCountAtt);
        purpleSkillFillingAtt.AppendChild(purpleSkillFillingText);
        purpleSkillElem.Attributes.Append(purpleSkillFillingAtt);
        level.AppendChild(purpleSkillElem);
        //сохраняем все изменения
        _saveX.Save(_path + "/SaveXML.xml");
    }


    public void UnnullTable()
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        //удаляем старое сохранение уровня
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode levelNode = xml.SelectSingleNode("currentLevel");
        if (levelNode != null)
            _saveX.DocumentElement.RemoveChild(levelNode);
        _saveX.Save(_path + "/SaveXML.xml");
    }
    public void SetSounds(int isOn)
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode node = xml.SelectSingleNode("settings");
        node.Attributes["soundsOn"].Value = isOn.ToString();
        _saveX.Save(_path + "/SaveXML.xml");
    }
    public void SetMusic(int isOn)
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode node = xml.SelectSingleNode("settings");
        node.Attributes["musicOn"].Value = isOn.ToString();
        _saveX.Save(_path + "/SaveXML.xml");
    }
    public void SetMaxScore(int score)
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode node = xml.SelectSingleNode("maxScore");
        node.Attributes["value"].Value = score.ToString();
        _saveX.Save(_path + "/SaveXML.xml");
    }
    public void SwitchOffAds()
    {
        _saveX = new XmlDocument();
        _saveX.Load(_path + "/SaveXML.xml");
        XmlNode xml = _saveX.SelectSingleNode("xml");
        XmlNode node = xml.SelectSingleNode("settings");
        node.Attributes["noAddsOn"].Value = 1.ToString();
        _saveX.Save(_path + "/SaveXML.xml");
    }
}
