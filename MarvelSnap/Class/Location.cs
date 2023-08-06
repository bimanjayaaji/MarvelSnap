using MarvelSnapEnum;
using MarvelSnapInterface;
using System.Runtime.Serialization; // added
namespace MarvelSnap;

[DataContract] public class Location : ILocation
{
	[DataMember] private int _id;
	[DataMember] private string? _name;
	[DataMember] private LocationType _type;
	[DataMember] private LocApplyType _applyType;
	[DataMember] private string? _description;
	private bool _revealed;
	
	public Location(int id, string name, LocationType type, LocApplyType applyType) 
	{
		_id = id;
		_name = name;
		_type = type;
		_applyType = applyType;
		_revealed = false;
	}
	
	public string? GetDesc()
	{
		return _description;
	}

	public int GetId()
	{
		return _id;
	}

	public string? GetName()
	{
		return _name;
	}

	public LocationType GetSkill()
	{
		return _type;
	}
	
	public LocApplyType GetApplyType()
	{
		return _applyType;
	}
	
	public bool IsRevealed()
	{
		return _revealed;
	}
	
	public bool SetIsRevealed(bool value)
	{
		_revealed = value;
		return true;
	}
}