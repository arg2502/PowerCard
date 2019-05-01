﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    protected Player player;

    public Image portraitImage;
    public Text nameText;

    [Header("Denigen")]
    public Text atkText;
    public Text shText;
    public Image type1Image;
    public Image type2Image;
    public Image resist1Image;
    public Image resist2Image;
    public Image resist3Image;
    public Image resist4Image;
    public Image weakness1Image;
    public Image weakness2Image;
    public Image weakness3Image;
    public Image weakness4Image;
    public int Stars
    {
        get
        {
            var inst = data as DenigenData;
            return inst.Stars;
        }
    }

    public CardData data;
    CardData dataInstance;

    float yRotate = -2;

    public bool CurrentCard { get; set; }

    public enum Position { HAND, FIELD }
    public Position position;

    public void Init(CardData _data, Player _player)
    {
        data = _data;
        dataInstance = Instantiate(data);

        transform.rotation = Quaternion.Euler(0, yRotate, 0);
            
        dataInstance.Init();
        AssignUI();

        player = _player;
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
            type1Image.sprite = GetIcon(denData.Types[0]);
            if (denData.Types.Count > 1)
                type2Image.sprite = GetIcon(denData.Types[1]);
            else
                type2Image.gameObject.SetActive(false);

            resist1Image.gameObject.SetActive(false);
            resist2Image.gameObject.SetActive(false);
            resist3Image.gameObject.SetActive(false);
            resist4Image.gameObject.SetActive(false);
            weakness1Image.gameObject.SetActive(false);
            weakness2Image.gameObject.SetActive(false);
            weakness3Image.gameObject.SetActive(false);
            weakness4Image.gameObject.SetActive(false);

            if (denData.Resistances.Count > 0) { resist1Image.gameObject.SetActive(true); resist1Image.sprite = GetIcon(denData.Resistances[0]); }
            if (denData.Resistances.Count > 1) { resist2Image.gameObject.SetActive(true); resist2Image.sprite = GetIcon(denData.Resistances[1]); }
            if (denData.Resistances.Count > 2) { resist3Image.gameObject.SetActive(true); resist3Image.sprite = GetIcon(denData.Resistances[2]); }
            if (denData.Resistances.Count > 3) { resist4Image.gameObject.SetActive(true); resist4Image.sprite = GetIcon(denData.Resistances[3]); }
            if (denData.Weaknesses.Count > 0) { weakness1Image.gameObject.SetActive(true); weakness1Image.sprite = GetIcon(denData.Weaknesses[0]); }
            if (denData.Weaknesses.Count > 1) { weakness2Image.gameObject.SetActive(true); weakness2Image.sprite = GetIcon(denData.Weaknesses[1]); }
            if (denData.Weaknesses.Count > 2) { weakness3Image.gameObject.SetActive(true); weakness3Image.sprite = GetIcon(denData.Weaknesses[2]); }
            if (denData.Weaknesses.Count > 3) { weakness4Image.gameObject.SetActive(true); weakness4Image.sprite = GetIcon(denData.Weaknesses[3]); }
        }
    }

    Sprite GetIcon(DenigenData.Type type)
    {
        return GameControl.control.TypeIconsDatabase.GetTypeSprite(type);
    }

    /// <summary>
    /// Player selects the card in an attempt to summon
    /// </summary>
    public void OnSelect()
    {
        print("denigen onselect");
        
        if (position == Position.HAND)
        {
            if (data is DenigenData)
            {
                // check if there are enough tributes for this card to be summoned
                var denData = (DenigenData)dataInstance;
                if (!player.EnoughTributes(denData))
                    return;

                player.BeginSummon(this);
            }
        }
    }


}