using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData {
	public static readonly int TRUMP_CNT = 53;
	public static readonly int MAX_NUM = 13;
	
	public static readonly int SUIT_SPADE = 0;
	public static readonly int SUIT_CLUB = 1;
	public static readonly int SUIT_DIA = 2;
	public static readonly int SUIT_HEART = 3;
	public static readonly int SUIT_JOKER = 4;
	
	public static readonly Dictionary<int, string> SUIT_TEXT_TABLE = new Dictionary<int, string>() {
		{ SUIT_SPADE,	"スペード" },
		{ SUIT_CLUB,	"クラブ" },
		{ SUIT_DIA, "ダイヤ" },
		{ SUIT_HEART,	"ハート" },
		{ SUIT_JOKER,	"ジョーカー" },
	};

	public int suit;
	public int number;
	public int power;

	public CardData(int suit, int number){
		this.suit = suit;
		if(this.suit == SUIT_JOKER){
			this.number = 0;
			this.power = MAX_NUM + 3;
		}else{
			this.number = number;
			this.power = number;
			if(this.number <=2){
				this.power += MAX_NUM;
			}
		}
	}
}
