using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public class Item
    {
        private readonly int id;
        private readonly string name;
        private int quantity;
        private double price;
        private double basePrice;
        private double multiplier;
        private double baseMultiplier;

        public Item(int id, string name, double basePrice, double baseMultiplier)
        {
            this.id = id;
            this.name = name;
            this.quantity = 0;
            this.price = basePrice;
            this.basePrice = basePrice;
            this.multiplier = baseMultiplier;
            this.baseMultiplier = baseMultiplier;
        }

        public int GetId(){ return id; }

        public string GetName(){ return name; }

        public int GetQuantity() { return quantity; }

        public void SetQuantity(int quantity) { this.quantity += quantity; }

        public double GetPrice() { return price; }

        public void SetPrice(double price){ this.price = price; }

        public double GetBasePrice() { return basePrice; }

        public void SetBasePrice(double basePrice) { this.basePrice = basePrice; }

        public double GetMultiplier() { return multiplier; }

        public void SetMultiplier(double multiplier) { this.multiplier *= multiplier; }

        public double GetBaseMultiplier() { return baseMultiplier; }

        public void SetBaseMultiplier(double baseMultiplier) { this.baseMultiplier = baseMultiplier; }
    }

    public List<Item> itens = new List<Item>();

    private Controller Controller;
    private UpgradeController upgrade;
    private Format format;
    private XML xml;

    private List<Button> buttons = new List<Button>();

    public RectTransform parent;
    
    private void Start()
    {
        Controller = GameObject.FindObjectOfType(typeof(Controller)) as Controller;
        upgrade = GameObject.FindObjectOfType(typeof(UpgradeController)) as UpgradeController;
        format = GameObject.FindObjectOfType(typeof(Format)) as Format;
        xml = GameObject.FindObjectOfType(typeof(XML)) as XML;

        foreach(Dictionary<string, string> dict in xml.parseFile("ShopItens", "shop", "itens", "item"))
        {
            itens.Add(new Item( int.Parse(dict["id"]), 
                                dict["name"], 
                                double.Parse(dict["basePrice"]), 
                                double.Parse(dict["baseMultiplier"]
                     )));
        }

        buttons = Controller.InstatiateButtons(parent, Resources.Load<GameObject>("btnShop"), itens.Count, "shop");

        foreach (Item item in itens)
        {
            UpdateShop(item.GetId());
        }
    }

    public void Click(int id)
    {
        if(Controller.cookie.GetCookies() >= itens[id].GetPrice())
        {
            Controller.cookie.SetCookies(Controller.cookie.GetCookies() - itens[id].GetPrice());

            itens[id].SetQuantity(1);
            itens[id].SetPrice(itens[id].GetPrice() * 1.15f);     
            
            foreach(var upgrade in upgrade.upgrades)
            {
                this.upgrade.UpdateUpgrades(upgrade.GetId());
            }

            UpdateShop(id);
        }
    }    

    public void UpdateShop(int id)
    {
        buttons[id].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itens[id].GetName();
        buttons[id].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = itens[id].GetQuantity().ToString();
        buttons[id].transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = format.NumberFormat(itens[id].GetPrice());
        buttons[id].transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = format.NumberFormat(itens[id].GetMultiplier() * itens[id].GetQuantity());
    }
}
