using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{

    private float _timer = 0f;
    private Rigidbody _rigidbody;
    
    public Vector3 forceDir;
    public float forceStrength;

    public CubeTypes cubeType;

    public GameObject cubeParticles;
    public GameObject crystal;

    private float _hitPoints = 100f;
   
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        SetCubeType(cubeType, true);
        

    }

    void Update()
    {
        
    }

    void Force(Vector3 forceDir, float forceStrength)
    {

        
        Vector3 completeForce;

        completeForce = forceDir * forceStrength;

        // we need some clamp to the force, because sometimes it shoots up like a madman
        completeForce.y = Mathf.Clamp(completeForce.y, -3000f, 9000f);

        if (_rigidbody != null)
            _rigidbody.AddForce(completeForce);

        if (transform.position.y > 1f)
        {
            TakeDamage(completeForce.magnitude);
        }

    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.CompareTag("Explosion"))
        {
            ExplosionHits(collider.GetComponent<Explosion>());
        }

    }

    private void ExplosionHits(Explosion incomingExplosion)
    {
        Vector3 distance = incomingExplosion.transform.position - transform.position;
        float yForce = 1.5f - distance.y;
        float xForce = distance.x / 2 * -1f;
        Vector3 distanceNew = new Vector3(xForce, yForce, 0f);
        float exploStrength = Mathf.Clamp(3f - distance.magnitude, 0f, 3f);

        exploStrength *= incomingExplosion.explosionSize;

        Force(distanceNew, exploStrength);
    }

    private void TakeDamage(float strength)
    {
        // the higher, the more damage you take. On the ground it's absorbed
        // check if it passes a treshhold and if so: spawn particles, initialize cubetype, etc.
        float damage = (strength / 1000f) * transform.position.y;

        _hitPoints -= damage;

        if (_hitPoints < 0)
        {
            Death();
        } 
        else
        {
            if (_hitPoints < 200f && cubeType == CubeTypes.Large)
            {
                cubeType = CubeTypes.Medium;
                SetCubeType(cubeType, false);
            } 
            else if (_hitPoints < 100f && cubeType == CubeTypes.Medium)
            {
                cubeType = CubeTypes.Small;
                SetCubeType(cubeType, false);
            }

            Debug.Log("OUCH, you hit for: " + strength + " hitpoints left: " + _hitPoints);
        }        

    }

    private void SetCubeType(CubeTypes cubeType, bool setHitpoints)
    {
        //set cubetype. change mass?

        switch (cubeType) 
        {
            case CubeTypes.Small:
                transform.localScale = Vector3.one * 0.7f;
                _rigidbody.mass = 10f;
                if (setHitpoints)
                {
                    _hitPoints = 100f;
                }
                break;
            case CubeTypes.Medium:
                transform.localScale = Vector3.one * 1.0f;
                _rigidbody.mass = 14f;
                if (setHitpoints)
                {
                    _hitPoints = 200f;
                }
                break;
            case CubeTypes.Large:
                transform.localScale = Vector3.one * 1.3f;
                _rigidbody.mass = 18f;
                if (setHitpoints)
                {
                    _hitPoints = 300f;
                }
                break;
        }
        
        //Spawn particles on sizechange
        if (!setHitpoints)
        {
            Instantiate(cubeParticles, transform.position, Quaternion.identity);
        }
    }

    private void Death()
    {
        // DEATH
        Instantiate(cubeParticles, transform.position, Quaternion.identity);

        for (int i = 0; i < 4; i++)
        {
            float randomFloat = Random.Range(0f, 360f);
            Vector3 randomRot = Vector3.one * randomFloat;
            
            var crystalObj = Instantiate(crystal, transform.position, Quaternion.Euler(randomRot));

            float xForce = Random.Range(-1f, 1f) * 1000f;
            crystalObj.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, 50f, 0f));
        }

        GameManager.Instance.currentLevelManager.ThisDestroyedEnemy(this);  
        
        Destroy(gameObject);
    }

}




public enum CubeTypes
{
    Small,
    Medium,
    Large
}