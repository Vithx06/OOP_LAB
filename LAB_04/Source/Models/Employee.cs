public class Employee
{
    private string _employeeId;
    private string _fullName;
    private string _department;
    private double _monthlyBonus;

    public string EmployeeId
    {
        get => _employeeId;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Employee ID không được rỗng");

            _employeeId = value;
        }
    }

    public string FullName
    {
        get => _fullName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Full Name không được rỗng");

            _fullName = value;
        }
    }

    public string Department
    {
        get => _department;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Department không được rỗng");

            _department = value;
        }
    }

    public double MonthlyBonus
    {
        get => _monthlyBonus;
        set
        {
            if (value < 0)
                throw new ArgumentException("Monthly Bonus không được âm");

            _monthlyBonus = value;
        }
    }

    // Constructor 1
    public Employee(string employeeId, string fullName)
    {
        EmployeeId = employeeId;
        FullName = fullName;
        Department = "Unassigned";
        MonthlyBonus = 0;
    }

    // Constructor 2
    public Employee(string employeeId, string fullName, string department) : this(employeeId, fullName)
    {
        Department = department;
    }

    // addBonus 1
    public void AddBonus(double amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Bonus amount phải lớn hơn 0");

        MonthlyBonus += amount;
    }

    // addBonus 2
    public void AddBonus(double amount, string reason)
    {
        if (amount <= 0)
            throw new ArgumentException("Bonus amount phải lớn hơn 0");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason không được rỗng");

        MonthlyBonus += amount;
    }

    // addBonus 3
    public void AddBonus(double rate, double referenceAmount, string reason)
    {
        if (rate <= 0 || rate > 0.5)
            throw new ArgumentException("Rate phải lớn hơn 0 và không quá 0.5");

        if (referenceAmount <= 0)
            throw new ArgumentException("Reference amount phải lớn hơn 0");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason không được rỗng");

        double bonus = rate * referenceAmount;
        MonthlyBonus += bonus;
    }

    // Các phương thức để lớp con override
    public virtual double CalculateGrossPay()
    {
        return MonthlyBonus;
    }

    public virtual string GetEmployeeType()
    {
        return "Employee";
    }

    public virtual void DisplayPayrollInfo()
    {
        Console.WriteLine($"Employee ID: {EmployeeId}");
        Console.WriteLine($"Full Name: {FullName}");
        Console.WriteLine($"Department: {Department}");
        Console.WriteLine($"Monthly Bonus: {MonthlyBonus}");
    }
}