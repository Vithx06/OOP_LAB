public class Payroll
{
    private string _period;
    private List<Employee> _employees;

    public string Period
    {
        get => _period;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Period không được rỗng");

            _period = value;
        }
    }

    public Payroll(string period)
    {
        Period = period;
        _employees = new List<Employee>();
    }

    public void AddEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (FindEmployee(employee.EmployeeId) != null)
            throw new ArgumentException("Employee ID đã tồn tại");

        _employees.Add(employee);
    }

    public Employee? FindEmployee(string employeeId)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new ArgumentException("Employee ID không được rỗng");

        foreach (Employee employee in _employees)
        {
            if (employee.EmployeeId == employeeId)
                return employee;
        }

        return null;
    }

    public double CalculateTotalPayroll()
    {
        double total = 0;

        foreach (Employee employee in _employees)
        {
            total += employee.CalculateGrossPay();
        }

        return total;
    }

    public double CalculatePayrollByDepartment(string department)
    {
        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException("Department không được rỗng");

        double total = 0;

        foreach (Employee employee in _employees)
        {
            if (employee.Department == department)
                total += employee.CalculateGrossPay();
        }

        return total;
    }

    public Employee? FindHighestPaidEmployee()
    {
        if (_employees.Count == 0)
            return null;

        Employee highestPaid = _employees[0];

        for (int i = 1; i < _employees.Count; i++)
        {
            if (_employees[i].CalculateGrossPay() > highestPaid.CalculateGrossPay())
                highestPaid = _employees[i];
        }

        return highestPaid;
    }

    public void DisplayPayroll()
    {
        Console.WriteLine($"Payroll Period: {Period}");
        Console.WriteLine("------------------------------");

        foreach (Employee employee in _employees)
        {
            employee.DisplayPayrollInfo();
            Console.WriteLine("------------------------------");
        }

        Console.WriteLine($"Total Payroll: {CalculateTotalPayroll()}");
    }
}