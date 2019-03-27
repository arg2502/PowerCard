using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Denigen : MonoBehaviour {

    public DenigenData data;

	// Use this for initialization
	void Start () {
        var instance = Instantiate(data);
        instance.Init();
        instance.Print();
    }
	
}
