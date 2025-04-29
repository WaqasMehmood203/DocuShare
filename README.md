The **DocuShare** is a secure and user-friendly platform designed for managing, sharing, and collaborating on documents. Users can register, upload files, share them securely, and manage profiles — all within a role-based access control system.

---

## 🚀 Features

- ✅ **User Registration & Authentication**
  - Secure login and signup using **JWT (JSON Web Tokens)**.
- 🔐 **Role-Based Access Control**
  - Assign roles (e.g., *User*, *Admin*) for feature access.
- 📁 **Document Management**
  - Upload, download, and share files with selected users or publicly.
- 👤 **Profile Management**
  - Update user information such as name, address, and profile image.
- 🗄️ **Secure Storage**
  - Only authorized users can access documents.

---

## 🛠️ Tech Stack

| Layer     | Technology              |
|-----------|--------------------------|
| Backend   | ASP.NET Core MVC (.NET 8) |
| ORM       | Entity Framework Core     |
| Frontend  | Razor Pages (MVC)         |
| Database  | SQL Server                |
| Auth      | JWT (JSON Web Token)      |

---

## ⚙️ Installation & Setup

### 📌 Prerequisites

- [.NET 8 SDK (LTS)](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

---

### 🧪 Steps to Run the Project

1. **Clone the repository**
   ```bash
   git clone https://github.com/WaqasMehmood203/Document-Management-System-Backend
   cd Document-Management-System-Backend
   
2. **Restore NuGet packages**
dotnet restore
3. **Update the database connection string**
   Open appsettings.Development.json and modify:
   "ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;"
}
Replace YOUR_SERVER and YOUR_DATABASE accordingly.
4. **Apply migrations to set up the database**
Add-Migration InitialSetup
Update-Database
5. **Run the project**
6. **It will open the browser using a localhost URL.**
7. **💡 Final Note**
Enjoy and use DocuShare – Document Management System. Feel free to contribute, raise issues, or fork the project!
