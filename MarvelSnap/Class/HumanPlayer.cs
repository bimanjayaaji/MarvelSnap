using MarvelSnapInterface;
namespace MarvelSnap;

public class HumanPlayer : IPlayer
{
	private string? _name = "";
	private int _id = 0;
	private static int _lastId = 0;
	private static List<int> _assignedId = new(); // List<> --> reusable in other game
	private static List<string?> _assignedName = new();
	
	public HumanPlayer(string? name)
	{
		SetName(name);
		SetId();
	}
	
	public int GetId()
	{
		return _id;
	}

	public string? GetName()
	{
		return _name;
	}

	private void SetId()
	{
		_id = _lastId + 1;
		_lastId = _id;
		_assignedId.Add(_id);
	}

	public bool SetId(int id)
	{
		if (id > 0 && !_assignedId.Contains(id)) 
		{
			_assignedId.Remove(this._id);
			this._id = id;
			_assignedId.Add(_id);
			return true;
		}
		return false;
	}

	public bool SetName(string? name)
	{
		if (name != null && !_assignedName.Contains(name)) 
		{
			if (this._name != "")
			{
				_assignedName.Remove(this._name);	
			}
			this._name = name;
			_assignedName.Add(this._name);
			return true;
		}
		return false;
	}
}