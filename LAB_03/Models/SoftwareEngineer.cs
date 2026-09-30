class SoftwareEngineer : Employee
{
    private string _primaryLanguage;
    private double _technicalAllowance;

    public string PrimaryLanguage
    {
        get => _primaryLanguage;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ngôn ngữ chính không được rỗng");

            _primaryLanguage = value;
        }
    }

    public double TechnicalAllowance
    {
        get => _technicalAllowance;
        set
        {
            if (value < 0)
                throw new ArgumentException("Phụ cấp kỹ thuật không được âm");

            _technicalAllowance = value;
        }
    }

    // Constructor 1
    public SoftwareEngineer(string id, string fullName, string primaryLanguage)
        : base(id, fullName)
    {
        PrimaryLanguage = primaryLanguage;
        TechnicalAllowance = 0;
    }

    // Constructor 2
    public SoftwareEngineer(string id, string fullName, double baseSalary, string primaryLanguage, double technicalAllowance)
        : base(id, fullName, baseSalary)
    {
        PrimaryLanguage = primaryLanguage;
        TechnicalAllowance = technicalAllowance;
    }

    // Tính chi phí hàng tháng
    public override double CalculateMonthlyCost()
    {
        return BaseSalary + TechnicalAllowance;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Ngôn ngữ chính: {PrimaryLanguage}");
        Console.WriteLine($"Phụ cấp kỹ thuật: {TechnicalAllowance}");
    }

    // Destructor
    ~SoftwareEngineer()
    {
        Console.WriteLine($"SoftwareEngineer {Id} đã được hủy.");
    }
}