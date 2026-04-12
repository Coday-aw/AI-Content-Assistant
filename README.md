## AI Content Assistant

AI Content Assistant is a modular, service‑oriented backend system designed to generate, store, and manage AI‑powered content.
The platform consists of two independent .NET services:

- ProxyApi — a secure gateway that validates API keys and forwards user prompts to an LLM provider (e.g., Groq, OpenAI, or local models).

- ContentApi — a content management service responsible for storing prompt history, querying historical data, and exposing REST endpoints for client applications.

The system is built with scalability, clean architecture, and developer‑friendly extensibility in mind.
Each service is isolated, communicates via HTTP, and follows SOLID principles to ensure maintainability and testability.


## Architecture Overview
### ProxyApi
- Acts as a secure gateway between clients and the LLM provider
- Validates API keys before processing requests
- Forwards user prompts to the LLM service (Groq)
- Returns structured responses using DTOs
- Uses HttpClient with retry policies for resilient outbound calls

### ContentApi
- Stores and retrieves prompt history
- Exposes queryable endpoints for filtering by category, date, or time range
- Uses Entity Framework Core with a SQL database
- Returns clean, typed DTOs for frontend consumption
- Designed for extensibility (categories, metadata, analytics, etc.)

## Tech Stack

- .NET 9 Web API
- ASP.NET Core Minimal Hosting Model
- Entity Framework Core (Code‑First)
- HTTP-based service-to-service communication
- Typed HttpClient with retry policies (Polly)
- DTO‑based request/response mapping
