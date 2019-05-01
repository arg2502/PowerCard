using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCard : MonoBehaviour {
        
    Card card;

    private void Start()
    {
        card = GetComponentInParent<Card>();
    }

    private void OnMouseOver()
    {
        //print("OVER");
        GetComponent<MeshRenderer>().material.color = Color.yellow;
        card.CurrentCard = true;
    }

    private void OnMouseExit()
    {
        //print("EXIT");
        GetComponent<MeshRenderer>().material.color = Color.white;
        card.CurrentCard = false;
    }

    private void OnMouseUpAsButton()
    {
        print(card);
        card.OnSelect();
    }
}
