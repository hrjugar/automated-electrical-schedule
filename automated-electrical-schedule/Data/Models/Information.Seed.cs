using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.Models;

public partial class Information
{
    private const string CircuitTypeTitle = "Circuit Type";
    private const string QuantityTitle = "Quantity";
    private const string VoltAmpereTitle = "Volt Ampere";
    private const string VoltageTitle = "Voltage";
    private const string AmpereLoadTitle = "Ampere Load";
    private const string AmpereTripTitle = "Ampere Trip";
    private const string PhaseTitle = "Phase";
    private const string PoleTitle = "Pole";
    private const string VoltageDropTitle = "Voltage Drop";
    private const string ConductorTitle = "Conductor";
    private const string GroundingTitle = "Grounding";
    private const string RacewayTitle = "Raceway";
    
    private const string LightingSubtitle = "Lighting Outlet";
    private const string ConvenienceSubtitle = "Convenience Outlet";
    private const string MotorSubtitle = "Motor Outlet";
    private const string ApplianceSubtitle = "Appliance Outlet";
    
    private const string SinglePhaseSubtitle = "Single Phase";
    private const string ThreePhaseSubtitle = "Three Phase";
    
    private const string EmtSubtitle = "EMT";
    private const string EntSubtitle = "ENT";
    private const string ImcSubtitle = "IMC";
    private const string FmcSubtitle = "FMC";
    private const string PvcSubtitle = "PVC";
    private const string RmcSubtitle = "RMC";
    private const string CableTraySubtitle = "Cable Tray";
    
    public static readonly Information TempInfo = new(
        "Title",
        "Subtitle",
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
    );
    
    /* --- CIRCUIT TYPE --- */
    
    public static readonly Information LightingCircuitType = new(
        CircuitTypeTitle,
        LightingSubtitle,
        "A lighting outlet is a type of electrical socket made specifically to connect light fixtures. Lighting fixtures like wall sconces, chandeliers, ceiling lights, and lamps receive the electricity they require from it."
    );

    public static readonly Information ConvenienceCircuitType = new(
        CircuitTypeTitle,
        ConvenienceSubtitle,
        "A convenience outlet, also known as a wall outlet or receptacle, is an electrical device installed in walls or floors to provide access to electrical power for appliances and devices."
    );

    public static readonly Information MotorCircuitType = new(
        CircuitTypeTitle,
        MotorSubtitle,
        "A motor outlet is an electrical connection point designed to supply power to an electric motor.The outlet is critical for safely powering motors, often equipped with features like grounding and protection against overloads or short circuits."
    );

    public static readonly Information ApplianceCircuitType = new(
        CircuitTypeTitle,
        ApplianceSubtitle,
        "An appliance outlet is an electrical receptacle designed to provide power to household or commercial appliances. Appliance outlets are commonly found in kitchens, laundry rooms, and other areas where large appliances are used."
    );
    
    /* --- QUANTITY --- */

    public static readonly Information Quantity = new(
        QuantityTitle,
        "Quantity refers to an amount or number of a material or immaterial thing."
    );
    
    /* --- VOLT AMPERE --- */
    
    // TODO: Add Lighting Outlet VA Information
    public static readonly Information LightingVoltAmpere = new(
        VoltAmpereTitle,
        LightingSubtitle,
        "Apparent power is the total power in an AC circuit, combining both active (real) power, which performs actual work, and reactive power, which sustains the electric and magnetic fields. It is measured in volt-amperes (VA) and calculated as the product of the circuit's voltage and current, without considering the phase angle between them."
    );

    public static readonly Information ConvenienceVoltAmpere = new(
        VoltAmpereTitle,
        ConvenienceSubtitle,
        [
            new Reference(
                "“Receptacle Outlets”, Philippine Electrical Code Part 1, Chap. 2.20, no. 2.20.2.3, pp. 55, 2017.",
                [
                    "All occupancies follow 180 VA receptacles on one yoke, when 4 or more receptacles, it should be 90-volt ampere per receptacle."
                ]
            )
        ]
    );

    public static readonly Information MotorVoltAmpere = new(
        VoltAmpereTitle,
        MotorSubtitle,
        "It is measured in volt-amperes (VA) and calculated as the product of the circuit's voltage and current, without considering the phase angle between them. it is acquired when the given HP of the motor has a designated current from the table, then multiply to 230V."
    );

