using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityMVVM.Enums;


public class drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    float myx, myy, myz;
    float orix, oriy, oriz;

    public Camera mycamera;
    public Sprite c, d;
    public GameObject pre;
    public GameObject a;

    public LayerMask unwalkablemask;

    GameObject p1;
    GameObject[] p2;

    public int team;
    public int type;

    void Start() {
        orix = this.transform.position.x;
        oriy = this.transform.position.y;
        oriz = this.transform.position.z;

        p2 = new GameObject[4];

        a = GameObject.FindGameObjectWithTag("a");
    }

    void Update() {
        
    }

    public void OnDrag(PointerEventData eventdata) {
        Ray ray = mycamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit)) {}

        myx = hit.point.x;
        myy = 10f;
        myz = hit.point.z;

        this.transform.position = new Vector3(myx, myy, myz);

        this.GetComponent<Image>().sprite = d;
    }

    public void OnEndDrag(PointerEventData eventdata) {

        a.GetComponent<controlcenter>().newunit = true;

        if (!Physics.CheckSphere(this.transform.position, 3f, unwalkablemask)) {
            if (team==(int)TeamNumber.One && this.transform.position.z <= -8f) {
                establishCharacter(1, TeamTag.Blue, Color.blue);
            } else if (team==(int)TeamNumber.Two && this.transform.position.z >= 8f) {
                establishCharacter(2, TeamTag.Red, Color.red, true);
            }
        } 

        this.transform.position = new Vector3(orix, oriy, oriz);
        this.GetComponent<Image>().sprite = c;
    }

    delegate void EstablishTroop(int team_number, string team_tag, Color team_color, bool turn);
    EstablishTroop establishTroop;

    void establishCharacter(int team_number, string team_tag, Color team_color, bool turn=false) {   
        switch (type) {
            case (int)UnitType.Unit:
                establishTroop = establishUnit;
                break;
            case (int)UnitType.Skeleton:
                establishTroop = establishSkeleton;
                break;
            case (int)UnitType.Shooter:
                establishTroop = establishShooter;
                break;
        }

        establishTroop(team_number, team_tag, team_color, turn);
    }

    void establishUnit(int team_number, string team_tag, Color team_color, bool turn) {
        p1 = (GameObject)Instantiate(pre, new Vector3(myx, 27f, myz), Quaternion.identity);
        p1.GetComponent<Unit>().team = team_number;
        if (turn)
            p1.transform.rotation = Quaternion.Euler(0, 180f, 0);
        p1.GetComponent<Unit>().mytag.tag = team_tag;
        p1.GetComponentInChildren<SkinnedMeshRenderer>().material.color = team_color;
    }

    void establishSkeleton(int team_number, string team_tag, Color team_color, bool turn) {
        p2[0] = (GameObject)Instantiate(pre, new Vector3(myx, 10f, myz), Quaternion.identity);
        p2[1] = (GameObject)Instantiate(pre, new Vector3(myx+15f, 10f, myz), Quaternion.identity);
        p2[2] = (GameObject)Instantiate(pre, new Vector3(myx, 10f, myz+15f), Quaternion.identity);
        p2[3] = (GameObject)Instantiate(pre, new Vector3(myx+15f, 10f, myz+15f), Quaternion.identity);

        for (int i=0; i<4; i++) {
            p2[i].GetComponent<skeleton>().team = team_number;
            if (turn)
                p2[i].transform.rotation = Quaternion.Euler(0, 180f, 0);
            p2[i].GetComponent<skeleton>().mytag.tag = team_tag;
            p2[i].GetComponentInChildren<SkinnedMeshRenderer>().material.color = team_color;
        }
    }

    void establishShooter(int team_number, string team_tag, Color team_color, bool turn) {
        p1 = (GameObject)Instantiate(pre, new Vector3(myx, 20f, myz), Quaternion.identity);
        p1.GetComponent<shooter>().team = team_number;
        if (turn)
            p1.transform.rotation = Quaternion.Euler(0, 180f, 0);
        p1.GetComponent<shooter>().mytag.tag = team_tag;
        p1.GetComponentInChildren<SkinnedMeshRenderer>().material.color = team_color;
    }

    public void OnBeginDrag(PointerEventData eventdata) {
    }
}
