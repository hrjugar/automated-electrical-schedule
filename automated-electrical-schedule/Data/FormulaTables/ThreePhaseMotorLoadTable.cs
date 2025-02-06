using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Data.Wrappers;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class ThreePhaseMotorLoadTable
{
    // GROUP ONE
    // Design B Efficiency, Squirrel Cage, Wound Rotor
    public static readonly Dictionary<BoardVoltage, List<double>> GroupOneLoadTable = new()
    {
        {
            BoardVoltage.V230,
            [
                2.2,
                3.2,
                4.2,
                6,
                6.8,
                9.6,
                15.2,
                22,
                28,
                42,
                54,
                68,
                80,
                104,
                130,
                154,
                192,
                248,
                312,
                360,
                480
            ]
        },
        {
            BoardVoltage.V400,
            [
                1.4,
                1.8,
                2.3,
                3.3,
                4.3,
                6.1,
                9.7,
                14,
                18,
                27,
                34,
                44,
                51,
                66,
                83,
                103,
                128,
                165,
                208,
                240,
                320
            ]
        },
        {
            BoardVoltage.V460,
            [
                1.1,
                1.6,
                2.1,
                3,
                3.4,
                4.8,
                7.6,
                11,
                14,
                21,
                27,
                34,
                40,
                52,
                65,
                77,
                96,
                124,
                156,
                180,
                240
            ]
        },
        {
            BoardVoltage.V575,
            [
                0.9,
                1.3,
                1.7,
                2.4,
                2.7,
                3.9,
                6.1,
                9,
                11,
                17,
                22,
                27,
                32,
                41,
                52,
                62,
                77,
                99,
                125,
                144,
                192
            ]
        }
    };

    public static readonly Dictionary<BoardVoltage, List<double>> SynchronousLoadTable = new()
    {
        {
            BoardVoltage.V230,
            [
                53,
                63,
                83,
                104,
                123,
                155,
                202,
                253,
                302,
                400
            ]
        },
        {
            BoardVoltage.V460,
            [
                26,
                32,
                41,
                52,
                61,
                78,
                101,
                126,
                151,
                201
            ]
        },
        {
            BoardVoltage.V575,
            [
                21,
                26,
                33,
                42,
                49,
                62,
                81,
                101,
                121,
                161
            ]
        }
    };

    public static readonly Dictionary<BoardVoltage, List<double>> FirePumpLoadTable = new()
    {
        {
            BoardVoltage.V230,
            [
                20,
                25,
                30,
                40,
                50,
                64,
                92,
                127,
                162,
                232,
                290,
                365,
                435,
                580,
                725,
                870,
                1085,
                1450,
                1815,
                2170,
                2900
            ]
        },
        {
            BoardVoltage.V400,
            [
                20,
                20,
                20,
                27,
                34,
                43,
                61,
                84,
                107,
                154,
                194,
                243,
                289,
                387,
                482,
                578,
                722,
                965,
                1207,
                1441,
                1927
            ]
        },
        {
            BoardVoltage.V460,
            [
                10,
                12.5,
                15,
                20,
                25,
                32,
                46,
                63.5,
                81,
                116,
                145,
                183,
                218,
                290,
                363,
                435,
                543,
                725,
                908,
                1085,
                1450
            ]
        },
        {
            BoardVoltage.V575,
            [
                8,
                10,
                12,
                16,
                20,
                25.56,
                36.8,
                50.8,
                64.8,
                93,
                116,
                146,
                174,
                232,
                290,
                348,
                434,
                580,
                726,
                868,
                1160
            ]
        }
    };


    // Use if not fire pump
    public static CalculationResult<double> GetMotorLoad(BoardVoltage voltage, MotorType motorType, string horsepower)
    {
        // return motorType switch
        // {
        //     MotorType.DesignBEnergyEfficient or
        //         MotorType.SquirrelCage or
        //         MotorType.WoundRotor => GroupOneLoadTable[voltage][
        //             DataConstants.GeneralThreePhaseHorsepowerValues.FindIndex(hp => hp.IsRoughlyEqualTo(horsepower))],
        //     MotorType.Synchronous => SynchronousLoadTable[voltage][
        //         DataConstants.SynchronousThreePhaseHorsepowerValues.FindIndex(hp => hp.IsRoughlyEqualTo(horsepower))],
        //     MotorType.InductionMotorFirePump => FirePumpLoadTable[voltage][
        //         DataConstants.GeneralThreePhaseHorsepowerValues.FindIndex(hp => hp.IsRoughlyEqualTo(horsepower))],
        //     _ => throw new ArgumentOutOfRangeException(nameof(motorType))
        // };

        int index;
        List<double> column;
        
        switch (motorType)
        {
            case MotorType.DesignBEnergyEfficient:
            case MotorType.SquirrelCage:
            case MotorType.WoundRotor:
                column = GroupOneLoadTable[voltage];
                index = DataConstants.GeneralThreePhaseHorsepowerValues.FindIndex(hp => hp == horsepower);
                break;
            case MotorType.Synchronous:
                column = SynchronousLoadTable[voltage];
                index = DataConstants.SynchronousThreePhaseHorsepowerValues.FindIndex(hp => hp == horsepower);
                break;
            case MotorType.InductionMotorFirePump:
                column = FirePumpLoadTable[voltage];
                index = DataConstants.GeneralThreePhaseHorsepowerValues.FindIndex(hp => hp == horsepower);
                break;
            case MotorType.SinglePhaseMotor:
            default:
                return CalculationResult<double>.Failure(CalculationErrorType.InvalidMotorType);
        }
        
        return index == -1
            ? CalculationResult<double>.Failure(CalculationErrorType.NoFittingHorsepower)
            : CalculationResult<double>.Success(column[index]);
    }
}