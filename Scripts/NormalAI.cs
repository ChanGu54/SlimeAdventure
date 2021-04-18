using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NormalAI : MonoBehaviour
{
    private GameManager gm;
    public GameObject gameClearPanel;

    public AudioClip audio_Find;
    public AudioClip audio_Lost;
    private AudioSource audioSource;
    private bool audioflag;

    private Transform raycastPos;
    private Transform player;
    private NavMeshAgent nvAgent;
    private LineRenderer lineRenderer;
    private Animator anim;

    private GameObject recently_watched;
    private bool isWatched;
    private bool isGoActive;

    public Transform[] checkPoint;
    private int idx;
    private int len;

    public float sightAngle;
    public float sightDistance;

    public GameObject questionPrefab;
    public GameObject exclamationPrefab;
    private GameObject go;
    private float delayTime;

    public float expressionTime;

    private RaycastHit hit;

    // Start is called before the first frame update

    void CheckTargetInSight()
    {
        Vector3 targetDir = (player.position - raycastPos.position).normalized; // 정규화된 방향벡터 생성
        float dot = Vector3.Dot(raycastPos.forward, targetDir); // 방향벡터와 Enemy 오브젝트와의 내적 수행

        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg; // 사이각 = ACos(내적)하고 Radian -> 360
        float dist = Vector3.Distance(player.position, raycastPos.position);

        if (theta <= sightAngle && dist <= sightDistance)
        {
            // rayCast위치에서 Player가 식별되는지 확인
            if (Physics.Raycast(raycastPos.position, targetDir, out hit, sightDistance)) 
            {
                if (hit.collider.tag.Equals("player") && !gm.clocked)
                {
                    Debug.DrawLine(raycastPos.position, hit.point, Color.blue);
                    Debug.Log("타겟과 AI의 각도 : " + theta);
                    Debug.Log("타겟과 AI의 거리 : " + dist);
                    recently_watched.transform.position = player.transform.position;
                    isWatched = true;
                    if (!isGoActive)
                    {
                        go = Instantiate(exclamationPrefab, transform);
                        isGoActive = true;
                    }
                }
                else
                    Debug.DrawLine(raycastPos.position, hit.point, Color.red);
            }
        }
    }

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        audioflag = false;
        audioSource = GetComponent<AudioSource>();
        raycastPos = transform.Find("RaycastPos");
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("player").transform;
        nvAgent = gameObject.GetComponent<NavMeshAgent>();

        recently_watched = new GameObject("Recently_Watched");

        isWatched = false;
        isGoActive = false;

        idx = 0;
        len = checkPoint.Length;
        delayTime = 0;

        nvAgent.destination = checkPoint[idx % len].position;
        lineRenderer = transform.Find("LineRender").GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isWalking", true);
        CheckTargetInSight();

        if (!isWatched)
        {
            if (nvAgent.remainingDistance <= nvAgent.stoppingDistance)
            {
                idx++;
                nvAgent.destination = checkPoint[idx % len].position;
                anim.SetBool("isWalking", false);
            }
            else
            {
                nvAgent.destination = checkPoint[idx % len].position;
            }
        }
        else
        {
            if (nvAgent.remainingDistance <= nvAgent.stoppingDistance)
            {

                delayTime += Time.deltaTime;

                if (audioflag)
                {
                    audioflag = !audioflag;
                    audioSource.clip = audio_Lost;
                    audioSource.Play();
                }

                if (go.activeSelf)
                {
                    if (!audioflag)
                    {
                        audioflag = !audioflag;
                        audioSource.clip = audio_Find;
                        audioSource.Play();
                    }

                    go.SetActive(false);
                    lineRenderer.positionCount = 0;
                    StartCoroutine(DestroyObjectInTime(expressionTime));
                }

                if (delayTime < expressionTime)
                {
                    anim.SetBool("isWalking", false);
                }

                else
                {
                    delayTime = 0;
                    isWatched = false;
                    Destroy(go);
                    isGoActive = false;
                    nvAgent.destination = checkPoint[idx % len].position;
                }
            }
            else
            {
                if (!audioflag)
                {
                    audioflag = !audioflag;
                    audioSource.clip = audio_Find;
                    audioSource.Play();
                }
                nvAgent.destination = recently_watched.transform.position;
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, recently_watched.transform.position, NavMesh.AllAreas, path); //Saves the path in the path variable.
                Vector3[] corners = path.corners;
                lineRenderer.positionCount = corners.Length;
                lineRenderer.SetPositions(corners);
            }
        }
    }

    IEnumerator DestroyObjectInTime(float time)
    {
        var ques = Instantiate(questionPrefab, transform);
        yield return new WaitForSeconds(time);
        Destroy(ques);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("player"))
        {
            gm.isGameOver = true;
            gameClearPanel.SetActive(true);
        }
    }
}
