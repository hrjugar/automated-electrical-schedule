using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class MainBoardGroundingSizeTable
{
    private static Dictionary<ConductorMaterial, List<double>> CopperConductorGroundingSizeTable = new()
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

    private static Dictionary<ConductorMaterial, List<double>> AluminumConductorGroundingSizeTable = new()
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

    public static CalculationResult<double> GetGroundingSize(ConductorMaterial conductorMaterial,
        ConductorMaterial groundingMaterial,
        CalculationResult<double> conductorSize)
    {
        if (conductorSize.HasError) return CalculationResult<double>.Failure(conductorSize.ErrorType);

        var table = conductorMaterial == ConductorMaterial.Copper
            ? CopperConductorGroundingSizeTable
            : AluminumConductorGroundingSizeTable;

        var index = DataConstants.ConductorSizes.FindIndex(size => size.IsRoughlyEqualTo(conductorSize.Value));
        return index == -1
            ? CalculationResult<double>.Failure(CalculationErrorType.NoFittingConductorSize)
            : CalculationResult<double>.Success(table[groundingMaterial][index]);
    }
}