using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapInit : MonoBehaviour
{
    public Transform[] stage;

    Vector3[] seeingPos;
    Vector3 lastTargetPosition = Vector3.zero;
    Vector3 currentTargetPosition = Vector3.zero;
    float currentLerpDistance = 0;
    public float cameraTrackingSpeed = 0.2f;

    public Material cleared;
    public Material notCleared;
    private Transform cam;

    public int seeing;

    private bool fadeEnded;
    private GameManager gm;

    public GameObject toolTip;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        seeingPos = new Vector3[stage.Length];

        for (int i = 0; i < stage.Length; i++)
        {
            if (gm.data.stars[i] == 0)
                stage[i].Find("Identifier").GetComponent<Renderer>().material = notCleared;
            else
                stage[i].Find("Identifier").GetComponent<Renderer>().material = cleared;

            seeingPos[i] = new Vector3(stage[i].position.x, cam.position.y, cam.position.z);
        }

        fadeEnded = false;
        StartCoroutine(FadeIn(GameObject.Find("FadeIn").GetComponent<Image>()));
    }

    void cameraMovement()
    {
        Vector3 currentCamPos = cam.position;

        Vector3 currentTrackingPos = seeingPos[seeing];

        if (currentCamPos.x == currentTrackingPos.x && currentCamPos.y == currentTrackingPos.y)
        {
            currentLerpDistance = 1f;
            lastTargetPosition = currentCamPos;
            currentTargetPosition = currentCamPos;
            return;
        }

        currentLerpDistance = 0f;

        lastTargetPosition = currentCamPos;

        currentTargetPosition = currentTrackingPos;
    }

    void LateUpdate()
    {
        if (fadeEnded)
        {
            cameraMovement();

            currentLerpDistance += cameraTrackingSpeed;
            cam.position = Vector3.Lerp(lastTargetPosition, currentTargetPosition, currentLerpDistance);
        }
    }

    IEnumerator FadeIn(Image target)
    {
        seeing = gm.data.lastPlayedStage;
        float val = 1;
        float iteration = 0.002f;
        var prev_pos = cam.position;
        var lerp_pos = new Vector3(stage[seeing].position.x, prev_pos.y, prev_pos.z);

        while (val > 0)
        {
            cam.position = Vector3.Lerp(prev_pos, lerp_pos, 1 - val);
            target.color = new Color(0, 0, 0, val);
            val = val *  0.98f - iteration;
            yield return new WaitForSeconds(0.01f);
        }
        target.color = new Color(0, 0, 0, 0);

        fadeEnded = true;
        target.gameObject.SetActive(false);

        toolTip.SetActive(true);

        GetComponent<ScrollCtrl>().enabled = true;

        yield return null;
    }
}
