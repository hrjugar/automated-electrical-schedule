using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class DistributionBoard
{
    public abstract DistributionBoard Clone();

    public List<BoardPhase> GetAllowedPhases()
    {
        if (ParentDistributionBoard is SinglePhaseDistributionBoard) return [BoardPhase.SinglePhase];

        if (ParentDistributionBoard is ThreePhaseDistributionBoard)
            return [BoardPhase.SinglePhase, BoardPhase.ThreePhase];

        if (SubDistributionBoards.Count > 0 || Circuits.Count > 0) return [Phase];

        return Enum.GetValues<BoardPhase>().ToList();
    }

    public abstract List<BoardVoltage> GetAllowedVoltages();
    public abstract List<LineToLineVoltage> GetAllowedLineToLineVoltages();

    public double GetVoltAmpere()
    {
        return Circuits.Sum(circuit => circuit.GetVoltAmpere());
    }

    public double GetAmpereLoad(LineToLineVoltage? lineToLineVoltage = null)
    {
        var childCircuitsAmpereLoad = lineToLineVoltage == null
            ? Circuits
                .Sum(circuit => circuit.GetAmpereLoad())
            : Circuits
                .Where(circuit => circuit.LineToLineVoltage == lineToLineVoltage)
                .Sum(circuit => circuit.GetAmpereLoad());

        // TODO: Fix sub board ampere load calculation, CURRENTLY NOT WORKINGGG 

        double subBoardsAmpereLoad = 0;

        foreach (var subBoard in SubDistributionBoards)
            if (subBoard.LineToLineVoltage == Enums.LineToLineVoltage.Abc)
                subBoardsAmpereLoad += subBoard.GetAmpereLoad(lineToLineVoltage);
            else if (subBoard.LineToLineVoltage == lineToLineVoltage || subBoard.LineToLineVoltage is null)
                subBoardsAmpereLoad += subBoard.GetAmpereLoad();


        // var subBoardsAmpereLoad = lineToLineVoltage == null
        //     ? SubDistributionBoards
        //         .Sum(subBoard => subBoard.GetAmpereLoad())
        //     : SubDistributionBoards
        //         .Where(subBoard => subBoard.LineToLineVoltage == lineToLineVoltage)
        //         .Sum(subBoard => subBoard.GetAmpereLoad());

        return childCircuitsAmpereLoad + subBoardsAmpereLoad;
    }

    public int GetAmpereTrip()
    {
        /* TODO: Edit formula for three phase */
        double highestMotorLoad = 0;
        if (Circuits.Count > 0)
        {
            var motorWithHighestLoad =
                Circuits.Where(c => c is MotorOutletCircuit).MaxBy(c => c.GetAmpereLoad());

            if (motorWithHighestLoad is MotorOutletCircuit) highestMotorLoad = motorWithHighestLoad.GetAmpereLoad();
        }

        var value = (GetAmpereLoad() + 0.25 * highestMotorLoad) / 0.8;
        return DataUtils.GetAmpereTrip(value, 20);
    }

    public int GetAmpereFrame()
    {
        return DataUtils.GetAmpereFrame(GetAmpereTrip());
    }

    public double GetR()
    {
        return VoltageDropTable.GetR(
            RacewayType,
            ConductorType.Material,
            GetConductorSize()
        );
    }

    public double GetX()
    {
        return VoltageDropTable.GetX(
            RacewayType,
            GetConductorSize()
        );
    }

    public double GetVoltageDrop()
    {
        if (RacewayType == RacewayType.CableTray || WireLength is null) return 0;

        return VoltageDropTable.GetVoltageDrop(
            this is ThreePhaseDistributionBoard threePhaseBoard ? threePhaseBoard.LineToLineVoltage : null,
            GetR(),
            GetX(),
            GetAmpereLoad(),
            WireLength.Value,
            SetCount,
            (int)Voltage
        );
    }

    public double GetConductorSize()
    {
        double minConductorSize;
        if (ParentDistributionBoard is null)
            minConductorSize = ConductorType.Material == ConductorMaterial.Copper ? 8.0 : 14.0;
        else
            minConductorSize = 3.5;

        return ConductorSizeTable.GetConductorSize(ConductorType, GetAmpereTrip(), minConductorSize);
    }

    public double GetGroundingSize()
    {
        return ParentDistributionBoard is null
            ? MainBoardGroundingSizeTable.GetGroundingSize(ConductorType.Material, Grounding.Material,
                GetConductorSize())
            : CircuitAndSubBoardGroundingSizeTable.GetGroundingSize(Grounding.Material, GetAmpereTrip());
    }

    public int GetConductorWireCount()
    {
        return LineToLineVoltage == Enums.LineToLineVoltage.Abc ? 3 : 2;
    }

    public int GetRacewaySize()
    {
        var wireCount = GetConductorWireCount() + 1;
        return RacewaySizeTable.GetRacewaySize(
            ConductorType.WireType,
            RacewayType,
            GetConductorSize(),
            wireCount
        );
    }
}