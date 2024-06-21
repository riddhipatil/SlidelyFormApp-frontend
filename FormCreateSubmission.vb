Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class FormCreateSubmission

    Private stopwatchRunning As Boolean = False
    Private stopwatchStart As DateTime
    Private elapsedPausedTime As TimeSpan = TimeSpan.Zero ' Track paused time

    Private Sub btnToggleStopwatch_Click(sender As Object, e As EventArgs) Handles btnToggleStopwatch.Click
        If stopwatchRunning Then
            ' Pause the stopwatch
            stopwatchRunning = False
            Timer1.Stop()
            elapsedPausedTime += DateTime.Now - stopwatchStart ' Accumulate paused time
        Else
            ' Resume the stopwatch
            stopwatchRunning = True
            stopwatchStart = DateTime.Now
            Timer1.Start()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim totalElapsed As TimeSpan = DateTime.Now - stopwatchStart
        lblStopwatch.Text = (totalElapsed + elapsedPausedTime).ToString("hh\:mm\:ss")
    End Sub

    Private Async Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ' Submit the form data to the backend.
        ' This part should include code to make an HTTP POST request to /submit
        ' with the form data (name, email, phone, github_link, stopwatch_time).
        Dim submission As New With {
            .name = txtName.Text,
            .email = txtEmail.Text,
            .phone = txtPhone.Text,
            .github_link = txtGithubLink.Text,
            .stopwatch_time = lblStopwatch.Text
        }

        Dim jsonData = JsonConvert.SerializeObject(submission)
        Dim content = New StringContent(jsonData, Encoding.UTF8, "application/json")

        Dim client As New HttpClient()
        Dim response As HttpResponseMessage = Nothing

        Try
            response = Await client.PostAsync("http://localhost:3000/submit", content)

            If response.IsSuccessStatusCode Then
                MessageBox.Show("Submission successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Optionally, clear form fields or perform other actions upon successful submission
            Else
                MessageBox.Show($"Submission failed: {response.ReasonPhrase}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show($"Error submitting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            client.Dispose()
        End Try
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.T) Then
            btnToggleStopwatch.PerformClick()
            Return True
        ElseIf keyData = (Keys.Control Or Keys.S) Then
            btnSubmit.PerformClick()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles lblStopwatch.Click

    End Sub

    Private Sub FormCreateSubmission_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the stopwatch display
        lblStopwatch.Text = "00:00:00"
    End Sub
End Class
