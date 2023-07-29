using MarvelSnapEnum;
using MarvelSnapInterface;
namespace MarvelSnap;

public abstract class Card : ICard, ICardAbility
{
	protected int _id;
	protected string? _name;
	protected CardType _type;
	protected int _energyCost;
	protected int _attackingPower;
	protected string? _description;
	
	public int GetId()
	{
		return _id;
	}
	
	public string? GetName()
	{
		return _name;
	}
	
	CardType ICardAbility.GetType()
	{
		return _type;
	}
	
	public string? GetDesc()
	{
		return _description;
	}
	
	public int GetEnergyCost()
	{
		return _energyCost;
	}
	
	public int GetAttackingPower()
	{
		return _attackingPower;
	}

	public bool SetEnergyCost(int energy)
	{
		throw new NotImplementedException();
	}

	public bool SetAttackingPower(int power)
	{
		throw new NotImplementedException();
	}	
}