# Event Booking API

A microservices-based event booking system with JWT authentication, Redis caching, and Docker orchestration.

## 🏗️ Architecture

This project implements a microservices architecture with two main services:

  * **BookingService** (Port 5001) - Manages events, tickets, users, and venues
  * **PaymentService** (Port 5079) - Handles payment processing and validates tickets
  * **PostgreSQL** (Port 5432) - Relational database
  * **Redis** (Port 6379) - Distributed caching

## 🚀 Technologies

  * **Backend:** ASP.NET Core 9.0
  * **Architecture:** Clean Architecture (Domain, Application, Infrastructure, Presentation)
  * **Database:** PostgreSQL 16 with Entity Framework Core
  * **Caching:** Redis with StackExchange.Redis
  * **Authentication:** JWT Bearer tokens
  * **Authorization:** Role-based (User, Admin)
  * **Containerization:** Docker + Docker Compose
  * **Documentation:** Swagger/OpenAPI
  * **Mapping:** AutoMapper
  * **Password Hashing:** BCrypt

## 📋 Features


### Security

  * JWT authentication and authorization
  * Password hashing with BCrypt
  * Role-based access control
  * Secure configuration management



## 🚀 Quick Start

### Option 1: Local Development (Without Docker)

#### 1\. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/EventBookingAPI.git
cd EventBookingAPI
```

#### 2\. Configure Services

Create `appsettings.json` files from the example templates.

```bash
# BookingService
cp BookingService/Presentation/appsettings.Example.json BookingService/Presentation/appsettings.json

# PaymentService
cp PaymentService/Presentation/appsettings.Example.json PaymentService/Presentation/appsettings.json
```

#### 3\. Update Configuration

Edit both `appsettings.json` files and replace the placeholders:

  * `your_username` -> `postgres`
  * `your_password` -> your PostgreSQL password
  * `your_super_secret_key` -> a strong secret key (min 32 characters)

> ⚠️ **Important:** Use the **same** JWT SecretKey in both services\!

#### 4\. Apply Database Migrations

```bash
# BookingService
cd BookingService
dotnet ef database update -p Infrastructure -s Presentation

# PaymentService
cd PaymentService
dotnet ef database update -p Infrastructure -s Presentation
```

#### 5\. Run Services

Run each service in a separate terminal.

```bash
# Terminal 1 - BookingService
cd BookingService/Presentation
dotnet run
```

```bash
# Terminal 2 - PaymentService
cd ../PaymentService/Presentation
dotnet run
```

#### 6\. Access Swagger UI

  * **BookingService:** `http://localhost:5001/swagger`
  * **PaymentService:** `http://localhost:5079/swagger`

-----

### Option 2: Docker Compose (Recommended)

#### 1\. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/EventBookingAPI.git
cd EventBookingAPI
```

#### 2\. Start All Services

This command will build and start all containers (PostgreSQL, Redis, and both API services) in detached mode.

```bash
docker-compose up -d
```

#### 3\. Apply Migrations (First Time Only)

Wait a few seconds for the services to be healthy, then run the migrations *inside* the running containers.

```bash
# BookingService
docker-compose exec booking-service dotnet ef database update

# PaymentService
docker-compose exec payment-service dotnet ef database update
```

#### 4\. Access Services

  * **BookingService Swagger:** `http://localhost:5001/swagger`
  * **PaymentService Swagger:** `http://localhost:5079/swagger`

#### 5\. Stop Services

```bash
# Stop and remove containers
docker-compose down

# Stop and remove containers + volumes (deletes all database data)
docker-compose down -v
```

## 📚 API Documentation

### BookingService Endpoints

#### Authentication

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| POST | /api/auth/register | Register new user | No |
| POST | /api/auth/login | Login and get JWT token | No |

#### Events

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/event | Get all events | Yes |
| GET | /api/event/{id} | Get event by ID | Yes |
| POST | /api/event | Create new event | Yes |
| PATCH | /api/event/{id} | Update event | Yes |
| DELETE | /api/event/{id} | Delete event | Yes |

#### Tickets

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/ticket | Get all tickets | Yes |
| GET | /api/ticket/{id} | Get ticket by ID | Yes |
| GET | /api/ticket/paged?page=1\&pageSize=10 | Get paginated tickets | Yes |
| POST | /api/ticket | Purchase ticket | Yes |
| PATCH | /api/ticket/{id} | Update ticket | Yes |
| DELETE | /api/ticket/{id} | Delete ticket | Yes |

#### Users

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/user | Get all users | Yes |
| GET | /api/user/{id} | Get user by ID | Yes |
| POST | /api/user | Create user | Yes |
| PATCH | /api/user/{id} | Update user | Yes |
| DELETE | /api/user/{id} | Delete user | Yes |

#### Venues

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/venue | Get all venues | Yes |
| GET | /api/venue/{id} | Get venue by ID | Yes |
| POST | /api/venue | Create venue | Yes |
| PATCH | /api/venue/{id} | Update venue | Yes |
| DELETE | /api/venue/{id} | Delete venue | Yes |

### PaymentService Endpoints

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/payment | Get all payments | No |
| GET | /api/payment/{id} | Get payment by ID | No |
| POST | /api/payment | Create payment (validates ticket) | No |

## 🐳 Docker Commands

*(Note: Replace `booking-service`, `payment-service`, and `postgres` with your service names from `docker-compose.yml` if they are different.)*

### Build and Start Services

```bash
# Build images (if needed)
docker-compose build

# Start services in foreground (shows logs)
docker-compose up

# Start services in background (detached)
docker-compose up -d

# Rebuild and start
docker-compose up --build -d
```

### Stop and Remove Services

```bash
# Stop services
docker-compose stop

# Stop and remove containers
docker-compose down

# Stop and remove containers + volumes (deletes database data)
docker-compose down -v
```

### View Logs

```bash
# Follow logs for all services
docker-compose logs -f

# Follow logs for a specific service
docker-compose logs -f booking-service
docker-compose logs -f payment-service
```

### Execute Commands in Containers

```bash
# Access container shell (e.g., booking-service)
docker-compose exec booking-service /bin/bash

# Run migrations (example)
docker-compose exec booking-service dotnet ef database update

# Check PostgreSQL
docker-compose exec postgres psql -U postgres -d bookingService
```

### Check Service Status

```bash
# List running containers for this project
docker-compose ps

# List all running containers on your system
docker ps
```
