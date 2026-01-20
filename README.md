# Students Fullstack

A full-stack CRUD template for students with related photos.

This project demonstrates a typical ASP.NET Core Web API + React architecture. 

Features:
- Create, update, view, and delete entities
- File uploads
- Keeping uploaded files on the server filesystem
- Server-side validation
- Pagination
- Sorting
- Search with debounced API requests (after user stops typing)
- Responsive UI (mobile-friendly layout)
- Running backend and database using Docker containers

## Screenshots

![Screenshot 2026-01-20 105605](https://github.com/user-attachments/assets/d12b948c-4cce-45f1-9673-b762c13c96d3)

![Screenshot 2026-01-20 110908](https://github.com/user-attachments/assets/a615ee31-757e-40a8-bf78-dcd8fb85f5a7)

![Screenshot 2026-01-20 110945](https://github.com/user-attachments/assets/071fe5c1-1725-44c6-ac6f-6f49bc8db2e7)

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
