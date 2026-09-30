using System.Collections.Generic;

class ProjectTeam
{
    private string _projectCode;
    private string _projectName;
    private Employee _leader;
    private List<Employee> _members;

    public string ProjectCode
    {
        get => _projectCode;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Mã dự án không được rỗng");

            _projectCode = value;
        }
    }

    public string ProjectName
    {
        get => _projectName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Tên dự án không được rỗng");

            _projectName = value;
        }
    }

    // Chỉ lớp ProjectTeam mới được đổi trưởng nhóm
    public Employee Leader
    {
        get => _leader;
        private set => _leader = value;
    }

    // Chỉ đọc, bên ngoài không thể Add/Remove trực tiếp
    public IReadOnlyList<Employee> Members
    {
        get => _members;
    }

    // Constructor 1
    public ProjectTeam(string projectCode, string projectName)
    {
        ProjectCode = projectCode;
        ProjectName = projectName;
        Leader = null;
        _members = new List<Employee>();
    }

    // Constructor 2
    public ProjectTeam(string projectCode, string projectName, Employee leader)
    {
        if (leader == null)
            throw new ArgumentException("Trưởng nhóm không được null");

        ProjectCode = projectCode;
        ProjectName = projectName;
        Leader = leader;

        _members = new List<Employee>();
        _members.Add(leader);
    }

    // Thêm thành viên
    public bool AddMember(Employee employee)
    {
        if (employee == null)
            throw new ArgumentException("Nhân sự không được null");

        if (Contains(employee.Id))
            return false;

        _members.Add(employee);
        return true;
    }

    // Thêm thành viên và đặt làm trưởng nhóm
    public bool AddMember(Employee employee, bool makeLeader)
    {
        if (employee == null)
            throw new ArgumentException("Nhân sự không được null");

        if (Contains(employee.Id))
            return false;

        _members.Add(employee);

        if (makeLeader)
        {
            Leader = employee;
        }

        return true;
    }

    // Xóa thành viên
    public bool RemoveMember(string employeeId)
    {
        if (Leader != null && Leader.Id == employeeId)
        {
            return false; // Không thể xóa trưởng nhóm hiện tại
        }

        for (int i = 0; i < _members.Count; i++)
        {
            if (_members[i].Id == employeeId)
            {
                _members.RemoveAt(i);
                return true;
            }
        }

        return false; // Không tìm thấy
    }

    // Đổi trưởng nhóm
    public void ChangeLeader(Employee employee)
    {
        if (employee == null)
            throw new ArgumentException("Trưởng nhóm mới không được null");

        if (!Contains(employee.Id))
        {
            _members.Add(employee);
        }

        Leader = employee;
    }

    // Kiểm tra thành viên tồn tại
    public bool Contains(string employeeId)
    {
        foreach (Employee employee in _members)
        {
            if (employee.Id == employeeId)
                return true;
        }

        return false;
    }

    // Tính tổng chi phí hàng tháng
    public double CalculateTotalMonthlyCost()
    {
        double total = 0;

        foreach (Employee employee in _members)
        {
            total += employee.CalculateMonthlyCost();
        }

        return total;
    }

    public void DisplayTeam()
    {
        Console.WriteLine($"Mã dự án: {ProjectCode}");
        Console.WriteLine($"Tên dự án: {ProjectName}");

        if (Leader != null)
            Console.WriteLine($"Trưởng nhóm: {Leader.FullName}");
        else
            Console.WriteLine("Trưởng nhóm: Chưa có");

        Console.WriteLine("Danh sách thành viên:");

        foreach (Employee employee in _members)
        {
            employee.DisplayInfo();
            Console.WriteLine();
        }
    }

    // Destructor
    ~ProjectTeam()
    {
        Console.WriteLine($"ProjectTeam {ProjectCode} đã được hủy.");
    }
}
