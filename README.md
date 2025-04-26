# 📦 AdventureWorks2022-Software

A **Windows Forms (WinForms)** application developed in **C#**, connected to **SQL Server** using the **AdventureWorks2022** database.

---

## ✨ Overview

This software provides a simple and efficient UI for managing data within the AdventureWorks2022 sample database.

**Main Features:**
- **Insert** and **update** customers, products, and orders
- **Create**, **modify**, and **approve** sales orders
- **Monitor** inventory and **reorder** stock based on levels
- Organized project structure for scalability and easy maintenance

---

## 🛠️ Built With

- **C#** (.NET Framework)
- **Windows Forms (WinForms)**
- **SQL Server** (AdventureWorks2022 Database)
- **ADO.NET** for database communication

---

## 🚀 Getting Started

1. **Clone the Repository:**  
   `git clone https://github.com/SocratesAlx/AdventureWorkss2022-Software.git`

2. **Open the Solution:**  
   Open `AdventureWorks2022-Software.sln` in Visual Studio.

3. **Configure Database Connection:**  
   Make sure the AdventureWorks2022 database is installed on your SQL Server instance.  
   Update your connection settings inside `/Helpers/ConnectToDB.cs` if necessary.

4. **Build and Run:**  
   - Build the solution (`Ctrl+Shift+B`)
   - Start the application (`F5`)

---

## 📂 Project Structure

- `/Forms/` – All form-related files (UI)
- `/Helpers/` – Database and UI utility classes
- `/Properties/` – Application properties and settings
- `/Resources/` – Images and resources
- `App.config` – Application configuration
- `AdventureWorks2022-Software.sln` – Visual Studio solution file
- `README.md`
- `.gitignore`
- `.gitattributes`

---

## ⚡ Requirements

- Visual Studio 2022 or newer
- .NET Framework
- SQL Server with the AdventureWorks2022 database

---

## ⚠️ Notes

- Database operations directly modify data — backup recommended
- Best suited for educational and experimental use

---

## 📬 Contact

Developed by [SocratesAlx](https://github.com/SocratesAlx)  
Feel free to ⭐ star the repository if you find it helpful!

