using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // level specific vars
    public float gravityForce;
    public float gravityForceDefault = 100f;


    private EnemyBase[] currentEnemies;
    public int maxEnemyCount;
    [SerializeField] private GameObject _targetCube;

    [SerializeField] private Transform _spawnBounds;

    //misc
    private WaitForSeconds _waitCheckEnemyList = new WaitForSeconds(2f);

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
        float levelBoundsY = _spawnBounds.localScale.y / 2 + _spawnBounds.transform.position.y;
        float rndSpawnX = Random.Range(-(levelBoundsX), levelBoundsX);
        float rndSpawnY = Random.Range(-(levelBoundsY), levelBoundsY);
        randomSpawnPos.x = rndSpawnX;
        randomSpawnPos.y = rndSpawnY;

        Instantiate(_targetCube, randomSpawnPos, Quaternion.identity);
    }

    private IEnumerator checkEnemyList()
    {
        // if enemylist.count < max enemy count then spawn one
        SpawnEnemy();

        yield return _waitCheckEnemyList;
    }
}
