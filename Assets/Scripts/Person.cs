using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    private Color[] colors = new[]
        { Color.blue, Color.black, Color.cyan, Color.green, Color.red, Color.yellow, Color.magenta };

    public int random;
    private PlayerAnimations _anim;
    public Vector3 orgPos;
    void Start()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().materials[1].color = colors[Random.Range(0, colors.Length)];
        _anim = GetComponent<PlayerAnimations>();
        random = Random.Range(0, 100000);
        orgPos = transform.position;

        _anim.Jubii();

        GetComponent<Animator>().speed += Random.value / 5;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = orgPos + new Vector3(0, Mathf.Abs(Mathf.Sin(random + Time.time*5))/1.5f,0);

    }
}
