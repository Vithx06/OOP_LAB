# LAB 06 – Hệ thống quản lý mượn thiết bị

## Thông tin sinh viên thực hiện

**Họ và tên: Đinh Thế Vinh**  
**MSSV: 202419017**  
**Mã lớp: 174373**

## 1. Bài toán

Xây dựng hệ thống quản lý mượn và trả thiết bị phục vụ giảng dạy trong trường học.

## 2. Các chức năng chính

- Quản lý thông tin và trạng thái thiết bị.
- Xử lý nghiệp vụ mượn và trả thiết bị.
- Kiểm tra các điều kiện và quy tắc nghiệp vụ.
- Xử lý ngoại lệ tùy chỉnh khi phát sinh các tình huống không hợp lệ.

## 3. Cấu trúc thư mục dự án

```text
LAB_06/
├── Problem/                          # Đề bài
│   └── Problem.pdf                    
├── UML/                           
│   ├── Bussiness_UML.png             # Sơ đồ UML các lớp nghiệp vụ
│   └── Exceptio_UML.png              # Sơ đồ UML các lớp ngoại lệ
└── Source/                           # Mã nguồn chương trình
    ├── Enum/                         # Các kiểu liệt kê
    │   └── DeviceStatus.cs
    ├── Exception/                    # Các lớp ngoại lệ tùy chỉnh
    │   └── Exceptions.cs
    ├── Interfaces/                   # Các giao diện
    │   └── IBorrower.cs
    ├── Models/                       # Các lớp mô hình đối tượng
    │   ├── Borrowing/                # Các lớp nghiệp vụ mượn thiết bị
    │   │   ├── BorrowingSlip.cs
    │   │   └── BorrowItem.cs
    │   ├── Devices/                  # Các lớp mô hình thiết bị
    │   │   ├── AirConditionerRemote.cs
    │   │   ├── Device.cs
    │   │   ├── Micro.cs
    │   │   └── ProjectorRemote.cs
    │   └── People/                   # Các lớp mô hình người tham gia
    │       ├── Lecturer.cs
    │       └── TeachingAssistant.cs
    ├── Program.cs                    # Chương trình chính
    └── Source.csproj                 # Tệp cấu hình dự án .NET
```


## 4. Kết quả đạt được

- Vận dụng các nguyên lý lập trình hướng đối tượng trong xây dựng hệ thống.
- Tổ chức mã nguồn theo các nhóm chức năng.
- Xây dựng các ngoại lệ để xử lý những trường hợp vi phạm quy tắc nghiệp vụ.
- Kiểm thử các trường hợp hợp lệ và không hợp lệ của chương trình.