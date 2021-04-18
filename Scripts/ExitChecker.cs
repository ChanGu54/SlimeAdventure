using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitChecker : MonoBehaviour
{
    public GameObject gameClearPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("player"))
        {
            gameClearPanel.SetActive(true);
        }
    }
}
