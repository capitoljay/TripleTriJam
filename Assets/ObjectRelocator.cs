using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRelocator : MonoBehaviour
{
    public Camera trackingCamera;
    public ScoreManager scoreMgr;
    public float xAxisMinimum = -37;
    public float xAxisMaximum = 37f;
    public Transform[] objectsToRelocateRecursive;
    public float objectRelocateTimeout = 2f;
    private float objectRelocateCountdown = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (scoreMgr == null)
            scoreMgr = gameObject.GetComponent<ScoreManager>();
        if (scoreMgr == null)
            scoreMgr = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //RelocateObject(trackingCamera.transform);
        //RelocateObject(scoreMgr.currentShip.transform);
        if (trackingCamera.transform.position.x < xAxisMinimum)
        {
            trackingCamera.transform.position -= new Vector3(xAxisMinimum - xAxisMaximum, 0f, 0f);
            scoreMgr.currentShip.transform.position -= new Vector3(xAxisMinimum - xAxisMaximum, 0f, 0f);
        }
        if (trackingCamera.transform.position.x > xAxisMaximum)
        {
            trackingCamera.transform.position -= new Vector3(xAxisMaximum - xAxisMinimum, 0f, 0f);
            scoreMgr.currentShip.transform.position -= new Vector3(xAxisMaximum - xAxisMinimum, 0f, 0f);
        }

        if (objectsToRelocateRecursive != null && objectsToRelocateRecursive.Length > 0)
        {
            objectRelocateCountdown -= Time.deltaTime;
            if (objectRelocateCountdown <= 0f)
            {
                objectRelocateCountdown = objectRelocateTimeout;
                foreach (var obj in objectsToRelocateRecursive)
                    RelocateObjectsRecursive(obj);
            }
        }
    }
    private void RelocateObjectsRecursive(Transform transform)
    {
        for(int x =0; x < transform.childCount; x++)
        {
            var child = transform.GetChild(x);
            if (child.childCount > 0)
            {
                RelocateObjectsRecursive(child);
            }
            else
            {
                RelocateObject(child);
            }
        }
    }
    private void RelocateObject(Transform transform)
    {
        if (transform.position.x < xAxisMinimum)
        {
            transform.position -= new Vector3(xAxisMinimum - xAxisMaximum, 0f, 0f);
        }
        if (transform.position.x > xAxisMaximum)
        {
            transform.position -= new Vector3(xAxisMaximum - xAxisMinimum, 0f, 0f);
        }
    }
        

}
