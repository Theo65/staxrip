Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.CompilerServices

''' <summary>
''' Prompts the user to select a folder.
''' </summary>
''' <remarks>
''' This class will use the Vista style Select Folder dialog if possible, or the regular FolderBrowserDialog
''' if it is not. Note that the Vista style dialog is very different, so using this class without testing
''' in both Vista and older Windows versions is not recommended.
''' </remarks>
<System.Drawing.ToolboxBitmap(GetType(System.Windows.Forms.FolderBrowserDialog), "FolderBrowserDialog.bmp"), DefaultEvent("HelpRequest"), Designer("System.Windows.Forms.Design.FolderBrowserDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), DefaultProperty("SelectedPath"), Description("Prompts the user to select a folder.")> _
Public Class FolderBrowserDialog
    Inherits CommonDialog
    Private _downlevelDialog As System.Windows.Forms.FolderBrowserDialog
    Private _description As String
    Private _useDescriptionForTitle As Boolean
    Private _selectedPath As String
    Private _rootFolder As System.Environment.SpecialFolder

    ''' <summary>
    ''' Occurs when the user clicks the Help button on the dialog box.
    ''' </summary>
    '<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    'Public Shadows Custom Event HelpRequest As EventHandler
    '	AddHandler(value As EventHandler)
    '		MyBase.HelpRequest += value
    '	End AddHandler
    '	RemoveHandler(value As EventHandler)
    '		MyBase.HelpRequest -= value
    '	End RemoveHandler
    'End Event

    ''' <summary>
    ''' Creates a new instance of the <see cref="FolderBrowserDialog" /> class.
    ''' </summary>
    Public Sub New()
        Me.New(False)
    End Sub

    ''' <summary>
    ''' Creates a new instance of the <see cref="FolderBrowserDialog" /> class.
    ''' </summary>
    ''' <param name="forceDownlevel">true to force the use of the old style folder browser dialog, even when the Vista
    ''' style dialog is supported; otherwise, false.</param>
    Public Sub New(forceDownlevel As Boolean)
        If forceDownlevel OrElse Not IsVistaFolderDialogSupported Then
            _downlevelDialog = New System.Windows.Forms.FolderBrowserDialog()
        Else
            Reset()
        End If
    End Sub

#Region "Public Properties"

    ''' <summary>
    ''' Gets a value that indicates whether the current OS supports Vista-style common userToken dialogs.
    ''' </summary>
    ''' <value>
    ''' Returns true on Windows Vista or newer operating systems; otherwise, false.
    ''' </value>
    <Browsable(False)> _
    Public Shared ReadOnly Property IsVistaFolderDialogSupported() As Boolean
        Get
            Return Environment.OSVersion.Platform = PlatformID.Win32NT AndAlso Environment.OSVersion.Version.Major >= 6
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the descriptive text displayed above the tree view control in the dialog box, or below the list view control
    ''' in the Vista style dialog.
    ''' </summary>
    ''' <value>
    ''' The description to display. The default is an empty string ("").
    ''' </value>
    <Category("Folder Browsing"), DefaultValue(""), Localizable(True), Browsable(True), Description("The descriptive text displayed above the tree view control in the dialog box, or below the list view control in the Vista style dialog.")> _
    Public Property Description() As String
        Get
            If _downlevelDialog IsNot Nothing Then
                Return _downlevelDialog.Description
            End If
            Return _description
        End Get
        Set(value As String)
            If _downlevelDialog IsNot Nothing Then
                _downlevelDialog.Description = value
            Else
                _description = If(value, [String].Empty)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the root folder where the browsing starts from. This property has no effect if the Vista style
    ''' dialog is used.
    ''' </summary>
    ''' <value>
    ''' One of the <see cref="System.Environment.SpecialFolder" /> values. The default is Desktop.
    ''' </value>
    ''' <exception cref="System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="System.Environment.SpecialFolder" /> values.</exception>
    <Localizable(False), Description("The root folder where the browsing starts from. This property has no effect if the Vista style dialog is used."), Category("Folder Browsing"), Browsable(True), DefaultValue(GetType(System.Environment.SpecialFolder), "Desktop")> _
    Public Property RootFolder() As System.Environment.SpecialFolder
        Get
            If _downlevelDialog IsNot Nothing Then
                Return _downlevelDialog.RootFolder
            End If
            Return _rootFolder
        End Get
        Set(value As System.Environment.SpecialFolder)
            If _downlevelDialog IsNot Nothing Then
                _downlevelDialog.RootFolder = value
            Else
                _rootFolder = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the path selected by the user.
    ''' </summary>
    ''' <value>
    ''' The path of the folder first selected in the dialog box or the last folder selected by the user. The default is an empty string ("").
    ''' </value>
    <Browsable(True), Editor("System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(System.Drawing.Design.UITypeEditor)), Description("The path selected by the user."), DefaultValue(""), Localizable(True), Category("Folder Browsing")> _
    Public Property SelectedPath() As String
        Get
            If _downlevelDialog IsNot Nothing Then
                Return _downlevelDialog.SelectedPath
            End If
            Return _selectedPath
        End Get
        Set(value As String)
            If _downlevelDialog IsNot Nothing Then
                _downlevelDialog.SelectedPath = value
            Else
                _selectedPath = If(value, String.Empty)
            End If
        End Set
    End Property

    Private _showNewFolderButton As Boolean

    ''' <summary>
    ''' Gets or sets a value indicating whether the New Folder button appears in the folder browser dialog box. This
    ''' property has no effect if the Vista style dialog is used; in that case, the New Folder button is always shown.
    ''' </summary>
    ''' <value>
    ''' true if the New Folder button is shown in the dialog box; otherwise, false. The default is true.
    ''' </value>
    <Browsable(True), Localizable(False), Description("A value indicating whether the New Folder button appears in the folder browser dialog box. This property has no effect if the Vista style dialog is used; in that case, the New Folder button is always shown."), DefaultValue(True), Category("Folder Browsing")> _
    Public Property ShowNewFolderButton() As Boolean
        Get
            If _downlevelDialog IsNot Nothing Then
                Return _downlevelDialog.ShowNewFolderButton
            End If
            Return _showNewFolderButton
        End Get
        Set(value As Boolean)
            If _downlevelDialog IsNot Nothing Then
                _downlevelDialog.ShowNewFolderButton = value
            Else
                _showNewFolderButton = value
            End If
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets a value that indicates whether to use the value of the <see cref="Description" /> property
    ''' as the dialog title for Vista style dialogs. This property has no effect on old style dialogs.
    ''' </summary>
    ''' <value>true to indicate that the value of the <see cref="Description" /> property is used as dialog title; false
    ''' to indicate the value is added as additional text to the dialog. The default is false.</value>
    <Category("Folder Browsing"), DefaultValue(False), Description("A value that indicates whether to use the value of the Description property as the dialog title for Vista style dialogs. This property has no effect on old style dialogs.")> _
    Public Property UseDescriptionForTitle() As Boolean
        Get
            Return _useDescriptionForTitle
        End Get
        Set(value As Boolean)
            _useDescriptionForTitle = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Resets all properties to their default values.
    ''' </summary>
    Public Overrides Sub Reset()
        _description = String.Empty
        _useDescriptionForTitle = False
        _selectedPath = String.Empty
        _rootFolder = Environment.SpecialFolder.Desktop
        _showNewFolderButton = True
    End Sub

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' Specifies a common dialog box.
    ''' </summary>
    ''' <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
    ''' <returns>true if the userToken could be opened; otherwise, false.</returns>
    Protected Overrides Function RunDialog(hwndOwner As IntPtr) As Boolean
        If _downlevelDialog IsNot Nothing Then
            Return _downlevelDialog.ShowDialog(If(hwndOwner = IntPtr.Zero, Nothing, New WindowHandleWrapper(hwndOwner))) = DialogResult.OK
        End If

        Dim dialog As IFileDialog = Nothing

        Try
            dialog = CType(New FileOpenDialogRCW(), IFileDialog)
            SetDialogProperties(dialog)
            Dim result As Integer = dialog.Show(hwndOwner)
            If result < 0 Then
                If result = NativeMethods.ERROR_CANCELLED Then
                    Return False
                Else
                    Throw System.Runtime.InteropServices.Marshal.GetExceptionForHR(result)
                End If
            End If
            GetResult(dialog)
            Return True
        Finally
            If dialog IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(dialog)
            End If
        End Try
    End Function

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="FileDialog" /> and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso _downlevelDialog IsNot Nothing Then
                _downlevelDialog.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region "Private Methods"

    Private Sub SetDialogProperties(dialog As IFileDialog)
        ' Description
        If Not String.IsNullOrEmpty(_description) Then
            If _useDescriptionForTitle Then
                dialog.SetTitle(_description)
            Else
                Dim customize As IFileDialogCustomize = DirectCast(dialog, IFileDialogCustomize)
                customize.AddText(0, _description)
            End If
        End If

        dialog.SetOptions(NativeMethods.FOS.FOS_PICKFOLDERS Or NativeMethods.FOS.FOS_FORCEFILESYSTEM Or NativeMethods.FOS.FOS_FILEMUSTEXIST)

        If Not String.IsNullOrEmpty(_selectedPath) Then
            Dim parent As String = Path.GetDirectoryName(_selectedPath)
            If parent Is Nothing OrElse Not Directory.Exists(parent) Then
                dialog.SetFileName(_selectedPath)
            Else
                Dim folder As String = Path.GetFileName(_selectedPath)
                dialog.SetFolder(NativeMethods.CreateItemFromParsingName(parent))
                dialog.SetFileName(folder)
            End If
        End If
    End Sub

    Private Sub GetResult(dialog As IFileDialog)
        Dim item As IShellItem = Nothing
        dialog.GetResult(item)
        item.GetDisplayName(NativeMethods.SIGDN.SIGDN_FILESYSPATH, _selectedPath)
    End Sub

#End Region

#Region "Interfaces"
    <ComImport(), Guid(IIDGuid.IModalWindow), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IModalWindow

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig()> _
        Function Show(<[In]()> parent As IntPtr) As Integer
    End Interface

    <ComImport(), Guid(IIDGuid.IFileDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IFileDialog
        Inherits IModalWindow
        ' Defined on IModalWindow - repeated here due to requirements of COM interop layer
        ' --------------------------------------------------------------------------------
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig()> _
        Overloads Function Show(<[In]()> parent As IntPtr) As Integer

        ' IFileDialog-Specific interface members
        ' --------------------------------------------------------------------------------
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetFileTypes(<[In]()> cFileTypes As UInteger, <[In](), MarshalAs(UnmanagedType.LPArray)> rgFilterSpec As NativeMethods.COMDLG_FILTERSPEC())

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetFileTypeIndex(<[In]()> iFileType As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetFileTypeIndex(piFileType As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub Advise(<[In](), MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, pdwCookie As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub Unadvise(<[In]()> dwCookie As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetOptions(<[In]()> fos As NativeMethods.FOS)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetOptions(pfos As NativeMethods.FOS)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetDefaultFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetFileName(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetTitle(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszTitle As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetOkButtonLabel(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszText As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetFileNameLabel(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddPlace(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As NativeMethods.FDAP)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetDefaultExtension(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszDefaultExtension As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetClientGuid(<[In]()> ByRef guid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub ClearClientData()

        ' Not supported:  IShellItemFilter is not defined, converting to IntPtr
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileOpenDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IFileOpenDialog
        Inherits IFileDialog
        ' Defined on IModalWindow - repeated here due to requirements of COM interop layer
        ' --------------------------------------------------------------------------------
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig()> _
        Overloads Function Show(<[In]()> parent As IntPtr) As Integer

        ' Defined on IFileDialog - repeated here due to requirements of COM interop layer
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetFileTypes(<[In]()> cFileTypes As UInteger, <[In]()> ByRef rgFilterSpec As NativeMethods.COMDLG_FILTERSPEC)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetFileTypeIndex(<[In]()> iFileType As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub GetFileTypeIndex(piFileType As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub Advise(<[In](), MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, pdwCookie As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub Unadvise(<[In]()> dwCookie As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetOptions(<[In]()> fos As NativeMethods.FOS)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub GetOptions(pfos As NativeMethods.FOS)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetDefaultFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetFileName(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetTitle(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszTitle As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetOkButtonLabel(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszText As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetFileNameLabel(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub AddPlace(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As NativeMethods.FDAP)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetDefaultExtension(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszDefaultExtension As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetClientGuid(<[In]()> ByRef guid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub ClearClientData()

        ' Not supported:  IShellItemFilter is not defined, converting to IntPtr
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Overloads Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)

        ' Defined by IFileOpenDialog
        ' ---------------------------------------------------------------------------------
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetResults(<MarshalAs(UnmanagedType.[Interface])> ppenum As IShellItemArray)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetSelectedItems(<MarshalAs(UnmanagedType.[Interface])> ppsai As IShellItemArray)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileSaveDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IFileSaveDialog
        Inherits IFileDialog

        ' Defined on IModalWindow - repeated here due to requirements of COM interop layer
        ' --------------------------------------------------------------------------------

        <PreserveSig()> Overloads Function Show(<[In]()> parent As IntPtr) As Integer

        ' Defined on IFileDialog - repeated here due to requirements of COM interop layer
        Overloads Sub SetFileTypes(<[In]()> cFileTypes As UInteger, <[In]()> ByRef rgFilterSpec As NativeMethods.COMDLG_FILTERSPEC)
        Overloads Sub SetFileTypeIndex(<[In]()> iFileType As UInteger)
        Overloads Sub GetFileTypeIndex(piFileType As UInteger)
        Overloads Sub Advise(<[In](), MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, pdwCookie As UInteger)
        Overloads Sub Unadvise(<[In]()> dwCookie As UInteger)
        Overloads Sub SetOptions(<[In]()> fos As NativeMethods.FOS)
        Overloads Sub GetOptions(pfos As NativeMethods.FOS)
        Overloads Sub SetDefaultFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Overloads Sub SetFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Overloads Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub SetFileName(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszName As String)
        Overloads Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)
        Overloads Sub SetTitle(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszTitle As String)
        Overloads Sub SetOkButtonLabel(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszText As String)
        Overloads Sub SetFileNameLabel(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)
        Overloads Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub AddPlace(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As NativeMethods.FDAP)
        Overloads Sub SetDefaultExtension(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszDefaultExtension As String)
        Overloads Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)
        Overloads Sub SetClientGuid(<[In]()> ByRef guid As Guid)
        Overloads Sub ClearClientData()
        'Not supported:  IShellItemFilter is not defined, converting to IntPtr
        Overloads Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)

        ' Defined by IFileSaveDialog interface
        ' -----------------------------------------------------------------------------------

        Sub SetSaveAsItem(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        ' Not currently supported: IPropertyStore
        Sub SetProperties(<[In](), MarshalAs(UnmanagedType.[Interface])> pStore As IntPtr)
        ' Not currently supported: IPropertyDescriptionList
        Sub SetCollectedProperties(<[In](), MarshalAs(UnmanagedType.[Interface])> pList As IntPtr, <[In]()> fAppendDefault As Integer)
        ' Not currently supported: IPropertyStore
        Sub GetProperties(<MarshalAs(UnmanagedType.[Interface])> ppStore As IntPtr)
        ' Not currently supported: IPropertyStore, IFileOperationProgressSink
        Sub ApplyProperties(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In](), MarshalAs(UnmanagedType.[Interface])> pStore As IntPtr, <[In](), ComAliasName("Interop.wireHWND")> ByRef hwnd As IntPtr, <[In](), MarshalAs(UnmanagedType.[Interface])> pSink As IntPtr)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileDialogEvents), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IFileDialogEvents
        ' NOTE: some of these callbacks are cancelable - returning S_FALSE means that 
        ' the dialog should not proceed (e.g. with closing, changing folder); to 
        ' support this, we need to use the PreserveSig attribute to enable us to return
        ' the proper HRESULT
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig()> _
        Function OnFileOk(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog) As HRESULT

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig()> _
        Function OnFolderChanging(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In](), MarshalAs(UnmanagedType.[Interface])> psiFolder As IShellItem) As HRESULT

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnFolderChange(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnSelectionChange(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnShareViolation(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, pResponse As NativeMethods.FDE_SHAREVIOLATION_RESPONSE)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnTypeChange(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnOverwrite(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, pResponse As NativeMethods.FDE_OVERWRITE_RESPONSE)
    End Interface

    <ComImport(), Guid(IIDGuid.IShellItem), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IShellItem
        ' Not supported: IBindCtx
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub BindToHandler(<[In](), MarshalAs(UnmanagedType.[Interface])> pbc As IntPtr, <[In]()> ByRef bhid As Guid, <[In]()> ByRef riid As Guid, ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetParent(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetDisplayName(<[In]()> sigdnName As NativeMethods.SIGDN,
                       <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetAttributes(<[In]()> sfgaoMask As UInteger, psfgaoAttribs As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub Compare(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In]()> hint As UInteger, piOrder As Integer)
    End Interface

    <ComImport(), Guid(IIDGuid.IShellItemArray), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IShellItemArray
        ' Not supported: IBindCtx
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub BindToHandler(<[In](), MarshalAs(UnmanagedType.[Interface])> pbc As IntPtr, <[In]()> ByRef rbhid As Guid, <[In]()> ByRef riid As Guid, ppvOut As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetPropertyStore(<[In]()> Flags As Integer, <[In]()> ByRef riid As Guid, ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetPropertyDescriptionList(<[In]()> ByRef keyType As NativeMethods.PROPERTYKEY, <[In]()> ByRef riid As Guid, ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetAttributes(<[In]()> dwAttribFlags As NativeMethods.SIATTRIBFLAGS, <[In]()> sfgaoMask As UInteger, psfgaoAttribs As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetCount(pdwNumItems As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetItemAt(<[In]()> dwIndex As UInteger, <MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)

        ' Not supported: IEnumShellItems (will use GetCount and GetItemAt instead)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub EnumItems(<MarshalAs(UnmanagedType.[Interface])> ppenumShellItems As IntPtr)
    End Interface

    <ComImport(), Guid(IIDGuid.IKnownFolder), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IKnownFolder
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetId(pkfid As Guid)

        ' Not yet supported - adding to fill slot in vtable
        Sub spacer1()
        '[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        'void GetCategory(out mbtagKF_CATEGORY pCategory);

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetShellItem(<[In]()> dwFlags As UInteger, ByRef riid As Guid, ppv As IShellItem)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetPath(<[In]()> dwFlags As UInteger, <MarshalAs(UnmanagedType.LPWStr)> ppszPath As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetPath(<[In]()> dwFlags As UInteger, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszPath As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetLocation(<[In]()> dwFlags As UInteger, <Out(), ComAliasName("Interop.wirePIDL")> ppidl As IntPtr)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetFolderType(pftid As Guid)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetRedirectionCapabilities(pCapabilities As UInteger)

        ' Not yet supported - adding to fill slot in vtable
        Sub spacer2()
        '[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        'void GetFolderDefinition(out tagKNOWNFOLDER_DEFINITION pKFD);
    End Interface


    <ComImport(), Guid(IIDGuid.IKnownFolderManager), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IKnownFolderManager
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub FolderIdFromCsidl(<[In]()> nCsidl As Integer, pfid As Guid)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub FolderIdToCsidl(<[In]()> ByRef rfid As Guid, pnCsidl As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetFolderIds(<Out()> ppKFId As IntPtr, <[In](), Out()> ByRef pCount As UInteger)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetFolder(<[In]()> ByRef rfid As Guid, <MarshalAs(UnmanagedType.[Interface])> ppkf As IKnownFolder)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetFolderByName(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszCanonicalName As String, <MarshalAs(UnmanagedType.[Interface])> ppkf As IKnownFolder)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub RegisterFolder(<[In]()> ByRef rfid As Guid, <[In]()> ByRef pKFD As NativeMethods.KNOWNFOLDER_DEFINITION)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub UnregisterFolder(<[In]()> ByRef rfid As Guid)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub FindFolderFromPath(<[In](), MarshalAs(UnmanagedType.LPWStr)> pszPath As String, <[In]()> mode As NativeMethods.FFFP_MODE, <MarshalAs(UnmanagedType.[Interface])> ppkf As IKnownFolder)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub FindFolderFromIDList(<[In]()> pidl As IntPtr, <MarshalAs(UnmanagedType.[Interface])> ppkf As IKnownFolder)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub Redirect(<[In]()> ByRef rfid As Guid, <[In]()> hwnd As IntPtr, <[In]()> Flags As UInteger, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszTargetPath As String, <[In]()> cFolders As UInteger, <[In]()> ByRef pExclusion As Guid, _
   <MarshalAs(UnmanagedType.LPWStr)> ppszError As String)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileDialogCustomize), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IFileDialogCustomize
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub EnableOpenDropDown(<[In]()> dwIDCtl As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddMenu(<[In]()> dwIDCtl As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddPushButton(<[In]()> dwIDCtl As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddComboBox(<[In]()> dwIDCtl As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddRadioButtonList(<[In]()> dwIDCtl As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddCheckButton(<[In]()> dwIDCtl As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String, <[In]()> bChecked As Boolean)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddEditBox(<[In]()> dwIDCtl As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszText As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddSeparator(<[In]()> dwIDCtl As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddText(<[In]()> dwIDCtl As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszText As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetControlLabel(<[In]()> dwIDCtl As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetControlState(<[In]()> dwIDCtl As Integer, <Out()> pdwState As NativeMethods.CDCONTROLSTATE)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetControlState(<[In]()> dwIDCtl As Integer, <[In]()> dwState As NativeMethods.CDCONTROLSTATE)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetEditBoxText(<[In]()> dwIDCtl As Integer, <Out()> ppszText As IntPtr)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetEditBoxText(<[In]()> dwIDCtl As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszText As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetCheckButtonState(<[In]()> dwIDCtl As Integer, <Out()> pbChecked As Boolean)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetCheckButtonState(<[In]()> dwIDCtl As Integer, <[In]()> bChecked As Boolean)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub AddControlItem(<[In]()> dwIDCtl As Integer, <[In]()> dwIDItem As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub RemoveControlItem(<[In]()> dwIDCtl As Integer, <[In]()> dwIDItem As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub RemoveAllControlItems(<[In]()> dwIDCtl As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetControlItemState(<[In]()> dwIDCtl As Integer, <[In]()> dwIDItem As Integer, <Out()> pdwState As NativeMethods.CDCONTROLSTATE)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetControlItemState(<[In]()> dwIDCtl As Integer, <[In]()> dwIDItem As Integer, <[In]()> dwState As NativeMethods.CDCONTROLSTATE)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetSelectedControlItem(<[In]()> dwIDCtl As Integer, <Out()> pdwIDItem As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetSelectedControlItem(<[In]()> dwIDCtl As Integer, <[In]()> dwIDItem As Integer)
        ' Not valid for OpenDropDown
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub StartVisualGroup(<[In]()> dwIDCtl As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub EndVisualGroup()
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub MakeProminent(<[In]()> dwIDCtl As Integer)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileDialogControlEvents), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IFileDialogControlEvents

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnItemSelected(<[In](), MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, <[In]()> dwIDCtl As Integer, <[In]()> dwIDItem As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnButtonClicked(<[In](), MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, <[In]()> dwIDCtl As Integer)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnCheckButtonToggled(<[In](), MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, <[In]()> dwIDCtl As Integer, <[In]()> bChecked As Boolean)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub OnControlActivating(<[In](), MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, <[In]()> dwIDCtl As Integer)
    End Interface

    <ComImport(), Guid(IIDGuid.IPropertyStore), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Friend Interface IPropertyStore
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetCount(<Out()> cProps As UInteger)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetAt(<[In]()> iProp As UInteger, pkey As NativeMethods.PROPERTYKEY)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub GetValue(<[In]()> ByRef key As NativeMethods.PROPERTYKEY, pv As Object)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub SetValue(<[In]()> ByRef key As NativeMethods.PROPERTYKEY, <[In]()> ByRef pv As Object)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        Sub Commit()
    End Interface

    ' Dummy base interface for CommonFileDialog coclasses
    Friend Interface NativeCommonFileDialog
    End Interface

    ' ---------------------------------------------------------
    ' Coclass interfaces - designed to "look like" the object 
    ' in the API, so that the 'new' operator can be used in a 
    ' straightforward way. Behind the scenes, the C# compiler
    ' morphs all 'new CoClass()' calls to 'new CoClassWrapper()'
    <ComImport(), Guid(IIDGuid.IFileOpenDialog), CoClass(GetType(FileOpenDialogRCW))>
    Friend Interface NativeFileOpenDialog
        Inherits IFileOpenDialog
    End Interface

    <ComImport(), Guid(IIDGuid.IFileSaveDialog), CoClass(GetType(FileSaveDialogRCW))>
    Friend Interface NativeFileSaveDialog
        Inherits IFileSaveDialog
    End Interface

    <ComImport(), Guid(IIDGuid.IKnownFolderManager), CoClass(GetType(KnownFolderManagerRCW))>
    Friend Interface KnownFolderManager
        Inherits IKnownFolderManager
    End Interface

    ' ---------------------------------------------------
    ' .NET classes representing runtime callable wrappers
    <ComImport(),
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(CLSIDGuid.FileOpenDialog)>
    Friend Class FileOpenDialogRCW
    End Class

    <ComImport(),
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(CLSIDGuid.FileSaveDialog)>
    Friend Class FileSaveDialogRCW
    End Class

    <ComImport(),
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(CLSIDGuid.KnownFolderManager)>
    Friend Class KnownFolderManagerRCW
    End Class
#End Region

    Friend Class WindowHandleWrapper
        Implements IWin32Window

        Private _handle As IntPtr

        Public Sub New(handle As IntPtr)
            _handle = handle
        End Sub

#Region "IWin32Window Members"

        Public ReadOnly Property Handle() As IntPtr Implements IWin32Window.Handle
            Get
                Return _handle
            End Get
        End Property

#End Region

    End Class

    Friend Class SafeModuleHandle
        Inherits System.Runtime.InteropServices.SafeHandle
        Public Sub New()
            MyBase.New(IntPtr.Zero, True)
        End Sub

        Public Overrides ReadOnly Property IsInvalid() As Boolean
            Get
                Return handle = IntPtr.Zero
            End Get
        End Property

        Protected Overrides Function ReleaseHandle() As Boolean
            Return NativeMethods.FreeLibrary(handle)
        End Function
    End Class

    Friend NotInheritable Class NativeMethods
        Private Sub New()
        End Sub
        ''' <value>
        ''' Returns true on Windows Vista or newer operating systems; otherwise, false.
        ''' </value>
        <Browsable(False)> _
        Public Shared ReadOnly Property IsVistaOrLater() As Boolean
            Get
                Return Environment.OSVersion.Platform = PlatformID.Win32NT AndAlso Environment.OSVersion.Version.Major >= 6
            End Get
        End Property

        Public Const BS_COMMANDLINK As Integer = &HE
        Public Const BCM_SETNOTE As Integer = &H1609
        Public Const BCM_SETSHIELD As Integer = &H160C

        Public Const TV_FIRST As Integer = &H1100
        Public Const TVM_SETEXTENDEDSTYLE As Integer = TV_FIRST + 44
        Public Const TVM_GETEXTENDEDSTYLE As Integer = TV_FIRST + 45
        Public Const TVM_SETAUTOSCROLLINFO As Integer = TV_FIRST + 59
        Public Const TVS_NOHSCROLL As Integer = &H8000
        Public Const TVS_EX_AUTOHSCROLL As Integer = &H20
        Public Const TVS_EX_FADEINOUTEXPANDOS As Integer = &H40
        Public Const GWL_STYLE As Integer = -16

        <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
        Friend Shared Function SendMessage(hWnd As IntPtr, Msg As UInt32, wParam As Integer, lParam As Integer) As Integer
        End Function

        <DllImport("user32", CharSet:=CharSet.Unicode)>
        Friend Shared Function SendMessage(hWnd As IntPtr, Msg As UInt32, wParam As IntPtr, lParam As String) As IntPtr
        End Function

        <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)>
        Public Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
        End Function

#Region "General Definitions"


        ' Various important window messages
        Friend Const WM_USER As Integer = &H400
        Friend Const WM_ENTERIDLE As Integer = &H121

        ' FormatMessage constants and structs
        Friend Const FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000

#End Region

#Region "File Operations Definitions"

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)> _
        Friend Structure COMDLG_FILTERSPEC
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszName As String
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszSpec As String
        End Structure

        Friend Enum FDAP
            FDAP_BOTTOM = &H0
            FDAP_TOP = &H1
        End Enum

        Friend Enum FDE_SHAREVIOLATION_RESPONSE
            FDESVR_DEFAULT = &H0
            FDESVR_ACCEPT = &H1
            FDESVR_REFUSE = &H2
        End Enum

        Friend Enum FDE_OVERWRITE_RESPONSE
            FDEOR_DEFAULT = &H0
            FDEOR_ACCEPT = &H1
            FDEOR_REFUSE = &H2
        End Enum

        Friend Enum SIATTRIBFLAGS
            SIATTRIBFLAGS_AND = &H1
            ' if multiple items and the attirbutes together.
            SIATTRIBFLAGS_OR = &H2
            ' if multiple items or the attributes together.
            SIATTRIBFLAGS_APPCOMPAT = &H3
            ' Call GetAttributes directly on the ShellFolder for multiple attributes
        End Enum

        Friend Enum SIGDN As UInteger
            SIGDN_NORMALDISPLAY = &H0
            ' SHGDN_NORMAL
            SIGDN_PARENTRELATIVEPARSING = &H80018001UI
            ' SHGDN_INFOLDER | SHGDN_FORPARSING
            SIGDN_DESKTOPABSOLUTEPARSING = &H80028000UI
            ' SHGDN_FORPARSING
            SIGDN_PARENTRELATIVEEDITING = &H80031001UI
            ' SHGDN_INFOLDER | SHGDN_FOREDITING
            SIGDN_DESKTOPABSOLUTEEDITING = &H8004C000UI
            ' SHGDN_FORPARSING | SHGDN_FORADDRESSBAR
            SIGDN_FILESYSPATH = &H80058000UI
            ' SHGDN_FORPARSING
            SIGDN_URL = &H80068000UI
            ' SHGDN_FORPARSING
            SIGDN_PARENTRELATIVEFORADDRESSBAR = &H8007C001UI
            ' SHGDN_INFOLDER | SHGDN_FORPARSING | SHGDN_FORADDRESSBAR
            SIGDN_PARENTRELATIVE = &H80080001UI
            ' SHGDN_INFOLDER
        End Enum

        <Flags()> _
        Friend Enum FOS As UInteger
            FOS_OVERWRITEPROMPT = &H2
            FOS_STRICTFILETYPES = &H4
            FOS_NOCHANGEDIR = &H8
            FOS_PICKFOLDERS = &H20
            FOS_FORCEFILESYSTEM = &H40
            ' Ensure that items returned are filesystem items.
            FOS_ALLNONSTORAGEITEMS = &H80
            ' Allow choosing items that have no storage.
            FOS_NOVALIDATE = &H100
            FOS_ALLOWMULTISELECT = &H200
            FOS_PATHMUSTEXIST = &H800
            FOS_FILEMUSTEXIST = &H1000
            FOS_CREATEPROMPT = &H2000
            FOS_SHAREAWARE = &H4000
            FOS_NOREADONLYRETURN = &H8000
            FOS_NOTESTFILECREATE = &H10000
            FOS_HIDEMRUPLACES = &H20000
            FOS_HIDEPINNEDPLACES = &H40000
            FOS_NODEREFERENCELINKS = &H100000
            FOS_DONTADDTORECENT = &H2000000
            FOS_FORCESHOWHIDDEN = &H10000000
            FOS_DEFAULTNOMINIMODE = &H20000000
        End Enum

        Friend Enum CDCONTROLSTATE
            CDCS_INACTIVE = &H0
            CDCS_ENABLED = &H1
            CDCS_VISIBLE = &H2
        End Enum

