## 🚀 Hướng dẫn cài đặt và sử dụng

### Các bước cài đặt

1.  **Clone repository:**
    ```bash
    git clone [https://github.com/minhthoai104/FaceRecoSystem.git](https://github.com/minhthoai104/FaceRecoSystem.git)
    ```

2.  **Cài đặt Cơ sở dữ liệu:**
    - Mở SQL Server Management Studio (SSMS).
    - Chạy file script SQL có trong thư mục `database` để tạo cơ sở dữ liệu và các bảng cần thiết.
    - Cập nhật chuỗi kết nối (connection string) trong file `DatabaseHelper.cs` và `App.config` cho phù hợp với cấu hình SQL Server.

3.  **Mở và Build Project:**
    - Mở file `.sln` bằng Visual Studio.
    - Visual Studio sẽ tự động khôi phục các gói NuGet cần thiết.
    - Nhấn **Build Solution** (Ctrl + Shift + B).
    - Chạy project (F5).
