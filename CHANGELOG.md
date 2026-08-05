# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- TouchCondition, it triggers on any touch screen contact and has static methods for one contact at a time
- Ability for Track.TouchCondition to claim a number of contacts and hold them until they lift or another condition takes one
- Track.TouchCondition, it tracks each contact separately so consuming one finger leaves the others free
- Pointer, a single position that follows the touch screen while something is touching it and the mouse otherwise
- PointerSource enum, it tells which device the pointer position came from
- HoldCondition, it wraps another condition and triggers once it's been held long enough, with a Progress from 0 to 1
- RepeatCondition, it repeats another condition after a delay the way a key repeats in a text field
- TotalMS to InputHelper, the clock that duration based conditions read
- OldTouch to InputHelper
- LostTouches to InputHelper, the contacts that went away without reporting Released

### Changed

- UpdateSetup now takes a GameTime, the version without it is gone
- NewTouchCollection is now NewTouch
- The touch panel's display size now follows the back buffer
- Pressed now triggers for keyboard and gamepad when the game gets focus back with a button still held, the way MouseCondition already did

### Fixed

- Released now triggers for keyboard, mouse, and gamepad when the game loses focus while a button is held

## [2.5.0] - 2026-01-06

### Added

- KNI package

### Changed

- Target framework is now net8.0

## [2.4.2] - 2022-01-29

### Changed

- Now gets the window width and height from the back buffer instead of the window client bounds.

## [2.4.1] - 2021-06-26

### Fixed

- MouseCondition focus fix for the tracking system

## [2.4.0] - 2021-06-26

### Added

- OldIsActive to InputHelper, it can be used to check if the window just got focused

### Fixed

- MouseCondition when the window just got focused by clicking on it. It was possible for Pressed to not trigger while HeldOnly would trigger

## [2.3.3] - 2021-03-21

### Added

- PointerMoved API to MouseCondition
- PointerDelta API to MouseCondition

## [2.3.2] - 2021-02-20

### Fixed

- MonoGame package reference so that the library can be used in any MonoGame platform

## [2.3.1] - 2021-01-27

### Fixed

- MonoGame package reference so that the library can be used with local MonoGame builds

## [2.3.0] - 2021-01-27

### Fixed

- AllCondition when empty

## [2.2.0] - 2021-01-24

### Added

- MouseSensor enum
- GamePadSensor enum
- Scrolled API to MouseCondition
- ScrollDelta API to MouseCondition
- Ability to consume Mouse or GamePad sensors

## [2.1.0] - 2021-01-19

### Added

- Static methods for the tracking system

## [2.0.1] - 2021-01-17

### Fixed

- AllCondition and AnyCondition were consuming the conditions during their checks

## [2.0.0] - 2021-01-12

### Added

- AnyGamePadCondition, it combines every gamepad as a single input condition
- Input tracking system

### Changed

- ICondition to accommodate the tracking system
- TextEvents are now TextInputEventArgs

## [1.0.1] - 2020-10-20

### Added

- Everything!

[Unreleased]: https://github.com/Apostolique/Apos.Input/compare/v2.5.0...HEAD
[2.5.0]: https://github.com/Apostolique/Apos.Input/compare/v2.4.2...v2.5.0
[2.4.2]: https://github.com/Apostolique/Apos.Input/compare/v2.4.1...v2.4.2
[2.4.1]: https://github.com/Apostolique/Apos.Input/compare/v2.4.0...v2.4.1
[2.4.0]: https://github.com/Apostolique/Apos.Input/compare/v2.3.3...v2.4.0
[2.3.3]: https://github.com/Apostolique/Apos.Input/compare/v2.3.2...v2.3.3
[2.3.2]: https://github.com/Apostolique/Apos.Input/compare/v2.3.1...v2.3.2
[2.3.1]: https://github.com/Apostolique/Apos.Input/compare/v2.3.0...v2.3.1
[2.3.0]: https://github.com/Apostolique/Apos.Input/compare/v2.2.0...v2.3.0
[2.2.0]: https://github.com/Apostolique/Apos.Input/compare/v2.1.0...v2.2.0
[2.1.0]: https://github.com/Apostolique/Apos.Input/compare/v2.0.1...v2.1.0
[2.0.1]: https://github.com/Apostolique/Apos.Input/compare/v2.0.0...v2.0.1
[2.0.0]: https://github.com/Apostolique/Apos.Input/compare/v1.0.1...v2.0.0
[1.0.1]: https://github.com/Apostolique/Apos.Input/releases/tag/v1.0.1
