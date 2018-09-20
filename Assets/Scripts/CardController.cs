using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour {

	private static readonly int PLAYER_NUM = 4;

	[SerializeField] private GameObject cardPrefab;
	[SerializeField] private GameObject[] playerFields;
	[SerializeField] private GameObject fieldObj;

	private List<Card>[] playersCardList;

	private CardData fieldCardData;
	private bool isEnemysTurn;
	private int passCount;
	private bool isGameEnd;

	void Awake() {
		playersCardList = new List<Card>[PLAYER_NUM];
		for (int i = 0; i < playersCardList.Length; ++i) {
			playersCardList [i] = new List<Card> ();
		}

		fieldCardData = null;
		isEnemysTurn = false;
		passCount = 0;
		isGameEnd = false;	
	}

	// Use this for initialization
	void Start () {
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			++passCount;
			Debug.Log ("Pass Player:" + passCount.ToString());
			PassCheck ();

			StartCoroutine (EnemyTurn ());
		}
	}

	void Initialize() {
		var allCardDataList = new List<CardData> ();

		for (int i = 0; i < CardData.TRUMP_CNT - 1; ++i) {
			var cardData = new CardData (i / CardData.MAX_NUM, i % CardData.MAX_NUM + 1);
			allCardDataList.Add (cardData);
		}
		allCardDataList.Add (new CardData (CardData.SUIT_JOKER, 0));

		// カードのリストをシャッフル
		for (int i = 0; i < allCardDataList.Count; ++i) {
			int index = Random.Range (0, allCardDataList.Count);
			var tmp = allCardDataList [i];
			allCardDataList [i] = allCardDataList [index];
			allCardDataList [index] = tmp;
		}

		for (int i = 0; i < PLAYER_NUM; ++i) {
			var list = allCardDataList.GetRange (i * CardData.MAX_NUM, CardData.MAX_NUM);
			if (i == 0) {
				list.Add (allCardDataList [allCardDataList.Count - 1]);
			}
			var sortedCardList = list.OrderBy (x => x.power);

			// カード生成
			var cardList = new List<Card>();
			foreach(var data in sortedCardList) {
				var obj = Instantiate (cardPrefab, playerFields [i].transform, false);
				var card = obj.GetComponent<Card> ();
				card.Initialize (data);
				cardList.Add (card);

				if (i == 0) {
					var button = obj.GetComponent<Button> ();
					button.onClick.AddListener (() => OnClickCard (card));
				}
			}
			playersCardList [i].AddRange (cardList);
		}

//		StartCoroutine (Test ());
	}

	void OnClickCard(Card card) {
		if (isGameEnd)
			return;

		if (isEnemysTurn)
			return;

		if (fieldCardData != null && fieldCardData.power >= card.data.power)
			return;

		passCount = 0;
		card.transform.SetParent (fieldObj.transform);
		card.transform.localPosition = Vector3.zero;
		fieldCardData = card.data;
		playersCardList [0].Remove (card);
		CheckVictory (0);

		StartCoroutine (EnemyTurn ());
	}

	IEnumerator EnemyTurn() {
		var waitForSeconds = new WaitForSeconds (1f);

		for (int i = 1; i <= 3; ++i) {
			yield return waitForSeconds;

			EnemyAiEasy (i);

			if (isGameEnd)
				yield break;
		}
	}

	void EnemyAiEasy(int index) {
		bool isHit = false;
		foreach(var card in playersCardList[index]) {
			if (fieldCardData == null || fieldCardData.power < card.data.power) {
				passCount = 0;
				card.transform.SetParent (fieldObj.transform);
				card.transform.localPosition = Vector3.zero;
				card.transform.localRotation = Quaternion.identity;
				fieldCardData = card.data;
				playersCardList [index].Remove (card);
				isHit = true;
				CheckVictory (index);
				break;
			}
		}

		if (!isHit) {
			++passCount;
			Debug.Log ("Pass Enemy" + index.ToString () + ":" + passCount.ToString());
			PassCheck ();
		}
	}

	void PassCheck() {
		if (passCount >= 3) {
			passCount = 0;
			fieldCardData = null;
			foreach(Transform childTransform in fieldObj.transform) {
				Destroy (childTransform.gameObject);
			}
		}
	}

	void CheckVictory(int index) {
		if (playersCardList[index].Count <= 0) {
			isGameEnd = true;
			Debug.Log (index.ToString () + " Victory!!!");
		}
	}

	IEnumerator Test() {
		int count = 1;
		while (count < CardData.TRUMP_CNT) {
			foreach (var cardList in playersCardList) {
				bool isHit = false;
				Card hitCard = null;

				foreach (var card in cardList) {
					if (card.data.suit * CardData.MAX_NUM + card.data.number == count) {
						isHit = true;
						hitCard = card;
						break;
					}
				}

				if (isHit) {
					++count;

					var color = Color.white;
					switch (hitCard.data.suit) {
						case 0:
							color = Color.blue;
							break;
						case 1:
							color = Color.green;
							break;
						case 2:
							color = Color.yellow;
							break;
						case 3:
							color = Color.red;
							break;
					}
					hitCard.gameObject.GetComponent<Image> ().color = color;
					break;
				}
			}

			yield return new WaitForSeconds (0.5f);
		}
	}
}
