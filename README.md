# PropertyAnalyzer

A modern .NET 9.0 web API for real estate property analysis and search, featuring PostgreSQL for data persistence and Elasticsearch for advanced search capabilities.

## 🏗️ Architecture Overview

PropertyAnalyzer follows a clean, layered architecture with the following components:

### Core Architecture
- **API Layer**: ASP.NET Core Web API with RESTful endpoints
- **Service Layer**: Business logic and analysis services
- **Repository Layer**: Data access abstraction
- **Data Layer**: Entity Framework Core with PostgreSQL
- **Search Layer**: Elasticsearch integration for advanced search and analytics
- **Background Services**: Automated property analysis worker

### Technology Stack
- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core 8.0**: ORM for PostgreSQL
- **PostgreSQL 15**: Primary database
- **Elasticsearch 8.13.0**: Search and analytics engine
- **NEST**: .NET client for Elasticsearch
- **Docker & Docker Compose**: Containerization and orchestration

## 📁 Project Structure

```
PropertyAnalyzer/
├── Controllers/                 # API endpoints
│   └── PropertyAnalysisController.cs
├── Services/                    # Business logic services
│   ├── IPropertyAnalysisService.cs
│   └── PropertyAnalysisService.cs
├── Repositories/               # Data access layer
│   ├── IPropertyRepository.cs
│   └── PropertyRepository.cs
├── Models/                     # Domain entities
│   └── Property.cs
├── DTOs/                       # Data transfer objects
│   ├── PropertySearchFilter.cs
│   └── AnalysisResultDto.cs
├── Data/                       # Database context and seeding
│   ├── ApplicationDbContext.cs
│   └── Seed/
│       └── DbSeeder.cs
├── Elasticsearch/              # Elasticsearch integration
│   ├── ElasticsearchClientFactory.cs
│   └── Services/
│       ├── IElasticsearchService.cs
│       ├── ElasticsearchService.cs
│       ├── IAnalyticsService.cs
│       ├── AnalyticsService.cs
│       └── SyncService.cs
├── Background/                 # Background services
│   └── PropertyAnalysisWorker.cs
├── Migrations/                 # EF Core migrations
├── Program.cs                  # Application entry point
├── appsettings.json           # Configuration
├── docker-compose.yml         # Container orchestration
└── Dockerfile                 # Container definition
```

## 🏠 Data Model

### Property Entity
```csharp
public class Property
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public decimal Price { get; set; }
    public double AreaInSqFt { get; set; }
    public string? City { get; set; }
    public DateTime ListedDate { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
```

### Search Filter DTO
```csharp
public class PropertySearchFilter
{
    public string? City { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double? MinArea { get; set; }
    public double? MaxArea { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? RadiusKm { get; set; } = "10km";
}
```

## 🚀 Features

### Core Features
- **Property Management**: CRUD operations for real estate properties
- **Advanced Search**: Multi-criteria property search with geospatial queries
- **Analytics**: Price analysis, city-based statistics, and market insights
- **Real-time Analysis**: Background worker for continuous property analysis
- **Data Synchronization**: Bidirectional sync between PostgreSQL and Elasticsearch

### Search Capabilities
- **Text Search**: City-based property search
- **Range Queries**: Price and area filtering
- **Geospatial Search**: Location-based property discovery with radius queries
- **Aggregations**: Average prices by city, top cities by property count

### Analytics Features
- **Price per Square Foot**: Automated calculation for property valuation
- **Market Statistics**: City-wise property distribution and pricing trends
- **Bulk Analysis**: Efficient processing of large property datasets

## 🛠️ Setup & Installation

### Prerequisites
- Docker and Docker Compose
- .NET 9.0 SDK (for local development)

