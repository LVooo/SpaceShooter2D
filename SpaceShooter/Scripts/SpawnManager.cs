using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _Enemies;
    private bool _StopSpawing = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn_enemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn_enemies()
    {
        while(_StopSpawing == false)
        {
            Instantiate(_Enemies, new Vector3(Random.Range(-8, 8), 6, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public void OnPlayerDead()
    {
        _StopSpawing = true;
    }
}
