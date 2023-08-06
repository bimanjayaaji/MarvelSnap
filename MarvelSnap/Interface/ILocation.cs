using MarvelSnapEnum;
namespace MarvelSnapInterface;

public interface ILocation
{
	int GetId();
	string? GetName();
	LocationType GetSkill();
	string? GetDesc();
}