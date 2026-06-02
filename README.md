# Blessings & Curses

[![Typing SVG](https://readme-typing-svg.demolab.com/?font=Alfa+Slab+One&size=30&pause=1000&color=98F7D0&vCenter=true&width=435&lines=Proyecto+Final+Paradigmas)](https://git.io/typing-svg)

## 👤 Contributors

- `Santiago Hernandez M`
- `Juan Jose Mesa Cardona`

---

## 📖 Description

Mythological card game where players compete using three types of cards: game, prize, and punishment. Each card has unique effects based on world mythology.

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        MVC Web App                               │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────────┐ │
│  │ Controllers  │  │  Services   │  │     Middleware          │ │
│  │ - Home       │  │ - Juego     │  │ - Authentication (JWT)  │ │
│  │ - Juego      │  │ - Home      │  │ - Authorization        │ │
│  │ - Reglas     │  │ - Reglas    │  │ - Session              │ │
│  │              │  │ - Leaderboard│ │ - Health Checks        │ │
│  └─────────────┘  └─────────────┘  └─────────────────────────┘ │
│                            │                                    │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │                    Data Layer (EF Core + SQLite)            ││
│  │         Usuarios | Estadisticas | Partidas                 ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      Class Library                               │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────────┐ │
│  │   Clases    │  │  Interfaces │  │    Aspectos (AOP)       │ │
│  │ - Carta     │  │ - IJuego    │  │ - AuthInterceptor       │ │
│  │ - Jugador   │  │ - IHome     │  │ - InterceptorValidacion │ │
│  │ - Juego     │  │ - IReglas   │  │ - InterceptorCargaArch  │ │
│  │ - Baraja    │  │             │  │                         │ │
│  └─────────────┘  └─────────────┘  └─────────────────────────┘ │
│                                                                  │
│  ┌─────────────────────────────────────────────────────────────┐ │
│  │                        Eventos                             │ │
│  │    Publisher_Eventos_Juego | Jugador | Cartas               │ │
│  └─────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🚀 Quick Start

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://docs.docker.com/get-docker/) (optional)

### Local Development

```bash
# Clone repository
cd "MVC Web App"

# Restore dependencies
dotnet restore

# Run in development
dotnet run
```

Open [http://localhost:5141](http://localhost:5141) in your browser.

### Docker

```bash
# Build and run
docker-compose up -d

# View logs
docker-compose logs -f

# Stop
docker-compose down
```

---

## 🃏 Deck Distribution

| Type | Quantity | Probability |
|------|----------|--------------|
| Game Cards | 30 | 60% |
| Punishment Cards | 17 | 24% |
| Prize Cards | 13 | 16% |

### Game Card Rarity

| Rarity | Points | Probability |
|--------|--------|--------------|
| Legendary | +2 | 10% |
| Epic | +1 | 20% |
| Rare | 0 | 20% |
| Special | -1 | 25% |
| Common | -2 | 25% |

---

## 🔧 Configuration

### Environment Variables

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  },
  "Jwt": {
    "Key": "YourSecretKeyOfAtLeast32Characters!",
    "Issuer": "BlessingsAndCurses",
    "Audience": "BlessingsAndCursesUsers",
    "ExpiryMinutes": 60
  }
}
```

### Health Endpoints

- `GET /health` - Basic health check
- `GET /health/ready` - Readiness check (verifies DB)

---

## 🔐 Security

- **Authentication**: JWT Bearer tokens
- **Passwords**: BCrypt hashing
- **Sessions**: 30 minute timeout, HttpOnly cookies
- **Data Protection**: Keys persisted for production

---

## 🧪 Testing

```bash
# Run all tests
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📊 Persistence API

### Entities

| Entity | Description |
|--------|-------------|
| `Usuario` | Nickname, password hash, registration date |
| `Partida` | Winner, points, players, date |
| `Estadistica` | Games played/won, average points |

## 📄 License

This project is licensed under the GNU Affero General Public License v3.0 or later (AGPL-3.0-or-later). See the [LICENSE](./LICENSE) file for details.

### Note on AGPL and Network Use

When this software is used over a network (such as a web application/SaaS), users who interact with a modified version of the software have the right to access the corresponding source code under the terms of AGPL-3.0-or-later.

---

**Copyright (C) 2026 Santiago Hernandez M**
