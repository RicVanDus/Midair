using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

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

    }

    void Force(Vector3 forceDir, float forceStrength)
    {
        Vector3 completeForce;

        completeForce = forceDir * forceStrength;
        
        if (_rigidbody != null)
            _rigidbody.AddForce(completeForce);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Vector3 distance = collider.transform.position - transform.position;
        float yForce = 1f - distance.y;
        float xForce = distance.x / 2 * -1f;
        Vector3 distanceNew = new Vector3(xForce, yForce, 0f);
        float exploStrength = Mathf.Clamp(3f - distance.magnitude, 0f, 3f);

        exploStrength *= 3000f;
        
        Force(distanceNew, exploStrength);
    }

}




public enum CubeTypes
{
    Small,
    Medium,
    Large
}