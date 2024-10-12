using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class Circuit
{
    public const int GroundingWireCount = 1;

    public List<CircuitType> AllowedCircuitTypes
    {
        get
        {
            if (ParentDistributionBoard.Voltage is BoardVoltage.V460 or BoardVoltage.V575)
                return
                [
                    CircuitType.MotorOutlet,
                    CircuitType.ApplianceEquipmentOutlet
                ];

            return
            [
                CircuitType.LightingOutlet,
                CircuitType.MotorOutlet,
                CircuitType.ConvenienceOutlet,
                CircuitType.ApplianceEquipmentOutlet
            ];
        }
    }

    public virtual List<CircuitProtection> AllowedCircuitProtections =>
    [
        CircuitProtection.MiniatureCircuitBreaker,
        CircuitProtection.MoldedCaseCircuitBreaker
    ];

    public List<LineToLineVoltage> AllowedLineToLineVoltages
    {
        get
        {
            if (ParentDistributionBoard is ThreePhaseDistributionBoard threePhaseDistributionBoard)
                return threePhaseDistributionBoard.LineToLineVoltage switch
                {
                    Enums.LineToLineVoltage.A => [Enums.LineToLineVoltage.A],
                    Enums.LineToLineVoltage.B => [Enums.LineToLineVoltage.B],
                    Enums.LineToLineVoltage.C => [Enums.LineToLineVoltage.C],
                    Enums.LineToLineVoltage.Abc => this switch
                    {
                        ConvenienceOutletCircuit or LightingOutletCircuit =>
                        [
                            Enums.LineToLineVoltage.A,
                            Enums.LineToLineVoltage.B,
                            Enums.LineToLineVoltage.C
                        ],
                        ApplianceEquipmentOutletCircuit or MotorOutletCircuit =>
                        [
                            Enums.LineToLineVoltage.A,
                            Enums.LineToLineVoltage.B,
                            Enums.LineToLineVoltage.C,
                            Enums.LineToLineVoltage.Abc
                        ],
                        _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
                    },
                    _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
                };

            return [];
        }
    }

    public int Voltage
    {
        get
        {
            if (
                ParentDistributionBoard is ThreePhaseDistributionBoard parentThreePhaseBoard &&
                parentThreePhaseBoard.ThreePhaseConfiguration == ThreePhaseConfiguration.Wye &&
                (
                    LineToLineVoltage == Enums.LineToLineVoltage.A ||
                    LineToLineVoltage == Enums.LineToLineVoltage.B ||
                    LineToLineVoltage == Enums.LineToLineVoltage.C
                )
            )
                return (int)BoardVoltage.V230;

            return (int)ParentDistributionBoard.Voltage;
        }
    }

    public int Phase => LineToLineVoltage == Enums.LineToLineVoltage.Abc ? 3 : 1;

    public int Pole
    {
        get
        {
            if (LineToLineVoltage == Enums.LineToLineVoltage.Abc) return 3;

            if (ParentDistributionBoard is ThreePhaseDistributionBoard parentThreePhaseBoard &&
                parentThreePhaseBoard.ThreePhaseConfiguration == ThreePhaseConfiguration.Delta)
                return 2;

            return 1;
        }
    }

    public abstract double VoltAmpere { get; }

    public abstract double AmpereLoad { get; }

    public abstract int AmpereTrip { get; }

    public int AmpereFrame => DataUtils.GetAmpereFrame(AmpereTrip);

    public double R => VoltageDropTable.GetR(
        RacewayType,
        ConductorType.Material,
        ConductorSize
    );

    public double X => VoltageDropTable.GetX(
        RacewayType,
        ConductorSize
    );

    public double VoltageDrop
    {
        get
        {
            if (RacewayType == RacewayType.CableTray) return 0;

            return VoltageDropTable.GetVoltageDrop(
                LineToLineVoltage,
                R,
                X,
                AmpereLoad,
                WireLength,
                SetCount,
                Voltage
            );
        }
    }

    public ConductorType ConductorType => ConductorType.FindById(ConductorTypeId);

    public virtual double ConductorSize => ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip);
    public int ConductorWireCount => LineToLineVoltage == Enums.LineToLineVoltage.Abc ? 3 : 2;

    public ConductorType Grounding => ConductorType.FindById(GroundingId);

    public double GroundingSize =>
        CircuitAndSubBoardGroundingSizeTable.GetGroundingSize(Grounding.Material, AmpereTrip);

    public int RacewaySize
    {
        get
        {
            var wireCount = ConductorWireCount + GroundingWireCount;
            return RacewaySizeTable.GetRacewaySize(
                ConductorType.WireType,
                RacewayType,
                ConductorSize,
                wireCount,
                SetCount
            );
        }
    }

    public abstract Circuit Clone();

    public void CorrectVoltageDrop()
    {
        while (VoltageDrop * 100 >= 3) SetCount += 1;
    }
}