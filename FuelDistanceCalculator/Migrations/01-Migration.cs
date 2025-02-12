using FluentMigrator;

namespace FuelDistanceCalculator.Migrations
{
    [Migration(20250212)]  // Vergib eine eindeutige Versionsnummer
    public class AddTankinfoModelTable : Migration
    {
        public override void Up()
        {
            Create.Table("TankinfoModel")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("FuelAmount").AsDouble().NotNullable()
                .WithColumn("FuelPrice1").AsDouble().NotNullable()
                .WithColumn("FuelPrice2").AsDouble().NotNullable()
                .WithColumn("FuelType").AsString().NotNullable()
                .WithColumn("NameGasStation1").AsString().NotNullable()
                .WithColumn("NameGasStation2").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("TankinfoModel");
        }
    }
}