using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gm;
    bool isclockBtnclicked;
    bool flag = false;
    public GameObject pausePanel;
    public Image fadeIn;
    public Text timeText;
    private float remainTime;

    public Material normalMat;
    public Material clockedMat;
    private Renderer slime;

    void Start()
    {
        slime = GameObject.Find("Slime/Slime").GetComponent<Renderer>();
        slime.material = normalMat;
        remainTime = 0;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (SceneManager.GetActiveScene().name != "Stage1")
        {
            StartCoroutine(FadeIn(fadeIn));
        }
    }

    void Update()
    {
        if (isclockBtnclicked)
        {
            if (float.Parse(timeText.text, CultureInfo.InvariantCulture) > 0)
            {
                gm.clocked = true;
                remainTime = float.Parse(timeText.text, CultureInfo.InvariantCulture) - Time.deltaTime;
                timeText.text = (Mathf.Round(remainTime * 100) / 100).ToString();
                if (!flag)
                {
                    flag = !flag;
                    slime.material = clockedMat;
                }
            }
            else
            {
                gm.clocked = false;
                isclockBtnclicked = false;
                if (flag)
                {
                    flag = !flag;
                    slime.material = normalMat;
                    gm.clocked = false;
                }
            }
        }
        else
        {
            if (flag)
            {
                flag = !flag;
                slime.material = normalMat;
                gm.clocked = false;
            }
        }
    }

    public void OnPauseBtnClicked()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }


    public void OnCancelBtnClicked()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoToMainMenuBtnClicked()
    {
        gm.data.lastPlayedStage = gm.stagePlaying;
        DataManager.Save(gm.data);
        Time.timeScale = 1;
        SceneManager.LoadScene("월드맵");
    }

    public void OnClockBtnClicked()
    {
        isclockBtnclicked = !isclockBtnclicked;
    }

    IEnumerator FadeIn(Image target)
    {
        float val = 1;

        while (val > 0)
        {
            target.color = new Color(0, 0, 0, val);
            val -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        target.color = new Color(0, 0, 0, 0);
        target.gameObject.SetActive(false);
        yield return null;
    }
}
