using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelection : MonoBehaviour
{
    bool flag = false;
    public Sprite[] banner;
    Color yellowStar;
    Color blackStar;
    List<Image> stars;
    GameManager gm;
    int star;

    WorldMapInit wmi;

    private AudioSource audioSource;
    public Image fadeIn;

    // Start is called before the first frame update

    void OnEnable()
    {
        if (flag)
        {
            star = gm.data.stars[wmi.seeing];
            transform.Find("배너(이미지)").GetComponent<Image>().sprite = banner[wmi.seeing];

            for (int i = 0; i < star; i++)
            {
                stars[i].color = yellowStar;
            }
        }
        else
        {
            wmi = GameObject.Find("Scripts").GetComponent<WorldMapInit>();
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();

            yellowStar = new Color(1, 1, 0);
            blackStar = new Color(0.5f, 0.5f, 0.5f);

            stars = new List<Image>();

            for (int i = 1; i <= 3; i++)
                stars.Add(transform.Find("별/Star" + i).GetComponent<Image>());

            star = gm.data.stars[wmi.seeing];
            transform.Find("배너(이미지)").GetComponent<Image>().sprite = banner[wmi.seeing];

            audioSource = GameObject.Find("BGM").GetComponent<AudioSource>();

            for (int i = 0; i < star; i++)
            {
                stars[i].color = yellowStar;
            }
        }
    }

    public void OnStartBtnClicked()
    {
        StartCoroutine(StartStage());
    }

    public void OnCancelBtnClicked()
    {
        flag = true;

        for (int i = 0; i < 3; i++)
        {
            stars[i].color = blackStar;
        }

        transform.parent.gameObject.SetActive(false);
    }

    IEnumerator StartStage()
    {
        float i = 0;
        fadeIn.gameObject.SetActive(true);

        while (i < 1) {
            audioSource.volume = 1 - i;
            fadeIn.color = new Color(0, 0, 0, i);
            i += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        audioSource.volume = 0;
        fadeIn.color = new Color(0, 0, 0, 1);
        yield return SceneManager.LoadSceneAsync("Stage" + (wmi.seeing + 1).ToString());
    }
}
