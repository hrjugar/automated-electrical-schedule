using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;
using automated_electrical_schedule.Data.Records;
using automated_electrical_schedule.Data.Validators;
using automated_electrical_schedule.Data.Wrappers;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.Models;

[CircuitValidator]
public abstract partial class Circuit
{
    // public const int GroundingWireCount = 1;

    public static List<CircuitType> GetAllowedCircuitTypesStatic(BoardVoltage voltage)
    {
        if (voltage is BoardVoltage.V460 or BoardVoltage.V575)
            return
            [
                CircuitType.MotorOutlet,
                CircuitType.ApplianceEquipmentOutlet,
                CircuitType.SpaceOutlet,
                CircuitType.SpareOutlet
            ];
    
        return
        [
            CircuitType.LightingOutlet,
            CircuitType.MotorOutlet,
            CircuitType.ConvenienceOutlet,
            CircuitType.ApplianceEquipmentOutlet,
            CircuitType.SpaceOutlet,
            CircuitType.SpareOutlet
        ];
    }
    
    public static List<LineToLineVoltage> GetAllowedLineToLineVoltagesStatic(
        DistributionBoard parentDistributionBoard,
        CircuitType circuitType)
    {
        if (parentDistributionBoard is SinglePhaseDistributionBoard) return [LineToLineVoltage.None];
        return circuitType switch
        {
            CircuitType.LightingOutlet or CircuitType.ConvenienceOutlet =>
            [
                LineToLineVoltage.A,
                LineToLineVoltage.B,
                LineToLineVoltage.C
            ],
            CircuitType.MotorOutlet or CircuitType.ApplianceEquipmentOutlet => 
                (parentDistributionBoard.Voltage is BoardVoltage.V460 or BoardVoltage.V575)
                    ? [LineToLineVoltage.Abc]
                    : [LineToLineVoltage.A, LineToLineVoltage.B, LineToLineVoltage.C, LineToLineVoltage.Abc],
            CircuitType.SpaceOutlet => [LineToLineVoltage.None],
            CircuitType.SpareOutlet => 
            [
                LineToLineVoltage.A,
                LineToLineVoltage.B,
                LineToLineVoltage.C,
                LineToLineVoltage.Abc
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(parentDistributionBoard))
        };
    }
    
    public List<CircuitType> AllowedCircuitTypes => GetAllowedCircuitTypesStatic(ParentDistributionBoard.Voltage);
    
    public List<LineToLineVoltage> AllowedLineToLineVoltages =>
        GetAllowedLineToLineVoltagesStatic(ParentDistributionBoard, CircuitType);
    
    public abstract Circuit Clone();
    
    // public void CorrectVoltageDrop()
    // {
    //     if (VoltageDrop.HasError) return;
    //     while (VoltageDrop.Value * 100 >= 3) SetCount += 1;
    // }
    //
    // private void AdjustSetCountForConductorSize()
    // {
    //     while (ConductorSize.ErrorType == CalculationErrorType.NoFittingAmpereTripForConductorSize)
    //     {
    //         SetCount += 1;
    //     }
    // }
    //
    // public void AdjustSetCountForSizes()
    // {
    //     AdjustSetCountForConductorSize();
    // }
}