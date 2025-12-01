# MyApp Aspire Boilerplate – .NET 10

Một boilerplate Aspire được thiết kế để bạn có thể nhận dự án mới, support microservices architecture ngay từ ngày đầu với PostgreSQL + Redis + RabbitMQ + Auth có sẵn.

## Tech Stack chính

| Thành phần               | Công nghệ                                     | Ghi chú                              |
|--------------------------|-----------------------------------------------|--------------------------------------|
| Runtime                  | .NET 10 (LTS)                                         | Đã lock trong global.json              |
| Orchestration            | Aspire 10 (AppHost + ServiceDefaults)                 | Service discovery, telemetry tự động |
| Database                 | PostgreSQL + Npgsql + EF Core 10                      | Connection string tự inject          |
| Cache / Session          | Redis                                                 | StackExchange.Redis                  |
| Message Broker           | RabbitMQ (có sẵn cấu hình để thay bằng Kafka sau)     | Aspire RabbitMQ component            |
| Authentication           | Duende IdentityServer 8 (nhúng trong solution)        | JWT + Reference token, PostgreSQL store |
| API Style                | Minimal APIs (dễ chuyển sang Controller sau)          | Clean Architecture light             |
| Observability            | OpenTelemetry (Traces, Metrics, Logs) → Aspire Dashboard + Seq | Full distributed tracing             |
| Health Checks            | /healthz + UI trong Aspire Dashboard                  |                                      |
| Logging                  | Serilog → Console + Seq                              |                                      |
| Swagger                  | Swashbuckle + hỗ trợ Bearer token                     |                                      |
| API Versioning           | Microsoft.AspNetCore.Mvc.Versioning (URI-based)       |                                      |
| Resilience               | Polly tự động qua ServiceDefaults                     | Retry, Circuit Breaker, Timeout  |

## Hướng dẫn sử dụng

### Dùng ngay 

```bash
# 1. Clone và đổi tên một lần duy nhất
git clone https://github.com/your-org/myapp-aspire-boilerplate.git MyNewProject
cd MyNewProject

# 2. Đổi tên solution/namespace tự động (Windows)
.\rename.ps1

#    Hoặc Linux/Mac
./rename.sh

# 3. Chạy thôi!
dotnet run --project ./src/AppHost
```

Mở http://localhost:20000 → Aspire Dashboard hiện ra  
Đăng nhập AuthServer: admin@myapp.com / Admin123!  
Tất cả service (AuthServer, BasketService, ProductService, postgres, redis, rabbitmq) đã chạy và kết nối với nhau.

## Cấu trúc thư mục

```
src/
├── AppHost/              ← Aspire orchestrator (chạy bằng dotnet run)
├── ServiceDefaults/      ← Shared: OpenTelemetry, Resilience, HealthChecks, Extensions
├── AuthServer/           ← Duende IdentityServer 8 + PostgreSQL store
├── BasketService/        ← Ví dụ service 1 (Minimal API)
├── ProductService/       ← Ví dụ service 2
├── SharedKernel/         ← Common DTOs, Exceptions, Result<T>, Extensions
```

## Khi bạn muốn mở rộng

Template đã chuẩn bị sẵn các option sau (sẽ bật trong phiên bản 1.1):

```bash
--with-kafka              Thay RabbitMQ bằng Kafka
--with-grpc               Thêm gRPC template + protobuf
--with-mongodb            Thêm MongoDB
--external-auth           Chuyển từ Duende nội bộ sang Keycloak bên ngoài
--no-auth                 Public API (loại bỏ hoàn toàn auth)
```

## Contributing

Rất hoan nghênh! Hãy mở issue hoặc PR nếu bạn muốn thêm:
- Azure AD / Entra ID mẫu
- EventStoreDB
- Dapr integration
- GitHub Actions CI/CD mẫu cho Azure Container Apps

## License

MIT © 2025 – Dùng thoải mái cho cả dự án thương mại.

Start fast. Scale later. Never rewrite auth again.