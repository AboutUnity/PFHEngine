# PFHEngine
Place for Hero Engine - open world story based game engine refined source code.

## Worlds
Open world system used Scene or GameObject for levels (parts of world). Async load/unload levels (GameObject use Addressables). Target follow or custom calc world state. You can processing loaded levels, generate world data and make custom LevelLoaderManager for support others level formats.

## Conditions
Condition check system. Built-in Force/And/Or/Platform conditions. You can create any conditions for check. Convenient to use for quests, setting object states, etc.

## GameActions
Game action system. Can perform any actions, sequentially or in parallel. You can customize the sequence of actions and reactions for triggers, the beginning of the game or actions in dialogues or before saving, etc.

### Next time...
* DI (simple Dependency Injection framework)
* Localizations (with Google Sheet import)
* Dialogs (width Node editor)
* Pools
* Sounds
* Vars
* Network (Photon based, Offline mode support)
* ...and other...

## Games
* [Place for Hero](https://store.steampowered.com/app/1551730/)
* The Watson - in progress...

## Dependencies
* [SerializeReference Editor](https://github.com/elmortem/serializereferenceeditor)
* DOTween

### Good luck.
