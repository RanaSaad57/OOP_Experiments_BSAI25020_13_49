# Interactive Physics and Chemistry Lab

Unity OOP lab submission by BSAI students 25020, 25013, and 25049.

This repository contains reusable laboratory modules and two complete physics
practicals:

- Experiment 4: acceleration of a rolling ball on an inclined plane.
- Experiment 5: determination of gravitational acceleration by free fall.

## Repository Structure

| Folder | Contents |
|---|---|
| `01_Modules_Source` | Complete Unity source project for the five reusable modules |
| `02_Modules_Demo_Video` | Recorded demonstration of all modules |
| `03_Modules_APK` | Android build for the modules demonstration |
| `04_Practicals_Source` | Complete Unity source project for Experiments 4 and 5 |
| `05_Practicals_Demo_Video` | Recorded gameplay demonstration of both practicals |
| `06_Practicals_APK` | Android build containing both practicals |

`ProgressDone.txt` provides an honest feature-by-feature progress report,
including completed work, limitations, reused modules, and remaining tasks.

## Requirements

- Unity Editor `6000.4.6f1`
- Universal Render Pipeline
- Android Build Support for APK builds

## Opening the Source Projects

1. Open Unity Hub.
2. Select **Add project from disk**.
3. Choose either `01_Modules_Source` or `04_Practicals_Source`.
4. Open the project using Unity `6000.4.6f1`.
5. Allow Unity to restore packages and import assets.

Do not add the repository root as a Unity project. Each source folder is a
separate Unity project.

## Running the Modules

Open `01_Modules_Source`, load the demonstration scene, and press Play. See
`01_Modules_Source/README.md` for prefab paths, controls, and integration notes.

## Running the Practicals

Open `04_Practicals_Source`, load
`Assets/Scenes/ExperimentLab.unity`, and press Play.

The application starts at an experiment menu. Each experiment provides an
information screen, guided and independent modes, instructions, a data
notebook, graph plotting, and calculation tools.

## Team Access

Repository owner: `RanaSaad57`

Collaborators:

- `Saad-Yasir786`
- `mahadaleem713`
