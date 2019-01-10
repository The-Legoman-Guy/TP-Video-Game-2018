using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Shoot = KeyCode.Space;

    // Use this for initialization
    void Start () {
		
	}

    void ModifyControls(Controls tmp)
    {
        Up = tmp.Up;
        Down = tmp.Down;
        Left = tmp.Left;
        Right = tmp.Right;
        Shoot = tmp.Shoot;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
