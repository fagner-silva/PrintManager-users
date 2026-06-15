# PrintManager.Users.Api

API de autenticação, usuários, empresas e membros do sistema 3D Print Manager.

## Stack

- .NET 8
- MongoDB Atlas
- JWT
- BCrypt
- Docker
- Render
- GitHub Actions

## Endpoints principais

### Users

- POST /api/users/register
- POST /api/users/login
- GET /api/users/me
- PATCH /api/users/change-password

### Companies

- POST /api/companies
- GET /api/companies/my

### Members

- POST /api/companies/{companyId}/members
- GET /api/companies/{companyId}/members
- PATCH /api/companies/{companyId}/members/{userId}/role
- PATCH /api/companies/{companyId}/members/{userId}/block
- PATCH /api/companies/{companyId}/members/{userId}/unblock
- DELETE /api/companies/{companyId}/members/{userId}