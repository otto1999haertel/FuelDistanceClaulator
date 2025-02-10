public class FuelPriceService
{
    private int fuelAmount;
    private decimal pricePerkilometer;

    public FuelPriceService(int FuelAmount, decimal pricePerkilometer){
        this.pricePerkilometer=pricePerkilometer;
        this.fuelAmount = FuelAmount;
    }

    public decimal CalculateEntireCost(decimal pricePerLiter, decimal distance){
        return (fuelAmount * pricePerLiter) + (distance *pricePerkilometer);
    }
}