# 🧠 Memory Game (C# & WPF)

## 📋 Description

This application is a WPF implementation of the classic [Memory Game](https://www.webgamesonline.com/memory/) using **C#**, following the **MVVM** architectural pattern and leveraging **data binding** to synchronize the UI with application logic.

---

## 🔑 Features

### ✅ 1. User Authentication
- Create a new user with a unique name and associate a local image (PNG).
- Save user data (including image path) to a local file (XML).
- Select an existing user to enter the game.
- "Play" and "Delete User" buttons remain inactive until a user is selected.

### 🧩 2. Gameplay
- Choose an image category before starting.
- Game modes:
  - **Standard** (4x4 board).
  - **Custom** (MxN, where M and N are between 2 and 6, total tiles must be even).
- User-configurable time limit.
- Countdown timer displayed during gameplay.
- Tiles flip when clicked; matched pairs become inactive.
- Non-matching tiles flip back with a 1 second timer.
- Randomized tile distribution for each new game.
- Same tile cannot be selected twice in a row.

### 💾 3. Save/Load Game
- Save current game state.
- Load previously saved game and continue from where it was left off.
- A user can only open games they have saved.

### 🧹 4. Delete User
- Completely remove selected user from the system.
- Delete user image reference, saved games, and statistics.

### 📊 5. Player Statistics
- Track total games played and games won.
- Display all users' statistics in a separate window:

### ℹ️ 6. Help – About
- A window showing student name, university email address, group, and specialization.

---

## 🧠 Architecture

This project follows the **MVVM (Model-View-ViewModel)** pattern:
- **Model**: Classes like `Card`, `GameStatistics`, `SavedGame`, `User`.
- **ViewModel**: Classes like `CreateUserViewModel`, `GameViewModel`, `MainViewModel`, `SignInViewModel`.
- **View**: `.xaml` files bound to ViewModels using data binding and commands (`ICommand`).

---

## 🗂 Data Storage

- User data and statistics are saved using local XML files.
- Image paths are stored as **relative** paths to ensure portability.
- Saved games are either overwritten or stored individually per user.

---

## ▶️ How to Run

1. Open the solution in **Visual Studio**.
2. Select the WPF App project (.NET Core/.NET Framework).
3. Build and run the application (`F5`).
4. Create a new user and start playing!

---

## 📌 Requirements

- .NET Framework or .NET Core (depending on your implementation).
- Visual Studio 2022 or later.
- Windows 10 or higher for full WPF support.

---

## 🧪 Testing

✔️ All main features have been tested manually:  
- User login ✔  
- Standard and custom gameplay ✔  
- Save/load game ✔  
- Timer and game-over logic ✔  
- Statistics ✔  
- Delete user ✔  
