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

## 5. Trả lời câu hỏi thiết kế

### 5.1. Hành vi nào chung cho mọi thiết bị và nên nằm trong lớp cơ sở?

`TeachingDevice` giữ thông tin chung gồm mã, tên, vị trí lưu trữ và trạng thái; constructor kiểm tra thông tin hợp lệ và thiết lập trạng thái ban đầu `Available`. Các thao tác `Borrow()` và `Return()` quản lý chuyển trạng thái theo cùng một quy trình cho mọi thiết bị. Trong mã hiện tại, `Borrow(roomId)` gọi các hành vi kiểm tra được đa hình hóa trước khi chuyển sang `Borrowed`, còn `Return()` chỉ thành công khi đang được mượn.

### 5.2. Hành vi nào thay đổi theo loại thiết bị và nên được ghi đè?

`IsReadyForUse()` và `GetDefaultPriorityScore(roomId)` thay đổi theo từng loại thiết bị, nên được khai báo trừu tượng và ghi đè ở các lớp dẫn xuất. Máy chiếu xét pin từ 20%, micro xét cả pin từ 30% và tín hiệu từ 40%, còn điều khiển điều hòa xét trạng thái sẵn sàng và tập phòng tương thích. Bản triển khai còn đưa `IsCompatibleWith(roomId)` vào lớp cơ sở để kiểm tra tương thích qua cùng một giao diện hành vi; máy chiếu và micro chấp nhận mã phòng hợp lệ, còn điều hòa kiểm tra phòng trong tập tương thích.

### 5.3. Kho lưu trữ có cần biết thiết bị cụ thể là micro hay điều khiển không?

Không. Kho chỉ cần đối tượng cung cấp `Id` qua `IIdentifiable` để thêm, tìm và xóa theo mã; nó không cần biết mức pin hay phòng tương thích. Các điều kiện nghiệp vụ được cung cấp từ bên ngoài qua predicate hoặc được xử lý bởi thiết bị và dịch vụ cấp phát. Nhờ đó cùng một kho generic có thể lưu nhiều loại đối tượng mà không đưa quy tắc riêng của từng loại vào kho.

### 5.4. Nhiều cách chọn thiết bị tốt nhất nên dùng if/else hay interface chiến lược?

Chọn `ISelectionStrategy<T>` để tách thuật toán lựa chọn khỏi `AllocationService<T>`. `HighestPriorityStrategy<T>` chọn điểm cao nhất qua hàm tính điểm được truyền vào, còn `FirstAvailableStrategy<T>` chọn ứng viên đầu tiên; `ChangeStrategy()` cho phép thay thế khi chạy. Đổi lại cần thêm interface và các lớp chiến lược, nhưng không phải sửa chuỗi phân nhánh trong dịch vụ mỗi khi thêm cách chọn mới.

### 5.5. Constraint nào bảo đảm kho chỉ lưu đối tượng có mã và vì sao cần nó?

`where T : IIdentifiable` được đặt trên `IRepository<T>` và `InMemoryRepository<T>`. Ràng buộc này bảo đảm mọi kiểu `T` được sử dụng đều có hợp đồng cung cấp `Id`, nên mã generic được phép truy cập `item.Id` và trình biên dịch từ chối kiểu không đáp ứng hợp đồng. Nó bảo đảm có thuộc tính định danh, không tự bảo đảm mã không rỗng hoặc duy nhất; các giá trị đó vẫn cần được kiểm tra khi thêm vào kho.

### 5.6. Có cần tạo một lớp generic cho mọi lớp trong hệ thống không?

Không. Generic phù hợp khi cùng một thuật toán hoặc cấu trúc cần làm việc với nhiều kiểu mà vẫn giữ an toàn kiểu, như repository, strategy và dịch vụ cấp phát. Các lớp `ProjectorRemote`, `AirConditionerRemote`, `WirelessMicrophone` đã biểu diễn các loại thiết bị cụ thể nên không cần tham số kiểu chỉ để đồng bộ hình thức. Dùng generic không cần thiết sẽ làm khai báo và ràng buộc phức tạp mà không tăng khả năng tái sử dụng.

### 5.7. Nếu bỏ constraint, FindById() gặp khó khăn gì?

Với `T` không có ràng buộc, trình biên dịch không biết đối tượng có thuộc tính `Id`, nên không thể dùng `item.Id` để kiểm tra hoặc lấy khóa khi xây dựng kho. Có thể truyền thêm hàm lấy mã, ép kiểu hoặc dùng reflection, nhưng đó là hợp đồng khác và có thêm độ phức tạp hoặc rủi ro lúc chạy. Trong thiết kế hiện tại, `Dictionary` dùng khóa đã lấy từ `Id` khi thêm, nên constraint còn cần cho việc xây dựng và duy trì phép ánh xạ theo mã, không chỉ riêng thân hàm tìm kiếm.

### 5.8. Nên dùng List<T> hay Dictionary<string, T>? Trade-off là gì?

Mã hiện tại chọn `Dictionary<string, T>` vì nghiệp vụ thường xuyên tìm, xóa và kiểm tra trùng theo mã; các thao tác tra cứu khóa có chi phí trung bình O(1). `List<T>` đơn giản, giữ thứ tự theo chỉ số và phù hợp khi chủ yếu duyệt tuần tự, nhưng tìm hoặc kiểm tra trùng mã thường phải duyệt O(n). Đổi lại, dictionary cần thêm bộ nhớ cho bảng băm và khóa phải ổn định; nếu muốn một thứ tự ưu tiên cụ thể thì phải quy định rõ cách tạo danh sách ứng viên. Lọc theo điều kiện vẫn cần duyệt các phần tử ở cả hai cách lưu.

### 5.9. Vì sao không tạo ba repository có mã gần giống nhau cho ba loại thiết bị?

Các thao tác thêm, xóa, tìm và lọc đều giống nhau, chỉ khác kiểu đối tượng được lưu. Ba lớp riêng sẽ lặp logic kiểm tra và khiến một thay đổi phải được sửa đồng thời ở nhiều nơi. `InMemoryRepository<T>` cho phép dùng cùng phần cài đặt với các kiểu khác nhau, trong khi vẫn giữ kiểu trả về cụ thể cho từng repository. Chỉ nên tách thêm thành phần riêng khi một loại thực sự có yêu cầu lưu trữ khác.
