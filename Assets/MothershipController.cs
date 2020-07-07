using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MothershipController : MonoBehaviour
{
    public float relativeYRetracted = 3.8f;
    public float relativeYEngaged = 2f;
    public float moveSpeed = 10f;
    public bool isEngaged = false;

    public ScoreManager scoreMgr;
    // Start is called before the first frame update
    void Start()
    {
        if (scoreMgr == null)
            scoreMgr = FindObjectOfType<ScoreManager>();

        if (scoreMgr != null)
            scoreMgr.mothershipActiveChangeEvent.AddListener(MothershipActiveChanged);
    }

    // Update is called once per frame
    void Update()
    {
        var distanceToMove = isEngaged
            ? relativeYEngaged - transform.localPosition.y
            : relativeYRetracted - transform.localPosition.y;

        if (distanceToMove < 0)
            transform.localPosition -= new Vector3(0f, Mathf.Clamp(moveSpeed * Time.deltaTime, 0f, distanceToMove * -1), 0f);
        else if (distanceToMove > 0)
            transform.localPosition += new Vector3(0f, Mathf.Clamp(moveSpeed * Time.deltaTime, 0f, distanceToMove), 0f);
    }

    void MothershipActiveChanged(bool active)
    {
        isEngaged = scoreMgr.mothershipActive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (scoreMgr != null)
        {
            var alien = collision.gameObject.GetComponent<Alien>();
            if (alien != null)
            {
                scoreMgr.AlienAbducted(alien);
            }
        }
    }
}
