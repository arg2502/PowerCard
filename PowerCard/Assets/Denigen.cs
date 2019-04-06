using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Denigen : Card {

    public DenigenData data;    

	// Use this for initialization
	void Start () {
        var dataInstance = Instantiate(data);
        dataInstance.Init();
        //instance.Print();
        portraitImage.sprite = data.image;
        nameText.text = data.name;
    }
	
}
