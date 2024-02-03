using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    private Color[] colors = new[]
        { Color.blue, Color.black, Color.cyan, Color.green, Color.red, Color.yellow, Color.magenta };

    public int random;

    public Vector3 orgPos;
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)];
        random = Random.Range(0, 100000);
        orgPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = orgPos + new Vector3(0, Mathf.Abs(Mathf.Sin(random + Time.time*5))/1.5f,0);

    }
}
