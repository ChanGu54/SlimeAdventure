using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnEventManager : MonoBehaviour
{
    public int stageNum;
    int[] data;
    SaveData savefile;
    Image fadeOut;
    public AudioSource clickSound;

    private void Start()
    {
        fadeOut = transform.Find("FadeOut").GetComponent<Image>();
        data = new int[stageNum];
        data = Enumerable.Repeat(1, stageNum).ToArray();
        Debug.Log(data);
        savefile = DataManager.Load();
    }

    public void Initial_Quit_Yes() 
    {
        Application.Quit();
    }

    public void Initial_Quit_No()
    {
        clickSound.Play();
        transform.Find("ExitPanel").gameObject.SetActive(false);
    }

    public void Initial_Quit()
    {
        clickSound.Play();
        transform.Find("ExitPanel").gameObject.SetActive(true);
    }

    public void Initial_Startgame()
    {
        clickSound.Play();
        if (savefile == null)
        {
            savefile = new SaveData(data, 0);
            DataManager.Save(savefile);
        }

        StartCoroutine(SceneInitializer());
    }

    IEnumerator SceneInitializer()
    {
        float val = 0;
        fadeOut.gameObject.SetActive(true);
        while (val < 1)
        {
            fadeOut.color = new Color(0, 0, 0, val);
            val += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        fadeOut.color = new Color(0, 0, 0, 1);
        
        yield return new WaitForSeconds(0.3f);
        if (savefile.lastPlayedStage == 0)
            SceneManager.LoadScene("스토리설명");
        else
            yield return SceneManager.LoadSceneAsync("월드맵");
    }
}
