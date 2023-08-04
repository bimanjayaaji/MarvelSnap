using MarvelSnapEnum;
using MarvelSnapInterface;
using System.Runtime.Serialization; // added
namespace MarvelSnap;

[DataContract] public class Location : ILocation
{
	[DataMember] private int _id;
	[DataMember] private string? _name;
	[DataMember] private LocationType _type;
	[DataMember] private string? _description;
	
	public Location(int id, string name, LocationType type) 
	{
		_id = id;
		_name = name;
		_type = type;
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

	LocationType ILocation.GetType()
	{
		return _type;
	}
}