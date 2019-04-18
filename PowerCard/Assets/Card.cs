using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public Image portraitImage;
    public Text nameText;

    [Header("Denigen")]
    public Text atkText;
    public Text shText;

    CardData data;
    CardData dataInstance;
    
    public void Init(CardData _data)
    {
        data = _data;
        dataInstance = Instantiate(data);
        
            
        dataInstance.Init();
        AssignUI();
        
    }

    void AssignUI()
    {
        portraitImage.sprite = data.image;
        nameText.text = data.name;
        
        if(data is DenigenData)
        {
            var denData = (DenigenData)dataInstance;
            atkText.text = denData.Atk.ToString();
            shText.text = denData.Sh.ToString();
        }
    }
}