    public static readonly Information ApplianceVoltAmpere = new(
        VoltAmpereTitle,
        ApplianceSubtitle,
        "Apparent power is the total power in an AC circuit, combining both active (real) power, which performs actual work, and reactive power, which sustains the electric and magnetic fields. It is measured in volt-amperes (VA) and calculated as the product of the circuit's voltage and current, without considering the phase angle between them."
    );
    
    /* --- VOLTAGE --- */

    public static readonly Information SinglePhaseVoltage = new(
        VoltageTitle,
        SinglePhaseSubtitle,
        "Voltage, also known as electric potential difference, is a measure of the potential energy per unit charge between two points in an electrical field. Since the program is in a Single Phase, the Schedule of Loads is constant at 230V."
    );

    public static readonly Information ThreePhaseVoltage = new(
        VoltageTitle,
        ThreePhaseSubtitle,
        "Voltage, also known as electric potential difference, is a measure of the potential energy per unit charge between two points in an electrical field. Since the program is in a Three Phase, the Schedule of Loads can be 230, 400, 460, and 575 Volts."
    );
    
    /* --- AMPERE LOAD --- */

    private const string SinglePhaseAmpereLoadDescription =
        "Electric current is the flow of electric charge in a conductor, typically measured in amperes (A).. To solve the current, Apparent power divided by Voltage (230V), multiple the demand factor.";

    public static readonly Information LightingAmpereLoad = new(
        AmpereLoadTitle,
        LightingSubtitle,
        SinglePhaseAmpereLoadDescription
    );
    
    public static readonly Information ConvenienceAmpereLoad = new(
        AmpereLoadTitle,
        ConvenienceSubtitle,
        SinglePhaseAmpereLoadDescription
    );

    public static readonly Information SinglePhaseMotorAmpereLoad = new(
        AmpereLoadTitle,
        $"{MotorSubtitle} - {SinglePhaseSubtitle}",
        [
            new Reference(
                "“Full-Load Current in Amperes, Single-Phase Alternating-Current Motors”, Philippine Electrical Code Part 1, Chap. 4.30, no. 4.30.14.2, pp. 370, 2017.",
                [
                    "Table for full-load current in single-phase motors, where it inputs the current based on the horsepower of the motor."
                ]
            )
        ]
    );

    public static readonly Information ThreePhaseMotorAmpereLoad = new(
        AmpereLoadTitle,
        $"{MotorSubtitle} - {ThreePhaseSubtitle}",
        [
            new Reference(
                "“Full-Load Current, Three-Phase Alternating - Current Motors”,\nPhilippine Electrical Code Part 1, Chap. 4.30,no. 4.30.14.4, pp. 372, 2017.",
                [
                    "Table for full-load current in Three-phase motors, where it inputs the current based on the horsepower of the motor."
                ]
            )
        ]
    );

    public static readonly Information ApplianceAmpereLoad = new(
        AmpereLoadTitle,
        ApplianceSubtitle,
        SinglePhaseAmpereLoadDescription
    );
    
    /* --- AMPERE TRIP --- */

    private static readonly Reference PermisibleLoadsReference = new(
        "“Permissible Loads, Multiple-Outlet Branch Circuits”, Philippine Electrical Code Part 1, Chap. 2.10, no. 2.10.2.5, pp. 45, 2017.",
        [
            "It states that it should not exceed 80% of the breaker."
        ]
    );
    
    private static readonly Reference StandardAmpereRatingsForFusesAndInverseTimeCircuitBreakersReference = new(
        "“Standard Ampere Ratings for Fuses and Inverse Time Circuit Breakers”, Philippine Electrical Code Part 1, Chap. 2.40, no. 2.40.1.6(A), pp. 86, 2017.",
        [
            "Table of Standard Ampere Ratings for Fuses and Inverse Time Circuit Breakers"
        ]
    );
    
    public static readonly Information LightingAmpereTrip = new(
        AmpereTripTitle,
        LightingSubtitle,
        [
            new Reference(
                "“Permissible Loads, Multiple-Outlet Branch Circuits”, Philippine Electrical Code Part 1, Chap. 2.10, no. 2.10.2.5, pp. 45, 2017.",
                [
                    "It states that it should not exceed 80% of the breaker.",
                    "15 and 20 AT are minimum requirements for lighting units."
                ]
            ),
            StandardAmpereRatingsForFusesAndInverseTimeCircuitBreakersReference
        ]
    );

