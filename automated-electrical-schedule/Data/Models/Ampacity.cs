// using automated_electrical_schedule.Data;
//
// namespace automated_electrical_schedule.Models;
//
// public class SizeAmpereList(int?[] ampereList)
// {
//     private static readonly double[] Sizes =
//     [
//         2.0,
//         3.5,
//         5.5,
//         8.0,
//         14,
//         22,
//         30,
//         38,
//         50,
//         60,
//         80,
//         100,
//         125,
//         150,
//         175,
//         200,
//         250,
//         325,
//         375,
//         400,
//         500
//     ];
//
//     private List<int?> AmpereList { get; } = ampereList.ToList();
//
//     public double GetConductorSize(int? ampacity)
//     {
//         return Sizes[AmpereList.IndexOf(ampacity)];
//     }
//
//     public int GetAmpereTrip(double ampereLoad)
//     {
//         return AmpereList.First(
//             ampacity => ampacity != null && ampacity >= ampereLoad
//         ) ?? 0;
//     }
// }
//
// public class AmpacityColumn(List<ConductorType> conductorTypes, int?[] ampereList)
// {
//     public List<ConductorType> ConductorTypes { get; } = conductorTypes;
//     public SizeAmpereList SizeAmpereList { get; } = new(ampereList);
// }
//
// public static class Ampacity
// {
//     public static readonly List<AmpacityColumn> Table =
//     [
//         new AmpacityColumn(
//             [
//                 PrepopulatedConductorTypes.TwCu60,
//                 PrepopulatedConductorTypes.UfCu60
//             ],
//             [
//                 15,
//                 20,
//                 30,
//                 40,
//                 55,
//                 70,
//                 85,
//                 100,
//                 115,
//                 130,
//                 155,
//                 185,
//                 210,
//                 240,
//                 260,
//                 275,
//                 315,
//                 370,
//                 395,
//                 400,
//                 445
//             ]
//         ),
//
//         new AmpacityColumn(
//             [
//                 PrepopulatedConductorTypes.RhwCu75,
//                 PrepopulatedConductorTypes.ThhwCu75,
//                 PrepopulatedConductorTypes.ThwCu75,
//                 PrepopulatedConductorTypes.ThwnCu75,
//                 PrepopulatedConductorTypes.XhhwCu75,
//                 PrepopulatedConductorTypes.UseCu75,
//                 PrepopulatedConductorTypes.ZwCu75
//             ],
//             [
//                 20,
//                 25,
//                 35,
//                 50,
//                 65,
//                 85,
//                 100,
//                 115,
//                 140,
//                 155,
//                 190,
//                 220,
//                 255,
//                 285,
//                 305,
//                 325,
//                 375,
//                 435,
//                 470,
//                 480,
//                 530
//             ]
//         ),
//
//         new AmpacityColumn(
//             [
//                 PrepopulatedConductorTypes.TbsCu90,
//                 PrepopulatedConductorTypes.SaCu90,
//                 PrepopulatedConductorTypes.SisCu90,
//                 PrepopulatedConductorTypes.FepCu90,
//                 PrepopulatedConductorTypes.FepbCu90,
//                 PrepopulatedConductorTypes.MiCu90,
//                 PrepopulatedConductorTypes.RhhCu90,
//                 PrepopulatedConductorTypes.Rhw2Cu90,
//                 PrepopulatedConductorTypes.ThhnCu90,
//                 PrepopulatedConductorTypes.ThhwCu90,
//                 PrepopulatedConductorTypes.Thw2Cu90,
//                 PrepopulatedConductorTypes.Thwn2Cu90,
//                 PrepopulatedConductorTypes.Use2Cu90,
//                 PrepopulatedConductorTypes.XhhCu90,
//                 PrepopulatedConductorTypes.XhhwCu90,
//                 PrepopulatedConductorTypes.Xhhw2Cu90,
//                 PrepopulatedConductorTypes.Zw2Cu90
//             ],
//             [
//                 25,
//                 30,
//                 40,
//                 55,
//                 75,
//                 95,
//                 115,
//                 130,
//                 150,
//                 170,
//                 205,
//                 240,
//                 285,
//                 320,
//                 345,
//                 360,
//                 425,
//                 490,
//                 530,
//                 595
//             ]
//         ),
//
//         new AmpacityColumn(
//             [
//                 PrepopulatedConductorTypes.TwAl60,
//                 PrepopulatedConductorTypes.UfAl60
//             ],
//             [
//                 null,
//                 15,
//                 25,
//                 30,
//                 40,
//                 55,
//                 65,
//                 75,
//                 90,
//                 100,
//                 120,
//                 140,
//                 165,
//                 190,
//                 205,
//                 220,
//                 255,
//                 300,
//                 315,
//                 320,
//                 365
//             ]
//         ),
//
//         new AmpacityColumn(
//             [
//                 PrepopulatedConductorTypes.RhwAl75,
//                 PrepopulatedConductorTypes.ThhwAl75,
//                 PrepopulatedConductorTypes.ThwAl75,
//                 PrepopulatedConductorTypes.ThwnAl75,
//                 PrepopulatedConductorTypes.XhhwAl75,
//                 PrepopulatedConductorTypes.UseAl75
//             ],
//             [
//                 null,
//                 20,
//                 30,
//                 40,
//                 50,
//                 65,
//                 80,
//                 90,
//                 110,
//                 120,
//                 145,
//                 170,
//                 200,
//                 230,
//                 245,
//                 265,
//                 305,
//                 355,
//                 380,
//                 385,
//                 435
//             ]
//         ),
//
//         new AmpacityColumn(
//             [
//                 PrepopulatedConductorTypes.TbsAl90,
//                 PrepopulatedConductorTypes.SaAl90,
//                 PrepopulatedConductorTypes.SisAl90,
//                 PrepopulatedConductorTypes.FepAl90,
//                 PrepopulatedConductorTypes.FepbAl90,
//                 PrepopulatedConductorTypes.MiAl90,
//                 PrepopulatedConductorTypes.RhhAl90,
//                 PrepopulatedConductorTypes.Rhw2Al90,
//                 PrepopulatedConductorTypes.ThhnAl90,
//                 PrepopulatedConductorTypes.ThhwAl90,
//                 PrepopulatedConductorTypes.Thw2Al90,
//                 PrepopulatedConductorTypes.Thwn2Al90,
//                 PrepopulatedConductorTypes.Use2Al90,
//                 PrepopulatedConductorTypes.XhhAl90,
//                 PrepopulatedConductorTypes.XhhwAl90,
//                 PrepopulatedConductorTypes.Xhhw2Al90,
//                 PrepopulatedConductorTypes.Zw2Al90
//             ],
//             [
//                 null,
//                 25,
//                 35,
//                 45,
//                 65,
//                 80,
//                 90,
//                 105,
//                 125,
//                 135,
//                 165,
//                 190,
//                 225,
//                 255,
//                 275,
//                 300,
//                 345,
//                 405,
//                 430,
//                 440,
//                 485
//             ]
//         )
//     ];
//
//     public static double? GetAmpereTrip(
//         ConductorType conductorType,
//         double ampereLoad
//     )
//     {
//         var column = Table.Find(
//             x => x.ConductorTypes.Any(
//                 type => type.Material == conductorType.Material &&
//                         type.TemperatureRating == conductorType.TemperatureRating &&
//                         type.WireType == conductorType.WireType
//             )
//         );
//
//         return column?.SizeAmpereList.GetAmpereTrip(ampereLoad);
//     }
// }

