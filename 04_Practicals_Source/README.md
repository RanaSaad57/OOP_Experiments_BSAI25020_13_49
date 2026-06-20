# Physics Practicals: Experiments 4 and 5

This folder is a standalone Unity `6000.4.6f1` Universal 3D project.

## Run

1. Add this folder to Unity Hub.
2. Open it using Unity `6000.4.6f1`.
3. Open `Assets/Scenes/ExperimentLab.unity`.
4. Press Play.

## Experiment 4

**Acceleration of a rolling ball on an inclined plane**

The user changes angle, friction, and target distance; releases the ball;
records repeated trials; and plots `2s` against `t^2`. The graph slope gives
acceleration.

Formula:

`s = 1/2 at^2`, therefore `a = 2s/t^2`.

## Experiment 5

**Determination of gravitational acceleration by free fall**

The user changes drop height, releases the steel ball, records repeated trials,
and plots `2h` against `t^2`. The graph slope gives `g`.

Formula:

`h = 1/2 gt^2`, therefore `g = 2h/t^2`.

## Interface

- **Info:** objective, apparatus, theory, procedure, and good practice.
- **Notebook:** automatic trial table.
- **Graph:** automatic or manual data points, scale labels, and slope.
- **Calculator:** latest-trial formula and manual scientific calculation.
- **Instructions:** visible in Guided Practice and optional in Independent Mode.
- Every popup includes a close or back control.

## Builder

If the scene must be regenerated, use:

`OOP Lab > Build Final Project - Experiment 4`

The command creates the complete menu and both experiments.

