using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody2D rb;

    public GameObject tractorObj;
    public GameObject firePrefab;

    public bool carryingAlien = false;
    public float horizontalAccelerationForce = 0.75f;
    public float verticalAccelerationForce = 0.35f;
    public float maxHorizontalVelocity = 15f;
    public float maxVerticalVelocity = 8f;
    public float maxVerticalVelocityWithTractor = 5f;
    public Vector2 currentVelocity = Vector2.zero;
    public float alienForceReduction = 2f;
    private bool facingForward = true;
    private float xAxisScale = 1f;
    private float xAxisScaleFire = 1f;
    private float xAxisScaleTractor = 1f;
    public bool isShooting = false;
    public Vector3 projectileOffset = new Vector3(0.2f, 0f, 0f);

    private SpriteRenderer renderer;
    private ScoreManager scoreMgr;
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
            rb = this.GetComponent<Rigidbody2D>();

        if (scoreMgr == null)
            scoreMgr = FindObjectOfType<ScoreManager>();

        xAxisScale = transform.localScale.x;

        if (firePrefab != null)
            xAxisScaleFire = firePrefab.transform.localScale.x;

        if (tractorObj != null)
            xAxisScaleTractor = tractorObj.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
    }

    void Shoot()
    {
        var fire = Input.GetAxis("Fire1");
        var tractor = Input.GetAxis("Fire3");

        if (firePrefab != null && fire != 0f & !isShooting)
        {
            isShooting = true;
            var newFireObj = Instantiate<GameObject>(firePrefab);
            var fireProjectile = newFireObj.GetComponent<Projectile>();

            if (fireProjectile != null)
            {
                fireProjectile.direction = facingForward ? Vector3.right : Vector3.left;
            }

            newFireObj.transform.SetParent(scoreMgr.projectileParent);
            newFireObj.transform.position = transform.position + (facingForward ? projectileOffset : projectileOffset  * -1);
            newFireObj.SetActive(true);

        }
        else
        {
            isShooting = fire != 0;
        }

        if (tractorObj != null)
        {
            carryingAlien = tractor != 0f;
            tractorObj.SetActive(tractor != 0f);
            scoreMgr.ActivateMothership(carryingAlien);
        }

        //if (tractor != 0f)
    }
    void Move()
    {
        var horiz = Input.GetAxis("Horizontal") * horizontalAccelerationForce / (carryingAlien ? alienForceReduction : 1f);
        var vert = Input.GetAxis("Vertical") * verticalAccelerationForce / (carryingAlien ? alienForceReduction : 1f);
        currentVelocity = rb.velocity;

        if (horiz != 0f || vert != 0f)
        {
            //Limit vertical velocity based on whether the tractor beam is on.
            var vertVelocity = carryingAlien ? maxVerticalVelocityWithTractor : maxVerticalVelocity;

            if ((rb.velocity.x < maxHorizontalVelocity && rb.velocity.y < vertVelocity) &&
                (rb.velocity.x > maxHorizontalVelocity * -1 && rb.velocity.y > vertVelocity * -1))
                rb.AddForce(new Vector2(horiz, vert), ForceMode2D.Impulse);


            if (horiz < 0)
                facingForward = false;
            else if (horiz > 0)
                facingForward = true;

            transform.localScale = new Vector3((facingForward ? xAxisScale : xAxisScale * -1), transform.localScale.y, transform.localScale.z);

            if (tractorObj != null)
                tractorObj.transform.localScale = new Vector3((facingForward ? xAxisScaleTractor : xAxisScaleTractor * -1), tractorObj.transform.localScale.y, tractorObj.transform.localScale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.collider);
    }

    private void CheckCollision(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            var projectile = collision.gameObject.GetComponent<Projectile>();
            var enemy = collision.gameObject.GetComponent<EnemyController>();

            if ((projectile != null && !projectile.onlyEnemies) || enemy != null)
            {
                if (enemy != null)
                    scoreMgr.EnemyKilled(enemy);

                scoreMgr.DestroyShip(this);

                Destroy(collision.gameObject);
            }
        }
    }
}