#End Region

#Region "KnownFolder Definitions"

        Friend Enum FFFP_MODE
            FFFP_EXACTMATCH
            FFFP_NEARESTPARENTMATCH
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)> _
        Friend Structure KNOWNFOLDER_DEFINITION
            Friend category As NativeMethods.KF_CATEGORY
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszName As String
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszCreator As String
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszDescription As String
            Friend fidParent As Guid
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszRelativePath As String
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszParsingName As String
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszToolTip As String
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszLocalizedName As String
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszIcon As String
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszSecurity As String
            Friend dwAttributes As UInteger
            Friend kfdFlags As NativeMethods.KF_DEFINITION_FLAGS
            Friend ftidType As Guid
        End Structure

        Friend Enum KF_CATEGORY
            KF_CATEGORY_VIRTUAL = &H1
            KF_CATEGORY_FIXED = &H2
            KF_CATEGORY_COMMON = &H3
            KF_CATEGORY_PERUSER = &H4
        End Enum

        <Flags()> _
        Friend Enum KF_DEFINITION_FLAGS
            KFDF_PERSONALIZE = &H1
            KFDF_LOCAL_REDIRECT_ONLY = &H2
            KFDF_ROAMABLE = &H4
        End Enum


        ' Property System structs and consts
        <StructLayout(LayoutKind.Sequential, Pack:=4)> _
        Friend Structure PROPERTYKEY
            Friend fmtid As Guid
            Friend pid As UInteger
        End Structure

