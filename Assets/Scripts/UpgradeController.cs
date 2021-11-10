using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeController : MonoBehaviour
{
    public class Upgrades
    {
        private readonly int id;
        private readonly int idReference;
        private readonly string name;
        private readonly string img;
        private readonly string description;
        private readonly double multiplier;
        private double price;
        private readonly double basePrice;
        private readonly double unlockCondition;
        private bool isUnlocked;

        public Upgrades(int id, int idReference, string name, string img,string description, double multiplier, double basePrice, double unlockCondition)
        {
            this.id = id;
            this.idReference = idReference;
            this.name = name;   
            this.img = img;
            this.description = description;
            this.multiplier = multiplier;
            this.price = basePrice;
            this.basePrice = basePrice;
            this.unlockCondition = unlockCondition;
            this.isUnlocked = false;
        }

        public int GetId() { return id; }
        
        public int GetIdReference() { return idReference;}

        public string GetName() { return name; }

        public string GetImg() { return img; }

        public string GetDescription() { return description; }

        public double GetMultiplier() { return multiplier; }

        public double GetPrice() { return price; }

        public void SetPrice(double price) { this.price = price; }

        public double GetBasePrice() { return basePrice; }

        public double GetUnlockCondition() { return unlockCondition; }
        
        public bool GetIsUnlocked() { return isUnlocked; }

        public void SetIsUnlocked(bool isUnlocked) { this.isUnlocked = isUnlocked; }
    }

    public List<Upgrades> upgrades = new List<Upgrades>();
    private List<Button> buttons = new List<Button>();

    private Controller Controller;
    private ShopController shop;
    private Format format;
    private XML xml;

    public RectTransform parent;

    private void Start()
    {
        Controller = GameObject.FindObjectOfType(typeof(Controller)) as Controller;
        shop = GameObject.FindObjectOfType(typeof(ShopController)) as ShopController;
        format = GameObject.FindObjectOfType(typeof(Format)) as Format;
        xml = GameObject.FindObjectOfType(typeof(XML)) as XML;

        List<List<Dictionary<string, string>>> allTextDic = new List<List<Dictionary<string, string>>>();
        allTextDic.Add(xml.parseFile("Upgrades", "upgrades", "cursors", "cursor"));
        allTextDic.Add(xml.parseFile("Upgrades", "upgrades", "grandmas", "grandma"));

        foreach (List<Dictionary<string, string>> textDic in allTextDic)
        {
            foreach(Dictionary<string, string> text in textDic)
            {
                upgrades.Add(new Upgrades(int.Parse(text["id"]),
                                         int.Parse(text["idReference"]),
                                         text["name"],
                                         "img",
                                         text["description"],
                                         double.Parse(text["multiplier"]),
                                         double.Parse(text["basePrice"]),
                                         double.Parse(text["unlockCondition"])
                                        ));
            }            
        }

        buttons = Controller.InstatiateButtons(parent, Resources.Load<GameObject>("btnUpgrades"), upgrades.Count, "upgrade");

        foreach(Upgrades upgrade in upgrades)
        {    
            UpdateUpgrades(upgrade.GetId());
        }
    }

    private void FixedUpdate()
    {

    }

    public void Click(int id)
    {
        if (Controller.cookie.GetCookies() >= upgrades[id].GetPrice())
        {
            int idReference = upgrades[id].GetIdReference();

            Controller.cookie.SetCookies(Controller.cookie.GetCookies() - upgrades[id].GetPrice());
            Controller.cookie.SetMultiplier(idReference == 0 ? Controller.cookie.GetMultiplier() * upgrades[id].GetMultiplier() : Controller.cookie.GetMultiplier());

            shop.itens[idReference].SetMultiplier(upgrades[id].GetMultiplier());
            shop.UpdateShop(idReference);

            upgrades[id].SetIsUnlocked(true);
            UpdateUpgrades(id);
        }
    }

    public void UpdateUpgrades(int id)
    {
        buttons[id].gameObject.SetActive((shop.itens[upgrades[id].GetIdReference()].GetQuantity() >= upgrades[id].GetUnlockCondition() && !upgrades[id].GetIsUnlocked()) ? true : false);
        buttons[id].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = format.NumberFormat(upgrades[id].GetPrice());
    }
}