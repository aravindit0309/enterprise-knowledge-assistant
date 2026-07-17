Running PostgreSQL locally (no Docker)

1) Install PostgreSQL on Windows (choose one):
   - Download the installer from https://www.postgresql.org/download/windows/ (EnterpriseDB) and run it.
   - Or install via Chocolatey (PowerShell as Admin):
	   choco install postgresql

2) Initialize and start the server (installer usually handles this). Note the postgres superuser password.

3) Create database and user (example using psql):
   - Open 'SQL Shell (psql)' or use psql from path:
	   psql -U postgres
   - In psql:
	   CREATE DATABASE eka_db;
	   CREATE USER eka_user WITH PASSWORD 'eka_pass';
	   GRANT ALL PRIVILEGES ON DATABASE eka_db TO eka_user;

4) Connection strings
   - When running the API on the host (Visual Studio):
	   Host=localhost;Port=5432;Database=eka_db;Username=eka_user;Password=eka_pass;
   - When running the API inside Docker on Windows/macOS use host.docker.internal:
	   Host=host.docker.internal;Port=5432;Database=eka_db;Username=eka_user;Password=eka_pass;

5) Configure the app
   - Update appsettings.Development.json (or secrets) ConnectionStrings:DefaultConnection to match the chosen connection string.
   - Or set the environment variable ConnectionStrings__DefaultConnection (docker-compose already sets an example).

6) Firewalls
   - Ensure PostgreSQL listens on 0.0.0.0 or the appropriate interface if you need remote access. By default local installer listens on localhost only.

7) Verify
   - From host: psql -h localhost -U eka_user -d eka_db
   - From container: psql -h host.docker.internal -U eka_user -d eka_db

If you want, I can add a migration script or update appsettings.Development.json with this connection string.
