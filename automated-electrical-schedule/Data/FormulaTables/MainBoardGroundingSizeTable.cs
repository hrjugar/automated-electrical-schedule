using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class MainBoardGroundingSizeTable
{
    public static Dictionary<ConductorMaterial, List<double>> CopperConductorGroundingSizeTable = new()
    {
        {
            ConductorMaterial.Copper,
            [
                8.0,
                8.0,
                8.0,
                8.0,
                8.0,
                8.0,
                8.0,

                14.0,
                14.0,

                22.0,
                22.0,

                30,
                30,
                30,
                30,

                50,
                50,
                50,

                60,
                60,
                60
            ]
        },
        {
            ConductorMaterial.Aluminum,
            [
                14.0,
                14.0,
                14.0,
                14.0,
                14.0,
                14.0,
                14.0,

                22.0,
                22.0,

                30,
                30,

                50,
                50,
                50,
                50,

                80,
                80,

                100,
                100,
                100
            ]
        }
    };

    public static Dictionary<ConductorMaterial, List<double>> AluminumConductorGroundingSizeTable = new()
    {
        {
            ConductorMaterial.Copper,
            [
                8.0,
                8.0,
                8.0,
                8.0,
                8.0,
                8.0,
                8.0,
                8.0,
                8.0,

                14.0,
                14.0,

                22.0,
                22.0,

                30,
                30,
                30,
                30,

                50,
                50,
                50,

                60
            ]
        },
        {
            ConductorMaterial.Aluminum,
            [
                14.0,
                14.0,
                14.0,
                14.0,
                14.0,
                14.0,
                14.0,
                14.0,
                14.0,

                22.0,
                22.0,

                30,
                30,

                50,
                50,
                50,
                50,

                80,
                80,

                100
            ]
        }
    };

    public static double GetGroundingSize(ConductorMaterial conductorMaterial, ConductorMaterial groundingMaterial,
        double conductorSize)
    {
        var table = conductorMaterial == ConductorMaterial.Copper
            ? CopperConductorGroundingSizeTable
            : AluminumConductorGroundingSizeTable;

        return DataConstants.ConductorSizes[
            table[groundingMaterial].FindIndex(size => size.IsRoughlyEqualTo(conductorSize))];
    }
}