public class SalesEmployee : Employee
{
    private double _baseSalary;
    private double _salesRevenue;
    private double _commissionRate;

    public double BaseSalary
    {
        get => _baseSalary;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Base Salary phải lớn hơn 0");

            _baseSalary = value;
        }
    }

    public double SalesRevenue
    {
        get => _salesRevenue;
        set
        {
            if (value < 0)
                throw new ArgumentException("Sales Revenue không được âm");

            _salesRevenue = value;
        }
    }

    public double CommissionRate
    {
        get => _commissionRate;
        set
        {
            if (value < 0 || value > 0.3)
                throw new ArgumentException("Commission Rate phải từ 0 đến 0.3");

            _commissionRate = value;
        }
    }

    // Constructor 1
    public SalesEmployee(string employeeId, string fullName) : base(employeeId, fullName)
    {
        BaseSalary = 0;
        SalesRevenue = 0;
        CommissionRate = 0;
    }

    // Constructor 2
    public SalesEmployee(string employeeId, string fullName, string department, double baseSalary, double salesRevenue, double commissionRate) : base(employeeId, fullName, department)
    {
        BaseSalary = baseSalary;
        SalesRevenue = salesRevenue;
        CommissionRate = commissionRate;
    }

    public override double CalculateGrossPay()
    {
        return BaseSalary + SalesRevenue * CommissionRate + MonthlyBonus;
    }

    public override string GetEmployeeType()
    {
        return "SalesEmployee";
    }

    public override void DisplayPayrollInfo()
    {
        Console.WriteLine($"Employee ID: {EmployeeId}");
        Console.WriteLine($"Full Name: {FullName}");
        Console.WriteLine($"Department: {Department}");
        Console.WriteLine($"Base Salary: {BaseSalary}");
        Console.WriteLine($"Sales Revenue: {SalesRevenue}");
        Console.WriteLine($"Commission Rate: {CommissionRate}");
        Console.WriteLine($"Monthly Bonus: {MonthlyBonus}");
        Console.WriteLine($"Gross Pay: {CalculateGrossPay()}");
    }

    public void UpdateSalesRevenue(double amount)
    {
        SalesRevenue = amount;
    }
}