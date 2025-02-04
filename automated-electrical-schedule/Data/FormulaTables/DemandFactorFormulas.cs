using automated_electrical_schedule.Data.Models;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class DemandFactorFormulas
{
    // Returns remainder ranges
    public static List<double> ApplyDemandFactorToDwellingUnitLightingAndConvenienceCircuits(double voltAmpere)
    {
        // double result = 0;
        //
        // if (voltAmpere > 120000)
        // {
        //     result += (voltAmpere - 120000) * 0.25;
        //     voltAmpere = 120000;
        // }
        //
        // if (voltAmpere > 3000)
        // {
        //     result += (voltAmpere - 3000) * 0.35;
        //     voltAmpere = 3000;
        // }
        //
        // result += voltAmpere;
        //
        // return result;
        double firstRangeValue, secondRangeValue, thirdRangeValue;

        if (voltAmpere > 120000)
        {
            thirdRangeValue = (voltAmpere - 120000) * 0.25;
            secondRangeValue = (120000 - 3000) * 0.35;
            firstRangeValue = 3000;
        } 
        else if (voltAmpere > 3000)
        {
            thirdRangeValue = 0;
            secondRangeValue = (voltAmpere - 3000) * 0.35;
            firstRangeValue = 3000;
        }
        else
        {
            thirdRangeValue = 0;
            secondRangeValue = 0;
            firstRangeValue = voltAmpere;
        }

        return [firstRangeValue, secondRangeValue, thirdRangeValue];
    }

    public static List<double> ApplyDemandFactorToHospitalLightingCircuits(double voltAmpere)
    {
        double firstRangeValue, secondRangeValue;
        
        if (voltAmpere > 50000)
        {
            secondRangeValue = (voltAmpere - 50000) * 0.2;
            firstRangeValue = 50000 * 0.4;
        }
        else
        {
            secondRangeValue = 0;
            firstRangeValue = voltAmpere * 0.4;
        }
        
        return [firstRangeValue, secondRangeValue];
    }

    public static List<double> ApplyDemandFactorToHotelMotelApartmentLightingCircuits(double voltAmpere)
    {
        double firstRangeValue, secondRangeValue, thirdRangeValue;
        
        if (voltAmpere > 100000)
        {
            thirdRangeValue = (voltAmpere - 100000) * 0.3;
            secondRangeValue = (100000 - 20000) * 0.4;
            firstRangeValue = 20000 * 0.5;
        }
        else if (voltAmpere > 20000)
        {
            thirdRangeValue = 0;
            secondRangeValue = (voltAmpere - 20000) * 0.4;
            firstRangeValue = 20000 * 0.5;
        }
        else
        {
            thirdRangeValue = 0;
            secondRangeValue = 0;
            firstRangeValue = voltAmpere * 0.5;
        }

        return [firstRangeValue, secondRangeValue, thirdRangeValue];
    }

    public static List<double> ApplyDemandFactorToWarehouseLightingCircuits(double voltAmpere)
    {
        double firstRangeValue, secondRangeValue;
        
        if (voltAmpere > 12500)
        {
            secondRangeValue = (voltAmpere - 12500) * 0.5;
            firstRangeValue = 12500;
        }
        else
        {
            secondRangeValue = 0;
            firstRangeValue = voltAmpere;
        }
        
        return [firstRangeValue, secondRangeValue];
    }

    public static List<double> ApplyDemandFactorToNonDwellingConvenienceCircuits(double voltAmpere)
    {
        double firstRangeValue, secondRangeValue;
        
        if (voltAmpere > 10000)
        {
            secondRangeValue = (voltAmpere - 10000) * 0.5;
            firstRangeValue = 10000;
        }
        else
        {
            secondRangeValue = 0;
            firstRangeValue = voltAmpere;
        }
        
        return [firstRangeValue, secondRangeValue];
    }

    public static double ApplyDemandFactorToDryers(List<ApplianceEquipmentOutletCircuit> dryers)
    {
        var count = dryers.Count();
        var voltAmpere = dryers.Sum(d => d.VoltAmpere.Value);
        
        var demandFactor = count switch
        {
            0 => 0,
            > 0 and <= 4 => 1,
            5 => 0.85,
            6 => 0.75,
            7 => 0.65,
            8 => 0.6,
            9 => 0.55,
            10 => 0.5,
            11 => 0.47,
            <= 23 => (47 - (count - 11.0)) / 100,
            <= 42 => (35 - 0.5 * (count - 23)) / 100,
            _ => 43
        };

        return voltAmpere * demandFactor;
    }
    
    public static List<double> ApplyDemandFactorToDwellingUnitKitchenEquipment(IEnumerable<ApplianceEquipmentOutletCircuit> kitchenEquipment)
    {
        var lessThan3500 = kitchenEquipment.Where(ke => ke.VoltAmpere.Value < 3500);
        var lessThan3500VoltAmpere = lessThan3500.Sum(ke => ke.VoltAmpere.Value);
        var lessThan3500DemandFactor = lessThan3500.Count() switch
        {
            0 => 0,
            1 => 0.8,
            2 => 0.75,
            3 => 0.7,
            4 => 0.66,
            5 => 0.62,
            6 => 0.59,
            7 => 0.56,
            8 => 0.53,
            9 => 0.51,
            10 => 0.49,
            11 => 0.47,
            12 => 0.45,
            13 => 0.43,
            14 => 0.41,
            15 => 0.40,
            16 => 0.39,
            17 => 0.38,
            18 => 0.37,
            19 => 0.36,
            20 => 0.35,
            21 => 0.34,
            22 => 0.33,
            23 => 0.32,
            24 => 0.31,
            _ => 0.30
        };
        
        var greaterThan3500 = kitchenEquipment.Where(ke => ke.VoltAmpere.Value >= 3500);
        var greaterThan3500VoltAmpere = greaterThan3500.Sum(ke => ke.VoltAmpere.Value);
        var greaterThan3500DemandFactor = greaterThan3500.Count() switch
        {
            0 => 0,
            1 => 0.8,
            2 => 0.65,
            3 => 0.55,
            4 => 0.5,
            5 => 0.45,
            6 => 0.43,
            7 => 0.4,
            8 => 0.36,
            9 => 0.35,
            10 => 0.34,
            <= 15 => 0.32,
            <= 20 => 0.28,
            <= 25 => 0.26,
            <= 30 => 0.24,
            <= 40 => 0.22,
            <= 50 => 0.2,
            <= 60 => 0.18,
            _ => 0.16
        };
        
        return [
            lessThan3500VoltAmpere * lessThan3500DemandFactor, 
            greaterThan3500VoltAmpere * greaterThan3500DemandFactor
        ];
    }

    public static double ApplyDemandFactorToNonDwellingUnitKitchenEquipment(
        IEnumerable<ApplianceEquipmentOutletCircuit> kitchenEquipment)
    {
        var sum = kitchenEquipment.Sum(ke => ke.VoltAmpere.Value);

        var demandFactor = kitchenEquipment.Count() switch
        {
            0 => 0,
            <= 2 => 1,
            3 => 0.9,
            4 => 0.8,
            5 => 0.7,
            _ => 0.65
        };
        
        return sum * demandFactor;
    }

    public static double ApplyDemandFactorToElevatorFeeders(IEnumerable<MotorOutletCircuit> feeders)
    {
        var sum = feeders.Sum(f => f.VoltAmpere.Value);

        var demandFactor = feeders.Count() switch
        {
            0 => 0,
            1 => 1,
            2 => 0.95,
            3 => 0.9,
            4 => 0.85,
            5 => 0.82,
            6 => 0.79,
            7 => 0.77,
            8 => 0.75,
            9 => 0.73
        };
        
        return sum * demandFactor;
    }

    public static double ApplyDemandFactorToCranesAndHoists(IEnumerable<MotorOutletCircuit> cranesAndHoists)
    {
        var sum = cranesAndHoists.Sum(ch => ch.VoltAmpere.Value);

        var demandFactor = cranesAndHoists.Count() switch
        {
            0 => 0,
            1 => 1,
            2 => 0.95,
            3 => 0.91,
            4 => 0.87,
            5 => 0.84,
            6 => 0.81,
            _ => 0.78,
        };
        
        return sum * demandFactor;
    }
}