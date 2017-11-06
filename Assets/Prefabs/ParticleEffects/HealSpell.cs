using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : MonoBehaviour {

    private int playerNo;


    // Use this for initialization
    void Start () {
		
	}

    public void setPlayerNo(int x)
    {
        playerNo = x;
    }

    // Update is called once per frame
    void Update () {

        //transform.SetParent(GetComponent<Player>().transform);
        Destroy(this.gameObject, 3);

    }

    
}
