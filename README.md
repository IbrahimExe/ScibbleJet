<div align="center">

![AppImg](https://github.com/user-attachments/assets/163e28d5-da6b-45c1-8884-2653a1777f4d)

# Scribble Jet - A Unity 2D Infinite Runner
</div>

> ⚠️ *This is my first attempt at a mobile game using the Unity Engine - as well as establishing the connection with Unity's Gaming Services.
> I hope to regularly continue to update and polish the game as it's the first game up on my Itch page!*

## 🎮 Overview
Scribble Jet is a 2D infinite runner game made in Unity, inspired by the gameplay of *Jetpack Joyride*.  
Players collect coins and dodge randomly spawned obstacles while flying using limited jetpack fuel.

<p align="center">
  <img src="https://github.com/user-attachments/assets/2a5f2e21-c99b-42ba-9810-1c6e4212b4c0" alt="ScribbleJetGameplay"> 
</p>

---

## Features

- ✅ Infinite side-scrolling world
- ✅ Custom made, original UI elements made in Adobe Illustrator
- ✅ A fuel system with recharge mechanic
- ✅ Coin and obstacle spawners
- ✅ Game over screen with "Play Again" support
- ✅ Coin score & distance tracker UI
- ✅ Unity Ads integration with **Rewarded Second Life** showcasing an advertisement
- ✅ Unity Leaderboards using Unity Gaming Services

---

## How It Was Made

This game was created in Unity from scratch and includes:

- Custom physics for player movement
- Modular code for spawners, collisions, and UI
- Manual references (no use of `FindObjectOfType` or `FindGameObjectWithTag`)
- Scene reloading for retry functionality
- Unity UI system (Canvas, TMP)
- Original paper-doodle aesthetic like vectors & menu screens made in Adobe Illustrator

---

## Leaderboards Integration

Integrated using **Unity Leaderboards** from Unity Gaming Services:

1. Linked project in the **Unity Dashboard**.
2. Enabled **Leaderboards** under Unity Services.
3. Used `Unity.Services.Leaderboards` package.
4. Sent player distance as the score on game over.

<p align="center">
  <img src="https://github.com/user-attachments/assets/27f696a3-60d1-4efc-b600-713baee4eb14" alt="ScribbleJetLeaderboard"> 
</p>
---

## Rewarded Ads – Second Life System

Implemented via **Unity Ads** & **UGS**:

- Rewarded ads let the player **watch to continue once** after crashing.
- Used Unity Mediation or Legacy Ads system.
- After second life is used, next crash triggers Game Over screen.

---

## Wanna Try Out ScribbleJet?

Experience **ScribbleJet** right in your browser!  
Play it now on **[itch.io](https://ibrahimexe.itch.io/scribblejet)**.

---

## Or Alternativley, Run it Locally:

1. Clone or download this repository.
2. Open the project in **Unity Hub**.
3. Open the `MainScene.unity` scene.
4. Press the ▶️ Play button to test.
5. Build via **File → Build Settings**.

---
  
Inspired by one of my favorite mobile games to date; *Jetpack Joyride*, I hope you enjoy!
