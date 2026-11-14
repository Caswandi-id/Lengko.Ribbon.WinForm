using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Lengko.Ribbon.WinForms;

namespace Lengko.Ribbon.Demo;

public class MainForm : Form
{
    private readonly LengkoRibbon _ribbon;
    private readonly ListBox _logBox;
    private readonly Label _statusLabel;
    private readonly Panel _contentContainer;

    public MainForm()
    {
        Text = "Lengko Ribbon Demo (C#) - Modern Responsive & Office Themes";
        Size = new Size(1180, 700);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(480, 420);

        _statusLabel = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 28,
            Text = " Tip: Press [Alt] for KeyTips, hover for Rich ScreenTips, resize window for Auto-Collapse!",
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.FromArgb(241, 245, 249),
            ForeColor = Color.FromArgb(71, 85, 105),
            Font = new Font("Segoe UI", 9f, FontStyle.Italic)
        };

        _logBox = new ListBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 9.5f),
            IntegralHeight = false,
            BorderStyle = BorderStyle.None
        };

        _contentContainer = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            BackColor = Color.White
        };
        _contentContainer.Controls.Add(_logBox);

        _ribbon = new LengkoRibbon
        {
            BuiltInStyle = RibbonBuiltInStyle.Light,
            FileButtonText = "File",
            UseBackstage = true,
            ShowSearchBox = true,
            SearchPlaceholder = "Tell me what you want to do..."
        };

        _ribbon.SearchSubmitted += (s, query) => Log($"Search / Tell Me executed: '{query}'");

        SetupFileMenu();
        SetupQuickAccessToolbar();
        SetupHomeTab();
        SetupInsertTab();
        SetupCustomHostTab();
        SetupCustomCanvasTab();
        SetupViewTab();

        Controls.Add(_contentContainer);
        Controls.Add(_statusLabel);
        Controls.Add(_ribbon);

        Log("Lengko Ribbon initialized with real vector icons and enhanced styling.");
    }

    private void SetupFileMenu()
    {
        var fileMenu = new ContextMenuStrip();
        fileMenu.Items.Add("New Document", RibbonIcons.NewDocument(16), (s, e) => Log("File -> New Document"));
        fileMenu.Items.Add("Open File...", RibbonIcons.OpenFolder(16), (s, e) => Log("File -> Open"));
        fileMenu.Items.Add(new ToolStripSeparator());
        fileMenu.Items.Add("Save", RibbonIcons.Save(16), (s, e) => Log("File -> Save"));
        fileMenu.Items.Add(new ToolStripSeparator());
        fileMenu.Items.Add("Exit", null, (s, e) => Close());

        _ribbon.FileButtonMenu = fileMenu;
    }

    private void SetupQuickAccessToolbar()
    {
        var qatSave = new RibbonQatItem(RibbonIcons.Save(16), "Quick Save")
        {
            ToolTipTitle = "Save (Ctrl+S)",
            ToolTipDescription = "Save document immediately",
            KeyTip = "1"
        };
        qatSave.Click += (s, e) => Log("QAT: Quick Save clicked");

        var qatUndo = new RibbonQatItem(RibbonIcons.Undo(16), "Undo")
        {
            ToolTipTitle = "Undo (Ctrl+Z)",
            ToolTipDescription = "Undo previous editing step",
            KeyTip = "2"
        };
        qatUndo.Click += (s, e) => Log("QAT: Undo clicked");

        var qatRedo = new RibbonQatItem(RibbonIcons.Redo(16), "Redo")
        {
            ToolTipTitle = "Redo (Ctrl+Y)",
            ToolTipDescription = "Reapply the last undone action",
            KeyTip = "3"
        };
        qatRedo.Click += (s, e) => Log("QAT: Redo clicked");

        _ribbon.QuickAccessItems.Add(qatSave);
        _ribbon.QuickAccessItems.Add(qatUndo);
        _ribbon.QuickAccessItems.Add(qatRedo);
    }

    private void SetupHomeTab()
    {
        var homeTab = new RibbonTab("Home") { KeyTip = "H" };

        // Group 1: Clipboard
        var grpClipboard = new RibbonGroup("Clipboard", RibbonIcons.Copy(24));
        var btnPaste = new RibbonButton("Paste", RibbonIcons.Paste(32), RibbonButtonSize.Large)
        {
            IsSplitButton = true,
            KeyTip = "V",
            ToolTipTitle = "Paste (Ctrl+V)",
            ToolTipDescription = "Paste contents of clipboard into document",
            ToolTipShortcut = "Ctrl+V"
        };
        var pasteMenu = new ContextMenuStrip();
        pasteMenu.Items.Add("Paste Regular", null, (s, e) => Log("Paste -> Regular"));
        pasteMenu.Items.Add("Paste as Plain Text", null, (s, e) => Log("Paste -> Plain Text"));
        btnPaste.DropDownMenu = pasteMenu;
        btnPaste.Click += (s, e) => Log("Clipboard: Paste clicked");

        var btnCut = new RibbonButton("Cut", RibbonIcons.Cut(16), RibbonButtonSize.Small)
        {
            KeyTip = "X",
            ToolTipTitle = "Cut (Ctrl+X)",
            ToolTipDescription = "Cut selected text to clipboard",
            ToolTipShortcut = "Ctrl+X"
        };
        btnCut.Click += (s, e) => Log("Clipboard: Cut clicked");

        var btnCopy = new RibbonButton("Copy", RibbonIcons.Copy(16), RibbonButtonSize.Small)
        {
            KeyTip = "C",
            ToolTipTitle = "Copy (Ctrl+C)",
            ToolTipDescription = "Copy selection to clipboard",
            ToolTipShortcut = "Ctrl+C"
        };
        btnCopy.Click += (s, e) => Log("Clipboard: Copy clicked");

        grpClipboard.Items.Add(btnPaste);
        grpClipboard.Items.Add(btnCut);
        grpClipboard.Items.Add(btnCopy);

        // Group 2: Font
        var grpFont = new RibbonGroup("Font", RibbonIcons.Bold(24))
        {
            ShowDialogLauncher = true
        };
        grpFont.DialogLauncherClick += (s, e) => Log("Font: Dialog Launcher clicked (Open Font Dialog)");

        var cbFont = new RibbonComboBox("Font:", 110);
        cbFont.Items.Add("Segoe UI");
        cbFont.Items.Add("Arial");
        cbFont.Items.Add("Calibri");
        cbFont.Items.Add("Consolas");
        cbFont.SelectedIndex = 0;
        cbFont.SelectedIndexChanged += (s, e) => Log($"Font changed to: {cbFont.SelectedItem}");

        var btnBold = new RibbonButton("Bold", RibbonIcons.Bold(16), RibbonButtonSize.Small)
        {
            Checkable = true,
            KeyTip = "B",
            ToolTipTitle = "Bold (Ctrl+B)",
            ToolTipDescription = "Make selected text bold",
            ToolTipShortcut = "Ctrl+B"
        };
        btnBold.CheckedChanged += (s, e) => Log($"Font: Bold toggled -> {btnBold.Checked}");

        var btnItalic = new RibbonButton("Italic", RibbonIcons.Italic(16), RibbonButtonSize.Small)
        {
            Checkable = true,
            KeyTip = "I",
            ToolTipTitle = "Italic (Ctrl+I)",
            ToolTipDescription = "Italicize selected text",
            ToolTipShortcut = "Ctrl+I"
        };
        btnItalic.CheckedChanged += (s, e) => Log($"Font: Italic toggled -> {btnItalic.Checked}");

        var btnUnderline = new RibbonButton("Underline", RibbonIcons.Underline(16), RibbonButtonSize.Small)
        {
            Checkable = true,
            KeyTip = "U",
            ToolTipTitle = "Underline (Ctrl+U)",
            ToolTipDescription = "Underline selected text",
            ToolTipShortcut = "Ctrl+U"
        };
        btnUnderline.CheckedChanged += (s, e) => Log($"Font: Underline toggled -> {btnUnderline.Checked}");

        grpFont.Items.Add(cbFont);
        grpFont.Items.Add(btnBold);
        grpFont.Items.Add(btnItalic);
        grpFont.Items.Add(btnUnderline);

        // Group 3: Quick Styles Gallery
        var grpStyles = new RibbonGroup("Quick Styles", RibbonIcons.NewDocument(24));
        var gallery = new RibbonGallery { VisibleColumns = 3, ItemWidth = 54 };
        gallery.Items.Add(new RibbonGalleryItem("Normal", CreateStyleSample("Aa", Color.FromArgb(30, 41, 59))));
        gallery.Items.Add(new RibbonGalleryItem("Heading 1", CreateStyleSample("H1", Color.FromArgb(43, 87, 154))));
        gallery.Items.Add(new RibbonGalleryItem("Heading 2", CreateStyleSample("H2", Color.FromArgb(33, 115, 70))));
        gallery.Items.Add(new RibbonGalleryItem("Title", CreateStyleSample("Title", Color.FromArgb(183, 71, 42))));
        gallery.Items.Add(new RibbonGalleryItem("Subtitle", CreateStyleSample("Sub", Color.FromArgb(100, 116, 139))));
        gallery.SelectedIndex = 0;
        gallery.SelectedIndexChanged += (s, e) => Log($"Quick Style applied: {gallery.SelectedItem?.Text}");
        grpStyles.Items.Add(gallery);

        // Group 4: Themes (Office Themes & Custom Styling)
        var grpThemes = new RibbonGroup("Themes & Styling", RibbonIcons.Save(24));
        var cbStyles = new RibbonComboBox("Style:", 155);
        cbStyles.Items.Add("Light (Fluent)");
        cbStyles.Items.Add("Dark (Modern Zinc)");
        cbStyles.Items.Add("Classic (Office Blue)");
        cbStyles.Items.Add("Word (Cobalt Blue)");
        cbStyles.Items.Add("Excel (Emerald)");
        cbStyles.Items.Add("PowerPoint (Orange)");
        cbStyles.Items.Add("OneNote (Purple)");
        cbStyles.Items.Add("Custom (Ruby Dark)");
        cbStyles.SelectedIndex = 0;
        cbStyles.SelectedIndexChanged += (s, e) =>
        {
            switch (cbStyles.SelectedIndex)
            {
                case 0:
                    _ribbon.BuiltInStyle = RibbonBuiltInStyle.Light;
                    ApplyThemeUi(Color.FromArgb(241, 245, 249), Color.FromArgb(71, 85, 105), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(243, 244, 246));
                    Log("Switched style: Light (Fluent)");
                    break;
                case 1:
                    _ribbon.BuiltInStyle = RibbonBuiltInStyle.Dark;
                    ApplyThemeUi(Color.FromArgb(30, 41, 59), Color.FromArgb(203, 213, 225), Color.FromArgb(15, 23, 42), Color.FromArgb(241, 245, 249), Color.FromArgb(15, 23, 42));
                    Log("Switched style: Dark (Modern Zinc) - 100% full dark window");
                    break;
                case 2:
                    _ribbon.BuiltInStyle = RibbonBuiltInStyle.Classic;
                    ApplyThemeUi(Color.FromArgb(214, 232, 255), Color.FromArgb(21, 66, 139), Color.FromArgb(245, 250, 255), Color.FromArgb(21, 66, 139), Color.FromArgb(214, 232, 255));
                    Log("Switched style: Classic (Office Aero Blue)");
                    break;
                case 3:
                    _ribbon.BuiltInStyle = RibbonBuiltInStyle.Word;
                    ApplyThemeUi(Color.FromArgb(240, 245, 252), Color.FromArgb(43, 87, 154), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(240, 245, 252));
                    Log("Switched style: Microsoft Word (Cobalt Blue)");
                    break;
                case 4:
                    _ribbon.BuiltInStyle = RibbonBuiltInStyle.Excel;
                    ApplyThemeUi(Color.FromArgb(240, 250, 244), Color.FromArgb(33, 115, 70), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(240, 250, 244));
                    Log("Switched style: Microsoft Excel (Emerald Green)");
                    break;
                case 5:
                    _ribbon.BuiltInStyle = RibbonBuiltInStyle.PowerPoint;
                    ApplyThemeUi(Color.FromArgb(254, 245, 242), Color.FromArgb(183, 71, 42), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(254, 245, 242));
                    Log("Switched style: Microsoft PowerPoint (Terracotta Orange)");
                    break;
                case 6:
                    _ribbon.BuiltInStyle = RibbonBuiltInStyle.OneNote;
                    ApplyThemeUi(Color.FromArgb(248, 242, 252), Color.FromArgb(128, 57, 123), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(248, 242, 252));
                    Log("Switched style: Microsoft OneNote (Purple)");
                    break;
                case 7:
                    // Fully customized colors via RibbonCustomStyle
                    var customRuby = new RibbonCustomStyle(new RibbonDarkStyle(), "Ruby Dark")
                    {
                        FileButtonBackColor = Color.FromArgb(190, 18, 60),      // Ruby red #be123c
                        FileButtonHoverBackColor = Color.FromArgb(225, 29, 72),
                        TabActiveTextColor = Color.FromArgb(251, 113, 133),
                        ItemCheckedBackColor = Color.FromArgb(76, 5, 25),
                        ItemCheckedBorderColor = Color.FromArgb(225, 29, 72)
                    };
                    _ribbon.Style = customRuby;
                    ApplyThemeUi(Color.FromArgb(30, 15, 25), Color.FromArgb(251, 113, 133), Color.FromArgb(15, 23, 42), Color.FromArgb(241, 245, 249), Color.FromArgb(20, 10, 18));
                    Log("Switched style: Custom Ruby Dark theme via RibbonCustomStyle");
                    break;
            }
        };

        grpThemes.Items.Add(cbStyles);

        // Group 5: Contextual Tab Trigger
        var grpContext = new RibbonGroup("Contextual", RibbonIcons.Table(24));
        var chkTable = new RibbonCheckBox("Select Table", false) { KeyTip = "T" };
        grpContext.Items.Add(chkTable);

        // Group 6: Document Actions
        var grpActions = new RibbonGroup("Document", RibbonIcons.Save(24));
        var btnNew = new RibbonButton("New", RibbonIcons.NewDocument(32), RibbonButtonSize.Large) { KeyTip = "N" };
        btnNew.Click += (s, e) => Log("Document: New clicked");

        var btnSave = new RibbonButton("Save", RibbonIcons.Save(32), RibbonButtonSize.Large) { KeyTip = "S" };
        btnSave.Click += (s, e) => Log("Document: Save clicked");

        var btnPrint = new RibbonButton("Print", RibbonIcons.Print(32), RibbonButtonSize.Large) { KeyTip = "P" };
        btnPrint.Click += (s, e) => Log("Document: Print clicked");

        grpActions.Items.Add(btnNew);
        grpActions.Items.Add(btnSave);
        grpActions.Items.Add(btnPrint);

        homeTab.Groups.Add(grpClipboard);
        homeTab.Groups.Add(grpFont);
        homeTab.Groups.Add(grpStyles);
        homeTab.Groups.Add(grpThemes);
        homeTab.Groups.Add(grpContext);
        homeTab.Groups.Add(grpActions);

        _ribbon.Tabs.Add(homeTab);

        SetupTableContextualTab(chkTable);
    }

    private void SetupInsertTab()
    {
        var insertTab = new RibbonTab("Insert") { KeyTip = "N" };

        var grpTables = new RibbonGroup("Tables", RibbonIcons.Table(24));
        var btnTable = new RibbonButton("Table", RibbonIcons.Table(32), RibbonButtonSize.Large) { KeyTip = "T" };
        btnTable.Click += (s, e) => Log("Insert: Table clicked");
        grpTables.Items.Add(btnTable);

        insertTab.Groups.Add(grpTables);
        _ribbon.Tabs.Add(insertTab);
    }

    private void SetupCustomHostTab()
    {
        var hostTab = new RibbonTab("Host Controls") { KeyTip = "C" };

        // Group 1: Hosted WinForms Controls inside Ribbon
        var grpHosted = new RibbonGroup("Hosted WinForms Controls", RibbonIcons.Table(24));

        var numUpDown = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 100,
            Value = 16,
            Width = 90
        };
        numUpDown.ValueChanged += (s, e) => Log($"Hosted NumericUpDown changed: {numUpDown.Value}");
        var hostNum = new RibbonControlHost(numUpDown, 100, 24)
        {
            ToolTipTitle = "Zoom Level",
            ToolTipDescription = "Hosted System.Windows.Forms.NumericUpDown inside Ribbon group."
        };

        var trackBar = new TrackBar
        {
            Minimum = 0,
            Maximum = 100,
            Value = 50,
            TickStyle = TickStyle.None,
            Width = 120,
            Height = 24
        };
        trackBar.ValueChanged += (s, e) => Log($"Hosted TrackBar slider changed: {trackBar.Value}%");
        var hostTrack = new RibbonControlHost(trackBar, 130, 24)
        {
            ToolTipTitle = "Opacity Slider",
            ToolTipDescription = "Hosted System.Windows.Forms.TrackBar inside Ribbon group."
        };

        var dtPicker = new DateTimePicker
        {
            Format = DateTimePickerFormat.Short,
            Width = 110
        };
        dtPicker.ValueChanged += (s, e) => Log($"Hosted DateTimePicker changed: {dtPicker.Value:d}");
        var hostDate = new RibbonControlHost(dtPicker, 120, 24)
        {
            ToolTipTitle = "Date Picker",
            ToolTipDescription = "Hosted System.Windows.Forms.DateTimePicker inside Ribbon group."
        };

        grpHosted.Items.Add(hostNum);
        grpHosted.Items.Add(hostTrack);
        grpHosted.Items.Add(hostDate);

        // Group 2: File Button Styles & Presets
        var grpFileOptions = new RibbonGroup("File Button Styles", RibbonIcons.NewDocument(24));

        var cbFileStyle = new RibbonComboBox("Style:", 130);
        cbFileStyle.Items.Add("Standard");
        cbFileStyle.Items.Add("Office Orb");
        cbFileStyle.Items.Add("Modern Pill");
        cbFileStyle.Items.Add("Icon Only");
        cbFileStyle.Items.Add("Hidden");
        cbFileStyle.SelectedIndex = 0;
        cbFileStyle.SelectedIndexChanged += (s, e) =>
        {
            _ribbon.FileButtonStyle = cbFileStyle.SelectedIndex switch
            {
                1 => RibbonFileButtonStyle.OfficeOrb,
                2 => RibbonFileButtonStyle.ModernPill,
                3 => RibbonFileButtonStyle.IconOnly,
                4 => RibbonFileButtonStyle.Hidden,
                _ => RibbonFileButtonStyle.Standard
            };
            Log($"File Button Style set to: {_ribbon.FileButtonStyle}");
        };

        var cbPreset = new RibbonComboBox("Preset:", 130);
        cbPreset.Items.Add("Document (Office)");
        cbPreset.Items.Add("Business & Data");
        cbPreset.Items.Add("Creative Media");
        cbPreset.SelectedIndex = 0;
        cbPreset.SelectedIndexChanged += (s, e) =>
        {
            var preset = cbPreset.SelectedIndex switch
            {
                1 => RibbonFileMenuPreset.BusinessData,
                2 => RibbonFileMenuPreset.MediaProject,
                _ => RibbonFileMenuPreset.StandardDocument
            };
            _ribbon.ApplyFileMenuPreset(preset);
            Log($"File Menu Preset applied: {preset}");
        };

        var chkBackstage = new RibbonCheckBox("Full Backstage View", true);
        chkBackstage.CheckedChanged += (s, e) =>
        {
            _ribbon.UseBackstage = chkBackstage.Checked;
            Log($"Backstage View mode toggled: {(_ribbon.UseBackstage ? "Office 365 Full Window" : "Dropdown Context Menu")}");
        };

        grpFileOptions.Items.Add(cbFileStyle);
        grpFileOptions.Items.Add(cbPreset);
        grpFileOptions.Items.Add(chkBackstage);

        hostTab.Groups.Add(grpHosted);
        hostTab.Groups.Add(grpFileOptions);
        _ribbon.Tabs.Add(hostTab);
    }

    private void SetupCustomCanvasTab()
    {
        var canvasTab = new RibbonTab("Custom Panel") { KeyTip = "P" };

        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(248, 250, 252)
        };

        var lbl = new Label
        {
            Text = "Arbitrary UserControl / Panel hosted directly in this RibbonTab's entire body area!",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(71, 85, 105),
            AutoSize = true,
            Location = new Point(16, 12)
        };

        var btnAction = new Button
        {
            Text = "Click Hosted Button",
            Location = new Point(16, 40),
            Size = new Size(160, 32),
            UseVisualStyleBackColor = true
        };
        btnAction.Click += (s, e) => Log("Custom Panel Button clicked inside RibbonTab!");

        var progressBar = new ProgressBar
        {
            Value = 65,
            Location = new Point(190, 44),
            Size = new Size(200, 24)
        };

        panel.Controls.Add(lbl);
        panel.Controls.Add(btnAction);
        panel.Controls.Add(progressBar);

        canvasTab.CustomControl = panel;
        _ribbon.Tabs.Add(canvasTab);
    }

    private string _lastSavedStateJson = string.Empty;

    private void SetupViewTab()
    {
        var viewTab = new RibbonTab("View") { KeyTip = "W" };

        var grpLayout = new RibbonGroup("Layout & State", RibbonIcons.NewDocument(24));

        var chkSimplified = new RibbonCheckBox("Simplified Ribbon", false);
        chkSimplified.CheckedChanged += (s, e) =>
        {
            _ribbon.LayoutMode = chkSimplified.Checked ? RibbonLayoutMode.Simplified : RibbonLayoutMode.Classic;
            Log($"Layout Mode set to: {_ribbon.LayoutMode}");
        };

        var chkQatBelow = new RibbonCheckBox("QAT Below Ribbon", false);
        chkQatBelow.CheckedChanged += (s, e) =>
        {
            _ribbon.QuickAccessPosition = chkQatBelow.Checked ? RibbonQatPosition.BelowRibbon : RibbonQatPosition.AboveRibbon;
            Log($"QAT Position set to: {_ribbon.QuickAccessPosition}");
        };

        var chkTouch = new RibbonCheckBox("Touch Mode", false);
        chkTouch.CheckedChanged += (s, e) =>
        {
            _ribbon.TouchMode = chkTouch.Checked;
            Log($"Touch Mode set to: {_ribbon.TouchMode}");
        };

        var chkAutoHide = new RibbonCheckBox("Auto-Hide", false);
        chkAutoHide.CheckedChanged += (s, e) =>
        {
            _ribbon.AutoHide = chkAutoHide.Checked;
            Log($"Auto-Hide Ribbon set to: {_ribbon.AutoHide}");
        };

        var btnCustomize = new RibbonButton("Customize...", RibbonIcons.NewDocument(16), RibbonButtonSize.Small);
        btnCustomize.Click += (s, e) =>
        {
            _ribbon.ShowCustomizeDialog(this);
            Log("Customize Ribbon dialog closed.");
        };

        var btnSaveState = new RibbonButton("Save State", RibbonIcons.Save(16), RibbonButtonSize.Small);
        btnSaveState.Click += (s, e) =>
        {
            _lastSavedStateJson = _ribbon.SaveStateJson();
            Log($"Ribbon State saved to JSON (length: {_lastSavedStateJson.Length} bytes).");
        };

        var btnLoadState = new RibbonButton("Load State", RibbonIcons.OpenFolder(16), RibbonButtonSize.Small);
        btnLoadState.Click += (s, e) =>
        {
            if (!string.IsNullOrEmpty(_lastSavedStateJson))
            {
                _ribbon.LoadStateJson(_lastSavedStateJson);
                chkSimplified.Checked = _ribbon.LayoutMode == RibbonLayoutMode.Simplified;
                chkQatBelow.Checked = _ribbon.QuickAccessPosition == RibbonQatPosition.BelowRibbon;
                chkTouch.Checked = _ribbon.TouchMode;
                chkAutoHide.Checked = _ribbon.AutoHide;
                Log("Ribbon State restored from JSON!");
            }
            else
            {
                Log("No saved state yet. Click 'Save State' first.");
            }
        };

        grpLayout.Items.Add(chkSimplified);
        grpLayout.Items.Add(chkQatBelow);
        grpLayout.Items.Add(chkTouch);
        grpLayout.Items.Add(chkAutoHide);
        grpLayout.Items.Add(btnCustomize);
        grpLayout.Items.Add(btnSaveState);
        grpLayout.Items.Add(btnLoadState);

        var grpShow = new RibbonGroup("Show", RibbonIcons.NewDocument(24));
        var chkRuler = new RibbonCheckBox("Ruler", false);
        chkRuler.CheckedChanged += (s, e) => Log($"View: Ruler -> {chkRuler.Checked}");

        var chkGrid = new RibbonCheckBox("Gridlines", true);
        chkGrid.CheckedChanged += (s, e) => Log($"View: Gridlines -> {chkGrid.Checked}");

        grpShow.Items.Add(chkRuler);
        grpShow.Items.Add(chkGrid);

        var grpZoom = new RibbonGroup("Zoom", RibbonIcons.Zoom(24));
        var btnZoom100 = new RibbonButton("Zoom", RibbonIcons.Zoom(32), RibbonButtonSize.Large) { KeyTip = "Q" };
        btnZoom100.Click += (s, e) => Log("Zoom: Zoom clicked");
        grpZoom.Items.Add(btnZoom100);

        viewTab.Groups.Add(grpLayout);
        viewTab.Groups.Add(grpShow);
        viewTab.Groups.Add(grpZoom);

        _ribbon.Tabs.Add(viewTab);
    }

    private void SetupTableContextualTab(RibbonCheckBox chkTable)
    {
        var tableDesign = new RibbonTab("Design") { KeyTip = "JD" };
        var grpDesign = new RibbonGroup("Table Styles", RibbonIcons.Table(24));
        var btnGridTable = new RibbonButton("Grid Table", RibbonIcons.Table(32), RibbonButtonSize.Large);
        btnGridTable.Click += (s, e) => Log("Table Design: Grid Table applied");
        grpDesign.Items.Add(btnGridTable);
        tableDesign.Groups.Add(grpDesign);

        var tableLayout = new RibbonTab("Layout") { KeyTip = "JL" };
        var grpLayout = new RibbonGroup("Rows & Columns", RibbonIcons.Table(24));
        var btnInsertRow = new RibbonButton("Insert Row", RibbonIcons.NewDocument(16), RibbonButtonSize.Small);
        btnInsertRow.Click += (s, e) => Log("Table Layout: Insert Row Above");
        grpLayout.Items.Add(btnInsertRow);
        tableLayout.Groups.Add(grpLayout);

        var ctxGroup = new RibbonContextualGroup("Table Tools", Color.FromArgb(245, 158, 11))
        {
            Visible = false
        };
        ctxGroup.Tabs.Add(tableDesign);
        ctxGroup.Tabs.Add(tableLayout);

        _ribbon.Tabs.Add(tableDesign);
        _ribbon.Tabs.Add(tableLayout);
        _ribbon.ContextualGroups.Add(ctxGroup);

        chkTable.CheckedChanged += (s, e) =>
        {
            ctxGroup.Visible = chkTable.Checked;
            if (chkTable.Checked)
            {
                _ribbon.SelectedTab = tableDesign;
                Log("Contextual Tab Group 'Table Tools' (Design & Layout) activated!");
            }
            else
            {
                _ribbon.SelectedTabIndex = 0;
                Log("Contextual Tab Group 'Table Tools' hidden.");
            }
        };
    }

    private void ApplyThemeUi(Color statusBg, Color statusFg, Color logBg, Color logFg, Color windowBg)
    {
        BackColor = windowBg;
        _contentContainer.BackColor = logBg;
        _statusLabel.BackColor = statusBg;
        _statusLabel.ForeColor = statusFg;
        _logBox.BackColor = logBg;
        _logBox.ForeColor = logFg;
    }

    private void Log(string msg)
    {
        _logBox.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {msg}");
    }

    private static Image CreateStyleSample(string label, Color c)
    {
        var bmp = new Bitmap(32, 24);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var brush = new SolidBrush(Color.FromArgb(241, 245, 249));
        g.FillRectangle(brush, 0, 0, 32, 24);

        using var font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        TextRenderer.DrawText(g, label, font, new Rectangle(0, 0, 32, 24), c,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        using var pen = new Pen(Color.FromArgb(203, 213, 225), 1f);
        g.DrawRectangle(pen, 0, 0, 31, 23);
        return bmp;
    }
}
