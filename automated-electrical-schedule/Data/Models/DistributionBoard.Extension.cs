using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class DistributionBoard
{
    public const int GroundingWireCount = 1;

    public static readonly List<CircuitProtection> AllowedCircuitProtections =
    [
        CircuitProtection.MiniatureCircuitBreaker,
        CircuitProtection.MoldedCaseCircuitBreaker,
        CircuitProtection.OilCircuitBreaker,
        CircuitProtection.AirBreakCircuitBreaker,
        CircuitProtection.AirBlastCircuitBreaker,
        CircuitProtection.VacuumCircuitBreaker,
        CircuitProtection.SulfurHexafluorideCircuitBreaker
    ];

    public List<BoardPhase> AllowedPhases
    {
        get
        {
            switch (ParentDistributionBoard)
            {
                case SinglePhaseDistributionBoard:
                    return [BoardPhase.SinglePhase];
                case ThreePhaseDistributionBoard:
                    return [BoardPhase.SinglePhase, BoardPhase.ThreePhase];
                default:
                    if (SubDistributionBoards.Count > 0 || Circuits.Count > 0) return [Phase];
                    return Enum.GetValues<BoardPhase>().ToList();
            }
        }
    }

    public abstract List<BoardVoltage> AllowedVoltages { get; }

    public abstract List<LineToLineVoltage> AllowedLineToLineVoltages { get; }

    public double VoltAmpere
    {
        get
        {
            return Circuits.Sum(circuit => circuit.VoltAmpere) +
                   SubDistributionBoards.Sum(subBoard => subBoard.VoltAmpere);
        }
    }

    // public double AmpereLoad
    // {
    //     get
    //     {
    //         var childCircuitsAmpereLoad = Circuits.Sum(circuit => circuit.AmpereLoad);
    //         var subBoardsAmpereLoad = SubDistributionBoards.Sum(subBoard => subBoard.AmpereLoad);
    //         return childCircuitsAmpereLoad + subBoardsAmpereLoad;
    //     }
    // }

    // public double GetAmpereLoad(LineToLineVoltage? lineToLineVoltage = null)
    // {
    //     var childCircuitsAmpereLoad = lineToLineVoltage == null
    //         ? Circuits
    //             .Sum(circuit => circuit.GetAmpereLoad())
    //         : Circuits
    //             .Where(circuit => circuit.LineToLineVoltage == lineToLineVoltage)
    //             .Sum(circuit => circuit.GetAmpereLoad());
    //
    //     // TODO: Fix sub board ampere load calculation, CURRENTLY NOT WORKINGGG 
    //
    //     double subBoardsAmpereLoad = 0;
    //
    //     foreach (var subBoard in SubDistributionBoards)
    //         if (subBoard.LineToLineVoltage == Enums.LineToLineVoltage.Abc)
    //             subBoardsAmpereLoad += subBoard.GetAmpereLoad(lineToLineVoltage);
    //         else if (subBoard.LineToLineVoltage == lineToLineVoltage || subBoard.LineToLineVoltage is null)
    //             subBoardsAmpereLoad += subBoard.GetAmpereLoad();
    //
    //
    //     // var subBoardsAmpereLoad = lineToLineVoltage == null
    //     //     ? SubDistributionBoards
    //     //         .Sum(subBoard => subBoard.GetAmpereLoad())
    //     //     : SubDistributionBoards
    //     //         .Where(subBoard => subBoard.LineToLineVoltage == lineToLineVoltage)
    //     //         .Sum(subBoard => subBoard.GetAmpereLoad());
    //
    //     return childCircuitsAmpereLoad + subBoardsAmpereLoad;
    // }

    // public int AmpereTrip
    // {
    //     get
    //     {
    //         double highestMotorLoad = 0;
    //         if (Circuits.Count > 0)
    //         {
    //             var motorWithHighestLoad =
    //                 Circuits.Where(c => c is MotorOutletCircuit).MaxBy(c => c.AmpereLoad);
    //
    //             if (motorWithHighestLoad is MotorOutletCircuit) highestMotorLoad = motorWithHighestLoad.AmpereLoad;
    //         }
    //         
    //         var value = (AmpereLoad + 0.25 * highestMotorLoad) / 0.8;
    //         return DataUtils.GetAmpereTrip(value, 20);
    //     }
    // }

    public abstract int AmpereTrip { get; }

    public abstract double AmpereLoad { get; }

    public int AmpereFrame => DataUtils.GetAmpereFrame(AmpereTrip);

    public double R => VoltageDropTable.GetR(RacewayType, ConductorType.Material, ConductorSize);

    public double X => VoltageDropTable.GetX(RacewayType, ConductorSize);

    public double VoltageDrop
    {
        get
        {
            if (RacewayType == RacewayType.CableTray || WireLength is null) return 0;

            return VoltageDropTable.GetVoltageDrop(
                this is ThreePhaseDistributionBoard threePhaseBoard ? threePhaseBoard.LineToLineVoltage : null,
                R,
                X,
                AmpereLoad,
                WireLength.Value,
                SetCount,
                (int)Voltage
            );
        }
    }

    public ConductorType ConductorType => ConductorType.FindById(ConductorTypeId);
    
    public double ConductorSize
    {
        get
        {
            double minConductorSize;
            if (ParentDistributionBoard is null)
                minConductorSize = ConductorType.Material == ConductorMaterial.Copper ? 8.0 : 14.0;
            else
                minConductorSize = 3.5;

            return ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip, minConductorSize);
        }
    }
    public int ConductorWireCount => LineToLineVoltage == Enums.LineToLineVoltage.Abc ? 3 : 2;

    public ConductorType Grounding => ConductorType.FindById(GroundingId);
    
    public double GroundingSize => ParentDistributionBoard is null
        ? MainBoardGroundingSizeTable.GetGroundingSize(ConductorType.Material, Grounding.Material,
            ConductorSize)
        : CircuitAndSubBoardGroundingSizeTable.GetGroundingSize(Grounding.Material, AmpereTrip);


    public int RacewaySize
    {
        get
        {
            if (RacewayType == RacewayType.CableTray)
                return CableTrayRacewaySizeTable.GetCableTrayRacewaySize(
                    SetCount,
                    ConductorWireCount,
                    ConductorSize,
                    GroundingWireCount,
                    GroundingSize
                );

            var wireCount = ConductorWireCount + GroundingWireCount;
            return RacewaySizeTable.GetRacewaySize(
                ConductorType.WireType,
                RacewayType,
                ConductorSize,
                wireCount
            );
        }
    }
    
    public ConductorType? BreakerConductorType => BreakerConductorTypeId is null ? null : ConductorType.FindById(BreakerConductorTypeId);
    
    public ConductorType? BreakerGrounding => BreakerGroundingId is null ? null : ConductorType.FindById(BreakerGroundingId);

    public abstract DistributionBoard Clone();
}