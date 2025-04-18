using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Data.Wrappers;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class RacewaySizeTable
{
    public static readonly Dictionary<RacewayType, List<int>> RacewaySizes = new()
    {
        {
            RacewayType.Emt,
            [
                15,
                20,
                25,
                32,
                40,
                50,
                55,
                65,
                80,
                90,
                110
            ]
        },
        {
            RacewayType.Ent,
            [
                15,
                20,
                25,
                32,
                40,
                50,
                65,
                80,
                90,
                110
            ]
        },
        {
            RacewayType.Imc,
            [
                15,
                20,
                25,
                32,
                40,
                50,
                65,
                80,
                90,
                100
            ]
        },
        {
            RacewayType.Fmc,
            [
                10,
                15,
                20,
                25,
                32,
                40,
                50,
                65,
                80,
                90,
                110
            ]
        },
        {
            RacewayType.Pvc,
            [
                20,
                25,
                32,
                40,
                50,
                65,
                80,
                90,
                110
            ]
        },
        {
            RacewayType.Rmc,
            [
                15,
                20,
                25,
                32,
                40,
                50,
                65,
                80,
                90,
                110,
                125,
                150
            ]
        }
    };

    /* --- CONDUCTOR GROUP ONE --- */

    private static readonly HashSet<ConductorWireType> ConductorGroupOneWireTypes =
    [
        ConductorWireType.Rhh,
        ConductorWireType.Rhw,
        ConductorWireType.Rhw2
    ];

    private static readonly Dictionary<RacewayType, List<List<int>>> ConductorGroupOneWireCountTableDict = new()
    {
        {
            RacewayType.Emt,
            [
                [4, 7, 11, 20, 27, 46, 80, 120, 157, 201],
                [3, 6, 9, 17, 23, 38, 66, 100, 131, 167],
                [2, 5, 8, 13, 18, 30, 53, 81, 105, 135],
                [1, 2, 4, 7, 9, 16, 28, 42, 55, 70],
                [1, 1, 3, 5, 8, 13, 22, 34, 44, 56],
                [1, 1, 2, 4, 6, 10, 17, 26, 34, 44],
                // [1, 1, 1, 4, 5, 9, 15, 23, 30, 38],
                [1, 1, 1, 3, 4, 7, 13, 20, 26, 33],
                [0, 1, 1, 1, 3, 5, 9, 13, 17, 22],
                [0, 1, 1, 1, 2, 4, 7, 11, 15, 19],
                [0, 1, 1, 1, 2, 4, 6, 10, 13, 17],
                [0, 0, 1, 1, 1, 3, 5, 8, 11, 14],
                [0, 0, 1, 1, 1, 3, 5, 7, 9, 12],
                [0, 0, 0, 1, 1, 1, 3, 5, 7, 9],
                [0, 0, 0, 1, 1, 1, 3, 5, 6, 8],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 7],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 7],
                [0, 0, 0, 0, 1, 1, 2, 3, 4, 6],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 0, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 3]
            ]
        },
        {
            RacewayType.Ent,
            [
                [4, 7, 11, 20, 27, 45],
                [3, 5, 9, 16, 22, 37],
                [2, 4, 7, 13, 18, 30],
                [1, 2, 4, 7, 9, 15],
                [1, 1, 3, 5, 7, 12],
                [1, 1, 2, 4, 6, 10],
                // [1, 1, 1, 4, 5, 8],
                [1, 1, 1, 3, 4, 7],
                [0, 1, 1, 1, 3, 5],
                [0, 1, 1, 1, 2, 4],
                [0, 0, 1, 1, 1, 3],
                [0, 0, 1, 1, 1, 3],
                [0, 0, 1, 1, 1, 2],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 0, 1],
                [0, 0, 0, 0, 0, 1],
                [0, 0, 0, 0, 0, 1],
                [0, 0, 0, 0, 0, 1]
            ]
        },
        {
            RacewayType.Imc,
            [
                [4, 8, 13, 22, 30, 49, 70, 108, 144, 186],
                [4, 6, 11, 18, 25, 41, 58, 89, 120, 154],
                [3, 5, 8, 15, 20, 33, 47, 72, 97, 124],
                [1, 5, 4, 8, 10, 17, 24, 38, 50, 65],
                [1, 3, 3, 6, 8, 14, 19, 30, 40, 52],
                [1, 1, 3, 5, 6, 11, 15, 23, 31, 41],
                // [1, 1, 2, 4, 6, 9, 13, 21, 28, 36],
                [1, 1, 1, 3, 5, 8, 11, 18, 24, 31],
                [0, 1, 1, 2, 3, 5, 7, 12, 16, 20],
                [0, 1, 1, 1, 3, 4, 6, 10, 14, 18],
                [0, 1, 1, 1, 2, 4, 6, 9, 12, 15],
                [0, 0, 1, 1, 1, 3, 5, 7, 10, 13],
                [0, 0, 1, 1, 1, 3, 4, 6, 9, 11],
                [0, 0, 1, 1, 1, 1, 3, 5, 6, 8],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 7],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 7],
                [0, 0, 0, 1, 1, 1, 2, 3, 5, 6],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 1, 1, 1, 1, 3, 4],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 3],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3]
            ]
        },
        {
            RacewayType.Fmc,
            [
                [1, 4, 7, 11, 17, 25, 44, 67, 96, 131, 171],
                [1, 3, 6, 9, 14, 21, 37, 55, 80, 109, 142],
                [1, 3, 5, 7, 11, 17, 30, 45, 64, 88, 115],
                [0, 1, 2, 4, 6, 9, 15, 23, 34, 46, 60],
                [0, 1, 1, 3, 5, 7, 12, 19, 27, 37, 48],
                [0, 1, 1, 2, 4, 5, 10, 14, 21, 29, 37],
                // [0, 1, 1, 1, 3, 5, 8, 13, 18, 25, 33],
                [0, 1, 1, 1, 3, 4, 7, 11, 16, 22, 28],
                [0, 0, 1, 1, 1, 2, 5, 7, 10, 14, 19],
                [0, 0, 1, 1, 1, 2, 4, 6, 9, 12, 16],
                [0, 0, 1, 1, 1, 1, 3, 5, 8, 11, 14],
                [0, 0, 0, 1, 1, 1, 3, 5, 7, 9, 12],
                [0, 0, 0, 1, 1, 1, 2, 4, 6, 8, 10],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 6, 8],
                [0, 0, 0, 0, 1, 1, 1, 2, 4, 5, 7],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 5, 6],
                [0, 0, 0, 0, 1, 1, 1, 1, 3, 4, 6],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 3],
                [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 3],
                [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 3],
                [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 3]
            ]
        },
        {
            RacewayType.Pvc,
            [
                [5, 9, 14, 24, 31, 49, 74, 112, 187],
                [4, 7, 12, 20, 26, 41, 61, 93, 155],
                [3, 6, 10, 16, 21, 33, 50, 75, 125],
                [1, 3, 5, 8, 11, 17, 26, 39, 65],
                [1, 2, 4, 6, 9, 14, 21, 31, 52],
                [1, 1, 3, 5, 7, 11, 16, 24, 41],
                // [1, 1, 3, 4, 6, 9, 14, 21, 36],
                [1, 1, 2, 4, 5, 8, 12, 18, 31],
                [0, 1, 1, 2, 3, 5, 8, 12, 20],
                [0, 1, 1, 2, 3, 5, 7, 10, 18],
                [0, 1, 1, 1, 2, 4, 6, 9, 15],
                [0, 1, 1, 1, 1, 3, 5, 8, 13],
                [0, 0, 1, 1, 1, 3, 4, 7, 11],
                [0, 0, 1, 1, 1, 1, 3, 5, 8],
                [0, 0, 1, 1, 1, 1, 3, 4, 7],
                [0, 0, 0, 1, 1, 1, 2, 4, 7],
                [0, 0, 0, 1, 1, 1, 2, 3, 6],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 0, 1, 1, 1, 2, 4],
                [0, 0, 0, 0, 1, 1, 1, 2, 4],
                [0, 0, 0, 0, 1, 1, 1, 1, 4],
                [0, 0, 0, 0, 1, 1, 1, 1, 3],
                [0, 0, 0, 0, 0, 1, 1, 1, 3]
            ]
        },
        {
            RacewayType.Rmc,
            [
                [4, 7, 12, 21, 28, 46, 66, 102, 136, 176, 276, 398],
                [3, 6, 10, 17, 23, 38, 55, 85, 113, 146, 229, 330],
                [3, 5, 8, 14, 19, 31, 44, 68, 91, 118, 185, 267],
                [1, 2, 4, 7, 10, 16, 23, 36, 48, 61, 97, 139],
                [1, 1, 3, 6, 8, 13, 18, 29, 38, 49, 77, 112],
                [1, 1, 2, 4, 6, 10, 14, 22, 30, 38, 60, 87],
                // [1, 1, 2, 4, 5, 9, 12, 19, 26, 34, 53, 76],
                [1, 1, 1, 3, 4, 7, 11, 17, 23, 29, 46, 66],
                [0, 1, 1, 1, 3, 5, 7, 11, 15, 19, 30, 44],
                [0, 1, 1, 1, 2, 4, 6, 10, 13, 17, 26, 38],
                [0, 1, 1, 1, 2, 4, 5, 8, 11, 14, 23, 33],
                [0, 0, 1, 1, 1, 3, 4, 7, 10, 12, 20, 28],
                [0, 0, 1, 1, 1, 3, 4, 6, 8, 11, 17, 24],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 8, 13, 18],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 7, 11, 16],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 6, 10, 15],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 6, 9, 13],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5, 8, 11],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4, 6, 9],
                [0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 6, 8],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 3, 5, 8],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 5, 7],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 5, 7]
            ]
        }
    };

    /* --- CONDUCTOR GROUP TWO --- */

    private static readonly HashSet<ConductorWireType> ConductorGroupTwoWireTypes =
    [
        ConductorWireType.Tw,
        ConductorWireType.Thhw,
        ConductorWireType.Thw,
        ConductorWireType.Thw2
    ];

    private static readonly Dictionary<RacewayType, List<List<int>>> ConductorGroupTwoWireCountTableDict = new()
    {
        {
            RacewayType.Emt,
            [
                [8, 15, 25, 43, 58, 96, 168, 254, 332, 424],
                [6, 11, 19, 33, 45, 74, 129, 195, 255, 326],
                [5, 8, 14, 24, 33, 55, 96, 145, 190, 243],
                [2, 5, 8, 13, 18, 30, 53, 81, 105, 135],
                [1, 3, 4, 8, 11, 18, 32, 48, 63, 81],
                [1, 1, 3, 5, 8, 13, 24, 36, 47, 60],
                // [1, 1, 3, 5, 7, 12, 20, 31, 40, 52],
                [1, 1, 2, 4, 6, 10, 17, 26, 34, 44],
                [1, 1, 1, 3, 4, 7, 12, 18, 24, 31],
                [0, 1, 1, 2, 3, 6, 10, 16, 20, 26],
                [0, 1, 1, 1, 3, 5, 9, 13, 17, 22],
                [0, 1, 1, 1, 2, 4, 7, 11, 15, 19],
                [0, 0, 1, 1, 1, 3, 6, 9, 12, 16],
                [0, 0, 1, 1, 1, 3, 5, 7, 10, 13],
                [0, 0, 1, 1, 1, 2, 4, 6, 8, 11],
                [0, 0, 0, 1, 1, 1, 4, 6, 7, 10],
                [0, 0, 0, 1, 1, 1, 3, 5, 7, 9],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 7],
                [0, 0, 0, 1, 1, 1, 2, 3, 4, 6],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 3, 5],
                [0, 0, 0, 0, 0, 1, 1, 2, 3, 4]
            ]
        },
        {
            RacewayType.Ent,
            [
                [8, 14, 24, 42, 57, 94],
                [6, 11, 18, 62, 44, 72],
                [4, 8, 13, 24, 32, 54],
                [2, 4, 7, 13, 18, 30],
                [1, 2, 4, 8, 11, 18],
                [1, 1, 3, 6, 8, 13],
                // [1, 1, 3, 5, 7, 11],
                [1, 1, 2, 4, 6, 10],
                [0, 1, 1, 3, 4, 7],
                [0, 1, 1, 2, 3, 6],
                [0, 1, 1, 1, 3, 5],
                [0, 1, 1, 1, 2, 4],
                [0, 0, 1, 1, 1, 3],
                [0, 0, 1, 1, 1, 3],
                [0, 0, 1, 1, 1, 2],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 0, 1]
            ]
        },
        {
            RacewayType.Imc,
            [
                [10, 17, 27, 47, 64, 104, 147, 228, 304, 392],
                [7, 13, 21, 36, 49, 80, 113, 175, 234, 301],
                [5, 9, 15, 27, 36, 59, 84, 130, 174, 224],
                [3, 5, 8, 15, 20, 33, 47, 72, 97, 124],
                [1, 3, 5, 9, 12, 20, 28, 43, 58, 75],
                [1, 2, 4, 6, 9, 15, 21, 32, 43, 56],
                // [1, 1, 3, 6, 8, 13, 18, 28, 37, 48],
                [1, 1, 3, 5, 6, 11, 15, 23, 31, 41],
                [1, 1, 1, 3, 4, 7, 11, 16, 22, 28],
                [1, 1, 1, 3, 4, 6, 9, 14, 19, 24],
                [0, 1, 1, 2, 3, 5, 8, 12, 16, 20],
                [0, 1, 1, 1, 3, 4, 6, 10, 13, 17],
                [0, 1, 1, 1, 2, 4, 5, 8, 11, 14],
                [0, 0, 1, 1, 1, 3, 4, 7, 9, 12],
                [0, 0, 1, 1, 1, 2, 4, 6, 8, 10],
                [0, 0, 1, 1, 1, 2, 3, 5, 7, 9],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 8],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 7],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4]
            ]
        },
        {
            RacewayType.Fmc,
            [
                [3, 9, 15, 23, 36, 53, 94, 141, 203, 277, 361],
                [2, 7, 11, 18, 28, 41, 72, 108, 156, 212, 277],
                [1, 5, 8, 13, 21, 30, 54, 81, 116, 158, 207],
                [1, 3, 5, 7, 11, 17, 30, 45, 64, 88, 115],
                [1, 1, 3, 4, 7, 10, 18, 27, 39, 53, 69],
                [0, 1, 1, 3, 5, 7, 13, 20, 29, 39, 51],
                // [0, 1, 1, 3, 4, 6, 11, 17, 25, 34, 44],
                [0, 1, 1, 2, 4, 5, 10, 14, 21, 29, 37],
                [0, 1, 1, 1, 2, 4, 7, 10, 15, 20, 26],
                [0, 0, 1, 1, 1, 3, 6, 9, 12, 17, 22],
                [0, 0, 1, 1, 1, 3, 5, 7, 10, 14, 19],
                [0, 0, 1, 1, 1, 2, 4, 6, 9, 12, 16],
                [0, 0, 0, 1, 1, 1, 3, 5, 7, 10, 13],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 8, 11],
                [0, 0, 0, 1, 1, 1, 2, 3, 5, 7, 9],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 6, 8],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 6, 7],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 5, 6],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 4],
                [0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 3]
            ]
        },
        {
            RacewayType.Pvc,
            [
                [11, 18, 31, 51, 67, 105, 157, 235, 395],
                [8, 14, 24, 39, 51, 80, 120, 181, 303],
                [6, 10, 18, 29, 38, 60, 89, 135, 226],
                [3, 6, 10, 16, 21, 33, 50, 75, 125],
                [1, 3, 6, 9, 13, 20, 30, 45, 75],
                [1, 2, 4, 7, 9, 15, 22, 33, 56],
                // [1, 1, 4, 6, 8, 13, 19, 29, 48],
                [1, 1, 3, 5, 7, 11, 16, 24, 41],
                [1, 1, 1, 3, 5, 7, 11, 17, 29],
                [1, 1, 1, 3, 4, 6, 10, 14, 24],
                [0, 1, 1, 2, 3, 5, 8, 12, 21],
                [0, 1, 1, 1, 3, 4, 7, 10, 17],
                [0, 1, 1, 1, 2, 4, 6, 9, 14],
                [0, 0, 1, 1, 1, 3, 4, 7, 12],
                [0, 0, 1, 1, 1, 2, 4, 6, 10],
                [0, 0, 1, 1, 1, 2, 3, 5, 9],
                [0, 0, 1, 1, 1, 1, 3, 5, 8],
                [0, 0, 0, 1, 1, 1, 2, 4, 7],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 1, 1, 1, 1, 3, 4],
                [0, 0, 0, 0, 1, 1, 1, 2, 4],
                [0, 0, 0, 0, 1, 1, 1, 2, 4]
            ]
        },
        {
            RacewayType.Rmc,
            [
                [9, 15, 25, 44, 59, 98, 140, 215, 288, 370, 581, 839],
                [7, 12, 19, 33, 45, 75, 107, 165, 221, 284, 446, 644],
                [5, 9, 14, 25, 34, 56, 80, 123, 164, 212, 332, 480],
                [3, 5, 8, 14, 19, 31, 44, 68, 91, 118, 185, 267],
                [1, 3, 5, 8, 11, 18, 27, 41, 55, 71, 111, 160],
                [1, 1, 3, 6, 8, 14, 20, 31, 41, 53, 83, 120],
                // [1, 1, 3, 5, 7, 12, 17, 26, 35, 45, 72, 103],
                [1, 1, 2, 4, 6, 10, 14, 22, 30, 38, 6, 87],
                [1, 1, 1, 3, 4, 7, 10, 15, 21, 27, 42, 61],
                [0, 1, 1, 2, 3, 6, 8, 13, 18, 23, 36, 52],
                [0, 1, 1, 2, 3, 5, 7, 11, 15, 19, 31, 44],
                [0, 1, 1, 1, 2, 4, 6, 9, 13, 16, 26, 37],
                [0, 0, 1, 1, 1, 3, 5, 8, 10, 14, 21, 31],
                [0, 0, 1, 1, 1, 3, 4, 6, 8, 11, 17, 25],
                [0, 0, 1, 1, 1, 3, 3, 5, 7, 9, 15, 22],
                [0, 0, 0, 1, 1, 2, 3, 5, 6, 8, 13, 19],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 7, 12, 17],
                [0, 0, 0, 1, 1, 1, 2, 3, 5, 6, 10, 14],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5, 8, 12],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4, 7, 10],
                [0, 0, 0, 1, 1, 1, 2, 3, 4, 7, 10],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4, 6, 9],
                [0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 6, 8]
            ]
        }
    };


    /* --- CONDUCTOR GROUP THREE --- */

    private static readonly HashSet<ConductorWireType> ConductorGroupThreeWireTypes =
    [
        ConductorWireType.Thhn,
        ConductorWireType.Thwn,
        ConductorWireType.Thwn2
    ];

    private static readonly Dictionary<RacewayType, List<List<int>>> ConductorGroupThreeWireCountTableDict = new()
    {
        {
            RacewayType.Emt,
            [
                [12, 22, 35, 61, 84, 138, 241, 364, 476, 608],
                [9, 16, 26, 45, 61, 101, 176, 266, 347, 443],
                [5, 10, 16, 28, 38, 63, 111, 167, 219, 279],
                [3, 6, 9, 16, 22, 36, 64, 96, 126, 161],
                [2, 4, 7, 12, 16, 26, 46, 69, 91, 116],
                [1, 2, 4, 7, 10, 16, 28, 43, 56, 71],
                // [1, 1, 3, 6, 8, 13, 24, 36, 47, 60],
                [1, 1, 3, 5, 7, 11, 20, 30, 40, 51],
                [1, 1, 1, 4, 5, 8, 15, 22, 29, 37],
                [1, 1, 1, 3, 4, 7, 12, 19, 25, 32],
                [0, 1, 1, 2, 3, 6, 10, 16, 20, 26],
                [0, 1, 1, 1, 3, 5, 8, 13, 17, 22],
                [0, 1, 1, 1, 2, 4, 7, 11, 14, 18],
                [0, 0, 1, 1, 1, 3, 6, 9, 11, 15],
                [0, 0, 1, 1, 1, 3, 5, 7, 10, 13],
                [0, 0, 1, 1, 1, 2, 4, 6, 9, 11],
                [0, 0, 0, 1, 1, 1, 4, 6, 8, 10],
                [0, 0, 0, 1, 1, 1, 3, 5, 6, 8],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 7],
                [0, 0, 0, 1, 1, 1, 2, 3, 4, 6],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 3, 4]
            ]
        },
        {
            RacewayType.Ent,
            [
                [11, 21, 34, 60, 82, 135],
                [8, 15, 25, 43, 59, 99],
                [5, 9, 15, 27, 37, 62],
                [3, 5, 9, 16, 21, 36],
                [1, 4, 6, 11, 15, 26],
                [1, 2, 4, 7, 9, 16],
                // [1, 1, 3, 6, 8, 13],
                [1, 1, 3, 5, 7, 11],
                [1, 1, 1, 3, 5, 8],
                [1, 1, 1, 3, 4, 7],
                [0, 1, 1, 2, 3, 6],
                [0, 1, 1, 1, 3, 5],
                [0, 1, 1, 1, 2, 4],
                [0, 0, 1, 1, 1, 3],
                [0, 0, 1, 1, 1, 3],
                [0, 0, 1, 1, 1, 2],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1]
            ]
        },
        {
            RacewayType.Imc,
            [
                [14, 24, 39, 68, 91, 149, 211, 326, 436, 562],
                [10, 17, 29, 49, 67, 109, 154, 238, 318, 410],
                [6, 11, 18, 31, 42, 69, 97, 150, 200, 258],
                [3, 6, 10, 18, 24, 39, 16, 86, 115, 149],
                [2, 4, 7, 13, 17, 28, 40, 62, 83, 107],
                [1, 3, 4, 8, 11, 17, 25, 38, 51, 66],
                // [1, 2, 4, 6, 9, 15, 21, 32, 43, 56],
                [1, 1, 3, 5, 7, 12, 17, 27, 36, 47],
                [1, 1, 2, 4, 5, 9, 13, 20, 27, 35],
                [1, 1, 1, 3, 4, 8, 11, 17, 23, 29],
                [1, 1, 1, 3, 4, 6, 9, 14, 19, 24],
                [0, 1, 1, 2, 3, 5, 7, 12, 16, 20],
                [0, 1, 1, 1, 2, 4, 6, 9, 13, 17],
                [0, 0, 1, 1, 1, 3, 5, 8, 10, 13],
                [0, 0, 1, 1, 1, 3, 4, 7, 9, 12],
                [0, 0, 1, 1, 1, 2, 4, 6, 8, 10],
                [0, 0, 1, 1, 1, 2, 3, 5, 7, 9],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 7],
                [0, 0, 0, 1, 1, 1, 2, 3, 5, 6],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4]
            ]
        },
        {
            RacewayType.Fmc,
            [
                [4, 13, 22, 33, 52, 76, 0, 202, 291, 396, 518],
                [3, 9, 16, 24, 38, 56, 135, 147, 212, 289, 378],
                [1, 6, 10, 15, 24, 35, 98, 93, 134, 182, 238],
                [1, 3, 6, 9, 14, 20, 62, 53, 77, 105, 137],
                [1, 2, 4, 6, 10, 14, 35, 38, 55, 76, 99],
                [0, 1, 2, 4, 6, 9, 25, 24, 34, 46, 61],
                // [0, 1, 1, 3, 5, 7, 16, 20, 29, 39, 1],
                [0, 1, 1, 3, 4, 6, 13, 17, 24, 33, 43],
                [0, 1, 1, 1, 3, 4, 11, 12, 18, 24, 32],
                [0, 0, 1, 1, 2, 4, 8, 10, 15, 20, 27],
                [0, 0, 1, 1, 1, 3, 7, 9, 12, 17, 22],
                [0, 0, 1, 1, 1, 2, 6, 7, 10, 14, 18],
                [0, 0, 1, 1, 1, 1, 5, 6, 8, 12, 15],
                [0, 0, 0, 1, 1, 1, 4, 5, 7, 9, 12],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 8, 11],
                [0, 0, 0, 1, 1, 1, 3, 3, 5, 7, 9],
                [0, 0, 0, 0, 1, 1, 2, 3, 5, 6, 8],
                [0, 0, 0, 0, 1, 1, 1, 2, 4, 5, 7],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 4]
            ]
        },
        {
            RacewayType.Pvc,
            [
                [16, 27, 44, 73, 96, 150, 225, 338, 566],
                [11, 19, 32, 53, 70, 109, 164, 246, 412],
                [7, 12, 20, 33, 44, 69, 103, 155, 260],
                [4, 7, 12, 19, 25, 40, 59, 89, 150],
                [3, 5, 8, 14, 18, 28, 43, 64, 108],
                [1, 3, 5, 8, 11, 17, 26, 39, 66],
                // [1, 2, 4, 7, 9, 15, 22, 33, 56],
                [1, 1, 3, 6, 8, 12, 19, 28, 47],
                [1, 1, 2, 4, 6, 9, 14, 21, 35],
                [1, 1, 2, 4, 5, 8, 11, 17, 29],
                [1, 1, 1, 3, 4, 6, 10, 14, 24],
                [0, 1, 1, 2, 3, 5, 8, 12, 20],
                [0, 1, 1, 1, 3, 4, 6, 10, 17],
                [0, 1, 1, 1, 2, 3, 5, 8, 14],
                [0, 0, 1, 1, 1, 3, 4, 7, 12],
                [0, 0, 1, 1, 1, 2, 4, 6, 10],
                [0, 0, 1, 1, 1, 2, 3, 5, 9],
                [0, 0, 1, 1, 1, 1, 3, 4, 7],
                [0, 0, 0, 1, 1, 1, 2, 3, 6],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 0, 1, 1, 1, 2, 4]
            ]
        },
        {
            RacewayType.Rmc,
            [
                [13, 22, 36, 63, 85, 140, 200, 309, 412, 531, 833, 1202],
                [9, 16, 26, 46, 62, 102, 146, 225, 301, 387, 608, 877],
                [6, 10, 17, 29, 39, 64, 92, 142, 189, 244, 383, 552],
                [3, 6, 9, 16, 22, 37, 53, 82, 109, 140, 221, 318],
                [2, 4, 7, 12, 16, 27, 38, 59, 79, 101, 159, 230],
                [1, 2, 4, 7, 10, 16, 23, 36, 48, 62, 98, 141],
                // [1, 1, 3, 6, 8, 14, 20, 31, 41, 53, 83, 120],
                [1, 1, 3, 5, 7, 11, 17, 26, 34, 44, 70, 100],
                [1, 1, 1, 4, 5, 8, 12, 19, 25, 33, 51, 74],
                [1, 1, 1, 3, 4, 7, 10, 16, 21, 27, 43, 63],
                [0, 1, 1, 2, 3, 6, 8, 13, 18, 23, 36, 52],
                [0, 1, 1, 1, 3, 5, 7, 11, 15, 19, 30, 43],
                [0, 1, 1, 1, 2, 4, 6, 9, 12, 16, 25, 36],
                [0, 0, 1, 1, 1, 3, 5, 7, 10, 13, 20, 29],
                [0, 0, 1, 1, 1, 3, 4, 6, 8, 11, 17, 25],
                [0, 0, 1, 1, 1, 2, 4, 5, 7, 10, 15, 22],
                [0, 0, 1, 1, 1, 2, 3, 5, 7, 8, 13, 20],
                [0, 0, 0, 1, 1, 1, 3, 4, 5, 7, 11, 16],
                [0, 0, 0, 1, 1, 1, 2, 3, 4, 6, 9, 13],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5, 8, 11],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5, 7, 11],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4, 7, 10],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4, 6, 9]
            ]
        }
    };


    /* --- CONDUCTOR GROUP FOUR --- */

    private static readonly HashSet<ConductorWireType> ConductorGroupFourWireTypes =
    [
        ConductorWireType.Xhhw,
        ConductorWireType.Zw,
        ConductorWireType.Xhhw2,
        ConductorWireType.Xhh
    ];

    private static readonly Dictionary<RacewayType, List<List<int>>> ConductorGroupFourWireCountTableDict = new()
    {
        {
            RacewayType.Emt,
            [
                [8, 15, 25, 43, 58, 96, 168, 254, 332, 424],
                [6, 11, 19, 33, 45, 74, 129, 195, 255, 326],
                [5, 8, 14, 24, 33, 55, 96, 145, 190, 243],
                [2, 5, 8, 13, 18, 30, 53, 81, 105, 135],
                [1, 3, 6, 10, 14, 22, 39, 60, 78, 100],
                [1, 2, 4, 7, 10, 16, 28, 43, 56, 72],
                // [1, 1, 3, 6, 8, 14, 24, 36, 48, 61],
                [1, 1, 3, 5, 7, 11, 20, 31, 40, 51],
                [1, 1, 1, 4, 5, 8, 15, 23, 30, 38],
                [1, 1, 1, 3, 4, 7, 13, 19, 25, 32],
                [0, 1, 1, 2, 3, 6, 10, 16, 21, 27],
                [0, 1, 1, 1, 3, 5, 9, 13, 17, 22],
                [0, 1, 1, 1, 2, 4, 7, 11, 14, 18],
                [0, 0, 1, 1, 1, 3, 6, 9, 12, 15],
                [0, 0, 1, 1, 1, 3, 5, 8, 10, 13],
                [0, 0, 1, 1, 1, 2, 4, 7, 9, 11],
                [0, 0, 0, 1, 1, 1, 4, 6, 8, 10],
                [0, 0, 0, 1, 1, 1, 3, 5, 6, 8],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 6],
                [0, 0, 0, 0, 1, 1, 2, 3, 4, 6],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 3, 4]
            ]
        },
        {
            RacewayType.Ent,
            [
                [8, 14, 24, 42, 57, 94],
                [6, 11, 18, 32, 44, 72],
                [4, 8, 13, 24, 32, 54],
                [2, 4, 7, 13, 18, 30],
                [1, 3, 5, 10, 13, 22],
                [1, 2, 4, 7, 9, 16],
                // [1, 1, 3, 6, 8, 13],
                [1, 1, 3, 5, 7, 11],
                [1, 1, 1, 3, 5, 8],
                [0, 1, 1, 3, 4, 7],
                [0, 1, 1, 2, 3, 6],
                [0, 1, 1, 1, 3, 5],
                [0, 1, 1, 1, 2, 4],
                [0, 0, 1, 1, 1, 3],
                [0, 0, 1, 1, 1, 3],
                [0, 0, 1, 1, 1, 2],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 1, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1],
                [0, 0, 0, 0, 1, 1]
            ]
        },
        {
            RacewayType.Imc,
            [
                [10, 17, 27, 47, 64, 104, 147, 228, 304, 392],
                [7, 13, 21, 36, 49, 80, 113, 175, 234, 301],
                [5, 9, 15, 27, 36, 59, 84, 130, 174, 224],
                [3, 5, 8, 15, 20, 33, 47, 72, 97, 124],
                [1, 4, 6, 11, 15, 24, 35, 53, 71, 92],
                [1, 3, 4, 8, 11, 18, 25, 39, 52, 67],
                // [1, 2, 4, 7, 9, 15, 21, 33, 44, 56],
                [1, 1, 3, 5, 7, 12, 18, 27, 37, 47],
                [1, 1, 2, 4, 6, 9, 13, 10, 27, 35],
                [1, 1, 1, 3, 5, 8, 11, 17, 23, 30],
                [1, 1, 1, 3, 4, 6, 9, 14, 19, 25],
                [0, 1, 1, 2, 3, 5, 7, 12, 16, 20],
                [0, 1, 1, 1, 2, 4, 6, 10, 13, 17],
                [0, 0, 1, 1, 1, 3, 5, 8, 11, 14],
                [0, 0, 1, 1, 1, 3, 4, 7, 9, 12],
                [0, 0, 1, 1, 1, 3, 4, 6, 8, 10],
                [0, 0, 1, 1, 1, 2, 3, 5, 7, 9],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 8],
                [0, 0, 0, 1, 1, 1, 2, 3, 5, 6],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 3]
            ]
        },
        {
            RacewayType.Fmc,
            [
                [3, 9, 15, 23, 36, 53, 94, 141, 203, 277, 362],
                [2, 7, 11, 18, 28, 41, 72, 108, 156, 212, 277],
                [1, 5, 8, 13, 21, 30, 54, 81, 116, 158, 307],
                [1, 3, 5, 7, 11, 17, 30, 45, 64, 88, 225],
                [1, 1, 3, 5, 8, 12, 22, 33, 48, 65, 85],
                [0, 1, 2, 4, 6, 9, 16, 24, 34, 47, 61],
                // [0, 1, 1, 3, 5, 7, 13, 20, 29, 40, 52],
                [0, 1, 1, 3, 4, 6, 11, 17, 24, 33, 44],
                [0, 1, 1, 1, 3, 5, 8, 13, 18, 25, 32],
                [0, 1, 1, 1, 2, 4, 7, 10, 15, 21, 27],
                [0, 0, 1, 1, 2, 3, 6, 9, 13, 17, 23],
                [0, 0, 1, 1, 1, 3, 5, 7, 10, 14, 19],
                [0, 0, 1, 1, 1, 2, 4, 6, 9, 12, 15],
                [0, 0, 0, 1, 1, 1, 3, 5, 7, 10, 13],
                [0, 0, 0, 1, 1, 1, 3, 4, 6, 8, 11],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 7, 9],
                [0, 0, 0, 0, 1, 1, 1, 3, 5, 6, 8],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5, 7],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 0, 1, 1, 1, 3, 4, 5],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 1, 1, 1, 2, 3, 4],
                [0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 4]
            ]
        },
        {
            RacewayType.Pvc,
            [
                [11, 18, 31, 51, 67, 105, 157, 235, 395],
                [8, 14, 24, 39, 51, 80, 120, 181, 303],
                [6, 10, 18, 29, 38, 60, 89, 135, 226],
                [3, 6, 10, 16, 21, 33, 50, 75, 125],
                [2, 4, 7, 12, 15, 24, 37, 55, 93],
                [1, 3, 5, 8, 11, 18, 26, 40, 67],
                // [1, 2, 4, 7, 9, 15, 22, 34, 57],
                [1, 1, 3, 6, 8, 12, 19, 28, 48],
                [1, 1, 3, 4, 6, 9, 14, 21, 35],
                [1, 1, 2, 4, 5, 8, 12, 18, 30],
                [1, 1, 1, 3, 4, 6, 10, 15, 25],
                [0, 1, 1, 2, 3, 5, 8, 12, 20],
                [0, 1, 1, 1, 3, 4, 7, 10, 17],
                [0, 1, 1, 1, 2, 3, 5, 8, 14],
                [0, 0, 1, 1, 1, 3, 5, 7, 12],
                [0, 0, 1, 1, 1, 3, 4, 6, 10],
                [0, 0, 1, 1, 1, 2, 3, 5, 9],
                [0, 0, 1, 1, 1, 1, 3, 4, 8],
                [0, 0, 0, 1, 1, 1, 2, 3, 6],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 1, 1, 1, 1, 3, 5],
                [0, 0, 0, 0, 1, 1, 1, 2, 4]
            ]
        },
        {
            RacewayType.Rmc,
            [
                [9, 15, 25, 44, 59, 98, 140, 215, 288, 370, 581, 839],
                [7, 12, 19, 33, 45, 75, 107, 165, 221, 284, 446, 644],
                [5, 9, 14, 25, 34, 56, 80, 123, 164, 212, 332, 480],
                [3, 5, 8, 14, 19, 31, 44, 68, 91, 118, 185, 267],
                [1, 3, 6, 10, 14, 23, 33, 51, 68, 87, 137, 197],
                [1, 2, 4, 7, 10, 16, 24, 37, 49, 63, 99, 143],
                // [1, 1, 3, 6, 8, 14, 20, 31, 41, 53, 84, 121],
                [1, 1, 3, 5, 7, 12, 17, 26, 45, 45, 70, 101],
                [1, 1, 1, 4, 5, 9, 12, 19, 26, 33, 52, 76],
                [1, 1, 1, 3, 4, 7, 10, 16, 22, 28, 44, 64],
                [0, 1, 1, 2, 3, 6, 9, 13, 18, 23, 37, 53],
                [0, 1, 1, 1, 3, 5, 7, 11, 15, 19, 30, 44],
                [0, 1, 1, 1, 2, 4, 6, 9, 12, 16, 25, 36],
                [0, 0, 1, 1, 1, 3, 5, 7, 10, 13, 20, 30],
                [0, 0, 1, 1, 1, 3, 4, 6, 9, 11, 18, 25],
                [0, 0, 1, 1, 1, 2, 3, 6, 7, 10, 15, 22],
                [0, 0, 1, 1, 1, 2, 3, 5, 7, 9, 14, 20],
                [0, 0, 0, 1, 1, 1, 2, 4, 5, 7, 11, 16],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 6, 9, 13],
                [0, 0, 0, 1, 1, 1, 1, 3, 4, 5, 8, 11],
                [0, 0, 0, 0, 1, 1, 1, 3, 4, 5, 7, 11],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4, 7, 10],
                [0, 0, 0, 0, 1, 1, 1, 2, 3, 4, 6, 9]
            ]
        }
    };

    public static CalculationResult<int> GetRacewaySize(ConductorWireType conductorWireType,
        RacewayType racewayType,
        CalculationResult<double> conductorSize,
        int wireCount,
        int setCount)
    {
        if (conductorSize.HasError) return CalculationResult<int>.Failure(conductorSize.ErrorType);

        Dictionary<RacewayType, List<List<int>>> conductorGroupWireCountTableDict;
        if (ConductorGroupOneWireTypes.Contains(conductorWireType))
            conductorGroupWireCountTableDict = ConductorGroupOneWireCountTableDict;
        else if (ConductorGroupTwoWireTypes.Contains(conductorWireType))
            conductorGroupWireCountTableDict = ConductorGroupTwoWireCountTableDict;
        else if (ConductorGroupThreeWireTypes.Contains(conductorWireType))
            conductorGroupWireCountTableDict = ConductorGroupThreeWireCountTableDict;
        else if (ConductorGroupFourWireTypes.Contains(conductorWireType))
            conductorGroupWireCountTableDict = ConductorGroupFourWireCountTableDict;
        else
            return CalculationResult<int>.Failure(CalculationErrorType.InvalidConductorWireType);

        var wireCountTable = conductorGroupWireCountTableDict[racewayType];
        
        var conductorSizeIndex = DataConstants.ConductorSizes.FindIndex(size => size.IsRoughlyEqualTo(conductorSize.Value));
        if (conductorSizeIndex == -1) return CalculationResult<int>.Failure(CalculationErrorType.NoFittingConductorSize);
        
        var wireCountColumn = wireCountTable[conductorSizeIndex];
        
        var columnIndex = wireCountColumn.FindIndex(columnWireCount => columnWireCount >= (wireCount * setCount));
        
        return columnIndex == -1
            ? CalculationResult<int>.Failure(CalculationErrorType.NoFittingRacewaySize)
            : CalculationResult<int>.Success(RacewaySizes[racewayType][columnIndex]);
    }
}