Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CustomerBindingSource.DataSource = New Customer()
        Me.LineItemBindingSource.DataSource = New List(Of LineItem)()
    End Sub

    Private Sub viewReportButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewReportButton.Click
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class
