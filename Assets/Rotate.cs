using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get Parent Position
        Vector3 parentPosition = transform.parent.transform.position;

        transform.RotateAround(parentPosition, Vector3.up, speed * Time.deltaTime);
    }
}
