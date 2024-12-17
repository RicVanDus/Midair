using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public float destroyMeAfterThisTime;

    private float _timer;

    void Start()
    {
        
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > destroyMeAfterThisTime)
            Destroy(gameObject);
    }
}
