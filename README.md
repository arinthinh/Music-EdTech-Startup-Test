# Music EdTech Startup Test

A Unity-based music education application featuring an interactive rhythm tap game with real-time MIDI processing and note movement visualization.

## 🎵 Overview

This project demonstrates a music education game where players interact with musical notes that move across the screen in sync with MIDI playback. The application reads MIDI files, processes note events, and creates an engaging rhythm-based learning experience.

## 🛠️ Technology Stack

### Core Framework
- **Unity 6000.0.56f1** - Game engine and development platform
- **C#** - Primary programming language
- **Universal Render Pipeline (URP)** - Graphics rendering pipeline

### MIDI Processing
- **MidiPlayerTK** - Professional MIDI toolkit for Unity
  - Real-time MIDI file reading and playback
  - MIDI event processing and timing synchronization
  - Multi-format MIDI file support

### Animation & UI
- **DOTween** - High-performance animation library
  - Smooth UI transitions and effects
  - Note movement animations
  - Screen transition effects
- **TextMeshPro** - Advanced text rendering system

### Asynchronous Operations
- **UniTask** - Async/await support for Unity
  - Non-blocking file loading operations
  - Smooth gameplay initialization
  - Performance-optimized async workflows

### Architecture Patterns
- **Event Bus System** - Decoupled component communication
- **Object Pooling** - Efficient memory management for notes
- **ScriptableObjects** - Data-driven configuration system
- **UI Management System** - Screen state management

## 🎼 MIDI Integration & Logic

### MIDI File Processing

The application uses a sophisticated MIDI processing pipeline:

```csharp
// MIDI file loading and configuration
_midiFilePlayer.MPTK_MidiIndex = _songData.MidiIndex;
_midiFilePlayer.MPTK_Load();
_midiFilePlayer.MPTK_EnableChangeTempo = true;
_midiFilePlayer.MPTK_Tempo = _songData.BPM;
```

### Note Event Conversion

MIDI events are converted to game-ready note data:

1. **Event Filtering**: Only `NoteOn` events are processed
2. **Octave Restriction**: Notes are filtered to octave 5 (60-71 MIDI range)
3. **Natural Notes Only**: Sharp/flat notes are excluded for simplicity
4. **Note Type Mapping**: MIDI numbers convert to musical note types (C, D, E, F, G, A, B)

```csharp
public static NoteType ConvertMidiNoteToNoteType(int midiNote, int octave = 5)
{
    if (midiNote < octave * 12 || midiNote >= (octave + 1) * 12)
        return NoteType.None;

    int noteInOctave = midiNote % 12;
    switch (noteInOctave)
    {
        case 0: return NoteType.C;   // C note
        case 2: return NoteType.D;   // D note
        case 4: return NoteType.E;   // E note
        case 5: return NoteType.F;   // F note
        case 7: return NoteType.G;   // G note
        case 9: return NoteType.A;   // A note
        case 11: return NoteType.B;  // B note
        default: return NoteType.None; // Skip sharps/flats
    }
}
```

### Real-time Playback Synchronization

The system maintains perfect synchronization between visual elements and audio:

- **Tick-based Timing**: Uses MIDI ticks for precise timing calculations
- **BPM Normalization**: Adjusts movement speed based on song tempo
- **Real-time Updates**: Continuously syncs visual elements with playback position

## 🎯 Note Movement System

### Movement Algorithm

Notes move horizontally across the screen using a tick-based calculation system:

```csharp
public void UpdatePosition(double currentTick, float distantPerTick)
{
    var offset = (float)(_noteData.Tick - currentTick) * distantPerTick;
    _rectTransform.anchoredPosition = new Vector2(offset, 0);
}
```

### Key Components:

1. **Tick Difference**: `(_noteData.Tick - currentTick)` - Time until note should be played
2. **Distance Scaling**: `distantPerTick` - Pixels per MIDI tick (BPM-dependent)
3. **Position Calculation**: Linear interpolation creates smooth movement

### Movement Phases:

1. **Spawn**: Notes appear at the right edge of the screen
2. **Approach**: Notes move leftward toward the hit zone
3. **Hit Zone**: Player can interact when note is at x-position 10 to -100
4. **Miss Zone**: Notes that pass x-position -170 are marked as missed
5. **Cleanup**: Notes are recycled when they reach x-position -500

### BPM-Based Speed Adjustment

