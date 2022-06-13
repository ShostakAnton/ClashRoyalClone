using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class building : MonoBehaviour {
    public float life = 100f;
    float maxlife = 100f;

    public GameObject mybuilding;

    Transform lifebar;

    public List<Transform> enemy;

    public Transform target;

    public GameObject cannon;
    public Transform spawnpoint;

    float mytimer;

    GameObject c1;

    public int team;

    public GameObject a;

    void Start() {
        enemy = new List<Transform>();
    }

    void Update() {

        mytimer += Time.deltaTime;
        if ((mytimer >= 1f) &&
            (target != null) &&
            (transform.position.x + 50f > target.position.x) &&
            (transform.position.x + 50f > target.position.x) &&
            (transform.position.z - 50f < target.position.z) &&
            (transform.position.z + 50f > target.position.z)) {

            mytimer = 0;
            c1 = (GameObject)Instantiate(cannon, spawnpoint.position, Quaternion.identity);
            c1.GetComponent<cannon>().target = target;

        } else {
            AddAllEnemy();
            targetEnemy(); 
        }
    }

    public void gethit(float getdamage) {
        if (life > 0) {
            life -= getdamage;
            if (life <= 0) {
                Destroy(mybuilding);
                a.GetComponent<controlcenter>().refreshgrid = true;
                Destroy(gameObject);
            }
        }

        lifebar = transform.Find("health");
        lifebar.localScale = new Vector3(life/maxlife * 1f, 0.15f, 0.15f);
    }

    void AddAllEnemy() {
		enemy.Clear();

        string enemy_team_color = team == 1 ? "red" : "blue";
		
        GameObject[] go = GameObject.FindGameObjectsWithTag(enemy_team_color);

		foreach (GameObject enemy1 in go) {
			AddEnemy(enemy1.transform.parent.transform);
		}
	}

	void AddEnemy(Transform myenemy) {
		enemy.Add(myenemy);
	}

	void sortTargetByDistance() {
		enemy.Sort(delegate(Transform t1, Transform t2) {
			return Vector3.Distance(t1.position, transform.position).CompareTo(Vector3.Distance(t2.position, transform.position));
		});
	}

	void targetEnemy() {
		if (enemy.Count > 0) {
			sortTargetByDistance();
			target = enemy[0];
		}
	}
}
