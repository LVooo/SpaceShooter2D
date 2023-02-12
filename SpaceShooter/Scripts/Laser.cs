using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _Speed = 8.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Moves up laser
        transform.Translate(Vector3.up * _Speed * Time.deltaTime);

        // Remove the laser after leaving game screen
        if (transform.position.y > 5.39f) {
            Destroy(this.gameObject);
        }
    }
}
