Phonebook Application

A full-stack contact management application built with Angular and .NET Core.

Project Structure

Frontend (Angular)
plaintext
src/app/
├── components/
│   ├── confirmation-dialog/
│   ├── contact-list/
│   │   └── contact-form/
│   └── home/
├── models/
│   ├── confirm.ts
│   └── contact.ts
├── services/
└── shared/

Backend (.NET Core)
PhonebookApi/
├── Controllers/
│   └── ContactsController.cs
├── Models/
│   └── Contact.cs
├── Data/
│   └── phonebook.db
├── Repositories/
│   ├── Interfaces/
│   │   └── IContactRepository.cs
│   └── ContactRepository.cs
└── Program.cs

Setup
Frontend
1.	Install dependencies
bash

**npm install**
2.	Run application
bash

**ng serve**
Backend
1.	Restore packages
bash

**dotnet restore**
2.	Run API
bash

**dotnet run**
Technologies
•	Frontend: **Angular with Material UI**
•	Backend: **.NET Core Web API**
•	Database: **SQLite**
Development **Server**
•	Frontend: http://localhost:4200
•	Backend: http://localhost:7160

