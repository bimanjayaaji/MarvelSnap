namespace MarvelSnap;

public class HumanPlayer : IPlayer
{
	private string? _name;
	private int _id;
	
	public int GetId()
	{
		return _id;
	}

	public string GetName()
	{
		return _name;
	}

	public bool SetId(int id)
	{
		if (id > 0) 
		{
			this._id = id;
			return true;
		}
		else 
		{
			return false;
		}
	}

	public bool SetName(string name)
	{
		if (name != null) 
		{
			this._name = name;
			return true;
		}
		else 
		{
			return false;
		}
	}
}