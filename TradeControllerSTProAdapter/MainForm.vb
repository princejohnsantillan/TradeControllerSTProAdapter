Imports SterlingLib
Imports SuperSocket.ClientEngine
Imports WebSocket4Net

Public Class MainFrom
    Private TCSTIApp As STIApp
    Private WithEvents TCSTIAcctMaint As STIAcctMaint
    Private WithEvents TCSTIEvents As STIEvents
    Private WithEvents TCSTIPosition As STIPosition
    Private WithEvents TCWebSocket As WebSocket
    Private TCTraderID As String = Nothing

    Private Sub MainFrom_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False

        Try
            TCSTIApp = New STIApp

            If Not TCSTIApp.IsApiEnabled() Then
                MsgBox("API is not Enabled. Please contact Sterling Trader support.")
                Environment.Exit(0)
            End If

            TCSTIAcctMaint = New STIAcctMaint
            TCSTIEvents = New STIEvents
            TCSTIPosition = New STIPosition

            TCSTIApp.SetModeXML(True)
            TCSTIEvents.SetOrderEventsAsStructs(True)

            TCTraderID = TCSTIApp.GetTraderName()
        Catch ex As Exception
            SterlingIsNotRunning()
        End Try
    End Sub

    Private Sub SendMetaData()
        Dim meta As String = ""

        Dim traderList As String() = Nothing
        Dim positionList As structSTIPositionUpdate() = Nothing 'TODO: convert to string

        TCSTIApp.GetTraderList(TCTraderID, traderList) 'TODO: cross check value with AcctMaint
        TCSTIPosition.GetPositionList(positionList)

        'TODO: collect meta data and send to websockets
        SendToWebsocket(meta)
    End Sub

    Private Sub TCSTIAcctMaint_OnSTIAcctUpdateXML(ByRef bstrAcct As String) Handles TCSTIAcctMaint.OnSTIAcctUpdateXML
        SendToWebsocket(bstrAcct)
    End Sub

    Private Sub TCSTIPosition_OnSTIPositionUpdateXML(ByRef bstrPosition As String) Handles TCSTIPosition.OnSTIPositionUpdateXML
        SendToWebsocket(bstrPosition)
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderConfirmXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderConfirmXML
        SendToWebsocket(bstrOrder)
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderRejectXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderRejectXML
        SendToWebsocket(bstrOrder)
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderUpdateXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderUpdateXML
        SendToWebsocket(bstrOrder)
    End Sub

    Private Sub TCSTIEvents_OnSTITradeUpdateXML(ByRef bstrTrade As String) Handles TCSTIEvents.OnSTITradeUpdateXML
        SendToWebsocket(bstrTrade)
    End Sub

    Private Sub SocketConnectButton_Click(sender As Object, e As EventArgs) Handles SocketConnectButton.Click
        StatusLabel.Text = "Connecting . . ."
        StatusLabel.ForeColor = Color.Black
        EndpointInput.Enabled = False
        SocketConnectButton.Enabled = False
        Try
            TCWebSocket = New WebSocket("wss://" + EndpointInput.Text + "/" + TCTraderID)
            TCWebSocket.Open()
        Catch ex As Exception
            StatusLabel.Text = "Disconnected"
            StatusLabel.ForeColor = Color.Red
            EndpointInput.Enabled = True
            SocketConnectButton.Enabled = True
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TCWebSocket_Opened(sender As Object, e As EventArgs) Handles TCWebSocket.Opened
        StatusLabel.Text = "Connected"
        StatusLabel.ForeColor = Color.Green

        SendMetaData()
    End Sub

    Private Sub TCWebSocket_MessageReceived(sender As Object, e As MessageReceivedEventArgs) Handles TCWebSocket.MessageReceived
        'TODO: Create Message Interpreter
        Console.WriteLine(e.Message)
    End Sub

    Private Sub TCWebSocket_DataReceived(sender As Object, e As DataReceivedEventArgs) Handles TCWebSocket.DataReceived
        'TODO: Create DATA Interpreter
        Console.WriteLine(e.Data)
    End Sub

    Private Sub TCWebSocket_Error(sender As Object, e As ErrorEventArgs) Handles TCWebSocket.[Error]
        Console.WriteLine(e.Exception)
    End Sub

    Private Sub TCWebSocket_Closed(sender As Object, e As EventArgs) Handles TCWebSocket.Closed
        StatusLabel.Text = "Disconnected"
        StatusLabel.ForeColor = Color.Red
        EndpointInput.Enabled = True
        SocketConnectButton.Enabled = True
    End Sub

    Private Sub TCSTIEvents_OnSTIShutdown() Handles TCSTIEvents.OnSTIShutdown
        SterlingIsNotRunning()
    End Sub

    Private Sub SendToWebsocket(message As String)
        Console.WriteLine(message)

        If TCWebSocket IsNot Nothing Then
            If TCWebSocket.State = WebSocketState.Open Then
                TCWebSocket.Send(message)
            End If
        End If
    End Sub

    Private Sub SterlingIsNotRunning()
        'TODO: send close message to websocket.
        MsgBox("Sterling Trading Pro must be running.")
        Environment.Exit(0)
    End Sub
End Class
