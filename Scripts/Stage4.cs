using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage4 : MonoBehaviour
{
    public Text text;
    private float time;
    int flag;
    float a = 0;

    public Image target;

    public Transform rotateObject;


    // Update is called once per frame
    void Start()
    {
        flag = 0;
        time = 0;
    }

    void Update()
    {
        rotateObject.rotation = Quaternion.Euler(new Vector3(0, rotateObject.rotation.eulerAngles.y + Time.deltaTime * 5, 0));

        if (flag == 0)
            Change_Text_Transparency_Up();
        else if (flag == 1)
            Change_Text_Transparency_Down();
        else if (flag == 2)
            StarWars();
        else
            return;
    }

    void StarWars()
    {
        flag = 3;
        StartCoroutine(FadeOut(target));
        return;
    }

    void Change_Text_Transparency_Up()
    {
        if (a > 1)
        {
            time += Time.deltaTime;
            if (time > 1f)
            {
                time = 0;
                flag = 1;
                a = 1;
            }
            text.color = new Color(0.6575857f, 0.6575857f, 1, 1);
            return;
        }

        text.color = new Color(0.6575857f, 0.6575857f, 1, a);
        a += 0.5f * Time.deltaTime;
    }

    void Change_Text_Transparency_Down()
    {
        if (a < 0)
        {
            time += Time.deltaTime;
            if (time > 1f)
            {
                flag = 2;
                time = 0;
                text.gameObject.SetActive(false);
            }
            return;
        }

        text.color = new Color(0.6575857f, 0.6575857f, 1, a);
        a -= 0.5f * Time.deltaTime;
    }

    IEnumerator FadeOut(Image target)
    {
        float val = 0;
        var sound = GameObject.Find("BGM").GetComponent<AudioSource>();

        while (val < 1)
        {
            sound.volume = 1 - val;
            target.color = new Color(0, 0, 0, val);
            val += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        target.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(0.5f);
        yield return SceneManager.LoadSceneAsync("initial");
    }
}