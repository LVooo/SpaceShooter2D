using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _Speed = 3.75f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _Speed * Time.deltaTime);
        if (transform.position.y < -6)
        {
            transform.position = new Vector3(Random.RandomRange(-8, 8), 6, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Animator _EnemyAnimation = GetComponent<Animator>();
            _EnemyAnimation.SetTrigger("OnEnemyDeathAnimationParameter");
            _Speed = 0;
            Player _PlayerScript = GameObject.Find("Player").GetComponent<Player>();
            _PlayerScript.PlayerDamage();
            Destroy(this.gameObject, 2.37f);
        }
        if (other.tag == "Laser")
        {
            Animator _EnemyAnimation = GetComponent<Animator>();
            _EnemyAnimation.SetTrigger("OnEnemyDeathAnimationParameter");
            _Speed = 0;
            UIManager _UIScript = GameObject.Find("Canvas").GetComponent<UIManager>();
            _UIScript.UpdateScore();
            Destroy(this.gameObject, 2.37f);
            Destroy(other.gameObject);
        }
    }
}
