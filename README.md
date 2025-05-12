# Video Games as a Novel Habit Research Paradigm

This repository contains the complete code, data, and analysis pipeline for the research project and MSc thesis: **"Video Games as a Novel Habit Research Paradigm"** by Paul Evers. The project investigates habit formation using a custom-built video game, web-based data collection, and statistical analysis.

---

## Table of Contents

- [Overview](#overview)
- [Repository Structure](#repository-structure)
- [Game](#game)
- [Web Interface](#web-interface)
- [Data Processing & Analysis](#data-processing--analysis)
- [How to Use](#how-to-use)
- [License](#license)

---

## Overview

This project explores how video games can be used as experimental platforms for studying habit formation. It includes:

- A Unity-based game for controlled behavioral experiments
- Web-based frontend and backend for participant management and data collection
- Scripts for data processing and statistical analysis (R and Stata)
- Example data and the full MSc thesis

---

## Repository Structure

```
.
├── Source code game/         # Unity project for the experimental game
├── Web/                     # Web frontend (React) and backend (PHP)
├── Data processing/         # Data cleaning/processing scripts (Node.js)
├── Data analysis/           # Analysis scripts (R, Stata)
└── MSc Thesis ...pdf        # Full thesis document
```

### Key Directories

- **Source code game/**: Unity project files for the HabitGameParadigm.
- **Web/paradigm-frontend/**: React + Vite frontend for participant and researcher interaction.
- **Web/Backend/**: PHP backend API for data collection and survey management.
- **Data analysis/R/**: R scripts for statistical modeling (e.g., mixed-effects models).
- **Data analysis/Stata/**: Stata scripts for descriptive and inferential statistics.
- **Data processing/Nodejs/**: Node.js scripts for data cleaning and transformation.

---

## Game

The game is developed using Unity 6000.0.26f1. We will describe some of the core mechanisms of the game based on each file:

- **GameManager.cs**: Implements a game state management system for multiple phases and days. It handles different game states (Introduction, Practice, Training, Test, and Debrief), manages rounds, scoring, and coin collection mechanics. The class coordinates between various components like maze generation, player movement, coin control, and data collection. It implements a singleton pattern and includes functionality for generating and managing level seeds, tracking player progress, and handling transitions between different game phases.
- **CointController.cs**: Manages coin spawning in a game using A\* Pathfinding. It calculates spawn positions based on path distance and percentage, placing coins at predetermined points along the path between start and finish locations. Uses Seeker and AIAgent as inputs for the path and travelled distance of the player.
- **DataController.cs**: Manages data collection and persistence for a game. It tracks time and distance measurements during gameplay every 0.02 seconds. The controller handles participant data through a database handler, including loading and adding participants, and manages round data. At the end of each round, the round log data is passed to the DatabaseHandler and uploaded.
- **DatabaseHandler.cs**: Manages communication between the game and the back-end server. The class includes methods to fetch participant information by email, add new participants, and record round data. The server endpoint is configurable between production and local development environments.

---

## Web Interface

- **Frontend**: Built with React and Vite. Provides user interfaces for game instructions
  and links to the actual experiment pages per day. See the FTP folder for the files on the
  FTP server.
- **Backend**: PHP API with endpoints for:
  - Adding participants, rounds, and round log data
  - Submitting survey data
  - Updating scores

The back-end uses the env.php file to load in environment variables that contains secret credentials to connect with the database, see
.example.env, edit these accordingly.

---

## Data Processing & Analysis

- **R scripts** (`Data analysis/R/`):
  - Mixed-effects modeling (e.g., reaction times by phase and day)
  - Data cleaning and visualization
- **Stata scripts** (`Data analysis/Stata/`):
  - Descriptive statistics, regression, and graphing
  - Correlation analyses with survey data
- **Node.js scripts** (`Data processing/Nodejs/`):
  - Data cleaning, merging, and transformation utilities

---

## How to Use

### 1. Clone the Repository

```bash
git clone <repo-url>
cd <repo-directory>
```

### 2. Game (Unity)

- Open `Source code game/` in Unity Hub (Unity 2020.3 or later recommended).
- Build and run the game for your platform.

### 3. Web Interface

#### Frontend

```bash
cd Web/paradigm-frontend
npm install
npm run dev
```

#### Backend

- Configure database credentials in `Web/Backend/.env` (or set environment variables).
- Deploy `Web/Backend/` on a PHP server with MySQL access.

---

## License

This repository is for academic and research purposes. For reuse or collaboration, please contact the author.

---

For more details, see the full thesis: `MSc Thesis Paul Evers - Video Games as a Novel Habit Research Paradigm.pdf`
