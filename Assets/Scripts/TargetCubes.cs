using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCubes : MonoBehaviour
{

    private float _timer = 0f;
    private Rigidbody _rigidbody;
    
    public Vector3 forceDir;
    public float forceStrength;

    public CubeTypes cubeType;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 3f)
        {
            Force(forceDir, forceStrength);
            _timer = 0f;
        }
    }

    void Force(Vector3 forceDir, float forceStrength)
    {
        Vector3 completeForce;

        completeForce = forceDir * forceStrength;
        
        if (_rigidbody != null)
            _rigidbody.AddForce(completeForce);
    }
    

}




public enum CubeTypes
{
    Small,
    Medium,
    Large
}