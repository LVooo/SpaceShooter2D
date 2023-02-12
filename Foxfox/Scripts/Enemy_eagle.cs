using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_eagle : Enemy
{
    private Rigidbody2D rb;
    private Collider2D Coll;
    private Animator Anim;

    public Transform toppoint, bottompoint;
    public float Speed;
    private float topy, bottomy;

    private bool isUp;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();

        topy = toppoint.position.y;
        bottomy = bottompoint.position.y;
        Destroy(toppoint.gameObject);
        Destroy(bottompoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if (transform.position.y > topy)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if (transform.position.y < bottomy)
            {
                isUp = true;
            }
        }
    }
}
