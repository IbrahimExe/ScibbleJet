using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGScroller : MonoBehaviour
{
    public new BoxCollider2D collider;
    public Rigidbody2D rb;

    private float width;
    private float scrollSpeed = -10f;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = collider.size.x;
        collider.enabled = false;

        //rb.linearVelocity = new Vector2(scrollSpeed, 0);
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }

    void Update()
    {
        if (transform.position.x < -width)
        {
            Vector2 resetPosition = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + resetPosition;
        }
    }
}
