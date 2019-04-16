using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public Image portraitImage;
    public Text nameText;

    CardData data;
    
    public void Init(CardData _data)
    {
        data = _data;
        var dataInstance = Instantiate(data);
        dataInstance.Init();
        portraitImage.sprite = data.image;
        nameText.text = data.name;
    }
}