    public static readonly Information ConvenienceAmpereTrip = new(
        AmpereTripTitle,
        ConvenienceSubtitle,
        [
            PermisibleLoadsReference,
            StandardAmpereRatingsForFusesAndInverseTimeCircuitBreakersReference,
            new Reference(
                "“Small Appliances (Receptacle Outlets Served)”, Philippine Electrical Code Part 1, Chap. 2.10, no. 2.10.3.3, pp. 46, 2017",
                [
                    "All receptacles outlets should use 20 ampere small appliance branch circuit or more for every wall, floor, countertop, and receptacle outlets."
                ]
            )
        ]
    );

    public static readonly Information MotorAmpereTrip = new(
        AmpereTripTitle,
        MotorSubtitle,
        [
            StandardAmpereRatingsForFusesAndInverseTimeCircuitBreakersReference,
            new Reference(
                "“Maximum Rating or setting of Motor Branch- Circuit short circuit and ground fault protective device”, Philippine Electrical Code Part 1, Chap. 4.30, no. 4.30.4.2, pp. 356, 2017",
                [
                    "Table of Motors protective device. All motors must have a safety allowance for sizing the breakers since motors draw high current during startups."
                ]
            )
        ]
    );

    public static readonly Information ApplianceAmpereTrip = new(
        AmpereTripTitle,
        ApplianceSubtitle,
        [
            PermisibleLoadsReference,
            StandardAmpereRatingsForFusesAndInverseTimeCircuitBreakersReference,
            new Reference(
                "“Small Appliances (Receptacle Outlets Served)”, Philippine Electrical Code Part 1, Chap. 2.10, no. 2.10.3.3, pp. 46, 2017",
                [
                    "All receptacles outlets should use 20 ampere small appliance branch circuit or more for every wall, floor, countertop, and receptacle outlets."
                ]
            )
        ]
    );
    
    /* --- PHASE --- */

    public static readonly Information SinglePhasePhase = new(
        PhaseTitle,
        SinglePhaseSubtitle,
        "Single-phase electricity, a type of alternating current, is commonly used in residential and commercial applications, involving live and neutral wires, and sometimes a ground wire for safety."
    );

    public static readonly Information ThreePhasePhase = new(
        PhaseTitle,
        ThreePhaseSubtitle,
        "A three-phase system is an efficient electrical power distribution method using three alternating currents, each offset by 120 degrees, commonly used in industrial and commercial settings for consistent power flow."
    );
    
    /* --- POLE --- */

    public static readonly Information SinglePhasePole = new(
        PoleTitle,
        SinglePhaseSubtitle,
        "The term \"pole\" in electrical system design refers to the number of circuits a breaker or switch can control, crucial for power distribution within a building. A double-pole (DP) breaker/switch controls two circuits simultaneously and has a Hot wire and neutral wire."
    );

    public static readonly Information ThreePhasePole = new(
        PoleTitle,
        ThreePhaseSubtitle,
        "The term \"pole\" in electrical system design refers to the number of circuits a breaker or switch can control, crucial for power distribution within a building. A 2-pole  breaker/switch controls two circuits simultaneously and has a Hot wire and neutral wire. While 3-pole have 3 poles, each for one phase (L1, L2, L3)."
    );
    
    /* --- VOLTAGE DROP --- */

    public static readonly Information VoltageDrop = new(
        VoltageDropTitle,
        [
            new Reference(
                "“Branch Circuit Ratings”, Philippine Electrical Code Part 1, Chap. 2.10.2, no. 2.10.2.2FPN No.4, pp. 42, 2017",
                [
                    "The Branch Circuit must not exceed a 3% Voltage Drop.",
                    "The feeders and Branch Circuit must not exceed 5%"
                ]
            ),
            new Reference(
                "“The alternating current resistance and reactance for 600 volts cables, 3-phase, 60Hz, 75oC three single conductors in a conduit”, Philippine Electrical Code Part 1, Chap. 10, no. 10.1.1.9, pp. 942, 2017.",
                [
                    "Table for resistance and reactance on solving the voltage drop"
                ]
            )
        ]
    );
    
    /* --- GROUNDING --- */

    public static readonly Information Grounding = new(
        GroundingTitle,
        [
            new Reference(
                "“Minimum size equipment Grounding Conductors for Grounding raceway and equipment”, Philippine Electrical Code Part 1, Chap. 2.50, no. 2.50.6.13, pp. 124, 2017.",
                [
                    "Table for minimum ground conductors"
                ]
            )
        ]
    );
    
