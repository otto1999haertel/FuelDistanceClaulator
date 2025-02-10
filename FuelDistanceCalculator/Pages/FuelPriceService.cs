public class FuelPriceService
{
    private int fuelAmount;
    private double pricePerkilometer;

    public FuelPriceService(int FuelAmount, double pricePerkilometer){
        this.pricePerkilometer=pricePerkilometer;
        this.fuelAmount = FuelAmount;
    }

    public double CalculateEntireCost(double pricePerLiter, double distance){
        return (fuelAmount * pricePerLiter) + (distance *pricePerkilometer);
    }
}