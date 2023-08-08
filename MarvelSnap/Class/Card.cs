using MarvelSnapEnum;
using MarvelSnapInterface;
using System.Runtime.Serialization;

namespace MarvelSnap;

/// <summary>
/// Represents a character card. Has some attributes associated with the character
/// and some methods to access and set the value of the attributes. Using DataContract
/// so that it can be used to generate all cards from .json file in GameRunner.
/// </summary>
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
	private bool _performed;
	private bool _locEffect;

	/// <summary>
	/// Constructor is used to set all the initial values of the card instance.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="name"></param>
	/// <param name="type"></param>
	/// <param name="applyType"></param>
	/// <param name="energyCost"></param>
	/// <param name="attackingPower"></param>
	public Card(int id, string name, CardType type, CardApplyType applyType, int energyCost, int attackingPower)
	{
		_id = id;
		_name = name;
		_type = type;
		_applyType = applyType;
		_energyCost = energyCost;
		_attackingPower = attackingPower;
		_placed = false;
		_performed = false;
		_locEffect = false;
	}
	
	public int GetId()
	{
		return _id;
	}
	
	public string? GetName()
	{
		return _name;
	}
	
	/// <summary>
	/// CardType defines the skill type that the character/card has.
	/// Refer to the CardType.cs Enum file.
	/// </summary>
	/// <returns></returns>
	public CardType GetSkill()
	{
		return _type;
	}
	
	/// <summary>
	/// CardApplyType defines when the effect of the card will affect the score.
	/// Consists of Normal, OnGoing, and OnReveal. Refer to the CardApplyType.cs Enum file.
	/// </summary>
	/// <returns></returns>
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
	
	/// <summary>
	/// Returns true if the card has already been placed on the board.
	/// </summary>
	/// <returns></returns>
	public bool IsPlayed()
	{
		return _placed;
	}
	
	public bool SetIsPlayed(bool state)
	{
		_placed = state;
		return true;
	}
	
	/// <summary>
	/// Returns true when the skill of the card has already been performed.
	/// This method would look obvious to differ OnReveal and OnGoing card type.
	/// </summary>
	/// <returns></returns>
	public bool IsPerformed()
	{
		return _performed;
	}
	
	public bool SetIsPerformed(bool state)
	{
		_performed = state;
		return true;
	}
	
	/// <summary>
	/// Returns true when the card has already been affected
	/// by the location's skill
	/// </summary>
	/// <returns></returns>
	public bool IsLocEffect()
	{
		return _locEffect;
	}
	
	public bool SetIsLocEffect(bool state)
	{
		_locEffect = state;
		return true;
	}
}