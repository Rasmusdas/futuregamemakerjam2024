using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnpackTesting : MonoBehaviour
{
    // Start is called before the first frame 
    
    List<Transform> children = new List<Transform>();
    void Start()
    {
        Unpack(transform);

        foreach (var child in children)
        {
            child.parent = null;

            child.AddComponent<Rigidbody>();
            child.AddComponent<BoxCollider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Unpack(Transform child)
    {
        children.Add(child);
        if (child.childCount > 0)
        {
            for (int i = 0; i < child.childCount; i++)
            {
                Unpack(child.GetChild(i));
            }
        }
    }
}