### Quick Start with Docker

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd PropertyAnalyzer
   ```

2. **Start the services**
   ```bash
   docker-compose up -d
   ```

3. **Access the application**
   - API: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger
   - Elasticsearch: http://localhost:9200

### Local Development Setup

1. **Install dependencies**
   ```bash
   dotnet restore
   ```

2. **Update connection strings** in `appsettings.json`
   ```json
   {
     "ConnectionStrings": {
       "Default": "Host=localhost;Database=PropertyAnalyzer;Username=postgres;Password=1234"
     },
     "Elasticsearch": {
       "Uri": "http://localhost:9200"
     }
   }
   ```

3. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Start the application**
   ```bash
   dotnet run
   ```

## 📡 API Endpoints

### Property Analysis
- `GET /api/PropertyAnalysis/analyze` - Analyze all properties
- `POST /api/PropertyAnalysis/sync` - Sync data to Elasticsearch

### Analytics
- `GET /api/PropertyAnalysis/avg-price-by-city` - Get average prices by city
- `GET /api/PropertyAnalysis/top-cities` - Get top cities by property count

### Search
- `POST /api/PropertyAnalysis/search` - Advanced property search

### Example Search Request
```json
{
  "city": "Lahore",
  "minPrice": 5000000,
  "maxPrice": 15000000,
  "minArea": 700,
  "maxArea": 1200,
  "latitude": 31.5204,
  "longitude": 74.3587,
  "radiusKm": "5km"
}
```

## 🔄 Background Services

### Property Analysis Worker
- **Purpose**: Automated property analysis every hour
- **Functionality**: Calculates price per square foot for all properties
- **Implementation**: `PropertyAnalysisWorker` class extending `BackgroundService`

## 🗄️ Database Design

### PostgreSQL Schema
- **Properties Table**: Main property data with indexes on City, ListedDate, and Price
- **Optimizations**: Proper indexing for search performance
- **Data Types**: 
  - Price: `numeric(18,2)` for precise financial data
  - Area: `double precision` for square footage
  - Coordinates: `double precision` for geospatial data

### Elasticsearch Mapping
- **Index**: `properties`
- **Features**: 
  - Full-text search on address and city
  - Numeric range queries on price and area
  - Geospatial queries with geo_distance filter
  - Aggregations for analytics

## 🔧 Configuration

### Environment Variables
- `ConnectionStrings:Default`: PostgreSQL connection string
- `Elasticsearch:Uri`: Elasticsearch server URL

### Docker Configuration
- **PostgreSQL**: Port 5432, database `propertiesdb`
- **Elasticsearch**: Port 9200, single-node setup
- **API**: Port 5000, exposed on host

## 🧪 Testing

### HTTP Client File
The project includes `PropertyAnalyzer.http` for testing API endpoints with tools like VS Code REST Client or JetBrains Rider.

### Sample Data
The application automatically seeds 1,000 sample properties across 5 Pakistani cities:
- Lahore, Karachi, Islamabad, Peshawar, Multan

## 🚀 Performance Optimizations

### Database Optimizations
- **Indexing**: Strategic indexes on frequently queried fields
- **Pagination**: Batch processing for large datasets
- **Async Operations**: Non-blocking database operations

### Elasticsearch Optimizations
- **Bulk Operations**: Efficient bulk indexing for large datasets
- **Aggregations**: Pre-computed analytics for fast retrieval
- **Geospatial Indexing**: Optimized location-based queries

### Application Optimizations
- **Concurrent Processing**: Parallel analysis of property batches
- **Memory Management**: Efficient data structures and disposal patterns
- **Caching**: Service-level caching for frequently accessed data

## 🔒 Security Considerations

- **Input Validation**: Proper model validation and sanitization
- **SQL Injection Prevention**: Parameterized queries via EF Core
- **CORS Configuration**: Configurable cross-origin resource sharing
- **Environment-based Configuration**: Separate settings for development/production

## 📈 Monitoring & Logging

- **Structured Logging**: Built-in ASP.NET Core logging
- **Performance Metrics**: Background service execution tracking
- **Error Handling**: Comprehensive exception handling and logging

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For issues and questions:
1. Check the existing issues
2. Create a new issue with detailed information
3. Include logs and error messages when applicable 