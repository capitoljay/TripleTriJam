using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRelocator : MonoBehaviour
{
    public Camera trackingCamera;
    public ScoreManager scoreMgr;
    public float xAxisMinimum = -37;
    public float xAxisMaximum = 37f;
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
        if (trackingCamera.transform.position.x < xAxisMinimum)
        {
            trackingCamera.transform.position -= new Vector3(xAxisMinimum-xAxisMaximum, 0f, 0f);
            scoreMgr.currentShip.transform.position -= new Vector3(xAxisMinimum - xAxisMaximum, 0f, 0f);
        }
        if (trackingCamera.transform.position.x > xAxisMaximum)
        {
            trackingCamera.transform.position -= new Vector3(xAxisMaximum-xAxisMinimum, 0f, 0f);
            scoreMgr.currentShip.transform.position -= new Vector3(xAxisMaximum - xAxisMinimum, 0f, 0f);
        }
    }
}
