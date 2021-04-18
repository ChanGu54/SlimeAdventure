using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    RectTransform rectBack;
    RectTransform rectJoystick;

    public Transform ctrl_Target;
    float max_Radius;
    public float speed = 1f;

    Vector3 vecMove;

    bool isTouching = false;
    GameManager gm;


    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        SlimeAnimCtrl.ctrl_Target = ctrl_Target.GetComponent<Animator>();
        SlimeAnimCtrl.Idle();

        rectBack = transform.GetComponent<RectTransform>();
        rectJoystick = transform.Find("Joystick").GetComponent<RectTransform>();

        // JoystickBackground의 반지름입니다.
        max_Radius = rectBack.rect.width * 0.5f;
        Debug.Log(max_Radius);
    }

    void Update()
    {
        if (isTouching && rectBack.gameObject.activeSelf && !gm.isGameOver)
        {
            ctrl_Target.position += vecMove;
        }
    }

    void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(vecTouch.x - rectBack.position.x, vecTouch.y - rectBack.position.y);


        // vec값을 max_Radius 이상이 되지 않도록 합니다.
        vec = Vector2.ClampMagnitude(vec, max_Radius);
        rectJoystick.localPosition = vec;

        // 조이스틱 배경과 조이스틱과의 거리 비율로 이동합니다.
        float fSqr = rectJoystick.localPosition.magnitude / max_Radius;
        Debug.Log("fSqr : " + fSqr + ", max_Radius : " + (rectBack.rect.height * 0.5f));

        // 터치위치 정규화
        Vector2 vecNormal = vec.normalized;

        // 이동 방향 정의
        vecMove = new Vector3(vecNormal.x, 0f, vecNormal.y) * speed * Time.deltaTime * fSqr;
        // 회전 방향 정의
        ctrl_Target.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
        SlimeAnimCtrl.Walking();
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        isTouching = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        isTouching = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 원래 위치로 되돌립니다.
        rectJoystick.localPosition = Vector2.zero;
        isTouching = false;
        SlimeAnimCtrl.Idle();
    }
}