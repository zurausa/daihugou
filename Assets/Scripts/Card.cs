using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

	[SerializeField] private Text suitText;
	[SerializeField] private Text numberText;

	public CardData data {get; private set;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Initialize(CardData cardData){
		data = cardData;
		suitText.text = CardData.SUIT_TEXT_TABLE[data.suit];
		numberText.text = data.number.ToString();
	}
}
