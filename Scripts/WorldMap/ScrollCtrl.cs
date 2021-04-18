using System;
using UnityEngine;

public class ScrollCtrl : MonoBehaviour
{
    private bool isSwiping;
    private Touch t;
    private Vector3 startPos;
    private Vector3 endPos;

    private Vector3 mouse;

    private WorldMapInit wmi;
    private GameManager gm;

    public GameObject StageSelectPanel;

    // Start is called before the first frame update
    void Start()
    {
        wmi = GetComponent<WorldMapInit>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        isSwiping = false;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        Pc_SwipeCtrl();
#elif UNITY_ANDROID
        Andorid_SwipeCtrl();
    
#endif
    }

    void Andorid_SwipeCtrl()
    {
        if (Input.touchCount > 0)
        {
            t = Input.GetTouch(0);
            if (isSwiping == false && t.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray touchray = Camera.main.ScreenPointToRay(t.position);
                Physics.Raycast(touchray, out hit);

                try
                {
                    if (hit.transform.tag.Equals("Stage"))
                    {
                        StageSelectPanel.SetActive(true);
                    }

                    else
                    {
                        isSwiping = true;
                        startPos = t.position;
                    }
                }
                catch (Exception)
                {
                    isSwiping = true;
                    startPos = t.position;
                }
            }
            if (isSwiping == true)
            {
                Andorid_SwipeCheck();
            }
            if (t.phase == TouchPhase.Ended)
            {
                isSwiping = false;
            }
        }
    }

    void Pc_SwipeCtrl()
    {
        if (Input.GetMouseButton(0))
        {
            mouse = Input.mousePosition;
            if (isSwiping)
            {
                Pc_SwipeCheck();
            }
            else if (!isSwiping)
            {
                RaycastHit hit;
                Ray touchray = Camera.main.ScreenPointToRay(mouse);
                Physics.Raycast(touchray, out hit);

                try
                {
                    if (hit.transform.tag.Equals("Stage"))
                    {
                        StageSelectPanel.SetActive(true);
                    }

                    else
                    {
                        isSwiping = true;
                        startPos = mouse;
                    }
                }
                catch (Exception)
                {
                    isSwiping = true;
                    startPos = mouse;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
            isSwiping = false;
    }

    void Andorid_SwipeCheck()
    {
        endPos = t.position;
        Vector3 swipeDirection = endPos - startPos;
        float swipeDetection = swipeDirection.x / Screen.width;
        if (swipeDetection < -1 / 10.0f)
        {
            isSwiping = false;
            int maxStage = -1;
            for (int i = 0; i < gm.data.stars.Length; i++)
            {
                if (gm.data.stars[i] == 0)
                {
                    maxStage = i;
                    break;
                }
            }
            if (wmi.seeing != gm.data.stars.Length - 1 && wmi.seeing < maxStage)
            {
                ++wmi.seeing;
            }

            Debug.Log("우측 슬라이드");
        }

        else if (swipeDetection > 1 / 10.0f)
        {
            isSwiping = false;
            if (wmi.seeing != 0)
                wmi.seeing--;
            Debug.Log("좌측 슬라이드");
        }
    }

    void Pc_SwipeCheck()
    {
        endPos = Input.mousePosition;
        Vector3 swipeDirection = endPos - startPos;
        float swipeDetection = swipeDirection.x / Screen.width;
        Debug.Log(swipeDirection);
        if (swipeDetection < -1 / 5.0f)
        {
            isSwiping = false;
            int maxStage = -1;

            Debug.Log(gm.data.stars);
            for (int i = 0; i < gm.data.stars.Length; i++)
            {
                if (gm.data.stars[i] == 0)
                {
                    maxStage = i;
                    break;
                }
            }
            if (wmi.seeing != gm.data.stars.Length - 1 && wmi.seeing < maxStage)
            {
                ++wmi.seeing;
            }

            Debug.Log("우측 슬라이드");
        }

        else if (swipeDetection > 1 / 5.0f)
        {
            isSwiping = false;
            if (wmi.seeing != 0)
                wmi.seeing--;

            Debug.Log("좌측 슬라이드");
        }
    }
}
