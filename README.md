# Students Fullstack

A full-stack CRUD template for students with related photos.

This project demonstrates a typical ASP.NET Core Web API + React architecture. 

Features:
- File uploads
- Keeping uploaded files on the server filesystem
- Pagination
- Sorting
- Create, update, view, and delete entities
- Running backend and database using Docker containers

## Screenshots

## Technology stack

**Backend**

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- RESTful API design
- File upload & storage (local filesystem)

 **Frontend**

- React (Vite)
- JavaScript
- Bootstrap 5

**Infrastructure**

- Docker
- docker-compose
- Nginx (reverse proxy)
- Linux server deployment 

## Project structure
```text
students-fullstack/
│
├── backend/
│   └── StudentsApi/
│       ├── Controllers/
│       │   ├── StudentsController.cs
│       │   ├── StudentPhotosController.cs
│       │   └── DebugFileController.cs
│       │
│       ├── Data/
│       │   └── ApplicationDbContext.cs
│       │
│       ├── Domain/
│       │   └── Entities/
│       │       ├── Student.cs
│       │       ├── PhotoAvatar.cs
│       │       └── PhotoEnclosure.cs
│       │
│       ├── Dtos/
│       │   ├── StudentCreateDto.cs
│       │   ├── StudentUpdateDto.cs
│       │   ├── StudentReadDto.cs
│       │   ├── PagedResultDto.cs
│       │   └── Photos/
│       │       ├── UploadAvatarRequest.cs
│       │       └── UploadEnclosuresRequest.cs
│       │
│       ├── Services/
│       │   ├── Students/
│       │   │   ├── IStudentService.cs
│       │   │   └── StudentService.cs
│       │   │
│       │   ├── Photos/
│       │   │   ├── IStudentPhotoService.cs
│       │   │   └── StudentPhotoService.cs
│       │   │
│       │   └── Storage/
│       │       ├── IFileStorage.cs
│       │       ├── LocalFileStorage.cs
│       │       └── FileStorageOptions.cs
│       │
│       ├── Migrations/
│       ├── Program.cs
│       └── appsettings.json
│
├── frontend/
│   └── students-app/
│       ├── src/
│       │   ├── api/
│       │   │   └── studentsApi.js
│       │   │
│       │   ├── components/
│       │   │   └── students/
│       │   │       ├── StudentTable.jsx
│       │   │       ├── StudentForm.jsx
│       │   │       ├── StudentSearchBar.jsx
│       │   │       └── PaginationBar.jsx
│       │   │
│       │   ├── pages/
│       │   │   └── students/
│       │   │       ├── StudentsAll.jsx
│       │   │       ├── StudentCreate.jsx
│       │   │       ├── StudentDetails.jsx
│       │   │       └── StudentUpdate.jsx
│       │   │
│       │   ├── App.jsx
│       │   └── main.jsx
│       │
│       ├── index.html
│       ├── package.json
│       └── vite.config.js
│
├── docker-compose.yml
├── .env
├── .env.example
└── README.md
```
## File storage

Uploaded files are stored outside containers on the host filesystem:

```text
/var/www/uploads/students-fullstack/
    └── students/
        └── {studentId}/
            └── enclosures/
```

The directory is mounted in the API container:

```text
volumes:
  - /var/www/uploads/students-fullstack:/uploads
```

## API features

- GET /api/students — paginated, sortable, searchable list
- POST /api/students — create student
- PUT /api/students/{id} — update student
- DELETE /api/students/{id} — delete student
- POST /api/students/{id}/avatar — upload avatar
- POST /api/students/{id}/enclosures — upload multiple files
- GET /api/students/{id}/enclosures — list enclosures
- GET /api/students/{id}/enclosures/{enclosureId} — download file

## Docker setup
The backend and database run in containers, frontend is served as static files.
```text
docker-compose up -d --build
```

Containers:
- students-api — ASP.NET Core Web API
- students-postgres — PostgreSQL database
