using congestion.calculator;
using congestion.calculator.Models;

namespace UnitTests
{
    public class CongestionTaxCalculatorTests
    {

        public static IEnumerable<object[]> CarData()
        {
            yield return new object[] { new Car(), new List<DateTime> {
                DateTime.Parse("2013-01-14 21:00:00"),
                DateTime.Parse("2013-01-15 21:00:00"),
                DateTime.Parse("2013-02-07 06:23:27"),
                DateTime.Parse("2013-02-07 15:27:00"),
                DateTime.Parse("2013-02-08 06:27:00"),
                DateTime.Parse("2013-02-08 06:20:27"),
                DateTime.Parse("2013-02-08 14:35:00"),
                DateTime.Parse("2013-02-08 15:29:00"),
                DateTime.Parse("2013-02-08 15:47:00"),
                DateTime.Parse("2013-02-08 16:01:00"),
                DateTime.Parse("2013-02-08 16:48:00"),
                DateTime.Parse("2013-02-08 17:49:00"),
                DateTime.Parse("2013-02-08 18:29:00"),
                DateTime.Parse("2013-02-08 18:35:00"),
                DateTime.Parse("2013-03-26 14:25:00"),
                DateTime.Parse("2013-03-28 14:07:27"),

            } };
        }
        public static IEnumerable<object[]> WeekendCarData()
        {
            // Create a list of weekend dates in 2013
            var weekendDates = new List<DateTime>
            {
                DateTime.Parse("2013-01-05 21:00:00"), // Saturday
                DateTime.Parse("2013-01-06 21:00:00"), // Sunday
                DateTime.Parse("2013-01-12 21:00:00"), // Saturday
                DateTime.Parse("2013-01-13 21:00:00"), // Sunday
                // Add more weekend dates as needed
            };

            yield return new object[] { new Car(), weekendDates };
        }
        public static IEnumerable<object[]> JulyCarData()
        {
            var julyDates = new List<DateTime>
            {
                DateTime.Parse("2013-07-14 21:00:00"),
                DateTime.Parse("2013-07-15 21:00:00"),
                DateTime.Parse("2013-07-07 06:23:27"),
                DateTime.Parse("2013-07-07 15:27:00"),
                DateTime.Parse("2013-07-08 06:27:00"),
                DateTime.Parse("2013-07-08 06:20:27"),
                DateTime.Parse("2013-07-08 14:35:00"),
                DateTime.Parse("2013-07-08 15:29:00"),
                DateTime.Parse("2013-07-08 15:47:00"),
                DateTime.Parse("2013-07-08 16:01:00"),
                DateTime.Parse("2013-07-08 16:48:00"),
                DateTime.Parse("2013-07-08 17:49:00"),
                DateTime.Parse("2013-07-08 18:29:00"),
                DateTime.Parse("2013-07-08 18:35:00"),
                DateTime.Parse("2013-07-26 14:25:00"),
                DateTime.Parse("2013-07-28 14:07:27"),
            };

            yield return new object[] { new Car(), julyDates };
        }
        public static IEnumerable<object[]> TollFreeVehicleData()
        {
            var dates = new List<DateTime>
                {
                    DateTime.Parse("2013-01-01 00:00:00"), // Any date can be used for toll-free vehicles
                };

            yield return new object[] { new Emergency(), dates };
            yield return new object[] { new Bus(), dates }; 
            yield return new object[] { new Diplomat(), dates };
            yield return new object[] { new Motorcycle(), dates };
            yield return new object[] { new Military(), dates };
            yield return new object[] { new Foreign(), dates }; 
        }
        public static IEnumerable<object[]> SingleChargeRuleData()
        {
            var dates = new List<DateTime>
                {
                    DateTime.Parse("2013-01-01 08:00:00"), // Fee: 8 SEK
                    DateTime.Parse("2013-01-01 08:30:00"), // Fee: 8 SEK (within 60 minutes, only highest fee counts)
                    DateTime.Parse("2013-01-01 09:00:00"), // Fee: 8 SEK (new 60-minute period)
                    DateTime.Parse("2013-01-01 09:30:00"), // Fee: 13 SEK (within 60 minutes, only highest fee counts)
                };

            yield return new object[] { new Car(), dates };
        }

        [Theory]
        [MemberData(nameof(CarData))]
        public void ShouldCalculateMaximumTaxForACar(Vehicle vehicle, List<DateTime> dates)
        {

            //Arrange
            var taxCalculator = new CongestionTaxCalculator();

            //Act
            var tax = taxCalculator.GetTax(vehicle, dates.ToArray());

            //Assert
            Assert.Equal(60, tax);
        }

        [Theory]
        [MemberData(nameof(WeekendCarData))]
        public void ShouldCalculateZeroTaxOnWeekends(Vehicle vehicle, List<DateTime> dates)
        {
            // Arrange
            var taxCalculator = new CongestionTaxCalculator();

            // Act
            var tax = taxCalculator.GetTax(vehicle, dates.ToArray());

            // Assert
            Assert.Equal(0, tax);
        }

        [Theory]
        [MemberData(nameof(JulyCarData))]
        public void ShouldCalculateZeroTaxForJuly(Vehicle vehicle, List<DateTime> dates)
        {
            // Arrange
            var taxCalculator = new CongestionTaxCalculator();

            // Act
            var tax = taxCalculator.GetTax(vehicle, dates.ToArray());

            // Assert
            Assert.Equal(0, tax);
        }

        [Theory]
        [MemberData(nameof(TollFreeVehicleData))]
        public void ShouldCalculateZeroTaxForAllTollFreeVehicleTypes(Vehicle vehicle, List<DateTime> dates)
        {
            // Arrange
            var taxCalculator = new CongestionTaxCalculator();

            // Act
            var tax = taxCalculator.GetTax(vehicle, dates.ToArray());

            // Assert
            Assert.Equal(0, tax);
        }

        //[Theory]
        //[MemberData(nameof(SingleChargeRuleData))]
        //public void ShouldApplySingleChargeRuleCorrectly(Vehicle vehicle, List<DateTime> dates)
        //{
        //    // Arrange
        //    var taxCalculator = new CongestionTaxCalculator();

        //    // Act
        //    var tax = taxCalculator.GetTax(vehicle, dates.ToArray());

        //    // Assert
        //    // The expected tax is 8 SEK for the first period and 13 SEK for the second period
        //    Assert.Equal(21, tax);
        //}
    }
}