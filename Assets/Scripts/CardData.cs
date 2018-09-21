using System.Collections.Generic;

public class CardData {
	public static readonly int TRUMP_COUNT	= 53;
	public static readonly int MAX_NUMBER	= 13;

	public static readonly int SUIT_SPADE	= 0;
	public static readonly int SUIT_CLUB	= 1;
	public static readonly int SUIT_DIAMOND	= 2;
	public static readonly int SUIT_HEART	= 3;
	public static readonly int SUIT_JOKER	= 4;

	public static readonly Dictionary<int, string> SUIT_TEXT_TABLE = new Dictionary<int, string>() {
		{ SUIT_SPADE,	"スペード" },
		{ SUIT_CLUB,	"クラブ" },
		{ SUIT_DIAMOND, "ダイヤ" },
		{ SUIT_HEART,	"ハート" },
		{ SUIT_JOKER,	"ジョーカー" },
	};

	public int suit;
	public int number;
	public int power;

	public CardData(int suit, int number) {
		this.suit = suit;
		this.number = number;
		this.power = this.number;
		if (this.number <= 2) {
			this.power += MAX_NUMBER;
			if (this.suit == SUIT_JOKER) {
				this.power += 3;
			}
		}
	}
}