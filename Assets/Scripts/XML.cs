using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using UnityEngine;

public class XML : MonoBehaviour
{
    public List<Dictionary<string, string>> parseFile(string fileName, string keyOne, string keyTwo, string keyThree)
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>(fileName);
        var doc = XDocument.Parse(txtXmlAsset.text);

        var allDict = doc.Element(keyOne).Elements(keyTwo).Elements(keyThree);
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();

        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try { dict.Add("id", oneDict.Attribute("id").Value); } catch { }
            try { dict.Add("idReference", oneDict.Parent.Attribute("id").Value); } catch { }
            try { dict.Add("name", oneDict.Element("name").Value); } catch { }
            try { dict.Add("description", oneDict.Element("description").Value); } catch { }
            try { dict.Add("multiplier", oneDict.Element("multiplier").Value); } catch { }
            try { dict.Add("baseMultiplier", oneDict.Element("baseMultiplier").Value); } catch { }
            try { dict.Add("basePrice", oneDict.Element("basePrice").Value); } catch { }
            try { dict.Add("unlockCondition", oneDict.Element("unlockCondition").Value); } catch { }

            allTextDic.Add(dict);
        }

        return allTextDic;
    }
}
