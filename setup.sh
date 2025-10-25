#!/bin/bash

# ParallelLab Setup Script
# This script sets up the development environment for ParallelLab

echo "Setting up ParallelLab Development Environment..."

# Check if .NET 8 is installed
echo "Checking .NET 8 installation..."
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    if [[ $DOTNET_VERSION == 8.* ]]; then
        echo "✓ .NET 8 found: $DOTNET_VERSION"
    else
        echo "✗ .NET 8 not found. Please install .NET 8 SDK"
        exit 1
    fi
else
    echo "✗ .NET not found. Please install .NET 8 SDK"
    exit 1
fi

# Check if Node.js is installed
echo "Checking Node.js installation..."
if command -v node &> /dev/null; then
    NODE_VERSION=$(node --version)
    echo "✓ Node.js found: $NODE_VERSION"
else
    echo "✗ Node.js not found. Please install Node.js 16+"
    exit 1
fi

# Restore .NET packages
echo "Restoring .NET packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "✗ Failed to restore .NET packages"
    exit 1
fi
echo "✓ .NET packages restored"

# Install frontend dependencies
echo "Installing frontend dependencies..."
cd frontend
npm install
if [ $? -ne 0 ]; then
    echo "✗ Failed to install frontend dependencies"
    exit 1
fi
echo "✓ Frontend dependencies installed"
cd ..

# Create environment file for frontend
echo "Creating frontend environment file..."
cat > frontend/.env.local << EOF
REACT_APP_API_URL=https://localhost:7000/api
EOF
echo "✓ Environment file created"

echo ""
echo "Setup completed successfully!"
echo ""
echo "To start the application:"
echo "1. Start the API: dotnet run --project src/ParallelLab.API"
echo "2. Start the frontend: cd frontend && npm start"
echo ""
echo "The application will be available at:"
echo "- API: https://localhost:7000"
echo "- Frontend: http://localhost:3000"


