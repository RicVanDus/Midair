using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float missileSpeed;

    public float explodeAfterSec = 3f;
    private float _timer;

    private Rigidbody _rigidbody;

    private Vector3 _forceDir;

    [SerializeField] private GameObject _explosionEffect;

    private bool _bCountDownToDestruction;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _forceDir = new Vector3(0f, 1f, 0f) * missileSpeed;
    }

    void Update()
    {
        _rigidbody.AddRelativeForce(_forceDir);

        if (_bCountDownToDestruction)
        {
            _timer += Time.deltaTime;
            if (_timer > explodeAfterSec)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
    
    private void Explode()
    {
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        //hide missile mesh, destroy the whole thing after 3 seconds
        //stop particle system

        GetComponent<MeshRenderer>().enabled = false;
        _bCountDownToDestruction = true;
        
    }
    
}
