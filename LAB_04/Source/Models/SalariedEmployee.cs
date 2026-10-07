public class SalariedEmployee : Employee
{
    private double _monthlySalary;
    private double _responsibilityAllowance;

    public double MonthlySalary
    {
        get => _monthlySalary;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Monthly Salary phải lớn hơn 0");

            _monthlySalary = value;
        }
    }

    public double ResponsibilityAllowance
    {
        get => _responsibilityAllowance;
        set
        {
            if (value < 0)
                throw new ArgumentException("Responsibility Allowance không được âm");

            _responsibilityAllowance = value;
        }
    }

    // Constructor 1
    public SalariedEmployee(string employeeId, string fullName) : base(employeeId, fullName)
    {
        MonthlySalary = 0;
        ResponsibilityAllowance = 0;
    }

    // Constructor 2
    public SalariedEmployee(string employeeId, string fullName, string department, double monthlySalary, double responsibilityAllowance) : base(employeeId, fullName, department)
    {
        MonthlySalary = monthlySalary;
        ResponsibilityAllowance = responsibilityAllowance;
    }

    public override double CalculateGrossPay()
    {
        return MonthlySalary + ResponsibilityAllowance + MonthlyBonus;
    }

    public override string GetEmployeeType()
    {
        return "SalariedEmployee";
    }

    public override void DisplayPayrollInfo()
    {
        Console.WriteLine($"Employee ID: {EmployeeId}");
        Console.WriteLine($"Full Name: {FullName}");
        Console.WriteLine($"Department: {Department}");
        Console.WriteLine($"Monthly Salary: {MonthlySalary}");
        Console.WriteLine($"Responsibility Allowance: {ResponsibilityAllowance}");
        Console.WriteLine($"Monthly Bonus: {MonthlyBonus}");
        Console.WriteLine($"Gross Pay: {CalculateGrossPay()}");
    }
}