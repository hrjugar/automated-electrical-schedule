namespace automated_electrical_schedule.Data.FormulaTables;

public static class CableTrayRacewaySizeTable
{
    public static readonly Dictionary<int, int> cableTrayRacewayDict = new()
    {
        { 1400, 50 },
        { 2800, 100 },
        { 4200, 150 },
        { 5600, 200 },
        { 6100, 225 },
        { 8400, 300 },
        { 11200, 400 },
        { 12600, 450 },
        { 14000, 500 },
        { 16800, 600 },
        { 21000, 750 },
        { 25200, 900 }
    };

    public static int GetCableTrayRacewaySize(
        int setCount,
        int conductorWireCount,
        double conductorWireSize,
        int groundingWireCount,
        double groundingWireSize)
    {
        var allowableFillArea =
            setCount * (conductorWireCount * conductorWireSize + groundingWireCount * groundingWireSize);

        var closestMaxAllowableFillArea = cableTrayRacewayDict.Keys.First(fillArea => fillArea >= allowableFillArea);
        return cableTrayRacewayDict[closestMaxAllowableFillArea];
    }
}