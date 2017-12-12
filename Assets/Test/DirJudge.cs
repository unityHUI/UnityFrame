using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirJudge : MonoBehaviour {
    public Transform Npc;
	// Use this for initialization
	void Start () {
        JudgeDir();
	}

    public void JudgeDir() {
        Vector3 dir = Npc.position - transform.position;
        if (Vector3.Dot(dir, transform.forward) > 0 && Vector3.Dot(dir, transform.right) > 0)
        {
            Debug.Log("Right");
        }
        else if (Vector3.Dot(dir, transform.forward) > 0 && Vector3.Dot(dir, -transform.right) > 0)
        {
            Debug.Log("Left");
        }
        else {
            Debug.Log("Back");
            print("print YGH");
          
        }
    }



}
