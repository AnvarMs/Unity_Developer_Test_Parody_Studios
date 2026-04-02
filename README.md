# Gravity Manipulation Puzzle Game

A 3D puzzle platformer where players manipulate gravity to navigate levels and collect cubes within a time limit.

## 🎮 Download & Play

### Standalone Builds

**Game Build:**
[Download for Windows/Mac](https://anvarms.itch.io/gravitypuzzle) - [itch.io Link]

## ⚠️ Platform Testing Status

| Platform | Build Status | Testing Status |
|----------|-------------|----------------|
| **Windows** | ✅ Built | ✅ Fully Tested |
| **Mac** | ✅ Built | ⚙️ Not Tested* |

**\*Mac Build Disclaimer:**  
The macOS build was generated using Unity's standard build pipeline with Universal 
architecture (Apple Silicon + Intel). However, I do not have access to Mac hardware 
for testing. The build follows Unity's recommended settings and should function 
correctly, but I cannot guarantee full compatibility without testing.

If you encounter any Mac-specific issues, please let me know and I'll be happy to 
address them. The Windows build has been thoroughly tested and verified working.

## 🎯 Objective

Collect all cubes scattered throughout the level within the 3-minute time limit. Use gravity manipulation to reach platforms that would otherwise be inaccessible.

## 🕹️ Controls

### Movement
- **W/A/S/D** - Move character`
- **Mouse** - Look around / Camera control

### Gravity Manipulation
- **Hold Right Mouse Button** - Enter gravity selection mode (hologram preview appears)
- **Move Mouse** - Rotate hologram to select gravity direction
- **Release Right Mouse Button** - Apply selected gravity direction


## 🎲 Gameplay Mechanics

### Gravity System
Players can change the direction of gravity to walk on walls and ceilings. Six gravity directions are available:
- Default (Down)
- Ceiling (Up)
- Left Wall
- Right Wall
- Front Wall
- Back Wall

### Win Condition
Collect all cubes before the 3-minute timer expires.

### Lose Conditions
- Timer runs out before all cubes are collected
- Player falls into the void (no surface contact for extended period)

## 🛠️ Technical Details

**Engine:** 6000.61f1 (or your version)
**Platform:** Windows, macOS
**Input System:** New Unity Input System
**Physics:** Custom Rigidbody-based character controller with dynamic gravity

### Features Implemented
- ✅ Third-person camera with mouse orbit
- ✅ Gravity manipulation with hologram preview
- ✅ Rigidbody-based physics character controller
- ✅ Timer system (2 minutes)
- ✅ Collectible system
- ✅ Game over conditions
- ✅ Character animations (idle, running, jumping)
- ✅ Standalone builds (Windows & Mac)

## 📁 Project Structure
```
Assets/
├── Scripts/
│   ├── PlayerController.cs
│   ├── CameraController.cs
│   ├── GravitySelector.cs
│   ├── GameManager.cs
│   └── ControlsDisplay.cs
├── Prefabs/
│   ├── Player
│   ├── Collectible
│   └── Hologram
├── Scenes/
│   └── MainLevel
└── Materials/
    └── HologramMaterial
```

## 🚀 Building from Source

### Prerequisites
- Unity 6000.61f1 or newer
- Git

### Setup Instructions

1. **Clone the repository:**
```bash
git clone https://github.com/AnvarMs/Unity_Developer_Test_Parody_Studios
```

2. **Open in Unity:**
   - Open Unity Hub
   - Click "Add" and select the cloned folder
   - Open the project

3. **Play in Editor:**
   - Open `Scenes/MainLevel`
   - Press Play button

4. **Build Standalone:**
   - File → Build Settings
   - Select platform (Windows/Mac)
   - Click "Build"

## 📝 Development Notes

**Development Time:** 3 days
**Key Challenges:**
- Implementing gravity-aware camera system
- Rigidbody orientation with dynamic gravity
- Mouse-based gravity direction selection
- Smooth transitions between gravity states

**Code Highlights:**
- Custom gravity system using `Quaternion.AngleAxis` for rotation
- Camera orbit using gravity-relative axes
- Hologram preview system for gravity selection
- Dot product-based direction finding algorithm

## 🎨 Assets Attribution

- **Character Model:** [Provided in base project]
- **Animations:** [Provided in base project]
- **Materials:** [Provided in base project]

## 📧 Contact

**Developer:** Anvar MS
- **Portfolio:** [anvarms.github.io/Portfolio](https://anvarms.github.io/Portfolio)
- **LinkedIn:** [linkedin.com/in/-anvar-ms](https://linkedin.com/in/-anvar-ms)
- **GitHub:** [github.com/AnvarMs](https://github.com/AnvarMs)
- **Itch.io:** [anvarms.itch.io](https://anvarms.itch.io)

## 📜 License

This project was created as a technical assessment for Unity Game Developer position.

---

**Assignment Details:**
- **Company:** Parody Studios
- **Position:** Associate Game Developer job
- **Submission Date:** 02-04-2026
- **Development Duration:** 3 days
```

