using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlcenter : MonoBehaviour {

    public bool refreshgrid;
    public bool newunit;

    float mytimer;

    int repeat;
    int repeat2;

    void Start() {
        
    }

    void Update() {
        mytimer += Time.deltaTime;

        if (mytimer >= 1f) {
            mytimer = 0;

            if (refreshgrid == true) {
                repeat += 1;
                if (repeat >= 2) {
                    repeat = 0;
                    GetComponent<Grid>().RefreshGrid();
                }
            }

            if (newunit == true) {
                repeat2 += 1;
                if (repeat2 >= 2) {
                    repeat2 = 0;
                    newunit = false;
                }
            }
        }
    }
}
