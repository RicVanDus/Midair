using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float missileSpeed;

    public float explodeAfterSec = 3f;
    private float _timer;

    private Rigidbody _rigidbody;

    public Vector3 forceDir;

    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private ParticleSystem _smokeTrail;
    
    private bool _bCountDownToDestruction;

    private Collider _collider;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _collider = GetComponent<Collider>();
    }

    void Update()
    {
        //_rigidbody.AddRelativeForce(_forceDir);
        
        // not accellerating, but always constant speed
        _rigidbody.linearVelocity = transform.forward * missileSpeed;

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
        _collider.enabled = false;
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        //hide missile mesh, destroy the whole thing after 3 seconds
        //stop particle system
        _smokeTrail.Stop();
        GetComponent<MeshRenderer>().enabled = false;
        _bCountDownToDestruction = true;
        
    }
    
}