    /* --- RACEWAY --- */

    public static readonly Information EmtRaceway = new(
        RacewayTitle,
        EmtSubtitle,
        [
            new Reference(
                "“Maximum Number of Conductors or fixture wires in Electrical Metallic Tubing (EMT)”, Philippine Electrical Code Part 1, Chap. 10, C.1, pp. 967-970, 2017.",
                [
                    "Electrical Metallic Tubing (EMT) is a thin wall raceway made of steel and aluminum, designed for physical protection and channeling conductors and cables. It is suitable for exposed and concealed environments, corrosive environments, cinder fill, and wet locations. However, it is not suitable for physical damage and corrosion protection is limited to enamel."
                ]
            )
        ]
    );

    public static readonly Information EntRaceway = new(
        RacewayTitle,
        EntSubtitle,
        [
            new Reference(
                "“Maximum Number of Conductors or Fixture Wires in Electrical Nonmetallic Tubing (ENT)”, Philippine Electrical Code Part 1, Chap. 10, C.2, pp. 973-976, 2017",
                [
                    "Electrical Nonmetallic Tubing (ENT) is a pliable, corrugated raceway used for installing electrical conductors in buildings not exceeding three floors above grade. It can be bent by hand without equipment. ENT must be concealed within walls, floors, and ceilings with thermal barriers or materials with a 15-minute finish rating. Not permitted uses include hazard locations, supporting luminaires, high ambient temperatures, direct earth burial, 600 Volt voltage, and physical damage."
                ]
            )
        ]
    );
    
    public static readonly Information ImcRaceway = new(
        RacewayTitle,
        ImcSubtitle,
        [
            new Reference(
                "Maximum Number of Conductors or Fixture Wires in Intermediate Metal Conduit”, Philippine Electrical Code Part 1, Chap. 10, C.4, pp. 985-988, 2017",
                [
                    "An Intermediate Metal Conduit is a circular steel raceway designed for physical protection, routing conductors, and equipment grounding. It is suitable in all atmospheric conditions, corrosion environments, cinder fill, and wet locations. In corrosion environments, it should be installed in concrete, in direct contact with the earth, and protected by noncinder concrete."
                ]
            )
        ]
    );
    
    public static readonly Information FmcRaceway = new(
        RacewayTitle,
        FmcSubtitle,
        [
            new Reference(
                "“Maximum Number of Conductors or Fixture Wires in Flexible Metal Conduit”, Philippine Electrical Code Part 1, Chap. 10, C.3, pp. 979-982, 2017.",
                [
                    "A flexible metal conduit is a circular raceway made of interlocking metal strips, suitable for exposed and concealed locations but not in wet, hoistways, hazardous areas, hazardous areas, or areas with deteriorating materials."
                ]
            )
        ]
    );

    public static readonly Information PvcRaceway = new(
        RacewayTitle,
        PvcSubtitle,
        [
            new Reference(
                "Maximum Number of Conductors or Fixture Wires in Polyvinylchloride Conduit”, Philippine Electrical Code Part 1, Chap. 10, C.12, pp. 1033-1036, 2017.",
                [
                    "Polyvinyl Chloride conduit (PVC) is a blend of plastic and vinyl, suitable for various applications such as concealed, corrosive, wet, dry, damp, exposed, underground, and support of conduit bodies. It is permitted in areas with frequent washing, exposed in areas of physical damage, and underground for direct burial and covered with concrete. However, it is not suitable for hazardous locations, physical damage, or ambient temperatures exceeding 50 degrees Celsius unless specified otherwise."
                ]
            )
        ]
    );
    
    public static readonly Information RmcRaceway = new(
        RacewayTitle,
        RmcSubtitle,
        [
            new Reference(
                "“Maximum Number of Conductors or Fixture Wires in Rigid Metal Conduit”, Philippine Electrical Code Part 1, Chap. 10, C.9, pp. 1015-1018, 2017",
                [
                    "Rigid Metal Conduit (RMC) is a circular cross section designed for physical protection and routing conductors and cables. It is made of steel with protective coatings or aluminum. RMC is permitted in all atmospheric conditions, corrosion environments, cinder fill, and wet locations. In corrosion environments, RMC should be installed in concrete, under cinder fill, or protected by corrosion protection."
                ]
            )
        ]
    );
    
