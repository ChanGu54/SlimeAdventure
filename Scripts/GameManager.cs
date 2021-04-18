using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool clocked;
    public SaveData data;
    public int stagePlaying;
    public int[] starCondition;
    float time;
    public bool isGameOver;

    // Start is called before the first frame update
    void Awake()
    {
        clocked = false;
        isGameOver = false;
        data = DataManager.Load();
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    public int CheckStar()
    {
        if(isGameOver)
            return 0;
        if(starCondition[0] > time)
        {
            if (starCondition[1] > time)
                return 3;
            return 2;
        }
        return 1;
    }

    private void OnApplicationQuit()
    {
        DataManager.Save(data);
    }
}