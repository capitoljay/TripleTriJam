using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public Collider2D tractorCollider;
    public float tractorForce = 2f;
    public Vector3 tractorOffset = new Vector3(0f, -.2f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        if (tractorCollider == null)
            tractorCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        var alien = collision.gameObject.GetComponent<Alien>();
        if (rb != null)
        {
            if (alien != null)
            {
                alien.transform.position = transform.position + tractorOffset;
                //rb.AddForce((transform.position - alien.transform.position).normalized * -1 * tractorForce);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        //var alien = collision.gameObject.GetComponent<Alien>();
        //if (rb != null)
        //{
        //    if (alien != null)
        //    {
        //        rb.AddForce((transform.position - alien.transform.position).normalized * -1 * tractorForce);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
