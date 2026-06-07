# Tap & Collect

## Game Flow
1. **Game & Goal:** Welcome to **Tap & Collect**! The main objective is to achieve the highest possible score before the time limit runs out, without losing your available lives.
2. **Tutorial:** 
   - Drag your finger to control the basket and catch falling objects.
   - The basket gets bigger when you collect power-ups.
   - Toggle the debug console to view game logs.
3. **Gameflow (Core Loop):** Items continuously spawn and fall from the top of the screen. The player controls the basket to collect them, racing against the clock and watching their life counter.
4. **Game Over:** The game ends when you run out of time or lives. Displays your final score and offers to try again.

## Architecture & Design Decisions

- **Event-Driven Architecture (ScriptableObject Event Channels):** We use ScriptableObject-based events (e.g., `VoidEventChannelSO`) to broadcast state changes (like `gameStartedEvent`, `gameOverEvent`, `scoreIncreaseEvent`). This heavily decouples systems like the `UIManager` and `ItemSpawner` from the `GameManager`, preventing tight coupling and making the codebase modular.
- **Object Pooling:** The `ItemSpawner` uses Unity's `IObjectPool` to manage falling items. This prevents the performance hits and Garbage Collection stutters that come with constantly calling `Instantiate()` and `Destroy()` during gameplay.
- **Centralized Input Management:** We use the new Unity Input System wrapped in a `DragInputModule` Singleton. Using the `<Pointer>` binding seamlessly handles Mouse, Touch, and Pen inputs across platforms while abstracting input logic away from the player's gameplay scripts.
- **Component-Based Behaviors:** Scripts like `FallingMovement` and `PlayerEffectController` are kept small and focused on a single responsibility. This makes the components highly reusable and easy to debug.

## AI Usage
- **My Contribution:** Designed the core game mechanics, game flow, and architectural guidelines. Provided the specific prompts and creative direction that drove development.
- **AI Contribution:** Assisted in writing the C# implementation, setting up Unity systems (Object Pooling, Input System, Event Channels), and structured the documentation.
