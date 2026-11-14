namespace Lengko.Ribbon.Demo.FSharp

open System
open System.Drawing
open System.Drawing.Drawing2D
open System.Windows.Forms
open Lengko.Ribbon.WinForms

module Program =

    let createStyleSample (label: string) (c: Color) : Image =
        let bmp = new Bitmap(32, 24)
        use g = Graphics.FromImage(bmp)
        g.SmoothingMode <- SmoothingMode.AntiAlias
        use brush = new SolidBrush(Color.FromArgb(241, 245, 249))
        g.FillRectangle(brush, 0, 0, 32, 24)
        use font = new Font("Segoe UI", 8.5f, FontStyle.Bold)
        TextRenderer.DrawText(g, label, font, Rectangle(0, 0, 32, 24), c,
            TextFormatFlags.HorizontalCenter ||| TextFormatFlags.VerticalCenter)
        use pen = new Pen(Color.FromArgb(203, 213, 225), 1.0f)
        g.DrawRectangle(pen, 0, 0, 31, 23)
        bmp :> Image

    type RibbonDemoForm() as this =
        inherit Form()

        let ribbon = new LengkoRibbon(
            BuiltInStyle = RibbonBuiltInStyle.Light,
            FileButtonText = "File",
            ShowSearchBox = true,
            SearchPlaceholder = "Tell me what you want to do..."
        )
        let statusLabel = new Label(
            Dock = DockStyle.Bottom,
            Height = 28,
            Text = " Tip: Press [Alt] for KeyTips, hover for Rich ScreenTips, resize window for Auto-Collapse!",
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.FromArgb(241, 245, 249),
            ForeColor = Color.FromArgb(71, 85, 105),
            Font = new Font("Segoe UI", 9.0f, FontStyle.Italic)
        )
        let logBox = new ListBox(
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 9.5f),
            IntegralHeight = false,
            BorderStyle = BorderStyle.None
        )
        let contentContainer = new Panel(
            Dock = DockStyle.Fill,
            Padding = Padding(12),
            BackColor = Color.White
        )

        let log msg =
            logBox.Items.Insert(0, sprintf "[%s] %s" (DateTime.Now.ToString("HH:mm:ss")) msg) |> ignore

        let applyThemeUi (sb: Color, sf: Color, lb: Color, lf: Color, wb: Color) =
            this.BackColor <- wb
            contentContainer.BackColor <- lb
            statusLabel.BackColor <- sb
            statusLabel.ForeColor <- sf
            logBox.BackColor <- lb
            logBox.ForeColor <- lf

        do
            this.Text <- "Lengko Ribbon Demo (F#) - Modern Responsive & Office Themes"
            this.Size <- Size(1150, 680)
            this.StartPosition <- FormStartPosition.CenterScreen
            this.MinimumSize <- Size(480, 420)

            contentContainer.Controls.Add(logBox)

            ribbon.SearchSubmitted.Add(fun query ->
                log (sprintf "Search / Tell Me executed: '%s'" query)
            )

            // Setup File menu
            let fileMenu = new ContextMenuStrip()
            fileMenu.Items.Add("New Document", RibbonIcons.NewDocument(16), fun _ _ -> log "File -> New") |> ignore
            fileMenu.Items.Add("Open File...", RibbonIcons.OpenFolder(16), fun _ _ -> log "File -> Open") |> ignore
            fileMenu.Items.Add(new ToolStripSeparator()) |> ignore
            fileMenu.Items.Add("Save", RibbonIcons.Save(16), fun _ _ -> log "File -> Save") |> ignore
            fileMenu.Items.Add(new ToolStripSeparator()) |> ignore
            fileMenu.Items.Add("Exit", null, fun _ _ -> this.Close()) |> ignore
            ribbon.FileButtonMenu <- fileMenu

            // QAT
            let qatSave = new RibbonQatItem(RibbonIcons.Save(16), "Quick Save",
                ToolTipTitle = "Save (Ctrl+S)",
                ToolTipDescription = "Save document immediately",
                KeyTip = "1")
            qatSave.Click.Add(fun _ -> log "QAT: Save clicked")

            let qatUndo = new RibbonQatItem(RibbonIcons.Undo(16), "Undo",
                ToolTipTitle = "Undo (Ctrl+Z)",
                ToolTipDescription = "Undo previous editing action",
                KeyTip = "2")
            qatUndo.Click.Add(fun _ -> log "QAT: Undo clicked")

            let qatRedo = new RibbonQatItem(RibbonIcons.Redo(16), "Redo",
                ToolTipTitle = "Redo (Ctrl+Y)",
                ToolTipDescription = "Reapply undone action",
                KeyTip = "3")
            qatRedo.Click.Add(fun _ -> log "QAT: Redo clicked")

            ribbon.QuickAccessItems.Add(qatSave)
            ribbon.QuickAccessItems.Add(qatUndo)
            ribbon.QuickAccessItems.Add(qatRedo)

            // Tab 1: Home
            let homeTab = new RibbonTab("Home", KeyTip = "H")

            // Clipboard Group
            let grpClip = new RibbonGroup("Clipboard", RibbonIcons.Copy(24))
            let btnPaste = new RibbonButton("Paste", RibbonIcons.Paste(32), RibbonButtonSize.Large,
                IsSplitButton = true,
                KeyTip = "V",
                ToolTipTitle = "Paste (Ctrl+V)",
                ToolTipDescription = "Paste contents of clipboard into document",
                ToolTipShortcut = "Ctrl+V")
            btnPaste.Click.Add(fun _ -> log "Clipboard: Paste clicked")

            let btnCut = new RibbonButton("Cut", RibbonIcons.Cut(16), RibbonButtonSize.Small,
                KeyTip = "X",
                ToolTipTitle = "Cut (Ctrl+X)",
                ToolTipDescription = "Cut selected text to clipboard",
                ToolTipShortcut = "Ctrl+X")
            btnCut.Click.Add(fun _ -> log "Clipboard: Cut clicked")

            let btnCopy = new RibbonButton("Copy", RibbonIcons.Copy(16), RibbonButtonSize.Small,
                KeyTip = "C",
                ToolTipTitle = "Copy (Ctrl+C)",
                ToolTipDescription = "Copy selection to clipboard",
                ToolTipShortcut = "Ctrl+C")
            btnCopy.Click.Add(fun _ -> log "Clipboard: Copy clicked")

            grpClip.Items.Add(btnPaste)
            grpClip.Items.Add(btnCut)
            grpClip.Items.Add(btnCopy)

            // Font Group
            let grpFont = new RibbonGroup("Font", RibbonIcons.Bold(24), ShowDialogLauncher = true)
            grpFont.DialogLauncherClick.Add(fun _ -> log "Font: Dialog Launcher clicked (Open Font Dialog)")

            let btnBold = new RibbonButton("Bold", RibbonIcons.Bold(16), RibbonButtonSize.Small,
                Checkable = true,
                KeyTip = "B",
                ToolTipTitle = "Bold (Ctrl+B)",
                ToolTipDescription = "Make selected text bold",
                ToolTipShortcut = "Ctrl+B")
            btnBold.CheckedChanged.Add(fun _ -> log (sprintf "Font: Bold toggled -> %b" btnBold.Checked))

            let btnItalic = new RibbonButton("Italic", RibbonIcons.Italic(16), RibbonButtonSize.Small,
                Checkable = true,
                KeyTip = "I",
                ToolTipTitle = "Italic (Ctrl+I)",
                ToolTipDescription = "Italicize selected text",
                ToolTipShortcut = "Ctrl+I")
            btnItalic.CheckedChanged.Add(fun _ -> log (sprintf "Font: Italic toggled -> %b" btnItalic.Checked))

            let btnUnderline = new RibbonButton("Underline", RibbonIcons.Underline(16), RibbonButtonSize.Small,
                Checkable = true,
                KeyTip = "U",
                ToolTipTitle = "Underline (Ctrl+U)",
                ToolTipDescription = "Underline selected text",
                ToolTipShortcut = "Ctrl+U")
            btnUnderline.CheckedChanged.Add(fun _ -> log (sprintf "Font: Underline toggled -> %b" btnUnderline.Checked))

            grpFont.Items.Add(btnBold)
            grpFont.Items.Add(btnItalic)
            grpFont.Items.Add(btnUnderline)

            // Quick Styles Gallery
            let grpStyles = new RibbonGroup("Quick Styles", RibbonIcons.NewDocument(24))
            let gallery = new RibbonGallery(VisibleColumns = 3, ItemWidth = 54)
            gallery.Items.Add(new RibbonGalleryItem("Normal", createStyleSample "Aa" (Color.FromArgb(30, 41, 59))))
            gallery.Items.Add(new RibbonGalleryItem("Heading 1", createStyleSample "H1" (Color.FromArgb(43, 87, 154))))
            gallery.Items.Add(new RibbonGalleryItem("Heading 2", createStyleSample "H2" (Color.FromArgb(33, 115, 70))))
            gallery.Items.Add(new RibbonGalleryItem("Title", createStyleSample "Title" (Color.FromArgb(183, 71, 42))))
            gallery.SelectedIndex <- 0
            gallery.SelectedIndexChanged.Add(fun _ ->
                match gallery.SelectedItem with
                | null -> ()
                | item -> log (sprintf "Quick Style applied: %s" item.Text)
            )
            grpStyles.Items.Add(gallery)

            // Themes Group
            let grpThemes = new RibbonGroup("Themes", RibbonIcons.Save(24))
            let cbTheme = new RibbonComboBox("Theme:", 155)
            cbTheme.Items.Add("Light")
            cbTheme.Items.Add("Dark")
            cbTheme.Items.Add("Classic")
            cbTheme.Items.Add("Word")
            cbTheme.Items.Add("Excel")
            cbTheme.Items.Add("PowerPoint")
            cbTheme.Items.Add("OneNote")
            cbTheme.Items.Add("Custom (Ruby)")
            cbTheme.SelectedIndex <- 0

            cbTheme.SelectedIndexChanged.Add(fun _ ->
                match cbTheme.SelectedItem with
                | "Dark" ->
                    ribbon.BuiltInStyle <- RibbonBuiltInStyle.Dark
                    applyThemeUi(Color.FromArgb(30, 41, 59), Color.FromArgb(203, 213, 225), Color.FromArgb(15, 23, 42), Color.FromArgb(241, 245, 249), Color.FromArgb(15, 23, 42))
                    log "Switched to Dark theme (full dark window)."
                | "Classic" ->
                    ribbon.BuiltInStyle <- RibbonBuiltInStyle.Classic
                    applyThemeUi(Color.FromArgb(214, 232, 255), Color.FromArgb(21, 66, 139), Color.FromArgb(245, 250, 255), Color.FromArgb(21, 66, 139), Color.FromArgb(214, 232, 255))
                    log "Switched to Classic theme."
                | "Word" ->
                    ribbon.BuiltInStyle <- RibbonBuiltInStyle.Word
                    applyThemeUi(Color.FromArgb(240, 245, 252), Color.FromArgb(43, 87, 154), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(240, 245, 252))
                    log "Switched to Word theme."
                | "Excel" ->
                    ribbon.BuiltInStyle <- RibbonBuiltInStyle.Excel
                    applyThemeUi(Color.FromArgb(240, 250, 244), Color.FromArgb(33, 115, 70), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(240, 250, 244))
                    log "Switched to Excel theme."
                | "PowerPoint" ->
                    ribbon.BuiltInStyle <- RibbonBuiltInStyle.PowerPoint
                    applyThemeUi(Color.FromArgb(254, 245, 242), Color.FromArgb(183, 71, 42), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(254, 245, 242))
                    log "Switched to PowerPoint theme."
                | "OneNote" ->
                    ribbon.BuiltInStyle <- RibbonBuiltInStyle.OneNote
                    applyThemeUi(Color.FromArgb(248, 242, 252), Color.FromArgb(128, 57, 123), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(248, 242, 252))
                    log "Switched to OneNote theme."
                | "Custom (Ruby)" ->
                    let customRuby = new RibbonCustomStyle(new RibbonDarkStyle(), "Ruby Dark")
                    customRuby.FileButtonBackColor <- Color.FromArgb(190, 18, 60)
                    customRuby.FileButtonHoverBackColor <- Color.FromArgb(225, 29, 72)
                    customRuby.TabActiveTextColor <- Color.FromArgb(251, 113, 133)
                    customRuby.ItemCheckedBackColor <- Color.FromArgb(76, 5, 25)
                    customRuby.ItemCheckedBorderColor <- Color.FromArgb(225, 29, 72)
                    ribbon.Style <- customRuby
                    applyThemeUi(Color.FromArgb(30, 15, 25), Color.FromArgb(251, 113, 133), Color.FromArgb(15, 23, 42), Color.FromArgb(241, 245, 249), Color.FromArgb(20, 10, 18))
                    log "Switched to Custom Ruby Dark via RibbonCustomStyle."
                | _ ->
                    ribbon.BuiltInStyle <- RibbonBuiltInStyle.Light
                    applyThemeUi(Color.FromArgb(241, 245, 249), Color.FromArgb(71, 85, 105), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(243, 244, 246))
                    log "Switched to Light theme."
            )
            grpThemes.Items.Add(cbTheme)

            // Contextual Group
            let grpContext = new RibbonGroup("Contextual", RibbonIcons.Table(24))
            let chkTable = new RibbonCheckBox("Select Table", false, KeyTip = "T")
            grpContext.Items.Add(chkTable)

            // Actions Group
            let grpActions = new RibbonGroup("Actions", RibbonIcons.Save(24))
            let btnNew = new RibbonButton("New", RibbonIcons.NewDocument(32), RibbonButtonSize.Large, KeyTip = "N")
            btnNew.Click.Add(fun _ -> log "Actions: New clicked")
            let btnSave = new RibbonButton("Save", RibbonIcons.Save(32), RibbonButtonSize.Large, KeyTip = "S")
            btnSave.Click.Add(fun _ -> log "Actions: Save clicked")
            let btnPrint = new RibbonButton("Print", RibbonIcons.Print(32), RibbonButtonSize.Large, KeyTip = "P")
            btnPrint.Click.Add(fun _ -> log "Actions: Print clicked")

            grpActions.Items.Add(btnNew)
            grpActions.Items.Add(btnSave)
            grpActions.Items.Add(btnPrint)

            homeTab.Groups.Add(grpClip)
            homeTab.Groups.Add(grpFont)
            homeTab.Groups.Add(grpStyles)
            homeTab.Groups.Add(grpThemes)
            homeTab.Groups.Add(grpContext)
            homeTab.Groups.Add(grpActions)
            ribbon.Tabs.Add(homeTab)

            // Contextual Tab: Table Design
            let tableTab = new RibbonTab("Table Design",
                ContextTitle = "Table Tools",
                ContextColor = Nullable(Color.FromArgb(245, 158, 11)),
                KeyTip = "J",
                Visible = false)
            let grpTableStyles = new RibbonGroup("Table Styles", RibbonIcons.Table(24))
            let btnGridTable = new RibbonButton("Grid Table", RibbonIcons.Table(32), RibbonButtonSize.Large)
            btnGridTable.Click.Add(fun _ -> log "Table Design: Grid Table clicked")
            grpTableStyles.Items.Add(btnGridTable)
            tableTab.Groups.Add(grpTableStyles)
            ribbon.Tabs.Add(tableTab)

            chkTable.CheckedChanged.Add(fun _ ->
                tableTab.Visible <- chkTable.Checked
                if chkTable.Checked then
                    ribbon.SelectedTab <- tableTab
                    log "Contextual Tab 'Table Design' activated!"
                else
                    ribbon.SelectedTabIndex <- 0
                    log "Contextual Tab 'Table Design' hidden."
            )

            // Tab 2: Hosted Controls
            let hostTab = new RibbonTab("Hosted Controls", KeyTip = "C")
            let grpHosted = new RibbonGroup("Hosted WinForms Controls", RibbonIcons.Table(24))

            let numUpDown = new NumericUpDown(Minimum = 1M, Maximum = 100M, Value = 16M, Width = 90)
            numUpDown.ValueChanged.Add(fun _ -> log (sprintf "Hosted NumericUpDown changed: %O" numUpDown.Value))
            let hostNum = new RibbonControlHost(numUpDown, 100, 24,
                ToolTipTitle = "Zoom Level",
                ToolTipDescription = "Hosted System.Windows.Forms.NumericUpDown inside Ribbon group.")

            let trackBar = new TrackBar(Minimum = 0, Maximum = 100, Value = 50, TickStyle = TickStyle.None, Width = 120, Height = 24)
            trackBar.ValueChanged.Add(fun _ -> log (sprintf "Hosted TrackBar changed: %d%%" trackBar.Value))
            let hostTrack = new RibbonControlHost(trackBar, 130, 24,
                ToolTipTitle = "Opacity Slider",
                ToolTipDescription = "Hosted System.Windows.Forms.TrackBar inside Ribbon group.")

            grpHosted.Items.Add(hostNum)
            grpHosted.Items.Add(hostTrack)
            hostTab.Groups.Add(grpHosted)
            ribbon.Tabs.Add(hostTab)

            // Tab 3: View
            let viewTab = new RibbonTab("View", KeyTip = "W")
            let grpView = new RibbonGroup("Options", RibbonIcons.Zoom(24))
            let chkGrid = new RibbonCheckBox("Gridlines", true)
            chkGrid.CheckedChanged.Add(fun _ -> log (sprintf "View: Gridlines -> %b" chkGrid.Checked))
            let chkRuler = new RibbonCheckBox("Ruler", false)
            chkRuler.CheckedChanged.Add(fun _ -> log (sprintf "View: Ruler -> %b" chkRuler.Checked))

            grpView.Items.Add(chkGrid)
            grpView.Items.Add(chkRuler)
            viewTab.Groups.Add(grpView)
            ribbon.Tabs.Add(viewTab)

            this.Controls.Add(contentContainer)
            this.Controls.Add(statusLabel)
            this.Controls.Add(ribbon)

            log "Ribbon initialized (F#) with real vector icons and enhanced styling."

    [<STAThread; EntryPoint>]
    let main _ =
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(false)
        Application.Run(new RibbonDemoForm())
        0
