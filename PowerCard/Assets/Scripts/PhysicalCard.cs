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
        card.OnHover();
        
    }

    private void OnMouseExit()
    {
        //print("EXIT");
        card.OnExit();
        
    }

    private void OnMouseUpAsButton()
    {
        //print(card);
        card.OnSelect();
    }
}
