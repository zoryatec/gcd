FROM dawidwozny/labview:sdk_23.3.6_x86_net8

WORKDIR /app

# Copy the solution and all source/test files
COPY gcd.sln ./
COPY src ./src
COPY tests ./tests

# Restore dependencies
RUN dotnet restore gcd.sln

# Build the solution
RUN dotnet build gcd.sln --configuration Release

# Run the end-to-end tests
CMD ["dotnet", "test", "tests/Gcd.Tests.EndToEnd.LabView/Gcd.Tests.EndToEnd.LabView.csproj"]
