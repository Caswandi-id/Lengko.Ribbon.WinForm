<p align="center">
  <img src="logo.png" alt="Lengko Ribbon WinForms Logo" width="140" />
</p>

# Lengko.Ribbon.WinForms — Demos & Samples

Modern, Office-style Ribbon control suite for **Windows Forms**. Complete implementation of ribbon tabs, groups, buttons, galleries, contextual tab groups, quick access toolbar (QAT), backstage view, and full theming with zero external dependencies. Pure WinForms + GDI+ double-buffered rendering.

[![Platform](https://img.shields.io/badge/Platform-Windows%20Forms-brightgreen.svg)]()
[![Languages](https://img.shields.io/badge/Languages-C%23%20%7C%20VB.NET%20%7C%20F%23-blue.svg)]()
[![Target](https://img.shields.io/badge/.NET-8.0--windows-512BD4.svg)]()
[![License](https://img.shields.io/badge/License-Freeware-orange.svg)](LICENSE.txt)

---

## Projects in this Folder

| Language | Project File | Description |
|---|---|---|
| **C#** | `CSharp/Lengko.Ribbon.Demo.CSharp.csproj` | Comprehensive C# showcase featuring Office themes (Word, Excel, PowerPoint, OneNote, Dark, Fluent), Backstage View, QAT, KeyTips, ScreenTips, Quick Styles Gallery, Contextual tabs, and hosted controls. |
| **VB.NET** | `VB/Lengko.Ribbon.Demo.VB.vbproj` | Full VB.NET showcase implementing QAT, tabs, groups, check boxes, combo boxes, custom styling, contextual tabs, and hosted controls. |
| **F#** | `FSharp/Lengko.Ribbon.Demo.FSharp.fsproj` | Functional F# WinForms demo demonstrating ribbon composition, vector icons, hosted WinForms controls, and theme switches. |

Dedicated solution: **`Lengko.Ribbon.sln`** (builds all 3 demos alongside the library).

---

## Controls & Components Demonstrated

- `LengkoRibbon`: Root ribbon container handling responsive collapsing, tab switching, and rendering.
- `RibbonTab`: Primary tab navigation (`Home`, `Insert`, `View`, etc.).
- `RibbonGroup`: Visual grouping of related commands with optional dialog launcher icon.
- `RibbonButton`: Large / Small ribbon push button, split button, and drop-down menu with vector icons.
- `RibbonCheckBox`: Two-state check box item for toggling options (e.g. Gridlines, Ruler).
- `RibbonComboBox`: Dropdown combo box item with search/selection support.
- `RibbonTextBox`: Text input field inside ribbon groups.
- `RibbonGallery` & `RibbonGalleryItem`: Visual styles gallery with scrolling and thumbnail rendering.
- `RibbonControlHost`: Embed any native WinForms control (`NumericUpDown`, `TrackBar`, `DateTimePicker`) directly into a ribbon group.
- `RibbonContextualGroup`: Context-sensitive colored tab headers (e.g. "Table Tools").
- `RibbonQatItem`: Quick Access Toolbar shortcuts with customizable placement.
- `RibbonBackstageView`: Full-screen Office backstage application menu.
- `RibbonBuiltInStyle` & `RibbonCustomStyle`: 8+ built-in presets (Fluent Light, Dark Zinc, Classic Blue, Word, Excel, PowerPoint, OneNote) plus custom color palettes.

---

## Quick Start / How to Run

### Via Visual Studio
Open `Lengko.Ribbon.sln`, select any demo project as Startup Project, and press `F5`.

### Via .NET CLI

```powershell
# Run C# Demo
dotnet run --project CSharp/Lengko.Ribbon.Demo.CSharp.csproj

# Run VB.NET Demo
dotnet run --project VB/Lengko.Ribbon.Demo.VB.vbproj

# Run F# Demo
dotnet run --project FSharp/Lengko.Ribbon.Demo.FSharp.fsproj
```

---

## Code Examples

### C#
```csharp
using Lengko.Ribbon.WinForms;

var ribbon = new LengkoRibbon { BuiltInStyle = RibbonBuiltInStyle.Light };

var homeTab = new RibbonTab("Home");
var grpClip = new RibbonGroup("Clipboard", RibbonIcons.Copy(24));
var btnPaste = new RibbonButton("Paste", RibbonIcons.Paste(32), RibbonButtonSize.Large);
btnPaste.Click += (s, e) => MessageBox.Show("Pasted!");

grpClip.Items.Add(btnPaste);
homeTab.Groups.Add(grpClip);
ribbon.Tabs.Add(homeTab);
this.Controls.Add(ribbon);
```

### VB.NET
```vb
Imports Lengko.Ribbon.WinForms

Dim ribbon As New LengkoRibbon With { .BuiltInStyle = RibbonBuiltInStyle.Light }

Dim homeTab As New RibbonTab("Home")
Dim grpClip As New RibbonGroup("Clipboard", RibbonIcons.Copy(24))
Dim btnPaste As New RibbonButton("Paste", RibbonIcons.Paste(32), RibbonButtonSize.Large)
AddHandler btnPaste.Click, Sub(s, e) MessageBox.Show("Pasted!")

grpClip.Items.Add(btnPaste)
homeTab.Groups.Add(grpClip)
ribbon.Tabs.Add(homeTab)
Me.Controls.Add(ribbon)
```

### F#
```fsharp
open Lengko.Ribbon.WinForms

let ribbon = new LengkoRibbon(BuiltInStyle = RibbonBuiltInStyle.Light)

let homeTab = new RibbonTab("Home")
let grpClip = new RibbonGroup("Clipboard", RibbonIcons.Copy(24))
let btnPaste = new RibbonButton("Paste", RibbonIcons.Paste(32), RibbonButtonSize.Large)
btnPaste.Click.Add(fun _ -> MessageBox.Show("Pasted!") |> ignore)

grpClip.Items.Add(btnPaste)
homeTab.Groups.Add(grpClip)
ribbon.Tabs.Add(homeTab)
form.Controls.Add(ribbon)
```

---

## License
Proprietary Freeware License (PFL). Free for personal, educational, and commercial use. See [LICENSE.txt](LICENSE.txt).
