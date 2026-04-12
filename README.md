# AI Content Assistant

AI Content Assistant is a modular, service‑oriented backend system designed to generate, store, and manage AI‑powered content.
The platform consists of two independent .NET services:

- ProxyApi — a secure gateway that validates API keys and forwards user prompts to an LLM provider (Groq).

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

## Tech Stack

- .NET 9 Web API
- ASP.NET Core Minimal Hosting Model
- Entity Framework Core (Code‑First)
- HTTP-based service-to-service communication
- Typed HttpClient with retry policies (Polly)
- DTO‑based request/response mapping

## User Secrets 

### ContentApi
"ProxyApi": {
  "BaseUrl": "http://localhost:5112/",
  "SecretKey": "<shared-secret-between-services>"
}

### ProxyApi 
"ProxyApi": {
  "ApiKey": "<your-llm-provider-api-key>",
  "ModelName": "llama-3.1-8b-instant",
  "SecretKey": "<shared-secret-between-services>"
}


## How to Run the Project

### 1. Clone the repository
   git clone <repository-url>
   cd <project-folder>

### 2. Configure User Secrets
 - Each service requires its own User Secrets configuration.
 - See the User Secrets section above for exact values.

### 3. Apply database migrations (ContentApi only)
 - dotnet ef database update

### 4. Start the services
- cd ContentApi
  dotnet run
- cd ProxyApi
  dotnet run






