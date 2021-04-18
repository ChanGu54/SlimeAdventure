using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    public Transform targetTr;
    private Vector3 targetPos;
    private Vector3 prevPos;

    private float distTarget;
    private float distPrev;

    private float lerpDist;
    public float lerpRate;

    public Vector3 rotationVal;

    public float curDist;

    private bool isFullMode;
    public Vector3 fullModeCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        isFullMode = false;
        prevPos = transform.position;
        lerpDist = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFullMode)
            FollowingCamera();
        else
            ViewEntireMap();
    }

    void FollowingCamera()
    {
        Quaternion rotate = Quaternion.Euler(rotationVal);
        Vector3 targetPoint = rotate * Vector3.back;
        targetPos = targetPoint * curDist + targetTr.position;
        transform.rotation = rotate;

        distTarget = Vector3.Distance(targetPos, transform.position);

        if (distTarget == 0)
            return;

        else
        {
            distPrev = Vector3.Distance(prevPos, targetPos);

            if (distPrev == 0 && lerpDist < 1)
            {
                lerpDist += lerpRate;
                transform.position = Vector3.Lerp(prevPos, targetPos, lerpDist);
            }
            else if (!(distPrev == 0))
            {
                lerpDist = lerpRate;
                transform.position = Vector3.Lerp(prevPos, targetPos, lerpRate);
            }

            prevPos = transform.position;
        }
    }

    void ViewEntireMap()
    {
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

        distPrev = Vector3.Distance(prevPos, fullModeCameraPos);

        if (distPrev == 0 && lerpDist < 1)
        {
            lerpDist += lerpRate;
            transform.position = Vector3.Lerp(prevPos, fullModeCameraPos, lerpDist);
        }
        else if (!(distPrev == 0))
        {
            lerpDist = lerpRate;
            transform.position = Vector3.Lerp(prevPos, fullModeCameraPos, lerpRate);
        }

        prevPos = transform.position;
    }

    public void OnClickFullScreenBtn()
    {
        isFullMode = !isFullMode;
        Debug.Log("Click Event Entered");
    }
}
