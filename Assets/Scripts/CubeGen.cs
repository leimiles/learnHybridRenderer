using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class CubeGen : MonoBehaviour
{
    [SerializeField]
    private int count = 0;
    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            if (value < 1)
            {
                count = 1;
            }
            else
            {
                count = value;
            }
        }
    }

    [SerializeField]
    private float margin = 0.1f;
    public float Margin
    {
        get
        {
            return margin;
        }
        set
        {
            if (value < 0.1f)
            {
                margin = 0.1f;
            }
            else
            {
                margin = value;
            }
        }
    }

    [SerializeField]
    GameObject obj;
    public GameObject Obj
    {
        get
        {
            return obj;
        }
        set
        {
            obj = value;
        }
    }

    List<GameObject> gameObjects = new List<GameObject>();

    public void Gen()
    {
        if (obj == null)
        {
            Debug.Log("nothing to gen");
            return;
        }
        for (int i = 0; i < this.count; i++)
        {
            gameObjects.Add(Instantiate(obj, GetPosition(i), GetRotation(i), this.transform));
        }
    }

    Vector3 GetPosition(int index)
    {
        return Random.insideUnitSphere * margin;
    }

    Quaternion GetRotation(int index)
    {
        return Quaternion.identity;
    }

    public void Clear()
    {
        if (gameObjects != null && gameObjects.Count > 0)
        {
            foreach (GameObject temp in gameObjects)
            {
                DestroyImmediate(temp);
            }
            gameObjects.Clear();
        }
    }

    public void Fetch()
    {
        if (gameObjects != null && gameObjects.Count > 0)
        {
            gameObjects.Clear();
        }
        Transform[] childrenTransform = this.GetComponentsInChildren<Transform>();
        foreach (Transform transform in childrenTransform)
        {
            if (transform.gameObject.name != this.gameObject.name)
            {
                gameObjects.Add(transform.gameObject);
            }
        }
        Debug.Log(gameObjects.Count + " fetched ");
    }
}
