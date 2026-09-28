.NET API
cd dot_net_api
dotnet run                      # http://localhost:5128
dotnet run --launch-profile https   # https://localhost:7001 + http://localhost:5128
dotnet watch                    # hot reload, like nodemon

Angular client
cd client
npm start                       # runs `ng serve`, http://localhost:4200

If you're coming from React: dotnet watch is the .NET version of nodemon or vite dev, and ng serve does the same job as vite/npm run dev.