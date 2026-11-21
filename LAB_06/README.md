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

## 5. Giải thích thiết kế

### 5.1. Vì sao phải kiểm tra điều kiện trước khi cập nhật trạng thái mượn, trả?

Nếu chuyển thiết bị sang `Borrowed` rồi mới kiểm tra giới hạn người mượn, một ngoại lệ có thể khiến thiết bị bị đánh dấu đã mượn dù chưa được thêm vào phiếu. Vì vậy cần kiểm tra dữ liệu, trùng mã, giới hạn số lượng, trạng thái sẵn sàng và thời hạn trước; sau đó tạo `BorrowItem` rồi mới cập nhật thiết bị và danh sách. Khi trả cũng cần kiểm tra thiết bị có trong phiếu, chưa được trả và ngày trả hợp lệ trước khi ghi nhận kết quả. Thứ tự này tránh thay đổi dữ liệu một phần khi thao tác bị từ chối bởi các điều kiện nghiệp vụ.

### 5.2. Vì sao dùng các ngoại lệ nghiệp vụ riêng và một lớp ngoại lệ cơ sở?

Các ngoại lệ riêng giúp phân biệt nguyên nhân thất bại, chẳng hạn thiết bị không sẵn sàng, vượt giới hạn, trùng thiết bị hoặc trả thiết bị không thuộc phiếu. Bên gọi có thể bắt từng loại để xử lý cụ thể hoặc bắt `BorrowingException` để xử lý chung nhóm lỗi mượn, trả, thay vì phân tích chuỗi thông báo. Những dữ liệu đi kèm như mã thiết bị, mã người mượn và giới hạn giúp hiển thị đúng ngữ cảnh; constructor nhận `innerException` cho phép giữ nguyên nguyên nhân nếu cần bọc một lỗi khác.

### 5.3. Nên đặt try, catch và finally ở đâu?

Các lớp nghiệp vụ phát sinh ngoại lệ khi thao tác không hợp lệ; lớp ứng dụng hoặc `Main()` bắt lỗi để thông báo và quyết định luồng xử lý. Không nên bắt rồi bỏ qua lỗi trong phương thức nghiệp vụ vì bên gọi có thể hiểu nhầm thao tác đã thành công. `finally` dành cho công việc dọn dẹp cần thực hiện khi rời khối xử lý, như đóng tài nguyên; không đặt thao tác đánh dấu đã mượn hoặc đã trả vào đó vì chúng không được thực hiện khi nghiệp vụ thất bại. Chương trình hiện minh họa `try` và `catch`, không cần thêm một khối `finally` rỗng khi chưa có tài nguyên phải dọn dẹp.
