# Acme Corp API

General framework for streamlining the creation process of .NET Core based micro-service APIs.

**Note**: This project was created using `.NET Core 8.0`.

## Features
- Inventory
    - Manage customers and their information (name, phone, address, and etc)
    - Manage orders and who is connected to them (amount, price, etc)
    - Manage products (name, sku, price, etc)
- Containerized
- Advanced Logging
- Dynamic & modular
    - Automatically detects and registers the following (albeit manual few imports still being required):
        - Endpoints
        - Models
- Secure communications
    - Authentication
        - Api Key
    - Salted password generation
    - Cryptographically strong RNG
    - SHA-512 hashing along with several other algorithms
- Object Relational Mapping (ORM) ready
    - Uses PostgreSQL for the core database technology

## Endpoints & Models
Pre-configured endpoints and models have been created to showcase possibility :
- Customers *(model)*
    - Create *(endpoint)*
    - Get *(endpoint)*
    - Update *(endpoint)*
    - Delete *(endpoint)*
- Products *(model)*
    - Create *(endpoint)*
    - Get *(endpoint)*
    - Update *(endpoint)*
    - Delete *(endpoint)*
- Orders *(model)*
    - Create *(endpoint)*
    - Get *(endpoint)*
    - Update *(endpoint)*
    - Delete *(endpoint)*

**BONUS**: States *(model)* with a Get *(endpoint)* for customer locations. 

## Starting the API
### With Docker
1. Change directories into the main folder:  
`cd acme`

2. Build an image of the current source code to actually run with docker:  
`docker build --tag acme-api .`

3. Run the image with docker and expose the correct ports with the `-p` flag:  
*(Adding `-d` will allow you to run in "detached" mode so you can run the image in the background!)*  
`docker run -p 5000:5000 acme-api`  

4. Open a terminal use the following command to test that the API works (it should display a hash):  
`curl http://0.0.0.0:5000/api/customers`

### With Docker-Compose
1. Change into the source directory (if still in the downloads folder):  
`cd acme` 

2. Start or run the boilerplate microservice API (it will build and start the service for you):  
`docker-compose up`

3. Open a terminal use the following command to test that the API works (it should display a list of randomly generated people):  
`curl -X GET "http://localhost:5000/api/customers" -H  "accept: text/plain" -H "AcmeApiKey: test-key-changeme"`

    (*Alternatively, navigate to http://localhost:5000 in your browser to view the API using Swagger.*)

**Note**: The API requires an API key to be present, otherwise, it will always nag you to providee one. However, you can comment out the middleware within the `startup.cs` file to use the API without a key and to see Swagger documentation.

**Tip**: *If on Linux or macOS, you can use the following command to stop, remove, and purge all images and containers at once:  
`docker ps -aq | xargs docker stop | xargs docker rm; docker image prune -a`*

## Integration & Unit Tests
1. Change directories into the main folder (if still in the downloads folder):  
`cd acme`

2. Run the native test command with dotnet:  
`dotnet test`

3. That's it! The test project should restore all required packages, migrate/populate the database tables, and start running all of the integration and unit tests within the testing project.

## Demonstration
https://github.com/jasondrawdy/Acme/assets/40871836/b470e55a-a8a1-486d-9a10-6c0ceaa6d08a

