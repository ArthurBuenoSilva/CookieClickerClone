using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{
    public int amount;

    private List<Image> pooledObjects = new List<Image>();
    private Image prefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            int random = Random.Range(0, 3);
            prefab = Resources.Load<Image>("Cookies/cookiePrefab" + random);

            Image obj = Instantiate(prefab);
            obj.transform.SetParent(transform, false);
            obj.gameObject.SetActive(false);

            pooledObjects.Add(obj);
        }
    }

    public Image GetPooledObject()
    {
        foreach(Image obj in pooledObjects)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }

        return null;
    }
}
