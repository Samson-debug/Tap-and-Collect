# Tap & Collect
## Getting Started
- **Unity Version:** Developed using Unity **6000.3.10f1**.
- **How to Open:** 
  1. Open **Unity Hub**.
  2. Click **Add** (or **Open**) and select the root folder of this project (`Tap & Collect`).
  3. Launch the project and open the main scene in `Assets/Scenes/` to hit Play!

## Game Flow
1. **Game & Goal:** Welcome to **Tap & Collect**! The main objective is to achieve the highest possible score before the time limit runs out, without losing your available lives.
2. **Tutorial:** 
   - Drag your finger to control the basket and catch falling objects.
   - The basket gets bigger when you collect power-ups.
   - Toggle the debug console to view game logs.
3. **Gameflow (Core Loop):** Items continuously spawn and fall from the top of the screen. The player controls the basket to collect them, racing against the clock and watching their life counter.
4. **Game Over:** The game ends when you run out of time or lives. Displays your final score and offers to try again.

## Architecture, Design Patterns & Trade-offs

- **Observer Pattern (Event-Driven Architecture):** Uses ScriptableObject-based events (`VoidEventChannelSO`) to broadcast state changes.
  - *Where applied:* Between `GameManager`, `UIManager`, and `ItemSpawner`.
  - *Trade-off:* Heavily decouples systems (which is great for modularity) but makes execution flow slightly harder to trace linearly compared to direct method calls.
- **Object Pooling Pattern:** Utilizes Unity's built-in `IObjectPool`.
  - *Where applied:* `ItemSpawner` for managing the falling items.
- **Singleton Pattern:** A single, globally accessible instance.
  - *Where applied:* `DragInputModule` to centralize the new Unity Input System.
  - *Trade-off:* Singletons can lead to global state issues and tight coupling in massive projects. We traded architectural purity for rapid development and straightforward input access.

## Future Improvements
Given more time, here are the immediate upgrades I'd make:
- Make animation for btns and object to make it more interactive
- Add some juice and visual effects to make the game more fun
- **Persistent Data:** Implement a save system (via `PlayerPrefs` or JSON) to track and display the player's All-Time High Score.
- **Dynamic Difficulty:** Gradually increase the item fall speed and spawn rate as the score/time progresses to maintain a challenging flow state.
- **Pity System:** Implement a "pity" counter for item spawning to ensure the random number generation doesn't feel unfair, guaranteeing a good item and power-up after a streak of hazards or regular items.

## AI Usage
- **My Contribution:** Designed the core game mechanics, game flow, and architectural guidelines. Provided the specific prompts and creative direction that drove development.
- **AI Contribution:** Assisted in writing the C# implementation, setting up Unity systems (Object Pooling, Input System, Event Channels), and structured the documentation.
