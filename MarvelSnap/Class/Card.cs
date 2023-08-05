using MarvelSnapEnum;
using MarvelSnapInterface;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MarvelSnap;

[DataContract] public class Card : ICard, ICardAbility
{
	[DataMember] private int _id;
	[DataMember] private string? _name;
	[DataMember] private CardType _type;
	[DataMember] private CardApplyType _applyType;
	[DataMember] private int _energyCost;
	[DataMember] private int _attackingPower;
	[DataMember] private string? _description;
	private bool _placed;

	public Card(int id, string name, CardType type, CardApplyType applyType, int energyCost, int attackingPower)
	{
		_id = id;
		_name = name;
		_type = type;
		_applyType = applyType;
		_energyCost = energyCost;
		_attackingPower = attackingPower;
		_placed = false;
		//_description = description;	
	}
	
	public int GetId()
	{
		return _id;
	}
	
	public string? GetName()
	{
		return _name;
	}
	
	public CardType GetSkill()
	{
		return _type;
	}
	
	public CardApplyType GetApplyType()
	{
		return _applyType;
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
		_attackingPower = power;
		return true;
	}	
	
	public bool IsPlaced()
	{
		return _placed;
	}
	
	public bool SetIsPlaced(bool state)
	{
		_placed = state;
		return true;
	}
}