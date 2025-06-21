TiendanaMP.SDK

**TiendanaMP.SDK** es una biblioteca en C# que permite la integración directa con la API REST de [Mercado Pago](https://www.mercadopago.com.co/developers/es) sin depender del SDK oficial. Implementa de forma segura y flexible los principales flujos de autenticación y pagos, incluyendo:

- 🔐 OAuth 2.0 con PKCE
- 💳 Pagos con tarjeta
- 🏦 Pagos PSE (transferencias bancarias)
- 🔄 Renovación automática de tokens
- 📦 Arquitectura modular y desacoplada
- ⚙️ Soporte para entornos productivos y sandbox

---

TiendanaMP.SDK/
├── Models/
│   ├── Request/
│   ├── Response/
│   └── Token_Request/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Utils/
├── MercadoPagoConstants.cs
├── TiendanaMP.SDK.csproj
└── ...

