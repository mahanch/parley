# Parley Discord-Clone - Architecture & Setup Guide

## Step 1: .NET Aspire AppHost Setup ✅ COMPLETED

### Overview
This step establishes the foundational infrastructure orchestration for the Parley Discord-clone application using .NET Aspire with NuGet packages (not the deprecated workload).

### Project Structure Created

```
Parley.sln
├── Parley.Domain (Class Library)
│   └── No external dependencies (Pure DDD)
├── Parley.Application (Class Library)
│   └── Depends on: Domain, MediatR
├── Parley.Infrastructure (Class Library)
│   └── Depends on: Application, EF Core, SignalR, Redis
├── Parley.Api (Web API)
│   └── Depends on: Application, Infrastructure, ServiceDefaults
├── Parley.ServiceDefaults (Class Library)
│   └── Shared service configuration for Aspire
└── Parley.AppHost (Console App)
    └── Orchestrates all services: PostgreSQL, Redis, LiveKit, API
```

### Dependency Graph

```
Domain (no dependencies)
  ↑
Application (depends on Domain + MediatR)
  ↑
Infrastructure (depends on Application + EF Core/SignalR/Redis)
  ↑
Api (depends on Application + Infrastructure + ServiceDefaults)
  ↑
AppHost (orchestrates all)
```

### Infrastructure Components

#### 1. **PostgreSQL**
- Container: `postgres:latest`
- Purpose: Primary relational database
- Connection: Service reference in AppHost
- Features:
  - JSONB support for MessageContent aggregation
  - Full-text search capabilities
  - JSON operators for filtering mentions
  
#### 2. **Redis**
- Container: `redis:latest`
- Purpose: SignalR backplane, caching, session store
- Use Cases:
  - Real-time message sync across multiple API instances
  - User online/offline status caching
  - Rate limiting and distributed caching

#### 3. **LiveKit**
- Container: `livekit/livekit-server:v1.11.0`
- Ports:
  - `7880`: RTC (Real-Time Communication)
  - `7881`: WebRTC signaling
  - `7882`: Admin API
- Purpose: Voice and Video call infrastructure
- Credentials:
  - API Key: `devkey`
  - API Secret: `secret`

### AppHost Configuration (Program.cs)

**Key Features:**
- Service references automatically wire connection strings
- Health checks enabled for PostgreSQL
- Environment variable injection for secure config
- Port bindings for LiveKit services
- HTTP endpoint configuration for API service

**Running the AppHost:**

```bash
cd Parley.AppHost
dotnet run
```

This will:
1. Start PostgreSQL container
2. Start Redis container
3. Start LiveKit container
4. Start Parley.Api service
5. Open Aspire Dashboard (localhost:15000)

### NuGet Packages Used

**Aspire Orchestration:**
- `Aspire.Hosting` v9.1.0
- `Aspire.Hosting.PostgreSQL` v9.1.0
- `Aspire.Hosting.Redis` v9.1.0

**Service Defaults:**
- `Microsoft.Extensions.ServiceDiscovery` v9.1.0
- `Microsoft.Extensions.Resilience` v9.1.0

**Infrastructure (in Parley.Infrastructure):**
- `Microsoft.EntityFrameworkCore` v9.0.0
- `Microsoft.EntityFrameworkCore.PostgreSQL` v9.0.0
- `Microsoft.EntityFrameworkCore.Tools` v9.0.0
- `Microsoft.AspNetCore.SignalR.Core` v9.0.0
- `StackExchange.Redis` v2.8.9

**Application (in Parley.Application):**
- `MediatR` v12.4.1

### Configuration Files

**Parley.Api/appsettings.json:**
- PostgreSQL connection string
- Redis host/port configuration
- LiveKit API endpoint and credentials
- Logging configuration

### Next Steps

👉 **Step 2:** Generate the complete `Parley.Domain` project
   - Chats Aggregate (Conversation, ConversationParticipant, Message)
   - Servers Aggregate (Server, ServerRole)
   - Value Objects and Enums
   - Domain Exceptions
   - Ensure strict DDD principles with private setters and rich behaviors

### Verification

To verify the AppHost setup works:

```bash
# Build entire solution
dotnet build

# Run AppHost (requires Docker)
cd Parley.AppHost
dotnet run
```

Expected output:
- Aspire Dashboard accessible at `http://localhost:15000`
- PostgreSQL running on port 5432
- Redis running on port 6379
- LiveKit running on ports 7880-7882
- Parley.Api service registered and running

---

**Status:** ✅ Step 1 Complete - Ready for Step 2

