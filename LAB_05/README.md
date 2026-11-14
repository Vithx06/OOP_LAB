# LAB 05 – Hệ thống quản lý thiết bị và chi phí bảo trì

## Thông tin sinh viên thực hiện

**Họ và tên: Đinh Thế Vinh**\
**MSSV: 202419017**\
**Mã lớp: 174373**

## 1. Bài toán

Xây dựng hệ thống quản lý các thiết bị trong phòng thực hành của trường đại học, bao gồm máy tính, máy in và máy chiếu. Mỗi loại thiết bị có thông tin riêng và cách tính chi phí bảo trì hằng năm khác nhau. Hệ thống áp dụng lớp trừu tượng, kế thừa, interface, kết tập và đa hình để quản lý thiết bị theo phòng, tổng hợp chi phí bảo trì và xử lý khả năng kết nối mạng của các thiết bị phù hợp.

## 2. Các chức năng chính

- Quản lý mã, tên, năm đưa vào sử dụng, giá mua và trạng thái hoạt động của thiết bị thông qua lớp trừu tượng `Device`.
- Quản lý thông tin riêng của máy tính, máy in và máy chiếu; mã thiết bị chỉ được thiết lập khi khởi tạo.
- Tính chi phí bảo trì máy tính bằng 5% giá mua, cộng thêm 2% nếu có GPU rời và 1% nếu đã sử dụng trên 5 năm.
- Tính chi phí bảo trì máy in bằng 4% giá mua, cộng thêm 500.000 đồng nếu đã in trên 100.000 trang và 300.000 đồng nếu là máy in màu.
- Tính chi phí bảo trì máy chiếu bằng 3% giá mua, cộng thêm 1.500.000 đồng nếu bóng đèn đã sử dụng trên 3.000 giờ.
- Biểu diễn khả năng kết nối mạng bằng `INetworkable`, được thực thi bởi `Computer` và `NetworkPrinter`; hỗ trợ kết nối, ngắt kết nối và kiểm tra trạng thái mạng.
- Quản lý phòng thực hành với mã, tên, sức chứa và danh sách thiết bị; thêm, xóa và tìm thiết bị theo mã.
- Từ chối thiết bị null, thiết bị trùng mã trong cùng phòng và thao tác thêm thiết bị khi phòng đã đầy.
- Tổng hợp chi phí bảo trì của từng phòng và liệt kê các thiết bị đang bảo trì hoặc đã sử dụng trên 5 năm.
- Hiển thị thông tin thiết bị dưới dạng chuỗi, kiểm tra dữ liệu đầu vào và xử lý các trường hợp không hợp lệ bằng ngoại lệ.

## 3. Cấu trúc thư mục dự án

```text
LAB_05/
├── Problem/                           # Đề bài
│   └── Problem.pdf
├── UML/                               # Sơ đồ thiết kế
│   └── UML.png                        # Sơ đồ UML nghiệp vụ
├── Source/                            # Mã nguồn chương trình
│   ├── Enum/                          # Các kiểu liệt kê
│   │   ├── DeviceStatus.cs
│   │   └── PrinterTechnology.cs
│   ├── Interfaces/                    # Các giao diện
│   │   └── INetworkable.cs
│   ├── Models/                        # Các lớp mô hình và nghiệp vụ
│   │   ├── Device.cs
│   │   ├── Computer.cs
│   │   ├── Printer.cs
│   │   ├── NetworkPrinter.cs
│   │   ├── Projector.cs
│   │   └── LabRoom.cs
│   ├── Program.cs                     # Chương trình chính và các tình huống kiểm thử
│   └── Source.csproj                  # Tệp cấu hình dự án .NET
└── README.md                          # Tài liệu giới thiệu dự án
```

## 4. Kết quả đạt được

- Sử dụng lớp trừu tượng `Device` để tập trung thông tin chung và quy định phương thức tính chi phí bảo trì cho các lớp thiết bị.
- Vận dụng kế thừa và ghi đè `CalculateAnnualMaintenanceCost()` để triển khai công thức bảo trì riêng cho từng loại thiết bị.
- Áp dụng interface để biểu diễn khả năng kết nối mạng độc lập với hành vi chung của thiết bị; tách `NetworkPrinter` khỏi `Printer` để phân biệt máy in có và không có khả năng mạng.
- Thể hiện quan hệ kết tập giữa `LabRoom` và `Device`, cho phép thiết bị tồn tại độc lập với phòng thực hành.
- Tổng hợp chi phí qua danh sách `Device` bằng lời gọi đa hình, không phân nhánh theo từng lớp thiết bị trong `LabRoom`.
- Tổ chức mã nguồn theo nhóm chức năng, tách biệt các kiểu liệt kê, giao diện và lớp mô hình.
- Xây dựng các tình huống kiểm thử trong `Program.cs` cho quản lý thiết bị theo phòng, tính chi phí bảo trì, thao tác mạng qua `INetworkable`, trùng mã, giới hạn sức chứa và dữ liệu không hợp lệ.
