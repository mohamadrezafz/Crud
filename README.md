# CRUD
This repository contains a simple CRUD application implemented in .NET that follows specific requirements and guidelines. The application allows users to perform CRUD operations on a Customer model.

## Model
The Customer model consists of the following properties:

- FirstName
- LastName
- DateOfBirth
- PhoneNumber
- Email
- BankAccountNumber

## Validations
The following validations are implemented in the application:

- Phone number validation using Google LibPhoneNumber
- Valid email and bank account number checks
- Uniqueness of customers in the database based on FirstName, LastName, and DateOfBirth
- Uniqueness of email in the database

## Delivery
To deliver the project, please follow these steps:

1. Clone this repository to your local machine.
2. Navigate to the project directory.
3. Run the following command to set up the SQL database and start the .NET project using Docker Compose:
   ```
   docker-compose up
   ```
4. Once the Docker containers are up and running, you can access the application at the specified URL or port.


