# 🎨 Interactive Museum Experience

![Unity](https://img.shields.io/badge/Engine-Unity-black)
![C#](https://img.shields.io/badge/Code-C%23-blue)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)

## 📎 Resources
- [MIDI Piano Library](https://github.com/Trophaigle/MIDI-Piano-Player)
- [Project Demo - Unity Play](https://play.unity.com/fr/games/00553f75-f7e4-4db1-bd7c-68bc2d99f070/echoes-of-the-museum)
- -Youtube link

## 📷 Preview
[![Watch the demo](https://img.youtube.com/vi/0yBprdF_1xc/0.jpg)](https://youtu.be/0yBprdF_1xc)

## 📌 Overview
This project is an **interactive virtual museum experience** set in a vintage, old-style interior environment inspired by classical artistic spaces.  
The player can freely explore the room, discover artworks, and interact with objects to trigger **visual and audio feedback**.

The goal of this project is to create a **fully immersive audiovisual experience**, blending environment storytelling, music, and interactivity.

---

## 🎮 Features

### 🖼️ Art Exploration
- Original paintings created by the author
- Interactive system:
  - Hover/select a painting → displays its **title**
  - Some artworks trigger **audio descriptions or soundscapes**

### 🎵 Interactive Audio Objects
- Multiple interactable objects:
  - Piano
  - Gramophone
  - Radio / Some drawing frames
- Each object can play music through different mediums
- Creates a **layered and contextual audio experience**

---

## 🎬 Cinematic Introduction
At application startup:
- A **cinematic sequence** introduces the environment
- Guides the player through key areas and objects
- Includes:
  - Camera transitions
  - Music
  - Titles

### Technologies used:
- Cinemachine
- Timeline system
- Sequence blending
- Animation tracks
- Activation tracks
- Shader Graphs (Animated Materials)

---

## 🧠 Technical Architecture

### 💻 Code Design (C#)
- Object-oriented architecture:
  - Use of **inheritance** and **interfaces**
- Optimized logic:
  - Reduced reliance on `Update()` calls
- Centralized control via a **GameManager**:
  - Handles global logic and experience flow

### 🖥️ UI System
- Built using:
  - Canvas
  - Panels
- Fully integrated with gameplay interactions

---

## 🎹 Piano System (MIDI Integration)
- Piano animation powered by:
  - MIDI-based playback system
- Features:
  - Real-time key animation synchronized with music
  - Uses external MIDI files

---

## 🔊 Audio Design
- Spatial audio system implemented:
  - Distance-based attenuation
- Enhances immersion by placing sound sources in 3D space

### 🚧 Work in Progress
- Advanced audio simulation:
  - Surface-based sound occlusion (walls, floors)
  - Planned integration with tools like Wwise

---

## 🎨 Rendering & Optimization
- Custom shader work:
  - Focus on **reducing memory usage**
  - Maintaining visual quality

---

## 🕶️ Future Improvements
- VR support (in progress)
- Mobile version (planned)
- Advanced audio simulation (Wwise integration)
- Expanded environment and interactions  (additionnal animations, doors ...)

---

## 🚀 How to Run
1. Clone the repository  
2. Open the project in Unity  
3. Run the main scene

   Or

Go on Unity Play Link at the beginning of the page. (compressed version, less quality visuals)

---

## 👤 Author
Personal project focused on:
- Interactive environments  
- Audio-visual immersion  
- Technical optimization  

---

## 📎 Additional Resources
- MIDI Piano Library: *(add your GitHub link here)*

---

## 💡 Notes
This project highlights both:
- **Creative direction** (art, sound, immersion)  
- **Technical skills** (code, optimization, systems design) 
