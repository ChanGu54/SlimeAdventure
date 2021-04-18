using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1UI : MonoBehaviour
{
    public Image fadeIn;
    public GameObject tutorialPanel;
    public GameObject joyStick;
    //public string[] tutorial_str;

    int tutorial_Pagenum;
    List<Transform> tutorial_Page;
    int tutorial_curPage;

    public int status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;

            if(_status == 0)
            {
                StartCoroutine(FadeIn(fadeIn));
            }
            else if(_status == 1)
            {
                fadeIn.gameObject.SetActive(false);
                tutorialPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    int _status;

    private void Start()
    {
        tutorial_Page = new List<Transform>();

        for (int i = 1; ; i++){
            var tr = tutorialPanel.transform.Find("하위모듈/" + i.ToString() + "번 페이지");
            Debug.Log(tr);
            if (tr == null)
                break;
            tutorial_Page.Add(tr);
        }

        tutorial_Pagenum = tutorial_Page.Count;
        tutorial_curPage = 0;
        tutorial_Page[tutorial_curPage].gameObject.SetActive(true);

        status = 0;
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

        status = 1;
        yield return null;
    }

    public void TutorialUI_Confirm()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void TutorialUI_Next()
    {
        tutorial_Page[tutorial_curPage].gameObject.SetActive(false);
        tutorial_curPage = ++tutorial_curPage % tutorial_Pagenum;
        tutorial_Page[tutorial_curPage].gameObject.SetActive(true);

        //StartCoroutine(PrintText());
    }

    public void TutorialUI_Previous()
    {
        tutorial_Page[tutorial_curPage].gameObject.SetActive(false);
        if (tutorial_curPage != 0)
            tutorial_curPage = --tutorial_curPage;
        else
            tutorial_curPage = tutorial_Pagenum - 1;
        tutorial_Page[tutorial_curPage].gameObject.SetActive(true);

        //StartCoroutine(PrintText());
    }

    //IEnumerator PrintText()
    //{
    //    var text = tutorial_Page[tutorial_curPage].transform.Find("Text").GetComponent<Text>();
    //    var length = tutorial_str[tutorial_curPage].Length;
    //    var str = "";

    //    for(int i = 0; i < length; i++)
    //    {
    //        str += tutorial_str[tutorial_curPage][i];
    //        text.text = str;
    //        yield return new WaitForSecondsRealtime(0.04f);
    //    }

    //    yield return null;
    //}
}
