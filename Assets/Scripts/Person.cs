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
    public static float BaseCheerSpeed = 1;
    public float addedSpeed;
    private Animator _animator;

    void Start()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().materials[1].color = colors[Random.Range(0, colors.Length)];
        _anim = GetComponent<PlayerAnimations>();
        random = Random.Range(0, 100000);
        orgPos = transform.position;
        _animator = GetComponent<Animator>();
        _anim.Jubii();
        addedSpeed = Random.value / 5;
    }

    // Update is called once per frame
    void Update()
    {
        _animator.speed = addedSpeed + BaseCheerSpeed;

    }
}
