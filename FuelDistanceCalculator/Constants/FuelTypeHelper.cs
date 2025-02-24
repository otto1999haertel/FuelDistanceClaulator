public static class FuelTypeHelper
{
    public static readonly Dictionary<FuelType, string> FuelTypeNames = new()
    {
        { FuelType.Diesel, "Diesel" },
        { FuelType.SuperE10, "Super E10" },
        { FuelType.SuperE5, "Super E5" },
        // { FuelType.SuplerPlus, "Super Plus" },
        // { FuelType.PermiumDiesel, "Premium Diesel" },
        // { FuelType.GTLDisel, "GTL-Diesel" },
        // { FuelType.LKWDiesel, "LKW-Diesel" },
        // { FuelType.HCODiesel, "HCO-Diesel" },
        // { FuelType.LPG, "LPG (Autogas)" },
        // { FuelType.CNG, "CNG (Erdgas)" },
        // { FuelType.LNG, "LNG (Flüssigerdgas)" },
        // { FuelType.Elektro, "Elektro" },
        // { FuelType.Gas, "Gas" },
        // { FuelType.Wasserstoff, "Wasserstoff" }
    };
}