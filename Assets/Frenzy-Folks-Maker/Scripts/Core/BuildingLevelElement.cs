public class BuildingLevelElement : MovableLevelElement
{
    protected override void Awake()
    {
        isBuilding = true;
        base.Awake();
    }
}
