using MarvelSnapEnum;
using MarvelSnapInterface;
namespace MarvelSnap;

public abstract class Location : ILocation
{
	protected int _id;
	protected string? _name;
	protected LocationType _type;
	protected string? _description;
	
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