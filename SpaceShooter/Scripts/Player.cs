using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _Speed = 6f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _Firerate = .5f;
    private float _Canfire = -1;
    [SerializeField]
    private int _Lives = 3;
    [SerializeField]
    private GameObject _FireLeft, _FireRight;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float _HorizentalInput = Input.GetAxis("Horizontal");
        float _VerticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(_HorizentalInput, _VerticalInput, 0) * _Speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 3.84f), 0);

        if (transform.position.x < -9.7f)
        {
            transform.position = new Vector3(9.7f, transform.position.y, 0);
        }
        else if (transform.position.x > 9.7f)
        {
            transform.position = new Vector3(-9.7f, transform.position.y, 0);
        }
        // Firing laser
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _Canfire)
        {
            _Canfire = Time.time + _Firerate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }

    }
    public void PlayerDamage()
    {
        _Lives = _Lives - 1;
        UIManager _UIScrpit = GameObject.Find("Canvas").GetComponent<UIManager>();
        _UIScrpit.UpdateLives(_Lives);

        if (_Lives == 2)
        {
            _FireLeft.SetActive(true);
        }
        else if (_Lives == 1)
        {
            _FireRight.SetActive(true);
        }

        if (_Lives < 1)
        {
            Destroy(this.gameObject);
            SpawnManager _SpawnScript = GameObject.Find("Spawn manager").GetComponent<SpawnManager>();
            _SpawnScript.OnPlayerDead();
        }
    }

}
