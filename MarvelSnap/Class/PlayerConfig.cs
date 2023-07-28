namespace MarvelSnap;

public class PlayerConfig
{
	private int _energyTotal;
	private List<Card>? _cardDeck;
	
	public PlayerConfig()
	{
		_cardDeck = new List<Card>(); 
		// need to be analysed more
	}
	
	public List<Card> GetCardDeck()
	{
		return _cardDeck;
	}
	
	public bool AddCardDeck(Card card)
	{
		return true;
		// need to be analysed more
	}
	
	public int GetEnergyTotal()
	{
		return _energyTotal;
	}
	
	public bool SetEnergyTotal(int energyTotal)
	{
		this._energyTotal = energyTotal;
		return true;
		// need to be analysed more
	}
}