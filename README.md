# Shipping Integration Microservices (.NET 8)
## Proje Mimarisi
### 1. Order Service
- Sipariş alma (POST /orders)
- Sipariş detaylarını getirme (GET /orders/{id})
- Siparişi oluşturduktan sonra `OrderPaidEvent` ile Shipping Service'e haber verir.
- Sipariş bilgilerini PostgreSQL veritabanında saklanır.

### 2. Shipping Service
- `OrderPaidEvent` mesajlarını RabbitMQ üzerinden alır.
- Shipping service kargo sürecini yönetir: Prepared → InTransit → Delivered
- Sipariş durumlarını Redis’e yazar.
- PostgreSQL veritabanına kayıt eder.
- Kargo durumunu takip etmek için (GET /track/{trackingId}) endpoint’i sunar.

## Kullanılan Teknolojiler
- .NET 8 – Web API (ASP.NET Core)
- PostgreSQL – Veritabanı
- RabbitMQ – Asenkron iletişim (MassTransit ile birlikte.)
- Redis – Shipment durumları için cache tutulması.
- Docker Compose – Tüm servisleri ayağa kaldırma işlemi için.
- Swagger – API dokümantasyonu
- xUnit – Birim testler

## Proje yapısı
![image](https://github.com/user-attachments/assets/49d8f00a-8f84-4e86-a7a7-4fa46ea9c09f)


## Docker ile Servisleri Başlatma
docker-compose up --build
