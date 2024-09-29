API Application Documentation 

Table of Contents 

Introduction 

Setup Instructions 

Running the Application 

Testing the API 

Assumptions and Trade-offs 

Known Issues and Future Improvements 

1. Introduction 

This API application is designed to manage products and categories, allowing users to create, retrieve, update, and delete products and categories, with relationships between them. The application uses ASP.NET Core for the backend, Entity Framework Core for database management, and Swagger for API testing. 

2. Setup Instructions 

Prerequisites 

.NET 6.0 SDK or later 

SQL Server (or a similar database) 

Entity Framework Core 

Steps 

Clone the repository: 

bash 

git clone https://github.com/your-repository-url.git 
 

Navigate to the project directory: 

bash 

cd YourProjectDirectory 
 

Install dependencies: For Bash: 

bash 

dotnet restore 
 

For NuGet Package Manager Console: 

powershell 

Update-Package 
 

Update the appsettings.json: Ensure that the database connection string in appsettings.json is correctly set to your local or hosted SQL Server instance: 

json 

"ConnectionStrings": { 
  "DefaultConnection": "Server=your-server;Database=your-database;User Id=your-username;Password=your-password;" 
} 
 

Apply Migrations: Use the following commands to apply migrations and seed the database: 

For Bash: 

bash 

dotnet ef database update 
 

For NuGet Package Manager Console: 

powershell 

Update-Database 
 

Build the project: Run the following command to build the application: 

For Bash: 

bash 

dotnet build 
 

For NuGet Package Manager Console: 

powershell 

Build-Solution 

 

3. Running the Application 

Run the API locally: Use the following command to start the application: 

For Bash: 

bash 

dotnet run 
 

For NuGet Package Manager Console: 

powershell 

Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run" 
 

Access the Swagger UI: Once the application is running, Swagger documentation will be available at: 

bash 

https://localhost:{port}/swagger/index.html 
 

Swagger allows you to interact with the API directly through your browser and test different endpoints. 

4. Testing the API 

Using Swagger 

Navigate to Swagger: Open your browser and go to https://localhost:{port}/swagger/index.html. 

Test Endpoints: 

GET /api/products: Retrieve all products. 

GET /api/products/{id}: Retrieve a specific product by ID. 

POST /api/products: Add a new product. Make sure to include required fields such as name, price, description, and associated categories. 

PUT /api/products/{id}: Update an existing product. 

DELETE /api/products/{id}: Delete a product. 

5. Assumptions and Trade-offs 

Assumptions: 

Products can be associated with multiple categories, and categories can contain multiple products (many-to-many relationship). 

All data inputs (e.g., product price, stock quantity) must be validated before submitting via the API. 

Trade-offs: 

Performance: I chose to use lazy loading for related entities. While this simplifies relationship management, it can lead to N+1 query issues. In future development, eager loading can be used to optimize performance. 

Error Handling: For simplicity, basic validation is performed on inputs. More robust error handling and validation should be added for production, particularly in relation to complex business rules. 

6. Known Issues and Future Improvements 

Error Handling: Some errors may not be caught gracefully, and logging is minimal. Future versions should implement more robust exception handling and logging mechanisms. 

Pagination: Currently, the API returns all products or categories in a single request. Adding pagination would improve performance when dealing with large datasets. 

Authentication: The API currently lacks authentication and authorization. Implementing JWT-based security or OAuth would be a good next step. 

 
 

 
