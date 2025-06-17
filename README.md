# GeoPoint
tanıtım videosu youtube linki: https://youtu.be/s8dJgY7qXPc?si=E144C305BQZqjz5r

## Development setup

1. Install dependencies for the React client (requires internet access):
   ```bash
   cd WebApplication6/ClientApp
   npm install
   ```
2. Start the React development server alongside the ASP.NET backend:
   ```bash
   npm start
   ```
   The API can be launched from `WebApplication6` using `dotnet run`.
3. After building the React app, static files will be served from `ClientApp/build` when the backend runs in production:
   ```bash
   npm run build
   ```
