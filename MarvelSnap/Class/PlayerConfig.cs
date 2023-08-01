namespace MarvelSnap;

public class PlayerConfig
{
	private int _energyTotal;
	private List<Card> _cardDeck;
	
	public PlayerConfig()
	{
		_energyTotal = 0;
		_cardDeck = new List<Card>(); 
	}
	
	public List<Card> GetCardDeck()
	{
		return _cardDeck;
	}
	
	public bool AddCardDeck(Card card)
	{
		_cardDeck.Add(card);
		return true;
	}
	
	public int GetEnergyTotal()
	{
		return _energyTotal;
	}
	
	public bool SetEnergyTotal(int energyTotal)
	{
		this._energyTotal = energyTotal;
		return true;
	}
}