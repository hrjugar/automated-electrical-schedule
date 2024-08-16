namespace automated_electrical_schedule.Models;

public class SizeAmpereList(int?[] ampereList)
{
    private static readonly double[] Sizes =
    [
        2.0,
        3.5,
        5.5,
        8.0,
        14,
        22,
        30,
        38,
        50,
        60,
        80,
        100,
        125,
        150,
        175,
        200,
        250,
        325,
        375,
        400,
        500
    ];

    private List<int?> AmpereList { get; } = ampereList.ToList();

    public double GetConductorSize(int? ampacity)
    {
        return Sizes[AmpereList.IndexOf(ampacity)];
    }

    public int GetAmpereTrip(double ampereLoad)
    {
        return AmpereList.First(
            ampacity => ampacity != null && ampacity >= ampereLoad
        ) ?? 0;
    }
}

public class AmpacityColumn(List<ConductorType> conductorTypes, int?[] ampereList)
{
    public List<ConductorType> ConductorTypes { get; } = conductorTypes;
    public SizeAmpereList SizeAmpereList { get; } = new(ampereList);
}

public static class Ampacity
{
    public static readonly List<AmpacityColumn> Table =
    [
        new AmpacityColumn(
            [
                ConductorType.TwCu60,
                ConductorType.UfCu60
            ],
            [
                15,
                20,
                30,
                40,
                55,
                70,
                85,
                100,
                115,
                130,
                155,
                185,
                210,
                240,
                260,
                275,
                315,
                370,
                395,
                400,
                445
            ]
        ),

        new AmpacityColumn(
            [
                ConductorType.RhwCu75,
                ConductorType.ThhwCu75,
                ConductorType.ThwCu75,
                ConductorType.ThwnCu75,
                ConductorType.XhhwCu75,
                ConductorType.UseCu75,
                ConductorType.ZwCu75
            ],
            [
                20,
                25,
                35,
                50,
                65,
                85,
                100,
                115,
                140,
                155,
                190,
                220,
                255,
                285,
                305,
                325,
                375,
                435,
                470,
                480,
                530
            ]
        ),

        new AmpacityColumn(
            [
                ConductorType.TbsCu90,
                ConductorType.SaCu90,
                ConductorType.SisCu90,
                ConductorType.FepCu90,
                ConductorType.FepbCu90,
                ConductorType.MiCu90,
                ConductorType.RhhCu90,
                ConductorType.Rhw2Cu90,
                ConductorType.ThhnCu90,
                ConductorType.ThhwCu90,
                ConductorType.Thw2Cu90,
                ConductorType.Thwn2Cu90,
                ConductorType.Use2Cu90,
                ConductorType.XhhCu90,
                ConductorType.XhhwCu90,
                ConductorType.Xhhw2Cu90,
                ConductorType.Zw2Cu90
            ],
            [
                25,
                30,
                40,
                55,
                75,
                95,
                115,
                130,
                150,
                170,
                205,
                240,
                285,
                320,
                345,
                360,
                425,
                490,
                530,
                595
            ]
        ),

        new AmpacityColumn(
            [
                ConductorType.TwAl60,
                ConductorType.UfAl60
            ],
            [
                null,
                15,
                25,
                30,
                40,
                55,
                65,
                75,
                90,
                100,
                120,
                140,
                165,
                190,
                205,
                220,
                255,
                300,
                315,
                320,
                365
            ]
        ),

        new AmpacityColumn(
            [
                ConductorType.RhwAl75,
                ConductorType.ThhwAl75,
                ConductorType.ThwAl75,
                ConductorType.ThwnAl75,
                ConductorType.XhhwAl75,
                ConductorType.UseAl75
            ],
            [
                null,
                20,
                30,
                40,
                50,
                65,
                80,
                90,
                110,
                120,
                145,
                170,
                200,
                230,
                245,
                265,
                305,
                355,
                380,
                385,
                435
            ]
        ),

        new AmpacityColumn(
            [
                ConductorType.TbsAl90,
                ConductorType.SaAl90,
                ConductorType.SisAl90,
                ConductorType.FepAl90,
                ConductorType.FepbAl90,
                ConductorType.MiAl90,
                ConductorType.RhhAl90,
                ConductorType.Rhw2Al90,
                ConductorType.ThhnAl90,
                ConductorType.ThhwAl90,
                ConductorType.Thw2Al90,
                ConductorType.Thwn2Al90,
                ConductorType.Use2Al90,
                ConductorType.XhhAl90,
                ConductorType.XhhwAl90,
                ConductorType.Xhhw2Al90,
                ConductorType.Zw2Al90
            ],
            [
                null,
                25,
                35,
                45,
                65,
                80,
                90,
                105,
                125,
                135,
                165,
                190,
                225,
                255,
                275,
                300,
                345,
                405,
                430,
                440,
                485
            ]
        )
    ];

    public static double? GetAmpereTrip(
        ConductorType conductorType,
        double ampereLoad
    )
    {
        var column = Table.Find(
            x => x.ConductorTypes.Any(
                type => type.Material == conductorType.Material &&
                        type.TemperatureRating == conductorType.TemperatureRating &&
                        type.WireType == conductorType.WireType
            )
        );

        return column?.SizeAmpereList.GetAmpereTrip(ampereLoad);
    }
}