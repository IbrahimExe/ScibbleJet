using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGScroller : MonoBehaviour
{
    public BoxCollider2D collider;
    public Rigidbody2D rb;

    private float width;
    private float scrollSpeed = -2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = collider.size.x;
        collider.enabled = false;

        rb.linearVelocity = new Vector2(scrollSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -width)
        {
            Vector2 resetPosition = new Vector2((width - 0.3f) * 2f, 0);
            transform.position = (Vector2)transform.position + resetPosition;
        }
    }
}
