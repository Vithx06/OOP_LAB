public class HourlyEmployee : Employee
{
    private double _hourlyRate;
    private double _workedHours;

    public double HourlyRate
    {
        get => _hourlyRate;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Hourly Rate phải lớn hơn 0");

            _hourlyRate = value;
        }
    }

    public double WorkedHours
    {
        get => _workedHours;
        set
        {
            if (value < 0 || value > 250)
                throw new ArgumentException("Worked Hours phải từ 0 đến 250");

            _workedHours = value;
        }
    }

    // Constructor 1
    public HourlyEmployee(string employeeId, string fullName) : base(employeeId, fullName)
    {
        HourlyRate = 0;
        WorkedHours = 0;
    }

    // Constructor 2
    public HourlyEmployee(string employeeId, string fullName, string department, double hourlyRate, double workedHours) : base(employeeId, fullName, department)
    {
        HourlyRate = hourlyRate;
        WorkedHours = workedHours;
    }

    public override double CalculateGrossPay()
    {
        double basePay;

        if (WorkedHours <= 160)
            basePay = WorkedHours * HourlyRate;
        else
            basePay = 160 * HourlyRate + (WorkedHours - 160) * HourlyRate * 1.5;

        return basePay + MonthlyBonus;
    }

    public override string GetEmployeeType()
    {
        return "HourlyEmployee";
    }

    public override void DisplayPayrollInfo()
    {
        Console.WriteLine($"Employee ID: {EmployeeId}");
        Console.WriteLine($"Full Name: {FullName}");
        Console.WriteLine($"Department: {Department}");
        Console.WriteLine($"Hourly Rate: {HourlyRate}");
        Console.WriteLine($"Worked Hours: {WorkedHours}");
        Console.WriteLine($"Monthly Bonus: {MonthlyBonus}");
        Console.WriteLine($"Gross Pay: {CalculateGrossPay()}");
    }
}