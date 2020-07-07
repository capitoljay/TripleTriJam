using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 0.5f;
    public float speed = 10f;
    internal Vector3 direction;
    private float lifeCountdown = 0f;
    public bool onlyEnemies = true;
    ScoreManager scoreMgr;

    // Start is called before the first frame update
    void Start()
    {
        lifeCountdown = lifeTime;
        if (scoreMgr == null)
            scoreMgr = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.transform.localScale.x < 0)
        //{
        //    this.transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        //}
        //else
        //{ 
        //    this.transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        //}
        this.transform.position += direction * speed * Time.deltaTime;

        lifeCountdown -= Time.deltaTime;
        if (lifeCountdown <= 0f)
        {
            Destroy(gameObject);
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
            //var projectile = collision.gameObject.GetComponent<Projectile>();
            var enemy = collision.gameObject.GetComponent<EnemyController>();

            //if ((projectile != null && !projectile.onlyEnemies) || enemy != null)
            if (enemy != null)
            {
                scoreMgr.EnemyKilled(enemy);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
