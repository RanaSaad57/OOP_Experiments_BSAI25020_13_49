# Reusable Laboratory Modules

This folder is a standalone Unity `6000.4.6f1` project containing five reusable
OOP laboratory modules.

## Prefabs

Prefabs are located in:

`Assets/_Project/Prefabs/Modules`

Available modules:

1. `ScientificCalculatorModule`
2. `DataRecordingNotebookModule`
3. `MeasurementReadingModule`
4. `GraphPlottingModule`
5. `WeighingBalanceModule`

Scripts are grouped by module in:

`Assets/_Project/Scripts/Modules`

## General Usage

1. Copy the required prefab together with its scripts, materials, and `.meta`
   files into the target Unity project.
2. Drag the prefab into the scene hierarchy.
3. For UI modules, place the prefab under a suitable Canvas unless the prefab
   already contains its own canvas.
4. Ensure an EventSystem exists in the scene.
5. Keep prefab references intact when moving files.

## Scientific Calculator

Purpose: reusable scientific calculation window.

Features:

- Basic arithmetic.
- Power, square, square root, logarithm.
- Sine, cosine, and tangent.
- Clear/reset.

The controller exposes public methods that may be connected to other UI buttons
or called by another experiment controller.

## Data Recording Notebook

Purpose: store experiment names and long observations.

Features:

- Adds timestamped records.
- Supports long text.
- Splits records across pages.
- Previous/next page navigation.
- Clear records.

## Measurement Reading UI

Purpose: central display for readings from laboratory instruments.

Features:

- Instrument name, value, unit, and precision.
- Previous/next instrument selection.
- Simulation and reset.

Integration note: another device should call the public reading method on
`MeasurementReadingController` when its measured value changes.

## Graph Plotting System

Purpose: plot experiment data and calculate slope.

Features:

- Manual X and Y entry.
- Sample data.
- Dynamic axes and grid.
- Connected line graph.
- Slope calculation.
- Clear graph.

## Weighing Balance

Purpose: interactive 3D digital mass display.

Features:

- Click the balance to open its panel.
- Displays grams and kilograms.
- Tare and reset.
- Close button.

Integration note: an external sample can provide mass through its Rigidbody or
through a shared class interface selected during final game integration.

