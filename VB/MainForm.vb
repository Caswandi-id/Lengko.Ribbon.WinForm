Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports Lengko.Ribbon.WinForms

Namespace Lengko.Ribbon.Demo.VB
    Public Class MainForm
        Inherits Form

        Private ReadOnly ribbon As LengkoRibbon
        Private ReadOnly logBox As ListBox
        Private ReadOnly statusLabel As Label
        Private ReadOnly contentContainer As Panel

        Public Sub New()
            Me.Text = "Lengko Ribbon Demo (VB.NET) - Responsive & Multi-Style"
            Me.Size = New Size(1150, 680)
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.MinimumSize = New Size(480, 420)

            statusLabel = New Label() With {
                .Dock = DockStyle.Bottom,
                .Height = 28,
                .Text = " Tip: Press [Alt] for KeyTips, hover for Rich ScreenTips, resize window for Auto-Collapse!",
                .TextAlign = ContentAlignment.MiddleLeft,
                .BackColor = Color.FromArgb(241, 245, 249),
                .ForeColor = Color.FromArgb(71, 85, 105),
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Italic)
            }

            logBox = New ListBox() With {
                .Dock = DockStyle.Fill,
                .Font = New Font("Consolas", 9.5F),
                .IntegralHeight = False,
                .BorderStyle = BorderStyle.None
            }

            contentContainer = New Panel() With {
                .Dock = DockStyle.Fill,
                .Padding = New Padding(12),
                .BackColor = Color.White
            }
            contentContainer.Controls.Add(logBox)

            ribbon = New LengkoRibbon() With {
                .BuiltInStyle = RibbonBuiltInStyle.Light,
                .FileButtonText = "File",
                .ShowSearchBox = True,
                .SearchPlaceholder = "Tell me what you want to do..."
            }

            AddHandler ribbon.SearchSubmitted, Sub(s, query) Log(String.Format("Search executed: '{0}'", query))

            SetupFileMenu()
            SetupQat()
            SetupHomeTab()
            SetupHostTab()
            SetupViewTab()

            Me.Controls.Add(contentContainer)
            Me.Controls.Add(statusLabel)
            Me.Controls.Add(ribbon)

            Log("Ribbon initialized (VB.NET) with real vector icons.")
        End Sub

        Private Sub SetupFileMenu()
            Dim menu As New ContextMenuStrip()
            menu.Items.Add("New Document", RibbonIcons.NewDocument(16), Sub(s, e) Log("File -> New"))
            menu.Items.Add("Open File...", RibbonIcons.OpenFolder(16), Sub(s, e) Log("File -> Open"))
            menu.Items.Add(New ToolStripSeparator())
            menu.Items.Add("Save", RibbonIcons.Save(16), Sub(s, e) Log("File -> Save"))
            menu.Items.Add(New ToolStripSeparator())
            menu.Items.Add("Exit", Nothing, Sub(s, e) Me.Close())
            ribbon.FileButtonMenu = menu
        End Sub

        Private Sub SetupQat()
            Dim qatSave As New RibbonQatItem(RibbonIcons.Save(16), "Quick Save") With {
                .ToolTipTitle = "Save (Ctrl+S)",
                .ToolTipDescription = "Save document immediately",
                .KeyTip = "1"
            }
            AddHandler qatSave.Click, Sub(s, e) Log("QAT: Save clicked")

            Dim qatUndo As New RibbonQatItem(RibbonIcons.Undo(16), "Undo") With {
                .ToolTipTitle = "Undo (Ctrl+Z)",
                .ToolTipDescription = "Undo previous action",
                .KeyTip = "2"
            }
            AddHandler qatUndo.Click, Sub(s, e) Log("QAT: Undo clicked")

            Dim qatRedo As New RibbonQatItem(RibbonIcons.Redo(16), "Redo") With {
                .ToolTipTitle = "Redo (Ctrl+Y)",
                .ToolTipDescription = "Redo action",
                .KeyTip = "3"
            }
            AddHandler qatRedo.Click, Sub(s, e) Log("QAT: Redo clicked")

            ribbon.QuickAccessItems.Add(qatSave)
            ribbon.QuickAccessItems.Add(qatUndo)
            ribbon.QuickAccessItems.Add(qatRedo)
        End Sub

        Private Sub SetupHomeTab()
            Dim homeTab As New RibbonTab("Home") With {.KeyTip = "H"}

            ' Clipboard Group
            Dim grpClip As New RibbonGroup("Clipboard", RibbonIcons.Copy(24))
            Dim btnPaste As New RibbonButton("Paste", RibbonIcons.Paste(32), RibbonButtonSize.Large) With {
                .IsSplitButton = True,
                .KeyTip = "V",
                .ToolTipTitle = "Paste (Ctrl+V)",
                .ToolTipDescription = "Paste from clipboard",
                .ToolTipShortcut = "Ctrl+V"
            }
            AddHandler btnPaste.Click, Sub(s, e) Log("Clipboard: Paste clicked")

            Dim btnCut As New RibbonButton("Cut", RibbonIcons.Cut(16), RibbonButtonSize.Small) With {
                .KeyTip = "X",
                .ToolTipTitle = "Cut (Ctrl+X)",
                .ToolTipDescription = "Cut selection to clipboard",
                .ToolTipShortcut = "Ctrl+X"
            }
            AddHandler btnCut.Click, Sub(s, e) Log("Clipboard: Cut clicked")

            Dim btnCopy As New RibbonButton("Copy", RibbonIcons.Copy(16), RibbonButtonSize.Small) With {
                .KeyTip = "C",
                .ToolTipTitle = "Copy (Ctrl+C)",
                .ToolTipDescription = "Copy selection to clipboard",
                .ToolTipShortcut = "Ctrl+C"
            }
            AddHandler btnCopy.Click, Sub(s, e) Log("Clipboard: Copy clicked")

            grpClip.Items.Add(btnPaste)
            grpClip.Items.Add(btnCut)
            grpClip.Items.Add(btnCopy)

            ' Font Group
            Dim grpFont As New RibbonGroup("Font", RibbonIcons.Bold(24)) With {
                .ShowDialogLauncher = True
            }
            AddHandler grpFont.DialogLauncherClick, Sub(s, e) Log("Font: Dialog Launcher clicked (Open Font Dialog)")

            Dim btnBold As New RibbonButton("Bold", RibbonIcons.Bold(16), RibbonButtonSize.Small) With {
                .Checkable = True,
                .KeyTip = "B",
                .ToolTipTitle = "Bold (Ctrl+B)",
                .ToolTipDescription = "Make selected text bold",
                .ToolTipShortcut = "Ctrl+B"
            }
            AddHandler btnBold.CheckedChanged, Sub(s, e) Log("Font: Bold toggled -> " & btnBold.Checked.ToString())

            Dim btnItalic As New RibbonButton("Italic", RibbonIcons.Italic(16), RibbonButtonSize.Small) With {
                .Checkable = True,
                .KeyTip = "I",
                .ToolTipTitle = "Italic (Ctrl+I)",
                .ToolTipDescription = "Italicize selected text",
                .ToolTipShortcut = "Ctrl+I"
            }
            AddHandler btnItalic.CheckedChanged, Sub(s, e) Log("Font: Italic toggled -> " & btnItalic.Checked.ToString())

            Dim btnUnderline As New RibbonButton("Underline", RibbonIcons.Underline(16), RibbonButtonSize.Small) With {
                .Checkable = True,
                .KeyTip = "U",
                .ToolTipTitle = "Underline (Ctrl+U)",
                .ToolTipDescription = "Underline selected text",
                .ToolTipShortcut = "Ctrl+U"
            }
            AddHandler btnUnderline.CheckedChanged, Sub(s, e) Log("Font: Underline toggled -> " & btnUnderline.Checked.ToString())

            grpFont.Items.Add(btnBold)
            grpFont.Items.Add(btnItalic)
            grpFont.Items.Add(btnUnderline)

            ' Quick Styles Gallery
            Dim grpStyles As New RibbonGroup("Quick Styles", RibbonIcons.NewDocument(24))
            Dim gallery As New RibbonGallery() With {.VisibleColumns = 3, .ItemWidth = 54}
            gallery.Items.Add(New RibbonGalleryItem("Normal", CreateStyleSample("Aa", Color.FromArgb(30, 41, 59))))
            gallery.Items.Add(New RibbonGalleryItem("Heading 1", CreateStyleSample("H1", Color.FromArgb(43, 87, 154))))
            gallery.Items.Add(New RibbonGalleryItem("Heading 2", CreateStyleSample("H2", Color.FromArgb(33, 115, 70))))
            gallery.Items.Add(New RibbonGalleryItem("Title", CreateStyleSample("Title", Color.FromArgb(183, 71, 42))))
            gallery.SelectedIndex = 0
            AddHandler gallery.SelectedIndexChanged, Sub(s, e)
                If gallery.SelectedItem IsNot Nothing Then
                    Log("Quick Style applied: " & gallery.SelectedItem.Text)
                End If
            End Sub
            grpStyles.Items.Add(gallery)

            ' Themes Group
            Dim grpThemes As New RibbonGroup("Themes", RibbonIcons.Save(24))
            Dim cbTheme As New RibbonComboBox("Theme:", 155)
            cbTheme.Items.Add("Light")
            cbTheme.Items.Add("Dark")
            cbTheme.Items.Add("Classic")
            cbTheme.Items.Add("Word")
            cbTheme.Items.Add("Excel")
            cbTheme.Items.Add("PowerPoint")
            cbTheme.Items.Add("OneNote")
            cbTheme.Items.Add("Custom (Ruby)")
            cbTheme.SelectedIndex = 0
            AddHandler cbTheme.SelectedIndexChanged, Sub(s, e)
                Select Case cbTheme.SelectedItem.ToString()
                    Case "Dark"
                        ribbon.BuiltInStyle = RibbonBuiltInStyle.Dark
                        ApplyThemeUi(Color.FromArgb(30, 41, 59), Color.FromArgb(203, 213, 225), Color.FromArgb(15, 23, 42), Color.FromArgb(241, 245, 249), Color.FromArgb(15, 23, 42))
                        Log("Switched to Dark theme (full dark window).")
                    Case "Classic"
                        ribbon.BuiltInStyle = RibbonBuiltInStyle.Classic
                        ApplyThemeUi(Color.FromArgb(214, 232, 255), Color.FromArgb(21, 66, 139), Color.FromArgb(245, 250, 255), Color.FromArgb(21, 66, 139), Color.FromArgb(214, 232, 255))
                        Log("Switched to Classic theme.")
                    Case "Word"
                        ribbon.BuiltInStyle = RibbonBuiltInStyle.Word
                        ApplyThemeUi(Color.FromArgb(240, 245, 252), Color.FromArgb(43, 87, 154), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(240, 245, 252))
                        Log("Switched to Microsoft Word theme.")
                    Case "Excel"
                        ribbon.BuiltInStyle = RibbonBuiltInStyle.Excel
                        ApplyThemeUi(Color.FromArgb(240, 250, 244), Color.FromArgb(33, 115, 70), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(240, 250, 244))
                        Log("Switched to Microsoft Excel theme.")
                    Case "PowerPoint"
                        ribbon.BuiltInStyle = RibbonBuiltInStyle.PowerPoint
                        ApplyThemeUi(Color.FromArgb(254, 245, 242), Color.FromArgb(183, 71, 42), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(254, 245, 242))
                        Log("Switched to Microsoft PowerPoint theme.")
                    Case "OneNote"
                        ribbon.BuiltInStyle = RibbonBuiltInStyle.OneNote
                        ApplyThemeUi(Color.FromArgb(248, 242, 252), Color.FromArgb(128, 57, 123), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(248, 242, 252))
                        Log("Switched to Microsoft OneNote theme.")
                    Case "Custom (Ruby)"
                        Dim customRuby As New RibbonCustomStyle(New RibbonDarkStyle(), "Ruby Dark")
                        customRuby.FileButtonBackColor = Color.FromArgb(190, 18, 60)
                        customRuby.FileButtonHoverBackColor = Color.FromArgb(225, 29, 72)
                        customRuby.TabActiveTextColor = Color.FromArgb(251, 113, 133)
                        customRuby.ItemCheckedBackColor = Color.FromArgb(76, 5, 25)
                        customRuby.ItemCheckedBorderColor = Color.FromArgb(225, 29, 72)
                        ribbon.Style = customRuby
                        ApplyThemeUi(Color.FromArgb(30, 15, 25), Color.FromArgb(251, 113, 133), Color.FromArgb(15, 23, 42), Color.FromArgb(241, 245, 249), Color.FromArgb(20, 10, 18))
                        Log("Switched to Custom Ruby Dark via RibbonCustomStyle.")
                    Case Else
                        ribbon.BuiltInStyle = RibbonBuiltInStyle.Light
                        ApplyThemeUi(Color.FromArgb(241, 245, 249), Color.FromArgb(71, 85, 105), Color.White, Color.FromArgb(15, 23, 42), Color.FromArgb(243, 244, 246))
                        Log("Switched to Light theme.")
                End Select
            End Sub
            grpThemes.Items.Add(cbTheme)

            ' Contextual Tool Trigger
            Dim grpContext As New RibbonGroup("Contextual", RibbonIcons.Table(24))
            Dim chkTable As New RibbonCheckBox("Select Table", False) With {.KeyTip = "T"}
            grpContext.Items.Add(chkTable)

            ' Actions Group
            Dim grpActions As New RibbonGroup("Actions", RibbonIcons.Save(24))
            Dim btnNew As New RibbonButton("New", RibbonIcons.NewDocument(32), RibbonButtonSize.Large) With {.KeyTip = "N"}
            AddHandler btnNew.Click, Sub(s, e) Log("Actions: New clicked")

            Dim btnSave As New RibbonButton("Save", RibbonIcons.Save(32), RibbonButtonSize.Large) With {.KeyTip = "S"}
            AddHandler btnSave.Click, Sub(s, e) Log("Actions: Save clicked")

            Dim btnPrint As New RibbonButton("Print", RibbonIcons.Print(32), RibbonButtonSize.Large) With {.KeyTip = "P"}
            AddHandler btnPrint.Click, Sub(s, e) Log("Actions: Print clicked")

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

            SetupTableContextualTab(chkTable)
        End Sub

        Private Sub SetupViewTab()
            Dim viewTab As New RibbonTab("View") With {.KeyTip = "W"}
            Dim grpView As New RibbonGroup("Options", RibbonIcons.Zoom(24))

            Dim chkGrid As New RibbonCheckBox("Gridlines", True)
            AddHandler chkGrid.CheckedChanged, Sub(s, e) Log("View: Gridlines -> " & chkGrid.Checked.ToString())

            Dim chkRuler As New RibbonCheckBox("Ruler", False)
            AddHandler chkRuler.CheckedChanged, Sub(s, e) Log("View: Ruler -> " & chkRuler.Checked.ToString())

            grpView.Items.Add(chkGrid)
            grpView.Items.Add(chkRuler)
            viewTab.Groups.Add(grpView)

            ribbon.Tabs.Add(viewTab)
        End Sub

        Private Sub SetupHostTab()
            Dim hostTab As New RibbonTab("Hosted Controls") With {.KeyTip = "C"}
            Dim grpHosted As New RibbonGroup("Hosted WinForms Controls", RibbonIcons.Table(24))

            Dim numUpDown As New NumericUpDown With {
                .Minimum = 1,
                .Maximum = 100,
                .Value = 16,
                .Width = 90
            }
            AddHandler numUpDown.ValueChanged, Sub(s, e) Log("Hosted NumericUpDown changed: " & numUpDown.Value.ToString())
            Dim hostNum As New RibbonControlHost(numUpDown, 100, 24) With {
                .ToolTipTitle = "Zoom Level",
                .ToolTipDescription = "Hosted System.Windows.Forms.NumericUpDown inside Ribbon group."
            }

            Dim trackBar As New TrackBar With {
                .Minimum = 0,
                .Maximum = 100,
                .Value = 50,
                .TickStyle = TickStyle.None,
                .Width = 120,
                .Height = 24
            }
            AddHandler trackBar.ValueChanged, Sub(s, e) Log("Hosted TrackBar changed: " & trackBar.Value.ToString() & "%")
            Dim hostTrack As New RibbonControlHost(trackBar, 130, 24) With {
                .ToolTipTitle = "Opacity Slider",
                .ToolTipDescription = "Hosted System.Windows.Forms.TrackBar inside Ribbon group."
            }

            grpHosted.Items.Add(hostNum)
            grpHosted.Items.Add(hostTrack)
            hostTab.Groups.Add(grpHosted)
            ribbon.Tabs.Add(hostTab)
        End Sub

        Private Sub SetupTableContextualTab(chkTable As RibbonCheckBox)
            Dim tableTab As New RibbonTab("Table Design") With {
                .ContextTitle = "Table Tools",
                .ContextColor = Color.FromArgb(245, 158, 11),
                .KeyTip = "J",
                .Visible = False
            }

            Dim grpTableStyles As New RibbonGroup("Table Styles", RibbonIcons.Table(24))
            Dim btnGridTable As New RibbonButton("Grid Table", RibbonIcons.Table(32), RibbonButtonSize.Large)
            AddHandler btnGridTable.Click, Sub(s, e) Log("Table Design: Grid Table applied")

            grpTableStyles.Items.Add(btnGridTable)
            tableTab.Groups.Add(grpTableStyles)

            ribbon.Tabs.Add(tableTab)

            AddHandler chkTable.CheckedChanged, Sub(s, e)
                tableTab.Visible = chkTable.Checked
                If chkTable.Checked Then
                    ribbon.SelectedTab = tableTab
                    Log("Contextual Tab 'Table Design' activated!")
                Else
                    ribbon.SelectedTabIndex = 0
                    Log("Contextual Tab 'Table Design' hidden.")
                End If
            End Sub
        End Sub

        Private Sub ApplyThemeUi(statusBg As Color, statusFg As Color, logBg As Color, logFg As Color, windowBg As Color)
            Me.BackColor = windowBg
            contentContainer.BackColor = logBg
            statusLabel.BackColor = statusBg
            statusLabel.ForeColor = statusFg
            logBox.BackColor = logBg
            logBox.ForeColor = logFg
        End Sub

        Private Sub Log(msg As String)
            logBox.Items.Insert(0, String.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, msg))
        End Sub

        Private Shared Function CreateStyleSample(label As String, c As Color) As Image
            Dim bmp As New Bitmap(32, 24)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.SmoothingMode = SmoothingMode.AntiAlias
                Using brush As New SolidBrush(Color.FromArgb(241, 245, 249))
                    g.FillRectangle(brush, 0, 0, 32, 24)
                End Using
                Using font As New Font("Segoe UI", 8.5F, FontStyle.Bold)
                    TextRenderer.DrawText(g, label, font, New Rectangle(0, 0, 32, 24), c,
                        TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
                End Using
                Using pen As New Pen(Color.FromArgb(203, 213, 225), 1.0F)
                    g.DrawRectangle(pen, 0, 0, 31, 23)
                End Using
            End Using
            Return bmp
        End Function
    End Class
End Namespace
