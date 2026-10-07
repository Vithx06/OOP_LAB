class Employee
{
    private string _id;
    private string _fullName;
    private double _baseSalary;

    public string Id
    {
        get => _id;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ID không được rỗng");

            _id = value;
        }
    }

    public string FullName
    {
        get => _fullName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Họ tên không được rỗng");

            _fullName = value;
        }
    }

    public double BaseSalary
    {
        get => _baseSalary;
        set
        {
            if (value < 0)
                throw new ArgumentException("Lương cơ bản không được âm");

            _baseSalary = value;
        }
    }
    
    // Constructor 1
    public Employee()
    {
        Id = "UNKNOWN";
        FullName = "Unnamed employee";
        BaseSalary = 0;
    }

    // Constructor 2
    public Employee(string id, string fullName)
    {
        Id = id;
        FullName = fullName;
        BaseSalary = 0;
    }

    // Constructor 3
    public Employee(string id, string fullName, double baseSalary)
    {
        Id = id;
        FullName = fullName;
        BaseSalary = baseSalary;
    }

    // Tăng lương cố định
    public void IncreaseSalary(double amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Giá trị tăng phải dương");

        BaseSalary += amount;
    }

    // Tăng lương theo tiền hoặc phần trăm
    public void IncreaseSalary(double value, bool byPercentage)
    {
        if (value <= 0)
            throw new ArgumentException("Giá trị tăng phải dương");

        if (byPercentage)
            BaseSalary += BaseSalary * value / 100;
        else
            BaseSalary += value;
    }

    // Tính chi phí hàng tháng
    public virtual double CalculateMonthlyCost()
    {
        return BaseSalary;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Họ tên: {FullName}");
        Console.WriteLine($"Lương cơ bản: {BaseSalary}");
    }

    // Destructor
    ~Employee()
    {
        Console.WriteLine($"Employee {Id} đã được hủy.");
    }
}