using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class skeleton  : MonoBehaviour {
	public Transform target;
	public float speed = 1;
	public float rotationSpeed=1;
	Vector3[] path;
	int targetIndex;
	float timer;
	Vector3 currenttarget;

	bool reachemptytarget=false;

	Animator anim;

	public List<Transform> enemy;

	public int team;

	public Transform mytag;

	public float life = 10f;
    float maxlife = 10f;

	Transform lifebar;

	public GameObject a;

	void Start() {

		anim = GetComponentInChildren<Animator>();

		a = GameObject.FindGameObjectWithTag("a");

		currenttarget = new Vector3 (0, 0, 0);
		//PathRequestManager.RequestPath(transform.position,target.position, OnPathFound);

		enemy = new List<Transform>();
		AddAllEnemy();
		targetEnemy();
	}

	void Update() {
		timer += Time.deltaTime;

		if (timer > 1f) {
			timer = 0;

			if (target != null) {
				if (anim.GetBool("attack") == true) {
                    if (target.GetComponent<building>())
					    target.GetComponent<building>().gethit(10f);
                    if (target.GetComponent<Unit>())
					    target.GetComponent<Unit>().gethit(10f);
                    if (target.GetComponent<skeleton>())
					    target.GetComponent<skeleton>().gethit(10f);
				} else {
					if (a.GetComponent<controlcenter>().newunit == true) {
						AddAllEnemy();
						targetEnemy();
					}
				}

				//if the target position have change already or you have reach a position where target is still far
				if ((target.position != currenttarget)||(reachemptytarget==true))
				{
					reachemptytarget = false;
					Debug.Log ("I Change path now at " + target.position);
					currenttarget = target.position;
					PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
				}

			} else {
				anim.SetBool("attack", false); 
				AddAllEnemy();
				targetEnemy();
			}
		}

	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath() {

		targetIndex = 0;

		Vector3 currentWaypoint = path[0];
		while (true) {

			if ((transform.position.x - 10f < target.position.x) &&
				(transform.position.x + 10f > target.position.x) &&
				(transform.position.z - 10f < target.position.z) &&
				(transform.position.z + 10f > target.position.z)
				) {
					anim.SetBool("attack", true); 
				}


			//if you reach a waypoint
			if (transform.position == currentWaypoint) 
			{
				//go to the next waypoint
				targetIndex ++;



				//if the next waypoint is bigger than the path length, reset the path to 0
				if (targetIndex >= path.Length) 
				{
					//if target is very far, mean you need to update the target and refind the path
					if ((transform.position.x + 2.0f < target.position.x)|| 
						(transform.position.x - 2.0f > target.position.x)|| 
						(transform.position.z + 2.0f < target.position.z)|| 
						(transform.position.z - 2.0f > target.position.z)
						)
					{
						Debug.Log ("reach my last empty waypoint");
						reachemptytarget = true;
					}

					targetIndex = 0;
					path = new Vector3[0];
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);



			Vector3 targetWaypointDirection = currentWaypoint - transform.position;
			if (targetWaypointDirection != Vector3.zero)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetWaypointDirection, Vector3.up), rotationSpeed * Time.fixedDeltaTime);
			}

			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}

	void AddAllEnemy() {
		enemy.Clear();

		string enemy_building_tag = team == 1 ? "red" : "blue";
		GameObject[] go = GameObject.FindGameObjectsWithTag(enemy_building_tag);

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

	public void gethit(float getdamage) {
        if (life > 0) {
            life -= getdamage;
            if (life <= 0) { 
                Destroy(gameObject);
            }
        }

        lifebar = transform.Find("health");
        lifebar.localScale = new Vector3(life/maxlife * 2f, 0.3f, 0.3f);
    }
}