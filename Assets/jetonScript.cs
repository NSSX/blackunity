using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetonScript : MonoBehaviour {

    public int value;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)){
            // Whatever you want it to do.
            if (gameController.instance.playerCoin - value >= 0)
            {
                print("PAID : " + value);
                gameController.instance.addBet(value);
            }
        }
    }
}
