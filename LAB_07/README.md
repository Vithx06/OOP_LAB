# LAB 07 – Hệ thống quản lý và phân bổ thiết bị

## Thông tin sinh viên thực hiện

**Họ và tên: Đinh Thế Vinh**  
**MSSV: 202419017**  
**Mã lớp: 174373**

## 1. Bài toán

Xây dựng hệ thống quản lý và phân bổ thiết bị phục vụ giảng dạy trong trường học, áp dụng lập trình hướng đối tượng, generic và đa hình để lưu trữ, tìm kiếm, lựa chọn và phân bổ thiết bị theo các điều kiện và chiến lược khác nhau.

## 2. Các chức năng chính

- Quản lý thông tin và trạng thái thiết bị giảng dạy.
- Lưu trữ và tìm kiếm thiết bị theo mã định danh.
- Lọc các thiết bị đáp ứng điều kiện cấp phát.
- Xây dựng các chiến lược lựa chọn thiết bị, bao gồm chọn thiết bị có mức ưu tiên cao nhất và chọn thiết bị sẵn sàng đầu tiên.
- Phân bổ thiết bị dựa trên trạng thái sẵn sàng, điều kiện bổ sung và chiến lược lựa chọn.
- Sử dụng generic và đa hình để tăng khả năng tái sử dụng mã nguồn và mở rộng hệ thống.
- Xử lý các trường hợp không hợp lệ thông qua ngoại lệ.

## 3. Cấu trúc thư mục dự án

```text
LAB_07/
├── Problem/                           # Đề bài
│   └── Problem.pdf
├── UML/                               # Sơ đồ thiết kế
│   |── Business_UML.png               # Sơ đồ UML nghiệp vụ
|   └── Exceptions_UML.png             # Sơ đồ UML các ngoại lệ
├── Source/                            # Mã nguồn chương trình
│   ├── Enum/                          # Các kiểu liệt kê
│   │   └── DeviceStatus.cs      
│   ├── Exceptions/                    # Các lớp ngoại lệ tùy chỉnh
│   │   └── Exceptions.cs
│   ├── Interfaces/                    # Các giao diện
│   │   └── Interfaces.cs
│   ├── Models/                        # Các lớp mô hình và nghiệp vụ
│   │   ├── Devices/                   # Các lớp thiết bị
│   │   │   ├── AirConditionerRemote.cs
│   │   │   ├── ProjectorRemote.cs
│   │   │   ├── TeachingDevice.cs
│   │   │   └── WirelessMicrophone.cs
│   │   ├── Repositories/              # Lưu trữ và truy xuất dữ liệu
│   │   │   └── InMemoryRepository.cs
│   │   ├── Services/                  # Xử lý nghiệp vụ
│   │   │   └── AllocationService.cs
│   │   └── Strategies/                # Các chiến lược lựa chọn thiết bị
│   │       ├── FirstAvailableStrategy.cs
│   │       └── HighestPriorityStrategy.cs
│   ├── Program.cs                     # Chương trình chính
│   └── Source.csproj                  # Tệp cấu hình dự án .NET
└── README.md                          # Tài liệu giới thiệu dự án
```

## 4. Kết quả đạt được

- Vận dụng các nguyên lý lập trình hướng đối tượng, đặc biệt là kế thừa và đa hình, trong xây dựng hệ thống.
- Sử dụng generic để xây dựng kho lưu trữ và các thành phần lựa chọn thiết bị có khả năng tái sử dụng.
- Xây dựng các chiến lược lựa chọn thiết bị độc lập, cho phép thay đổi cách phân bổ mà không cần sửa nhiều lớp nghiệp vụ.
- Tổ chức mã nguồn theo các nhóm chức năng, tách biệt mô hình thiết bị, kho lưu trữ, dịch vụ phân bổ và chiến lược lựa chọn.
- Xử lý các trường hợp không hợp lệ bằng ngoại lệ và kiểm thử các chức năng chính của hệ thống.