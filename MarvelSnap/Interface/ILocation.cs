using MarvelSnapEnum;
namespace MarvelSnapInterface;

public interface ILocation
{
	int GetId();
	string? GetName();
	LocationType GetSkill();
	string? GetDesc();
	LocApplyType GetApplyType(); 
    bool IsRevealed();
    bool SetIsRevealed(bool state);
}