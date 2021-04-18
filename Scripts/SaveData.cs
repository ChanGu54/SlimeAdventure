using System;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [SerializeField] public int[] stars;
    [SerializeField] public int lastPlayedStage;

    //생성자
    public SaveData(int[] stars, int lastPlayedStage)
    {
        this.stars = new int[stars.Length];
        Array.Copy(this.stars, stars, stars.Length);
        this.lastPlayedStage = lastPlayedStage;
    }
}
