

# **Conway's Game of Life API**

## **📌 Problem Description**
Conway's Game of Life is a **zero-player game** that simulates cellular automation. It consists of a **grid of cells**, each either **alive (1) or dead (0)**. The game progresses through **generations**, with each cell following simple rules based on its **neighboring cells**.

### **Rules:**
1️⃣ Any live cell with **fewer than two** live neighbors **dies** (underpopulation).  
2️⃣ Any live cell with **two or three** live neighbors **survives** to the next generation.  
3️⃣ Any live cell with **more than three** live neighbors **dies** (overpopulation).  
4️⃣ Any dead cell with **exactly three** live neighbors **comes to life** (reproduction).  

The **goal** is to track **board evolution**, detecting stable patterns, oscillators, and chaotic behavior.

---

## **🚀 Steps to Run Locally**
### **1️⃣ Prerequisites**
Make sure you have:
- **.NET 9 SDK** installed
- **SQL Server** (or update `appsettings.json` for SQLite)
- **Docker** (optional for containerization)

### **2️⃣ Setup**
1. Clone the repository:
   ```sh
   git clone https://github.com/adansklevanskis/GameOfLife.git
   ```
2. Navigate to the project folder:
   ```sh
   cd GameOfLifeAPI
   ```
3. Install dependencies:
   ```sh
   dotnet restore
   ```
4. Set up the database:
   ```sh
   dotnet ef migrations add InitialCreate --project src
   dotnet ef database update --project src
   ```

### **3️⃣ Run the API**
```sh
dotnet run
```
✔ API will be available at: `http://localhost:5000/api/gameoflife`

---

## **🛠 Explanation of the Solution & Thought Process**
### **📌 Key Considerations**
✅ **Modular Design:** Separates concerns into **Controllers, Services, Models, and Data Layers**.  
✅ **Persistence:** Uses **EF Core** with **SQL Server**, ensuring boards persist across API calls.  
✅ **Performance Optimization:** Implements **O(N × M) complexity** with neighbor counting.  
✅ **Error Handling:** Uses a **Notification Pattern** for structured error reporting, avoiding crashes.  
✅ **Environment Configuration:** Uses **Options Pattern** to set **MaxIterations** dynamically.  
✅ **Logging & Debugging:** Integrates **ILogger** for structured logging.  
✅ **Validation:** Ensures input correctness using **FluentValidation**.  
✅ **Unit Testing:** Covers core logic, error handling, and configurations with **xUnit + Moq**.  
✅ **API Documentation:** Designed for easy endpoint interaction.  
✅ **Scalability:** Supports containerization via **Docker** for cloud deployment.  

---

## **📑 Assumptions & Trade-offs**
### **📌 Assumptions**
✔ The grid is **finite** – no infinite expansion handling.  
✔ Boards persist only while the database exists – **no file-based storage**.  
✔ API is **single-threaded** – concurrency optimizations (e.g., parallel processing) are not included.  
✔ The system assumes **valid JSON payloads** in requests – minimal input sanitization.  

### **📌 Trade-offs**
🔹 **Naive neighbor counting:** While `O(N × M)` is efficient, precomputed neighbor matrices could reduce redundant calculations.  
🔹 **Database-dependent state storage:** Boards **persist** using SQL Server, but an **in-memory cache** (e.g., Redis) could improve speed.  
🔹 **No graphical UI:** Focused on API functionality instead of a visual simulator.  
🔹 **Limited load testing:** Stress testing can be introduced if needed for large-scale environments.  

---

### **📦 Containerization (Optional)**
1. Build the Docker image:
   ```sh
   docker build -t gameoflife-api .
   ```
2. Run the container:
   ```sh
   docker run -p 5000:5000 gameoflife-api
   ```
✔ API will be accessible at `http://localhost:5000/api/gameoflife` from within the container.

---
