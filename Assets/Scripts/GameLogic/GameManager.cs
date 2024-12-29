using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager currentLevelManager;

    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

   
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
