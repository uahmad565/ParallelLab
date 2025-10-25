# ParallelLab Setup Script
# This script sets up the development environment for ParallelLab

Write-Host "Setting up ParallelLab Development Environment..." -ForegroundColor Green

# Check if .NET 8 is installed
Write-Host "Checking .NET 8 installation..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    if ($dotnetVersion -like "9.*") {
        Write-Host "✓ .NET 8 found: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "✗ .NET 8 not found. Please install .NET 8 SDK" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ .NET not found. Please install .NET 8 SDK" -ForegroundColor Red
    exit 1
}

# Check if Node.js is installed
Write-Host "Checking Node.js installation..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    Write-Host "✓ Node.js found: $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "✗ Node.js not found. Please install Node.js 16+" -ForegroundColor Red
    exit 1
}

# Restore .NET packages
Write-Host "Restoring .NET packages..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Failed to restore .NET packages" -ForegroundColor Red
    exit 1
}
Write-Host "✓ .NET packages restored" -ForegroundColor Green

# Install frontend dependencies
Write-Host "Installing frontend dependencies..." -ForegroundColor Yellow
Set-Location frontend
npm install
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Failed to install frontend dependencies" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Frontend dependencies installed" -ForegroundColor Green
Set-Location ..

# Create environment file for frontend
Write-Host "Creating frontend environment file..." -ForegroundColor Yellow
$envContent = @"
REACT_APP_API_URL=https://localhost:7000/api
"@
$envContent | Out-File -FilePath "frontend\.env.local" -Encoding UTF8
Write-Host "✓ Environment file created" -ForegroundColor Green

Write-Host "`nSetup completed successfully!" -ForegroundColor Green
Write-Host "`nTo start the application:" -ForegroundColor Cyan
Write-Host "1. Start the API: dotnet run --project src/ParallelLab.API" -ForegroundColor White
Write-Host "2. Start the frontend: cd frontend && npm start" -ForegroundColor White
Write-Host "`nThe application will be available at:" -ForegroundColor Cyan
Write-Host "- API: https://localhost:7000" -ForegroundColor White
Write-Host "- Frontend: http://localhost:3000" -ForegroundColor White

