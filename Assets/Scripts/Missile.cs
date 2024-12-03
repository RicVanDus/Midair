using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float missileSpeed;


    private Rigidbody _rigidbody;

    private Vector3 _forceDir;

    [SerializeField] private GameObject _explosionEffect;
    
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _forceDir = new Vector3(0f, 1f, 0f) * missileSpeed;
    }

    void Update()
    {
        _rigidbody.AddForce(_forceDir);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BOOM");
    }
    
    
}
