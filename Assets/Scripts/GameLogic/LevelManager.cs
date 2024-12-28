using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // level specific vars
    public float gravityForce;
    public float gravityForceDefault = 100f;


    public int maxEnemyCount;
    private List<GameObject> _enemyList = new List<GameObject>();
    [SerializeField] private GameObject _targetCube;
    [SerializeField] private Transform _spawnBounds;

    //misc
    private WaitForSeconds _waitCheckEnemyList = new(3f);

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        
    }

    // Level initialization
    public void Initialize()
    {
        StartCoroutine("checkEnemyList");
    }

    private void SpawnEnemy()
    {
        // randomSpawn position through levelbounds
        Vector3 randomSpawnPos = Vector3.zero;
        float levelBoundsX = _spawnBounds.localScale.x / 2;
        float levelBoundsYmin = (_spawnBounds.localScale.y / 2 *-1) + _spawnBounds.transform.position.y;
        float levelBoundsYmax = (_spawnBounds.localScale.y / 2) + _spawnBounds.transform.position.y;
        float rndSpawnX = Random.Range(-(levelBoundsX), levelBoundsX);
        float rndSpawnY = Random.Range(levelBoundsYmin, levelBoundsYmax);
        randomSpawnPos.x = rndSpawnX;
        randomSpawnPos.y = rndSpawnY;

        var newEnemy = Instantiate(_targetCube, randomSpawnPos, Quaternion.identity);
        
        _enemyList.Add(newEnemy);
    }

    public void ThisDestroyedEnemy(EnemyBase nme)
    {
        // delete from list
        _enemyList.Remove(nme.gameObject);
    }

    private IEnumerator checkEnemyList()
    {
        // if enemylist.count < max enemy count then spawn one
        do
        {
            if (_enemyList.Count < maxEnemyCount)
            {
                SpawnEnemy();                
            }

            yield return _waitCheckEnemyList;
        } while (true);
    }
}