    // TODO: Cable tray raceway information area
    public static readonly Information CableTrayRaceway = new(
        RacewayTitle,
        CableTraySubtitle,
        [
            new Reference(
                "“Allowable Cable Fill Area for Single conductor Cables in Ladder, Ventilated Trough, or Solid Bottom Cable Trays for Cables Rated 2000 Volts or less”, Philippine Electrical Code Part 1, Chap. 3.92, no. 3.92.2.13(A), pp. 269, 2017.",
                [
                    "Cable tray is a set of units forming a structural system used to fasten or support Cables and Raceways. Cable Trays are permitted in following areas: Support system for service conductors, feeders, branch circuits, communications circuits, control circuits and signal circuits. Furthermore, it can be also used in exposed sunlight, whereas, the cables must be sun resistant"
                ]
            )
        ]
    );
    
    /* --- METHODS --- */

    public static Information GenerateCircuitTypeInfo(CircuitType circuitType)
    {
        return circuitType switch
        {
            CircuitType.LightingOutlet => LightingCircuitType,
            CircuitType.ConvenienceOutlet => ConvenienceCircuitType,
            CircuitType.MotorOutlet => MotorCircuitType,
            CircuitType.ApplianceEquipmentOutlet => ApplianceCircuitType,
            _ => TempInfo
        };
    }

    public static Information GenerateVoltAmpereInfo(CircuitType circuitType)
    {
        return circuitType switch
        {
            CircuitType.LightingOutlet => LightingVoltAmpere,
            CircuitType.ConvenienceOutlet => ConvenienceVoltAmpere,
            CircuitType.MotorOutlet => MotorVoltAmpere,
            CircuitType.ApplianceEquipmentOutlet => ApplianceVoltAmpere,
            _ => TempInfo
        };
    }

    public static Information GenerateVoltageInfo(int phase)
    {
        return phase switch
        {
            1 => SinglePhaseVoltage,
            3 => ThreePhaseVoltage,
            _ => TempInfo
        };
    }

    public static Information GenerateAmpereLoadInfo(CircuitType circuitType, LineToLineVoltage lineToLineVoltage)
    {
        return circuitType switch
        {
            CircuitType.LightingOutlet => LightingAmpereLoad,
            CircuitType.ConvenienceOutlet => ConvenienceAmpereLoad,
            CircuitType.MotorOutlet => lineToLineVoltage == LineToLineVoltage.Abc
                ? ThreePhaseMotorAmpereLoad
                : SinglePhaseMotorAmpereLoad,
            CircuitType.ApplianceEquipmentOutlet => ApplianceAmpereLoad,
            _ => TempInfo
        };
    }

    public static Information GenerateAmpereTripInfo(CircuitType circuitType)
    {
        return circuitType switch
        {
            CircuitType.LightingOutlet => LightingAmpereTrip,
            CircuitType.ConvenienceOutlet => ConvenienceAmpereTrip,
            CircuitType.MotorOutlet => MotorAmpereTrip,
            CircuitType.ApplianceEquipmentOutlet => ApplianceAmpereTrip,
            _ => TempInfo
        };
    }
    
    public static Information GeneratePhaseInfo(int phase)
    {
        return phase switch
        {
            1 => SinglePhasePhase,
            3 => ThreePhasePhase,
            _ => TempInfo
        };
    }
    
    public static Information GeneratePoleInfo(int pole)
    {
        return pole switch
        {
            1 => SinglePhasePole,
            2 or 3 => ThreePhasePole,
            _ => TempInfo
        };
    }

