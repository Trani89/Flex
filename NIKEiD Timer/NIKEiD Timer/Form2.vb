Public Class Form2

    'Private db1 As String = "C:\Users\" & Environment.UserName & "\Documents\dbTimer\soft_log.txt"
    'Private db2 As String = "C:\Users\" & Environment.UserName & "\Documents\dbTimer\hard_log.txt"

    'Viewer paths
    Dim unit As String = "B"
    Private db1 As String = unit & ":\dbTimer\soft_log.txt"
    Private db2 As String = unit & ":\dbTimer\hard_log.txt"


    Private currenTimer As Integer = CInt(Form1.TextBox1.Text)
    Dim time As Date = Now
    'Private timeStart As Double = CDbl(time.DayOfYear & "00" & Hour(time) * 60 * 60 + Minute(time) * 60 + Second(time))
    Private timeStart As Double = CDbl("100" & Hour(time) * 60 * 60 + Minute(time) * 60 + Second(time))

    Const MSG_EMPTY_FIELD = "Escanear el serial y el lado correcto del calzado"
    Const TIME_LAPSE = 31 'minutes
    Const STATUS_RUNNING As String = "Running"
    Const STATUS_COMPLETED As String = "Completed"

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cleanForm()
        Me.Text = "NIKEiD Timer Set " & currenTimer & " at " & timeStart
        TextBox2.Select()
        Me.Left = My.Computer.Screen.Bounds.Width / 2 - Me.Width / 2
        Me.Top = My.Computer.Screen.Bounds.Height / 2 - Me.Height / 2
        TextBox1.Text = timeSerialToClock(timeStart)
    End Sub
    Sub cleanForm()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then

        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        TextBox2.MaxLength = 17
        TextBox2.Text = TextBox2.Text.Trim()
        If e.KeyChar = ChrW(Keys.Enter) Then
            If TextBox2.Text.Trim.Length < 17 Or InStr(16, TextBox2.Text, ("L")) Then
                MsgBox(MSG_EMPTY_FIELD)
                TextBox2.Clear()
                Exit Sub
            End If
            TextBox3.Select()
        End If
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        TextBox3.MaxLength = 17
        TextBox3.Text = TextBox3.Text.Trim()
        If e.KeyChar = ChrW(Keys.Enter) Then
            If TextBox3.Text.Trim.Length < 17 Or InStr(16, TextBox3.Text, ("R")) Then
                MsgBox(MSG_EMPTY_FIELD)
                TextBox3.Clear()
                Exit Sub
            End If
            Me.Hide()
            Dim logArrayList As ArrayList = getlog()
            Dim logArrayListHard As ArrayList = getlogHard()
            log(logArrayList.Count, currenTimer, timeStart, getTimeEnd(timeStart), TextBox2.Text, TextBox3.Text)
            logHard(logArrayListHard.Count, currenTimer, timeStart, getTimeEnd(timeStart), TextBox2.Text, TextBox3.Text)
            cleanForm()
            Me.Close()
        End If
    End Sub

    Function getTimeEnd(ByVal timeStart As Double) As Double
        Dim timeStartA As Double = Split(CStr(timeStart), "00")(1)
        Dim timeEnd As Double = timeStartA + TIME_LAPSE * 60
        Dim timeEndA As Double = CDbl(Split(CStr(timeStart), "00")(0) & "00" & timeEnd)
        Return timeEndA
    End Function

    Function log(ByVal logID As String, ByVal timerID As Integer, ByVal timeStart As String, ByVal timeEnd As String, ByVal snR As String, ByVal snL As String) As Boolean
        Try
            If TextBox2.Text = "" Or TextBox3.Text = "" Then
                MsgBox("Ingresa el número de serie del calzado")
                Exit Function
            End If
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter(db1, True)
            file.WriteLine(logID & "_" & timerID & "_" & timeStart & "_" & timeEnd & "_" & snR & "_" & snL & "_" & STATUS_RUNNING)
            file.Close()
            Return True
        Catch ex As Exception
            MsgBox("Error 2: " & ex.Message)
            Return False
        End Try
    End Function
    Function logHard(ByVal logID As String, ByVal timerID As Integer, ByVal timeStart As String, ByVal timeEnd As String, ByVal snR As String, ByVal snL As String) As Boolean
        Try
            If TextBox2.Text = "" Or TextBox3.Text = "" Then
                Exit Function
            End If
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter(db2, True)
            file.WriteLine(logID & "_" & timerID & "_" & timeStart & "_" & timeEnd & "_" & snR & "_" & snL & "_" & STATUS_RUNNING)
            file.Close()
            Return True
        Catch ex As Exception
            MsgBox("Error 2: " & ex.Message)
            Return False
        End Try
    End Function
    Function getlog() As ArrayList
        Dim lineArray As New ArrayList()
        Try
            Dim lines() As String = IO.File.ReadAllLines(db1)
            For x As Integer = 0 To lines.GetUpperBound(0)
                lineArray.Add(lines(x))
            Next
            Return lineArray
        Catch ex As Exception
            Return lineArray
        End Try

    End Function
    Function getlogHard() As ArrayList
        Dim lines() As String = IO.File.ReadAllLines(db2)
        Dim lineArray As New ArrayList()
        For x As Integer = 0 To lines.GetUpperBound(0)
            lineArray.Add(lines(x))
        Next
        Return lineArray
    End Function

    Public Function timeSerialToClock(ByVal time As String) As String

        Dim timeArray() As String = Split(time, "00")
        Dim day As Integer = CInt(timeArray(0))
        Dim secondOfDay As Integer = CInt(timeArray(1))
        Dim hourOfDay As Integer = Math.Floor(secondOfDay / (60 * 60))
        Dim minuteOfHour As Integer = Math.Floor((secondOfDay - hourOfDay * (60 * 60)) / 60)
        Dim secondOfMinute As Integer = (secondOfDay - (hourOfDay * (60 * 60) + minuteOfHour * 60))

        If hourOfDay = 24 Then
            hourOfDay = 0
        End If

        Return hourOfDay & ":" & minuteOfHour & ":" & secondOfMinute

    End Function
End Class
