using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannon : MonoBehaviour {
    
    public Transform target;

    void Start() {
        
    }

    void Update() {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);    
        else
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.GetComponent<Unit>()) {
            col.gameObject.GetComponent<Unit>().gethit(5f);
            Destroy(gameObject);
        }
        if (col.gameObject.GetComponent<skeleton>()) {
            col.gameObject.GetComponent<skeleton>().gethit(5f);
            Destroy(gameObject);
        }
        if (col.gameObject.GetComponent<shooter>()) {
            col.gameObject.GetComponent<shooter>().gethit(5f);
            Destroy(gameObject);
        }
        if (col.gameObject.GetComponent<building>()) {
            col.gameObject.GetComponent<building>().gethit(5f);
            Destroy(gameObject);
        }
    }
}
