using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearPanel : MonoBehaviour
{
    Color yellowStar;
    List<Image> stars;
    GameManager gm;
    int star;
    public AudioClip clearAudio;
    public AudioClip deathAudio;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        yellowStar = new Color(1, 1, 0);

        stars = new List<Image>();

        for (int i = 1; i <= 3; i++)
            stars.Add(transform.Find("별/Star" + i).GetComponent<Image>());

        for (int i = 0; i < 2; i++)
            stars[i + 1].transform.Find("Text").GetComponent<Text>().text = gm.starCondition[i].ToString() + stars[i + 1].transform.Find("Text").GetComponent<Text>().text;
        
        star = gm.CheckStar();
        audioSource = GetComponent<AudioSource>();

        GameObject.Find("Humans").SetActive(false);

        if (star == 0)
        {
            audioSource.clip = deathAudio;
            audioSource.Play();
            transform.Find("배너(이미지)").gameObject.SetActive(false);
            return;
        }

        audioSource.clip = clearAudio;
        audioSource.Play();

        for (int i = 0; i < star; i++)
        {
            stars[i].color = yellowStar;
        }

        if(star > gm.data.stars[gm.stagePlaying])
        {
            gm.data.stars[gm.stagePlaying] = star;
        }
        gm.data.lastPlayedStage = gm.stagePlaying;
    }

    public void OnConfirmBtnClicked()
    {
        DataManager.Save(gm.data);
        SceneManager.LoadScene("월드맵");
    }

    public void OnRestartBtnClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
