using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    public class Cookie
    {
        private double cookies;
        private double cookiesPerSecond;
        private double multiplier;

        public Cookie()
        {
            this.cookies = 0; 
            this.cookiesPerSecond = 0;
            this.multiplier = 1f;
        }

        public double GetCookies() { return cookies; }

        public void SetCookies(double cookies) { this.cookies = cookies; }

        public double GetCookiesPerSecond() { return cookiesPerSecond; }

        public void SetCookiesPerSecond(double cookiesPerSecond) { this.cookiesPerSecond = cookiesPerSecond; }

        public double GetMultiplier() { return multiplier; }

        public void SetMultiplier(double multiplier) { this.multiplier = multiplier; }  
    }

    private ShopController shop;
    private UpgradeController upgrade;
    private Format format;
    private ObjectPool pool;

    public Cookie cookie = new Cookie();    

    public GameObject txt_CookieQuantity;
    public GameObject txt_CookiePerSecond;
    public GameObject canvas;
    public Vector3 mousePos;

    private float elapsed = 0;

    private void Start()
    {
        shop = GameObject.FindObjectOfType(typeof(ShopController)) as ShopController;
        upgrade = GameObject.FindObjectOfType(typeof(UpgradeController)) as UpgradeController;
        format = GameObject.FindObjectOfType(typeof(Format)) as Format; 
        pool = GameObject.FindObjectOfType(typeof(ObjectPool)) as ObjectPool;  
    }

    private void FixedUpdate()
    {
        elapsed += Time.deltaTime;

        //Repete a cada 10 segundos
        if(elapsed >= 1f)
        {
            elapsed %= 0.1f;
            AutoClick();
        }

        mousePos = MousePosInWorldSpace();

        SetCookieQuantity(cookie.GetCookies());
        SetCookiePerSecond(cookie.GetCookiesPerSecond());
    }

    public Vector3 MousePosInWorldSpace()
    {
        Vector3 mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
    }

    /* ------ Clicks ------ */
    public void Click()
    {
        cookie.SetCookies(cookie.GetCookies() + (1 * cookie.GetMultiplier()));
        InstatiateCookie();
    }

    public void AutoClick()
    {
        double multiplier = 0;

        foreach (var item in shop.itens)
        {
            multiplier += item.GetQuantity() * item.GetMultiplier();
        }

        cookie.SetCookiesPerSecond(multiplier);
        cookie.SetCookies(cookie.GetCookies() + cookie.GetCookiesPerSecond());
    }

    /* ------ Cookie UIController ------ */
    public void SetCookieQuantity(double cookie)
    {
        txt_CookieQuantity.GetComponent<TextMeshProUGUI>().text = format.NumberFormat(cookie) + " cookies";
    }

    public void SetCookiePerSecond (double cookie)
    {
        txt_CookiePerSecond.GetComponent<TextMeshProUGUI>().text = "per second: " + format.NumberFormat(cookie);
    }

    /* ------ Instantiate Objects ------ */
    public List<Button> InstatiateButtons(RectTransform parent, GameObject prefab, int quantity, string name)
    {
        List<Button> buttons = new List<Button>();

        for(int i = 0; i < quantity; i++)
        {
            int id = i;

            GameObject btn = Instantiate(prefab);
            btn.name += id;
            btn.transform.SetParent(parent, false);
            btn.transform.localScale = new Vector3(1, 1, 1);
            btn.GetComponent<Button>().onClick.RemoveAllListeners();

            if (name.Equals("shop"))
                btn.GetComponent<Button>().onClick.AddListener(() => shop.Click(id));
            else if(name.Equals("upgrade"))
                btn.GetComponent<Button>().onClick.AddListener(() => upgrade.Click(id));

            buttons.Add(btn.GetComponent<Button>());
        }

        return buttons;
    }

    public void InstatiateCookie()
    {
        Image tempCookie = pool.GetPooledObject();

        if(tempCookie != null)
        {
            tempCookie.gameObject.SetActive(true);
            tempCookie.transform.position = mousePos;
            tempCookie.transform.localScale = new Vector3(1, 1, 1);

            StartCoroutine(DeactivateObj(tempCookie));
        }      
    }

    IEnumerator DeactivateObj(Image tempCookie)
    {
        yield return new WaitForSeconds(1f);
        tempCookie.gameObject.SetActive(false);
    }

    /* ------ 😈 HACK 😈 ------ */
    public void Hack()
    {
        cookie.SetCookies(cookie.GetCookies() * 10f);
    }
}