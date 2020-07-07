using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public Transform followObject;
    public float followSpeed = 1f;
    public float horizontalOffsetMax = 8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followObject != null)
        {
            var followOffset = transform.position.x - followObject.position.x;
            if (Mathf.Abs(followOffset) > 0)
            {
                var speed = Mathf.Lerp(0f, followSpeed * Time.fixedDeltaTime, Mathf.Clamp(Mathf.Abs(followOffset) / horizontalOffsetMax, 0f, 1f)) * (followOffset < 0 ? -1 : 1);
                //transform.Translate(new Vector3(speed, 0f, 0f));
                transform.position -= new Vector3(speed, 0f, 0f);
            }
        }
    }
}
