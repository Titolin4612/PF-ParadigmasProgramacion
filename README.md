# Mythological Card Game - Proyecto Final Paradigmas de Programación

[![Typing SVG](https://readme-typing-svg.demolab.com/?font=Alfa+Slab+One&size=30&pause=1000&color=98F7D0&vCenter=true&width=435&lines=Proyecto+Final+Paradigmas)](https://git.io/typing-svg)

## 👤 Integrantes

- `Santiago Hernández Morantes`
- `Juan José Mesa Cardona`

---

## 📖 Descripción

Juego de cartas mitológico donde jugadores compiten usando cartas de tres tipos: juego, premio y castigo. Cada carta tiene efectos únicos basados en la mitología mundial.

---

## 🏗️ Arquitectura

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

## 🚀 Inicio Rápido

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://docs.docker.com/get-docker/) (opcional)

### Local Development

```bash
# Clonar repositorio
cd "MVC Web App"

# Restaurar dependencias
dotnet restore

# Ejecutar en desarrollo
dotnet run
```

Abre [http://localhost:5141](http://localhost:5141) en tu navegador.

### Docker

```bash
# Build y ejecución
docker-compose up -d

# Ver logs
docker-compose logs -f

# Detener
docker-compose down
```

---

## 🃏 Distribución de la Baraja

| Tipo | Cantidad | Probabilidad |
|------|----------|--------------|
| Cartas de Juego | 30 | 60% |
| Cartas de Castigo | 17 | 24% |
| Cartas de Premio | 13 | 16% |

### Rareza de Cartas de Juego

| Rareza | Puntos | Probabilidad |
|--------|--------|--------------|
| Legendaria | +2 | 10% |
| Épica | +1 | 20% |
| Rara | 0 | 20% |
| Especial | -1 | 25% |
| Común | -2 | 25% |

---

## 🔧 Configuración

### Variables de Entorno

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  },
  "Jwt": {
    "Key": "TuClaveSecretaDeAlMenos32Caracteres!",
    "Issuer": "MythologicalCardGame",
    "Audience": "MythologicalCardGameUsers",
    "ExpiryMinutes": 60
  }
}
```

### Endpoints de Salud

- `GET /health` - Health check básico
- `GET /health/ready` - Readiness check (verifica DB)

---

## 🔐 Seguridad

- **Autenticación**: JWT Bearer tokens
- **Contraseñas**: BCrypt hashing
- **Sesiones**: 30 minutos timeout, HttpOnly cookies
- **Data Protection**: Keys persistidas para producción

---

## 🧪 Testing

```bash
# Ejecutar todos los tests
dotnet test

# Con coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📊 API de Persistencia

### Entidades

| Entidad | Descripción |
|---------|-------------|
| `Usuario` | Nickname, password hash, fecha registro |
| `Partida` | Ganador, puntos, jugadores, fecha |
| `Estadistica` | Partidas jugadas/ganadas, promedio puntos |

---

## 📝 Changelog

[**📝 Ver ChangeLog**](./ChangeLog.md)

---

## 📄 Licencia

MIT License