    public static Information GenerateConductorInfo(ConductorWireType conductorWireType)
    {
        var subtitle = conductorWireType.GetDisplayName();

        var name = string.Empty;
        var application = string.Empty;
        string? insulation = null;
        string? outerCovering = null;
        var temp = string.Empty;

        if (conductorWireType == ConductorWireType.Rhh)
        {
            name = "Thermoset";
            temp = "90";
            application = "in dry and damp locations";
            outerCovering = "moisture-resistant, flame-retardant, and non-metallic";
        } 
        else if (conductorWireType is ConductorWireType.Rhw or ConductorWireType.Rhw2)
        {
            name = "Moisture-resistant thermoset";
            temp = conductorWireType is ConductorWireType.Rhw ? "75" : "90";
            application = "in dry and wet locations";
            insulation = "flame-retardant and moisture-resistant";
            outerCovering = "moisture-resistand, flame-retardant, and non-metallic thermoset";
        } 
        else if (conductorWireType is ConductorWireType.Thhn)
        {
            name = "Heat-resistant thermoplastic";
            temp = "90";
            application = "in dry and damp locations";
            insulation = "flame-retardant and heat-resistant";
            outerCovering = "nylon jacket or any equivalent thermoplastic";
        }
        else if (conductorWireType is ConductorWireType.Thhw)
        {
            name = "Moisture and heat resistant thermoplastic";
            temp = "75/90";
            application = "in dry and wet locations";
            insulation = "flame-retardant, moisture and heat-resistant thermoplastic";
        } 
        else if (conductorWireType is ConductorWireType.Thw)
        {
            name = "Moisture and heat resistant thermoplastic";
            temp = "75/90";
            application = "in dry and wet locations, and has special applications within electric discharge lighting equipment limited to 1000 open circuit volts or less";
            insulation = "flame-retardant, moisture and heat-resistant thermoplastic";
        }
        else if (conductorWireType is ConductorWireType.Thw2)
        {
            name = "Moisture and heat resistant thermoplastic";
            temp = "90";
            application = "in dry and wet locations";
            insulation = "flame-retardant, moisture and heat-resistant thermoplastic";
        }
        else if (conductorWireType is ConductorWireType.Thwn or ConductorWireType.Thwn2)
        {
            name = "Moisture and heat resistant thermoplastic";
            temp = conductorWireType is ConductorWireType.Thwn ? "75" : "90";
            application = "in dry and wet locations";
            insulation = "flame-retardant, moisture and heat-resistant thermoplastic";
            outerCovering = "nylon jacket or any equivalent";
        }
        else if (conductorWireType is ConductorWireType.Tw)
        {
            name = "Moisture resistant thermoplastic";
            temp = "60";
            application = "in dry and wet locations";
            insulation = "moisture and heat-resistant thermoplastic";
        }
        else if (conductorWireType is ConductorWireType.Xhh)
        {
            name = "Thermoset";
            temp = "90";
            application = "in dry and damp locations";
            insulation = "flame-retardant and thermoset";
            outerCovering = "nylon jacket or any equivalent";
        }
        else if (conductorWireType is ConductorWireType.Xhhw)
        {
            name = "Moisture-resistant thermoset";
            temp = "90";
            application = "in dry and damp or wet locations";
            insulation = "flame-retardant and moisture-resistant thermoset";
        }
        else if (conductorWireType is ConductorWireType.Xhhw2)
        {
            name = "Moisture-resistant thermoset";
            temp = "90";
            application = "in dry and wet locations";
            insulation = "flame-retardant and moisture-resistant thermoset";
        } else if (conductorWireType is ConductorWireType.Zw)
        {
            name = "Modified ethylene tetrafluoroethylene";
            temp = "90";
            application = "in dry locations, and is specially used in dry and wet locations";
            insulation = "modified ethylene tetrafluoroethylene";
        }
        
        return new Information(
            ConductorTitle,
            subtitle,
            [
                new Reference(
                    "Reference (1): “Conductor Applications and Insulation Rated 600 Volts”, Philippine Electrical Code Part 1, Chap. 2.50, no. 3.10.3.1(A), pp. 187, 2017.",
                    [
                        "TableConductor - Applications and Insulation Rated 600 Volts",
                        $"{name} ({conductorWireType.GetDisplayName()}) has a max temperature of {temp}\u00b0C. It is used {application}. {(insulation is null ? "" : $"Its insulation is {insulation}.")} {(outerCovering is null ? "" : $"Its outer covering is {outerCovering}.")}"
                    ]
                ),
                new Reference(
                    "“Allowable Ampacities of Insulated Conductors Rated up to and including 2000 volts 60o through 90o C, not more than three current- carrying conductors in raceways, Cable, or Earth”, Philippine Electrical Code Part 1, Chap. 3.10, no. 3.10.2.6(B)(16), pp. 174, 2017.",
                    [
                        "A conductor table is common for referencing size of the conductors rated up to 2000 V"
                    ]
                )
            ]
        );
    }

    public static Information GenerateRacewayInfo(RacewayType raceway)
    {
        return raceway switch
        {
            RacewayType.Emt => EmtRaceway,
            RacewayType.Ent => EntRaceway,
            RacewayType.Imc => ImcRaceway,
            RacewayType.Fmc => FmcRaceway,
            RacewayType.Pvc => PvcRaceway,
            RacewayType.Rmc => RmcRaceway,
            RacewayType.CableTray => CableTrayRaceway,
            _ => TempInfo
        };
    }
}