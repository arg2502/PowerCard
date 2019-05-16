using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCard : MonoBehaviour {
        
    Card card;
    private void Start() { card = GetComponentInParent<Card>(); }
    private void OnMouseOver() { card.OnHover(); }
    private void OnMouseExit() { card.OnExit(); }
    private void OnMouseUpAsButton() { card.OnSelect(); }
}