Movement speed adapts to song tempo:
```csharp
var speedMultiplier = _songData.BPM / 160f; // Normalized to 160 BPM baseline
note.UpdatePosition(currentTick, speedMultiplier);
```

## 🎮 Game Mechanics

### Hit Detection System

The game uses position-based hit detection:

```csharp
private void HandleNoteButtonPressed(NoteType noteType)
{
    foreach (var note in _activeNotes)
    {
        if (note.IsScored) continue;
        
        // Check if note is in hit zone
        if (note.CurrentPositionX is < 10 and > -100)
        {
            if (noteType == note.NoteType)
            {
                // Successful hit
                note.SetScored(true);
                _score += 100;
                return;
            }
            else
            {
                // Wrong note type
                return;
            }
        }
    }
}
```

### Scoring System

- **Hit**: +100 points for correct note at correct time
- **Miss**: No penalty, but no points awarded
- **Wrong Note**: No points, but no penalty to encourage experimentation

### Visual Feedback

- **Note Highlighting**: Visual feedback for successful hits
- **Smooth Animations**: DOTween-powered UI transitions
- **Real-time Score Updates**: Immediate feedback display

## 🏗️ Architecture Overview

### Component Structure

```
GameManager
├── UIManager
│   ├── LoadingScreen
│   ├── RhythmTapScreen (Main Game)
│   └── WinScreen
└── Audio System
    └── MidiFilePlayer

RhythmTapScreen
├── NoteFactory (Object Pool)
├── MovingNote Components
├── Lane Management
└── Input Handling
```

### Data Flow

1. **Initialization**: MIDI file loads → Note data extraction → Note spawning
2. **Gameplay Loop**: Playback update → Note position update → Input processing
3. **Hit Processing**: Input detection → Hit validation → Score update → Visual feedback

### Event System

The application uses a decoupled event system for component communication:

```csharp
// Event registration
NoteButton.OnNoteButtonPressed += HandleNoteButtonPressed;

// Event triggering
public static event Action<NoteType> OnNoteButtonPressed;
```

## 🚀 Setup & Usage

### Prerequisites

- Unity 6000.0.56f1 or later
- Git (for package dependencies)

### Installation

1. Clone the repository
2. Open the project in Unity
3. Allow Unity to import packages and dependencies
4. Open the main scene: `Assets/_App/Scenes/Main.unity`

### Dependencies

The project automatically installs these packages:
- UniTask (Async operations)
- DOTween (Animations)
- Universal Render Pipeline
- Input System

### Running the Game

1. Press Play in Unity Editor
2. Wait for the loading screen
3. Click "Start" to begin the rhythm game
4. Press the corresponding note buttons (C, D, E, F, G, A, B) when notes reach the hit zone

## 🔧 Configuration

### Song Setup

Configure songs via ScriptableObjects:
- **MIDI Index**: Reference to MIDI file in MidiPlayerTK database
- **BPM**: Tempo for movement speed calculation
- **Difficulty Settings**: Future expansion for complexity levels

### Visual Customization

- **Note Sprites**: Configurable via `NoteSpriteConfigSO`
- **Lane Positions**: Adjustable transform array in RhythmTapScreen
- **Animation Timing**: DOTween sequences can be modified for different feels

## 📈 Performance Optimizations

### Object Pooling

Efficient note management prevents garbage collection:
```csharp
// Note recycling
_noteFactory.Release(note); // Return to pool
var note = _noteFactory.Get(); // Reuse from pool
```

### Frame Rate Optimization

- **Conditional Updates**: Only active notes are processed
- **Efficient Algorithms**: Linear time complexity for hit detection
- **Memory Management**: Proper cleanup and pooling strategies

## 🎯 Educational Value

This project demonstrates several key concepts:

1. **Music Theory**: Visual representation of musical notes and timing
2. **Hand-Eye Coordination**: Rhythm-based interaction training
3. **Audio-Visual Synchronization**: Understanding the relationship between sound and visual cues
4. **Pattern Recognition**: Musical pattern identification and response

## 🔮 Future Enhancements

- **Multiple Difficulty Levels**: Adjustable note speeds and complexity
- **Sharp/Flat Note Support**: Extended musical range
- **Custom MIDI Upload**: User-provided MIDI file support
- **Progress Tracking**: Score history and improvement metrics
- **Multiplayer Support**: Competitive and collaborative modes

---

*This Music EdTech project showcases the integration of professional audio processing, real-time game mechanics, and educational principles in a Unity-based interactive application.*