#End Region

        '800704C7

        Public Const ERROR_CANCELLED As Integer = &H800704C7 'UI

        <Flags()> _
        Public Enum FormatMessageFlags
            FORMAT_MESSAGE_ALLOCATE_BUFFER = &H100
            FORMAT_MESSAGE_IGNORE_INSERTS = &H200
            FORMAT_MESSAGE_FROM_STRING = &H400
            FORMAT_MESSAGE_FROM_HMODULE = &H800
            FORMAT_MESSAGE_FROM_SYSTEM = &H1000
            FORMAT_MESSAGE_ARGUMENT_ARRAY = &H2000
        End Enum

        'HRESULT SHCreateItemFromParsingName(          PCWSTR pszPath,
        '    IBindCtx *pbc,
        '    REFIID riid,
        '    void **ppv
        ');

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)> _
        Public Shared Function LoadLibrary(name As String) As SafeModuleHandle
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Shared Function FreeLibrary(hModule As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Shared Function LoadString(hInstance As SafeModuleHandle, uID As UInteger, lpBuffer As StringBuilder, nBufferMax As Integer) As Integer
        End Function

        <DllImport("Kernel32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Shared Function FormatMessage(<MarshalAs(UnmanagedType.U4)> dwFlags As FormatMessageFlags, lpSource As IntPtr, dwMessageId As UInteger, dwLanguageId As UInteger, ByRef lpBuffer As IntPtr, nSize As UInteger, Arguments As String()) As UInteger
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
        Public Shared Function SHCreateItemFromParsingName(pszPath As String,
                                                           pbc As IntPtr,
                                                           ByRef riid As Guid,
                                                           <MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As Object) As Integer
        End Function

        Public Shared Function CreateItemFromParsingName(path As String) As IShellItem
            Dim item As Object = Nothing
            Dim guid As New Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")
            ' IID_IShellItem
            Dim hr As Integer = NativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, guid, item)
            If hr <> 0 Then
                Throw New System.ComponentModel.Win32Exception(hr)
            End If
            Return DirectCast(item, IShellItem)
        End Function
    End Class

    Friend NotInheritable Class IIDGuid
        Private Sub New()
        End Sub

        ' IID GUID strings for relevant COM interfaces
        Friend Const IModalWindow As String = "b4db1657-70d7-485e-8e3e-6fcb5a5c1802"
        Friend Const IFileDialog As String = "42f85136-db7e-439c-85f1-e4075d135fc8"
        Friend Const IFileOpenDialog As String = "d57c7288-d4ad-4768-be02-9d969532d960"
        Friend Const IFileSaveDialog As String = "84bccd23-5fde-4cdb-aea4-af64b83d78ab"
        Friend Const IFileDialogEvents As String = "973510DB-7D7F-452B-8975-74A85828D354"
        Friend Const IFileDialogControlEvents As String = "36116642-D713-4b97-9B83-7484A9D00433"
        Friend Const IFileDialogCustomize As String = "8016b7b3-3d49-4504-a0aa-2a37494e606f"
        Friend Const IShellItem As String = "43826D1E-E718-42EE-BC55-A1E261C37BFE"
        Friend Const IShellItemArray As String = "B63EA76D-1F85-456F-A19C-48159EFA858B"
        Friend Const IKnownFolder As String = "38521333-6A87-46A7-AE10-0F16706816C3"
        Friend Const IKnownFolderManager As String = "44BEAAEC-24F4-4E90-B3F0-23D258FBB146"
        Friend Const IPropertyStore As String = "886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"
    End Class

    Friend NotInheritable Class CLSIDGuid
        Private Sub New()
        End Sub

        ' CLSID GUID strings for relevant coclasses
        Friend Const FileOpenDialog As String = "DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7"
        Friend Const FileSaveDialog As String = "C0B4E2F3-BA21-4773-8DBA-335EC946EB8B"
        Friend Const KnownFolderManager As String = "4df0c730-df9d-4ae3-9153-aa6b82e9795a"
    End Class

    Friend NotInheritable Class KFIDGuid
        Private Sub New()
        End Sub

        Friend Const ComputerFolder As String = "0AC0837C-BBF8-452A-850D-79D08E667CA7"
        Friend Const Favorites As String = "1777F761-68AD-4D8A-87BD-30B759FA33DD"
        Friend Const Documents As String = "FDD39AD0-238F-46AF-ADB4-6C85480369C7"
        Friend Const Profile As String = "5E6C858F-0E22-4760-9AFE-EA3317B67173"
    End Class

    Friend Enum HRESULT As Long
        S_FALSE = &H1
        S_OK = &H0
        E_INVALIDARG = &H80070057UI
        E_OUTOFMEMORY = &H8007000EUI
    End Enum
End Class