namespace MarvelSnap;

public class PlayerConfig
{
	private int _finalScore;
	private int _totalScore;
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
	
	public bool RemoveCard(Card card)
	{
		// card.SetIsPlayed(true);
		_cardDeck.Remove(card);
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
	
	public bool SetFinalScore(int finalScore)
	{
		_finalScore = finalScore;
		return true;
	}
	
	public bool AddFinalScore()
	{
		_finalScore += 1;
		return true;
	}
	
	public int GetFinalScore()
	{
		return _finalScore;
	}
	
	public bool SetTotalScore(int totalScore)
	{
		_totalScore = totalScore;
		return true;
	}
	
	public bool AddTotalScore(int score)
	{
		_totalScore += score;
		return true;
	}
	
	public int GetTotalScore()
	{
		return _totalScore;
	}
}