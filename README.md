# Playlist Service API

A robust, highly scalable REST API built with ASP.NET 10 to manage user playlists and songs. This service is architected using **Clean Architecture** principles and the **SOLID** design philosophy to ensure maintainability, testability, and clear separation of concerns.

## 🏗️ Architecture Overview

This project strictly adheres to Clean Architecture, dividing the system into four decoupled layers:

* **1. Domain (`PlaylistService.Domain`)**
  The core of the system. Contains enterprise-wide logic, fundamental entities (e.g., `Playlist`, `Song`), custom exceptions, and enums. It has **zero dependencies** on any other layer or external framework.

* **2. Application (`PlaylistService.Application`)**
  Contains the business logic and use cases. It defines interfaces (such as repository contracts) that the Infrastructure layer will implement. It only depends on the Domain layer.

* **3. Infrastructure (`PlaylistService.Infrastructure`)**
  Handles external concerns, primarily data access. This layer contains the Entity Framework Core `DbContext`, database migrations, and the concrete implementation of the repository interfaces defined in the Application layer.

* **4. API (`PlaylistService.Api`)**
  The presentation layer. It acts as the entry point for the system, exposing RESTful endpoints via Controllers. It is kept intentionally thin, delegating all business logic execution to the Application layer.

## 🛠️ Tech Stack

* **Framework:** .NET 10 (ASP.NET Core Web API)
* **Architecture:** Clean Architecture / N-Tier
* **Data Access:** Entity Framework Core (Relational SQL)
* **Testing:** xUnit & Moq *(Planned)*

## 🚀 Getting Started

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* Visual Studio Code (with C# Dev Kit) or Visual Studio 2022+
* SQL Server or PostgreSQL *(Database configuration pending in Phase 2)*

### Build Instructions

To build the solution and verify the dependency graph from the terminal, run the following command from the root directory:

```bash
dotnet build
