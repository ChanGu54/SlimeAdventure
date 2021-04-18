using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    Color target_color;
    Color color_to_change;

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Collision Detected!");
        if (col.gameObject.tag.Equals("Paint"))
        {
            Debug.Log("Item Detected!");
            StartCoroutine(Color_Changing(col.GetComponent<Renderer>().material.color, 0.01f));
        }
    }

    IEnumerator Color_Changing(Color color_to_change, float time)
    {
        target_color = gameObject.GetComponentInChildren<Renderer>().material.color;
        float temp = 0;

        while (temp < 1)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.Lerp(target_color, color_to_change, temp);
            temp += 0.05f;
            yield return new WaitForSeconds(time);
        }
        gameObject.GetComponentInChildren<Renderer>().material.color = color_to_change;
    }
}
