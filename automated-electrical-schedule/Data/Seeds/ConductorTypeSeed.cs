using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;

namespace automated_electrical_schedule.Data.Seeds;

public static class ConductorTypeSeed
{
    public static readonly ConductorType TwCu60 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C60,
        ConductorWireType.Tw
    );

    // public static readonly ConductorType UfCu60 = new(
    //     ConductorMaterial.Copper,
    //     ConductorTemperatureRating.C60,
    //     ConductorWireType.Uf
    // );

    public static readonly ConductorType RhwCu75 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C75,
        ConductorWireType.Rhw
    );

    public static readonly ConductorType ThhwCu75 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C75,
        ConductorWireType.Thhw
    );

    public static readonly ConductorType ThwCu75 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C75,
        ConductorWireType.Thw
    );

    public static readonly ConductorType ThwnCu75 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C75,
        ConductorWireType.Thwn
    );

    public static readonly ConductorType XhhwCu75 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C75,
        ConductorWireType.Xhhw
    );

    // public static readonly ConductorType UseCu75 = new(
    //     ConductorMaterial.Copper,
    //     ConductorTemperatureRating.C75,
    //     ConductorWireType.Use
    // );

    public static readonly ConductorType ZwCu75 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C75,
        ConductorWireType.Zw
    );

    // public static readonly ConductorType TbsCu90 = new(
    //     ConductorMaterial.Copper,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Tbs
    // );

    // public static readonly ConductorType SaCu90 = new(
    //     ConductorMaterial.Copper,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Sa
    // );
    
    // public static readonly ConductorType SisCu90 = new(
    //     ConductorMaterial.Copper,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Sis
    // );

    public static readonly ConductorType FepCu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Fep
    );

    public static readonly ConductorType FepbCu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Fepb
    );

    // public static readonly ConductorType MiCu90 = new(
    //     ConductorMaterial.Copper,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Mi
    // );

    public static readonly ConductorType RhhCu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Rhh
    );

    public static readonly ConductorType Rhw2Cu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Rhw2
    );

    public static readonly ConductorType ThhnCu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Thhn
    );

    public static readonly ConductorType ThhwCu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Thhw
    );

    public static readonly ConductorType Thw2Cu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Thw2
    );

    public static readonly ConductorType Thwn2Cu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Thwn2
    );

    // public static readonly ConductorType Use2Cu90 = new(
    //     ConductorMaterial.Copper,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Use2
    // );

    public static readonly ConductorType XhhCu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Xhh
    );

    public static readonly ConductorType XhhwCu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Xhhw
    );

    public static readonly ConductorType Xhhw2Cu90 = new(
        ConductorMaterial.Copper,
        ConductorTemperatureRating.C90,
        ConductorWireType.Xhhw2
    );

    // public static readonly ConductorType Zw2Cu90 = new(
    //     ConductorMaterial.Copper,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Zw2
    // );

    public static readonly ConductorType TwAl60 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C60,
        ConductorWireType.Tw
    );

    // public static readonly ConductorType UfAl60 = new(
    //     ConductorMaterial.Aluminum,
    //     ConductorTemperatureRating.C60,
    //     ConductorWireType.Uf
    // );

    public static readonly ConductorType RhwAl75 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C75,
        ConductorWireType.Rhw
    );

    public static readonly ConductorType ThhwAl75 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C75,
        ConductorWireType.Thhw
    );

    public static readonly ConductorType ThwAl75 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C75,
        ConductorWireType.Thw
    );

    public static readonly ConductorType ThwnAl75 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C75,
        ConductorWireType.Thwn
    );

    public static readonly ConductorType XhhwAl75 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C75,
        ConductorWireType.Xhhw
    );

    // public static readonly ConductorType UseAl75 = new(
    //     ConductorMaterial.Aluminum,
    //     ConductorTemperatureRating.C75,
    //     ConductorWireType.Use
    // );
    
    // public static readonly ConductorType TbsAl90 = new(
    //     ConductorMaterial.Aluminum,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Tbs
    // );
    
    // public static readonly ConductorType SaAl90 = new(
    //     ConductorMaterial.Aluminum,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Sa
    // );

    // public static readonly ConductorType SisAl90 = new(
    //     ConductorMaterial.Aluminum,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Sis
    // );

    public static readonly ConductorType FepAl90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Fep
    );

    public static readonly ConductorType FepbAl90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Fepb
    );

    // public static readonly ConductorType MiAl90 = new(
    //     ConductorMaterial.Aluminum,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Mi
    // );

    public static readonly ConductorType RhhAl90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Rhh
    );

    public static readonly ConductorType Rhw2Al90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Rhw2
    );

    public static readonly ConductorType ThhnAl90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Thhn
    );

    public static readonly ConductorType ThhwAl90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Thhw
    );

    public static readonly ConductorType Thw2Al90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Thw2
    );

    public static readonly ConductorType Thwn2Al90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Thwn2
    );

    // public static readonly ConductorType Use2Al90 = new(
    //     ConductorMaterial.Aluminum,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Use2
    // );

    public static readonly ConductorType XhhAl90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Xhh
    );

    public static readonly ConductorType XhhwAl90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Xhhw
    );

    public static readonly ConductorType Xhhw2Al90 = new(
        ConductorMaterial.Aluminum,
        ConductorTemperatureRating.C90,
        ConductorWireType.Xhhw2
    );

    // public static readonly ConductorType Zw2Al90 = new(
    //     ConductorMaterial.Aluminum,
    //     ConductorTemperatureRating.C90,
    //     ConductorWireType.Zw2
    // );

    public static readonly List<ConductorType> All =
    [
        TwCu60,
        // UfCu60,
        RhwCu75,
        ThhwCu75,
        ThwCu75,
        ThwnCu75,
        XhhwCu75,
        // UseCu75,
        ZwCu75,
        // TbsCu90,
        // SaCu90,
        // SisCu90,
        FepCu90,
        FepbCu90,
        // MiCu90,
        RhhCu90,
        Rhw2Cu90,
        ThhnCu90,
        ThhwCu90,
        Thw2Cu90,
        Thwn2Cu90,
        // Use2Cu90,
        XhhCu90,
        XhhwCu90,
        Xhhw2Cu90,
        // Zw2Cu90,
        TwAl60,
        // UfAl60,
        RhwAl75,
        ThhwAl75,
        ThwAl75,
        ThwnAl75,
        XhhwAl75,
        // UseAl75,
        // TbsAl90,
        // SaAl90,
        // SisAl90,
        FepAl90,
        FepbAl90,
        // MiAl90,
        RhhAl90,
        Rhw2Al90,
        ThhnAl90,
        ThhwAl90,
        Thw2Al90,
        Thwn2Al90,
        // Use2Al90,
        XhhAl90,
        XhhwAl90,
        Xhhw2Al90,
        // Zw2Al90
    ];
}