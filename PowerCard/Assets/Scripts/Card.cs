using System.Collections;
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
            var inst = dataInstance as DenigenData;
            return inst.Stars;
        }
    }

    public CardData data;
    public CardData dataInstance;

    float yRotate = -2;

    public bool CurrentCard { get; set; }

    public enum Position { HAND, FIELD }
    public Position position;
    public enum Face { FACEUP, FACEDOWN }
    public Face face;

    Color hoverColor = Color.yellow;
    Color inactiveHoverColor = Color.gray;
    Color normalColor = Color.white;

    public void Init(CardData _data, Player _player)
    {
        data = _data;
        dataInstance = Instantiate(data);

        transform.rotation = Quaternion.Euler(0, yRotate, 0);
            
        dataInstance.Init();
        AssignUI();

        player = _player;
    }

    public void Summon(bool facedown)
    {
        position = Card.Position.FIELD;
        if (facedown)
        {
            face = Card.Face.FACEDOWN;
            transform.Rotate(Vector3.up, 180f);
        }
        else
            face = Card.Face.FACEUP;

        // temp
        transform.SetParent(player.fieldPos.transform);
        //transform.position += Vector3.up * 2;
        for(int i = 0; i < player.denCardSlots.Count; i++)
        {
            if(!player.denCardSlots[i].filled)
            {
                transform.position = player.denCardSlots[i].transform.position;
                player.denCardSlots[i].filled = true;
                break;
            }
        }
    }

    public void Discard()
    {
        data = null;
        dataInstance = null;
        GameControl.control.cardObjBank.Add(this.gameObject);
        this.gameObject.SetActive(false);
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
    
    public void OnSelect()
    {
        print("denigen onselect");
        
        if (position == Position.HAND)
        {
            player.SelectInHand(this);            
        }
        else if(position == Position.FIELD)
        {
            player.SelectOnField(this);
        }
    }

    public void OnHover()
    {
        if (data is DenigenData)
        {
            var denData = dataInstance as DenigenData;
            if (player.EnoughTributes(denData))
                GetComponentInChildren<MeshRenderer>().material.color = hoverColor;
            else
                GetComponentInChildren<MeshRenderer>().material.color = inactiveHoverColor;
        }
        else
            GetComponentInChildren<MeshRenderer>().material.color = hoverColor;
        CurrentCard = true;
    }

    public void OnExit()
    {
        GetComponentInChildren<MeshRenderer>().material.color = normalColor;
        CurrentCard = false;
    }


}
