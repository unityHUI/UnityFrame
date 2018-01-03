using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
public class DirJudge : MonoBehaviour {
    public Transform Npc;
	// Use this for initialization
	void Start () {
        //     JudgeDir();
        string hostName = Dns.GetHostName();
        IPAddress[] ips = Dns.GetHostEntry(hostName).AddressList;
        for (int i = 0; i < ips.Length; i++) {
            print("ip = " + ips[i]);
        }
        Debug.Log(IPAddress.Any);
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
