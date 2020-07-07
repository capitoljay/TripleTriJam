using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class ScoreManager : MonoBehaviour
{
    public int lives = 5;
    public long score = 0;
    public long level = 1;
    public int aliensInLevel = 10;
    public int aliensAbducted = 0;
    public GameObject shipPrefab;
    public Transform shipParent;
    public float newLifeTimeout = 3f;
    private float newLifeCountdown = 0;
    private bool shipDestroyed = false;
    public ParticleSystem destroyedParticles;
    public AudioSource destroyedAudio;

    public Transform projectileParent;

    public Transform followObject;
    public Camera followCamera;
    public float followSpeed = 15f;
    public float followHorizontalOffset = 2f;

    private Vector3 lastPosition = Vector3.zero;

    internal bool mothershipActive = false;

    public ScoreChangedEvent scoreChangedEvent;
    public LivesChangedEvent livesChangedEvent;
    public MothershipActiveChangeEvent mothershipActiveChangeEvent;

    public bool forceUpdateLives = false;
    public long basePointsDestroyingEnemy = 10;
    public long pointsForAbductingAlien = 20;
    internal GameObject currentShip;
    public string playerName = "Player";
    private string oldPlayerName = "Player";

    public ObjectSpawner[] spawners;
    internal List<float> seedVals = new List<float>();
    internal float extraMoveSpeed = 0f;
    public float enemyMoveSpeedIncrement = 2f;

    // Start is called before the first frame update
    void Start()
    {
        if (shipParent == null)
            shipParent = transform;

        if (projectileParent == null)
            projectileParent = transform;

        if (spawners == null)
            spawners = FindObjectsOfType<ObjectSpawner>();

        scoreChangedEvent = new ScoreChangedEvent();
        livesChangedEvent = new LivesChangedEvent();
        mothershipActiveChangeEvent = new MothershipActiveChangeEvent();

        if (currentShip == null)
            CreateShip();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerName != oldPlayerName)
        {
            oldPlayerName = playerName;
            seedVals.Clear();
            var seed = System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.Default.GetBytes(playerName));
            for (int x = 0; x < seed.Length; x++)
            {
                seedVals.Add(((float)seed[x])/64f);
            }
        }    
        if (newLifeCountdown <= 0)
        {
            if (shipDestroyed || currentShip == null)
            {
                CreateShip();
            }
        }
        else
        {
            newLifeCountdown -= Time.deltaTime;
        }


        if (forceUpdateLives)
        {
            forceUpdateLives = false;
            LivesChanged();
        }
    }

    internal void LoadLevel(long level)
    {
        this.level = level;

        foreach(var spawner in spawners)
        {
            spawner.Spawn(level);
        }
    }

    void CreateShip()
    {
        extraMoveSpeed = 0;
        shipDestroyed = false;
        var newShip = Instantiate(shipPrefab);
        newShip.transform.SetParent(shipParent);
        newShip.transform.localPosition = lastPosition;
        followObject = newShip.transform;
        currentShip = newShip;
    }
    void FixedUpdate()
    {
        if (followObject != null)
        {
            var followOffset = followCamera.transform.position.x - followObject.position.x;
            if (Mathf.Abs(followOffset) > 0)
            {
                var speed = Mathf.Lerp(0f, followSpeed * Time.fixedDeltaTime, Mathf.Clamp(Mathf.Abs(followOffset) / followHorizontalOffset, 0f, 1f)) * (followOffset < 0 ? -1 : 1);
                followCamera.transform.position -= new Vector3(speed, 0f, 0f);
            }
        }
    }

    internal void ActivateMothership(bool activate)
    {
        if (activate != mothershipActive)
        {
            mothershipActive = activate;
            if (mothershipActiveChangeEvent != null)
                mothershipActiveChangeEvent.Invoke(mothershipActive);
        }
    }

    public void EnemyKilled(EnemyController enemy)
    {
        score += basePointsDestroyingEnemy * enemy.pointMultiplier;
        if (scoreChangedEvent != null)
            scoreChangedEvent.Invoke(score);
    }

    public void AlienAbducted(Alien alien)
    {
        score += pointsForAbductingAlien;
        aliensAbducted++;
        if (scoreChangedEvent != null)
            scoreChangedEvent.Invoke(score);

        extraMoveSpeed += enemyMoveSpeedIncrement;

        Destroy(alien.gameObject);
    }

    internal void DestroyShip(ShipController shipController)
    {
        if (lives > 0)
        {
            lives--;
            LivesChanged();
            lastPosition = shipController.transform.localPosition;
            shipDestroyed = true;
            newLifeCountdown = newLifeTimeout;

            if (destroyedParticles != null)
            {
                destroyedParticles.transform.position = shipController.transform.position;
                destroyedParticles.Play();
            }

            if (destroyedAudio != null)
                destroyedAudio.Play();
        }
        else
        {
            //GameOver();
        }
        Destroy(shipController.gameObject);

    }

    private void LivesChanged()
    {
        if (livesChangedEvent != null)
            livesChangedEvent.Invoke(lives);
    }
}
[System.Serializable]
public class LivesChangedEvent : UnityEvent<int>
{
}
[System.Serializable]
public class ScoreChangedEvent : UnityEvent<long>
{
}

[System.Serializable]
public class MothershipActiveChangeEvent : UnityEvent<bool>
{
}