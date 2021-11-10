using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Format : MonoBehaviour
{
    private static List<string> nFormat = new List<string>();

    void Awake()
    {
        nFormat.Add("");
        nFormat.Add("K");
        nFormat.Add("M");
        nFormat.Add("B");
        nFormat.Add("T");
        nFormat.Add("aa");
        nFormat.Add("bb");
        nFormat.Add("cc");
        nFormat.Add("dd");
        nFormat.Add("ee");
        nFormat.Add("ff");
        nFormat.Add("gg");
        nFormat.Add("hh");
        nFormat.Add("ii");
        nFormat.Add("Infinite");
        //...
    }

    public string NumberFormat(double number)
    {
        int num = 0;

        while (number >= 1000d)
        {
            num++;
            number /= 1000d;
        }

        if (nFormat[num].Equals("Infinite"))
            return nFormat[num].ToString();
        else
            return number.ToString("F2") + nFormat[num];
    }
}
