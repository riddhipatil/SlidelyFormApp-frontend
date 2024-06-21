Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class FormViewSubmissions

    Private currentIndex As Integer = 0
    Private client As New HttpClient()
    Private isLoading As Boolean = False ' Flag to track if a fetch operation is in progress

    Private Async Sub FormViewSubmissions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadSubmission(currentIndex)
    End Sub

    Private Async Function LoadSubmission(index As Integer) As Task
        Try
            isLoading = True ' Set loading flag to true to prevent concurrent requests

            ' Make an HTTP GET request to fetch submission data
            Dim response = Await client.GetStringAsync($"http://localhost:3000/read?index={index}")
            Dim submission = JObject.Parse(response)

            ' Update UI with submission data
            txtName.Text = submission("name").ToString()
            txtEmail.Text = submission("email").ToString()
            txtPhone.Text = submission("phone").ToString()
            txtGithubLink.Text = submission("github_link").ToString()
            txtStopwatchTime.Text = submission("stopwatch_time").ToString()
        Catch ex As Exception
            MessageBox.Show($"Error loading submission: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            isLoading = False ' Reset loading flag after request completes
        End Try
    End Function

    Private Async Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If Not isLoading Then ' Check if not already loading
            If currentIndex > 0 Then
                currentIndex -= 1
                Await LoadSubmission(currentIndex)
            End If
        End If
    End Sub

    Private Async Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Not isLoading Then ' Check if not already loading
            currentIndex += 1
            Await LoadSubmission(currentIndex)
        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.P) Then
            btnPrevious.PerformClick()
            Return True
        ElseIf keyData = (Keys.Control Or Keys.N) Then
            btnNext.PerformClick()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

End Class
