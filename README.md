# RESTful API Tests - xUnit + C#

This project contains automated API tests for:
https://restful-api.dev/

## Technologies Used

- C#
- .NET 8
- xUnit
- HttpClient

## Test Scenarios Covered

1. Get list of all objects
2. Create a new object using POST
3. Get a single object using created ID
4. Update the created object
5. Delete the object
6. Verify deleted object returns 404
7. Negative validation test

## Prerequisites

Install the following:

- .NET SDK 8.0 or later

Verify installation:

```bash
dotnet --version

How to Run the Tests
Clone the repository
git clone <your-github-repo-url>
Navigate to project
cd RestfulApiTests
Restore dependencies
dotnet restore
Run tests
dotnet test
Expected Output

You should see output similar to:

Passed!  - Failed: 0, Passed: 7

Output can be viewed under Reports/TestResultsOutput.txt