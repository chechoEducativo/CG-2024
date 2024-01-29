using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] private int initialSize;
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool fillOnAwake;

    private List<GameObject> pool = new List<GameObject>();

    private GameObject Spawn()
    {
        GameObject instance = Instantiate(prefab);
        instance.SetActive(false);
        pool.Add(instance);
        return instance;
    }
    
    private void Fill()
    {
        for (int i = 0; i < initialSize; i++)
        {
            Spawn();
        }
    }

    private void Awake()
    {
        if (fillOnAwake)
        {
            Fill();
        }
    }

    public GameObject Get(bool activate = true)
    {
        GameObject ret = null;
        if (pool.All(obj => obj.activeSelf))
        {
            ret = Spawn();
        }
        else
        {
            ret = pool.First(obj => !obj.activeSelf);
        }

        if(activate) ret.SetActive(true);
        
        return ret;
    }

    public TComponent Get<TComponent>(bool activate = true) where TComponent : Component
    {
        GameObject obj = Get(activate);
        Debug.Log(obj);
        TComponent comp = obj.GetComponent<TComponent>();
        if (comp == null)
        {
            Debug.LogError($"Component of type \"{typeof(TComponent)}\" not found");
            obj.SetActive( false);
        }

        return comp;
    }
}
