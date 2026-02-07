# 🛒 B2C E-Commerce Project

![Framework](https://img.shields.io/badge/.NET%20Core-MVC-purple)
![Architecture](https://img.shields.io/badge/Architecture-N--Layer-blue)
![Payment](https://img.shields.io/badge/Integration-Iyzico-orange)

## 📖 Overview
This project is an E-Commerce application developed to practice **N-Layer Architecture** and **Payment System Integration**. The main goal was to build a structured backend using **.NET Core** and implement a real-world payment scenario.

## 🏗️ Architecture & Design
The project follows the **N-Layer Architecture** to separate concerns:
* **DataAccess Layer:** Handles database operations using **Repository Pattern**.
* **Entity Layer:** Defines the database models.
* **Business Layer:** Contains the logic and rules.
* **UI Layer:** The web interface for users.

## 🚀 Features

* **Layered Structure:** Clean separation between database, logic, and UI.
* **Payment Integration:** Integrated with **Iyzico API** for basic transaction testing.
* **Database Configuration:** Used **Fluent API** to manage table relationships and constraints instead of data annotations.
* **Product Management:** Basic listing and management of products.

## 🛠️ Tech Stack

| Component | Technology |
| :--- | :--- |
| **Framework** | .NET Core MVC |
| **Architecture** | N-Layer |
| **Data Access** | Entity Framework Core (Code First) |
| **Database** | MSSQL |
| **Payment** | Iyzico API |
| **Frontend** | Bootstrap, HTML, CSS |

---
*Developed by [Bugra Ozturk](https://github.com/ozturkbugra)*
