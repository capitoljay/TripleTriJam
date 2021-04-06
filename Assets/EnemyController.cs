using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public long pointMultiplier = 1;
    public ScoreManager scoreMgr;
    public GameObject projectile;

    public float minProjectileTime = 3f;
    public float maxProjectileTime = 7f;
    public float minShipDistance = 20f;
    private float projectileTimeout = 0f;

    public float moveSpeed = 5f;
    public float maxSpeed = 15f;
    private Vector3 moveDirection = Vector3.zero;
    private float moveCheckCountdown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (scoreMgr == null)
            scoreMgr = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        moveCheckCountdown -= Time.deltaTime;
        if (moveCheckCountdown <= 0)
        {
            moveCheckCountdown = Random.Range(1f, 6f);
            var check = Random.value;
            if (check < 0.3f) //Move Horizontally Right
            {
                moveDirection += new Vector3(Random.value, 0f, 0f);
            }
            else if (check < 0.5f)//Move Horizontally Left
            {
                moveDirection += new Vector3(Random.value * -1, 0f, 0f);
            }
            else if (check < 0.7f)//Move Vertically Up
            {
                moveDirection += new Vector3(0f, Random.value, 0f);
            }
            else //Move vertically Down
            {
                moveDirection += new Vector3(0f, Random.value * -1, 0f);
            }

            if (Mathf.Abs(moveDirection.x) > maxSpeed)
            {
                moveDirection.x = moveDirection.x < 0 ? maxSpeed * -1 : maxSpeed;
            }
            if (Mathf.Abs(moveDirection.y) > maxSpeed)
            {
                moveDirection.y = moveDirection.y < 0 ? maxSpeed * -1 : maxSpeed;
            }
        }

        transform.Translate(moveDirection * (moveSpeed + scoreMgr.extraMoveSpeed ) * Time.deltaTime);
    }

    private void Shoot()
    {
        if (projectileTimeout <= 0f)
        {
            projectileTimeout = Random.Range(minProjectileTime, maxProjectileTime);
            var dist = Vector3.Distance(transform.position, scoreMgr.currentShip.transform.position);
            if (dist < minShipDistance) 
            {
                var newProj = Instantiate(projectile);
                var newProjectile = newProj.GetComponent<Projectile>();
                newProjectile.direction = newProj.transform.position - scoreMgr.currentShip.transform.position;
                newProj.transform.SetParent(transform);
                newProj.gameObject.SetActive(true);
            }
        }
        else
        {
            projectileTimeout -= Time.deltaTime;
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
        var enemyBoundary = collision.gameObject.GetComponent<EnemyBoundary>();
        if (enemyBoundary != null && enemyBoundary.otherBoundary != null)
        {
            transform.position = new Vector3(
                transform.position.x,
                enemyBoundary.otherBoundary.transform.position.y + enemyBoundary.spawnOffset,
                transform.position.z
                );
        }
        //if (collision.gameObject != null)
        //{
        //    var projectile = collision.gameObject.GetComponent<Projectile>();
        //    var enemy = collision.gameObject.GetComponent<EnemyController>();

        //    if ((projectile != null && !projectile.onlyEnemies) || enemy != null)
        //    {
        //        if (enemy != null)
        //            scoreMgr.EnemyKilled(enemy);

        //        scoreMgr.DestroyShip(this);

        //        Destroy(collision.gameObject);
        //    }
        //}
    }
}
