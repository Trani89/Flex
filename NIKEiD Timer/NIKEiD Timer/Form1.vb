Imports System.IO

Public Class Form1
    Private timers As New ArrayList




    'Normal paths
    'Private db1 As String = "C:\Users\" & Environment.UserName & "\Documents\dbTimer\soft_log.txt"
    'Private db2 As String = "C:\Users\" & Environment.UserName & "\Documents\dbTimer\hard_log.txt"
    'Private dbTest As String = "C:\Users\" & Environment.UserName & "\Documents\dbTimer\test.txt"

    'Viewer paths
    Dim unit As String = "B"
    Private db1 As String = unit & ":\dbTimer\soft_log.txt"
    Private db2 As String = unit & ":\dbTimer\hard_log.txt"
    'Private dbTest As String = unit & ":\test.txt"


    Private masterList As New List(Of String)


    Const MSG_COPMLETED As String = " Completed "
    Const STATUS_RUNNING As String = "Running"
    Const STATUS_COMPLETED As String = "Completed"

    Sub resetDashbaord()
        Dim control As Control
        If timers.Count = 0 Then
            For Each control In TableLayoutPanel2.Controls
                timers.Insert(0, control)
            Next
        End If

        TableLayoutPanel2.BackColor = Color.Black
        TableLayoutPanel1.BackColor = Color.Black
        lbl_second.ForeColor = Color.RoyalBlue
        Dim serial_font As New System.Drawing.Font("Arial", 12)
        Dim timer_font As New System.Drawing.Font("Arial", 14, FontStyle.Bold)
        Dim status_font As New System.Drawing.Font("Arial", 12)
        Dim serialR_color As Color = Color.Blue
        Dim serialL_color As Color = Color.Purple
        Dim timer_color As Color = Color.White

        Dim timer As TableLayoutPanel
        Dim lbl As Label

        MenuStrip1.ForeColor = Color.White
        Label127.ForeColor = Color.White
        Dim timerCount As Integer = 0

        For Each timer In timers
            timer.BorderStyle = BorderStyle.None
            timer.BackColor = Color.MidnightBlue
            timer.Dock = DockStyle.Fill
            'Debug.Print("*************************")
            'Debug.Print("Information timer: " & timer.Name)
            Dim lblCount As Integer = 1

            For Each lbl In timer.Controls
                lbl.Dock = DockStyle.Fill
                'Debug.Print("---------------------")
                'Debug.Print("Label: " & lbl.Name)
                Select Case lblCount
                    Case 1
                        lbl.Font = timer_font
                        lbl.ForeColor = timer_color
                        lbl.Text = timerCount & " Available"

                    Case 2
                        lbl.Font = serial_font
                        lbl.ForeColor = serialR_color
                        lbl.Text = "Left"

                    Case 3
                        lbl.Font = serial_font
                        lbl.ForeColor = serialL_color
                        lbl.Text = "Right"

                    Case 4
                        lbl.Font = status_font
                        lbl.Text = "Status"
                End Select
                lblCount = lblCount + 1
            Next
            timerCount = timerCount + 1
        Next

        Debug.Print("*************************")
        Debug.Print("*************************")
        Debug.Print("Report")
        Debug.Print("Number of timers: " & timers.Count)
        createDB()
        Debug.Print("*************************")
        Debug.Print("*************************")

        TextBox1.Select()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetDashbaord()
    End Sub

    Public Function modifyTimer(ByVal index As String, ByVal timeStart As String, ByVal timeEnd As String, ByVal snR As String, ByVal snL As String, Optional status As Boolean = False)

        Dim timer As TableLayoutPanel = timers.Item(index)
        Dim lbl As Label
        Dim lblCount = 1

        Debug.Print("Point 1")
        For Each lbl In timer.Controls
            Select Case lblCount
                Case 1
                    If timeStart.Contains("X") Or timeEnd.Contains("X") Then

                    Else
                        lbl.Text = index & " - " & timeSerialToClock(timeStart) & " ► " & timeSerialToClock(timeEnd)
                    End If
                Case 2
                    lbl.Text = snR
                Case 3
                    lbl.Text = snL
                Case 4
                    lbl.Text = STATUS_RUNNING
                    Debug.Print("modifying timer")
                    timer.BackColor = Color.Gold 'Color.DarkOrange 'Color.Yellow
            End Select
            lblCount += 1
        Next
    End Function

    Public Function modifyTimer(ByVal index As String, ByVal timeStart As String, ByVal timeEnd As String, Optional ByVal status As String = STATUS_RUNNING)

        Dim timer As TableLayoutPanel = timers.Item(index)
        Dim lbl As Label
        Dim lblCount = 1
        For Each lbl In timer.Controls
            Select Case lblCount
                Case 1
                    If timeStart.Contains("X") Or timeEnd.Contains("X") Then
                        'lbl.Text = index & " - " & MSG_COPMLETED
                    Else
                        lbl.Text = index & " - " & timeSerialToClock(timeStart) & " => " & timeSerialToClock(timeEnd)
                    End If
                Case 2

                Case 3
            End Select
            lblCount += 1
        Next
    End Function

    'To modify just the status
    Public Function modifyTimer(ByVal index As String, ByVal status As String)

        Dim timer As TableLayoutPanel = timers.Item(index)
        Dim lbl As Label
        Dim lblCount = 1
        For Each lbl In timer.Controls
            Select Case lblCount
                Case 1

                Case 2

                Case 3

                Case 4
                    lbl.Text = status
                    If status = STATUS_COMPLETED Then
                        timer.BackColor = Color.LawnGreen
                    End If
            End Select
            lblCount += 1
        Next
    End Function

    Public Function timeSerialToClock(ByVal time As String, Optional mode As Boolean = False) As String

        Dim timeArray() As String = Split(time, "00")
        Dim day As Integer = CInt(timeArray(0))
        Dim secondOfDay As Integer = CInt(timeArray(1))
        Dim hourOfDay As Integer = Math.Floor(secondOfDay / (60 * 60))
        Dim minuteOfHour As Integer = Math.Floor((secondOfDay - hourOfDay * (60 * 60)) / 60)
        Dim secondOfMinute As Integer = (secondOfDay - (hourOfDay * (60 * 60) + minuteOfHour * 60))

        If hourOfDay = 24 Then
            hourOfDay = 0
        End If

        If mode Then
            Return hourOfDay & ":" & minuteOfHour & ":" & secondOfMinute
        Else
            Return hourOfDay & ":" & minuteOfHour ' & ":" & secondOfMinute
        End If


    End Function

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If Not IsNumeric(TextBox1.Text) Then
                MsgBox("Ingresa la posición de descanso.")
                TextBox1.Clear()
                Exit Sub
            End If
            If CInt(TextBox1.Text) > 41 Or CInt(TextBox1.Text) < 0 Then
                MsgBox("Ingresa una poisción de descanso válida.")
                TextBox1.Clear()
                Exit Sub
            End If

            If CType(timers.Item(TextBox1.Text), TableLayoutPanel).Controls.Item(3).Text = STATUS_COMPLETED Then
                Dim selectedTimer As Integer = TextBox1.Text
                Form2.Show()
                TextBox1.Clear()
            ElseIf CType(timers.Item(TextBox1.Text), TableLayoutPanel).Controls.Item(3).Text = STATUS_RUNNING Then
                MsgBox("La posición está ocupada. Elige otra posición.")
                TextBox1.Clear()
            Else
                Dim selectedTimer As Integer = TextBox1.Text
                Form2.Show()
                TextBox1.Clear()
            End If

        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Dim time As Date = Now
            'Dim totalNumber As Integer = Hour(time) * 60 * 60 + Minute(time) * 60 + Second(time)
            'Trying to remove a few digits from the number
            'Dim totalNumber As Double = time.DayOfYear & "00" & (Hour(time) * 60 * 60 + Minute(time) * 60 + Second(time))
            Dim totalNumber As Double = "100" & (Hour(time) * 60 * 60 + Minute(time) * 60 + Second(time))
            Debug.Print("Time received to convert: " & totalNumber)
            lbl_second.Text = timeSerialToClock(totalNumber, True)
        Catch ex As Exception

        End Try

    End Sub

    Sub isTimerDone()

        Dim time As Date = Now
        Dim currentTime As Double = CDbl("100" & Hour(time) * 60 * 60 + Minute(time) * 60 + Second(time))
        'Initializing timer counter
        Dim timerIndex As Integer = 0
        'Obtaining the current logged status of timers
        Dim logArraList As ArrayList = getlog()
        'logArraList.Reverse()
        Dim lineCount As Integer
        Dim linesToRemove As New ArrayList

        Dim newlogArraList As New ArrayList

        For Each timer In timers

            Try
                Dim timersReviewed As New ArrayList

                For Each item In logArraList

                    If item.ToString = "" Then
                        'Debug.Print("no data logged.")
                        'skips lines with length 0
                        Continue For
                    End If

                    Dim data() As String = item.ToString().Split("_")
                    If data.Length = 0 Then
                        'Debug.Print("no data logged.")
                        'skips lines with length 0
                        Continue For
                    End If
                    If item.ToString().Contains(STATUS_COMPLETED) Then
                        'skips the lines marked as completed
                        Continue For
                    End If

                    Dim indexs As Integer = data(1)
                    Dim timeend As Double
                    Dim dashboardStatus As String = timer.Controls.Item(3).Text


                    If timerIndex = indexs And timersReviewed.IndexOf(indexs) < 0 Then
                        timersReviewed.Add(indexs)
                        timeend = data(3)
                        Debug.Print("te = " & timeend & " ct = " & currentTime & " test = " & CStr(getSecondsOfTime(timeend) < getSecondsOfTime(currentTime)))
                        If getDayOfTime(timeend) < getDayOfTime(currentTime) And dashboardStatus = STATUS_RUNNING Then
                            'isdone = true
                            modifyTimer(timerIndex, STATUS_COMPLETED)
                        ElseIf getSecondsOfTime(timeend) < getSecondsOfTime(currentTime) And dashboardStatus = STATUS_RUNNING Then
                            'isdone = true
                            modifyTimer(timerIndex, STATUS_COMPLETED)
                        Else

                        End If
                    End If
                    lineCount = lineCount + 1
                Next


                lineCount = 0
            Catch ex As Exception
                MsgBox("error 2: " & ex.Message)
            End Try
            timerIndex = timerIndex + 1
        Next



    End Sub

    Sub updatelog(ByVal linesToRemove As ArrayList)
        Try
            'Debug.Print("******************************")
            'Debug.Print("Starting updatelog(ByVal linesToRemove As ArrayList)")
            'Debug.Print("Removing lines: " & linesToRemove.Count)
            Dim TheFileLines As New List(Of String)
            TheFileLines.AddRange(System.IO.File.ReadAllLines(db1))

            Dim sResult As String = String.Join(", ", linesToRemove.ToArray())
            'Debug.Print("List of items to remove: " & vbNewLine & sResult)


            For Each item In linesToRemove
                If item >= TheFileLines.Count Then Exit Sub
                'Debug.Print("Removing: " & item)
                TheFileLines.RemoveAt(item)
                System.IO.File.WriteAllLines(db1, TheFileLines.ToArray)
            Next
            'Debug.Print("Ending ByVal linesToRemove As ArrayList")
            'Debug.Print("******************************")
        Catch ex As Exception
            MsgBox("Error 4: " & ex.Message)
        End Try
    End Sub

    Function getDayOfTime(ByVal time As Double) As Integer
        Dim day As Integer = CInt(Split(CStr(time), "00")(0))
        Return day
    End Function

    Function getSecondsOfTime(ByVal time As Double) As Double
        Dim seconds As Double = CInt(Split(CStr(time), "00")(1))
        Return seconds
    End Function

    Private Sub createDB()
        If Not Directory.Exists(unit & ":\") Then
            Directory.CreateDirectory(unit & ":\")
            'If Not Directory.Exists("c:\users\" & Environment.UserName & "\documents\dbtimer\") Then
            File.Create(db1)
                File.Create(db2)
                'Debug.Print("DB created")
            Else
                If Not File.Exists(db1) Then
                Dim fs As FileStream = File.Create(db1)
                fs.Close()
            End If
            If Not File.Exists(db2) Then
                Dim fs As FileStream = File.Create(db2)
                fs.Close()
            End If
            Debug.Print("DB already exists")
        End If
    End Sub

    Function getlog() As ArrayList

        Dim lines() As String = IO.File.ReadAllLines(db1)
        Dim lineArray As New ArrayList()
            For x As Integer = 0 To lines.GetUpperBound(0)
                lineArray.Insert(0, lines(x))
            Next
        Return lineArray
    End Function

    Sub updateDahsboard()
        'Debug.Print("*******************+")
        'Debug.Print("updating")
        Debug.Print("Point 3")
        Dim logArraList As ArrayList = getlog()
        Dim time As Date = Now
        Dim currentTime As Double = CDbl("100" & Hour(time) * 60 * 60 + Minute(time) * 60 + Second(time))
        'logArraList.Reverse()
        Dim itemCount As Integer
        Debug.Print("Point 4")
        Try

            For Each item In logArraList
                Debug.Print("Point 5")
                Dim data() As String = item.split("_")

                Dim timer As TableLayoutPanel = timers.Item(data(1))
                Dim timerIndex As String = data(1)
                Dim timerStart As String = data(2)
                Dim timerEnd As String = data(3)
                Dim snR As String = data(4)
                Dim snL As String = data(5)
                Dim dashboardSerialRight As String = timer.Controls.Item(2).Text
                Dim dashboardStatus As String = timer.Controls.Item(3).Text
                Dim logSerialRight As String = data(2)
                Debug.Print("Point 6")

                Debug.Print("timerEnd = " & timerEnd)
                Debug.Print("currentTime = " & currentTime)
                Debug.Print("timerEnd = " & timerEnd & " [" & timerEnd > currentTime & "] " & currentTime)
                If timerEnd > currentTime Then
                    'If 2 > 1 Then
                    Debug.Print("Point 7")
                    If logSerialRight <> dashboardSerialRight Or dashboardStatus = "Status" Then
                        Debug.Print("Point 2")
                        modifyTimer(timerIndex, timerStart, timerEnd, snR, snL)
                    End If
                End If
                Debug.Print("point 8")
                itemCount += 1
            Next
        Catch ex As Exception
            MsgBox("Error 1: " & ex.Message)
        End Try
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Try
            updateDahsboard()
            'Running through the timers looking for finished timers
            isTimerDone()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        resetDashbaord()
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Dim timer As TableLayoutPanel
        For Each timer In timers
            If timer.Controls.Item(3).Text = "Completed" Then
                If timer.BackColor = Color.LawnGreen Then
                    timer.BackColor = Color.MidnightBlue
                Else
                    timer.BackColor = Color.LawnGreen
                End If
            End If
        Next
    End Sub
End Class
