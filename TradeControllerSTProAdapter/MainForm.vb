Imports Newtonsoft.Json
Imports SterlingLib
Imports WebSocket4Net

Public Class MainFrom

    Private WithEvents TCSTIAcctMaint As STIAcctMaint

    Private WithEvents TCSTIEvents As STIEvents

    Private WithEvents TCSTIPosition As STIPosition

    Private WithEvents TCWebSocket As WebSocket

    Private TCSTIApp As STIApp

    Private TCSTIOrder As STIOrder

    Private TCSTIOrderMaint As STIOrderMaint

    Private TCTraderID As String = Nothing

    Private Sub SetDisconnectedStatus()
        StatusLabel.Text = "Disconnected"
        StatusLabel.ForeColor = Color.Red
        EndpointInput.Enabled = True
        SocketConnectButton.Enabled = True
    End Sub

    Private Sub SetConnectingStatus()
        StatusLabel.Text = "Connecting . . ."
        StatusLabel.ForeColor = Color.Black
        EndpointInput.Enabled = False
        SocketConnectButton.Enabled = False
    End Sub

    Private Sub SetConnectedStatus()
        StatusLabel.Text = "Connected"
        StatusLabel.ForeColor = Color.Green
        EndpointInput.Enabled = False
        SocketConnectButton.Enabled = False
    End Sub

    Private Sub SterlingIsNotRunning()
        MsgBox("Sterling Trading Pro must be running.")
        Environment.Exit(0)
    End Sub

    Private Sub MainFrom_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False

        Text = "Trade Console - STPro " + ProductVersion

        SetDisconnectedStatus()

        Try
            TCSTIApp = New STIApp

            If Not TCSTIApp.IsApiEnabled Then
                MsgBox("API is not Enabled. Please contact Sterling Trader support.")
                Environment.Exit(0)
            End If

            TCSTIAcctMaint = New STIAcctMaint
            TCSTIEvents = New STIEvents
            TCSTIOrder = New STIOrder
            TCSTIOrderMaint = New STIOrderMaint
            TCSTIPosition = New STIPosition

            TCSTIApp.SetModeXML(True)
            TCSTIEvents.SetOrderEventsAsStructs(True)

            TCSTIPosition.RegisterForPositions()

            TCTraderID = TCSTIApp.GetTraderName
        Catch ex As Exception
            SterlingIsNotRunning()
        End Try
    End Sub

    Private Sub SendMetadata()
        Dim AccountList() As String = Nothing
        Dim TraderList() As String = Nothing
        Dim DestinationList() As String = Nothing

        TCSTIAcctMaint.GetAccountList(AccountList)
        TCSTIApp.GetTraderList(TCTraderID, TraderList)
        TCSTIApp.GetDestinationList(DestinationList)

        Dim Data As DataLayer = New DataLayer
        Data.SetAccountList(AccountList)
        Data.SetTraderList(TraderList)
        Data.SetDestinationList(DestinationList)

        SendToWebsocket(Data)
    End Sub

    Private Sub TCSTIEvents_OnSTIShutdown() Handles TCSTIEvents.OnSTIShutdown
        SterlingIsNotRunning()
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderUpdateXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderUpdateXML
        Dim Data As DataLayer = New DataLayer
        Data.SetOrderUpdate(StructConvert.ToSTIOrderUpdate(bstrOrder))

        SendToWebsocket(Data)
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderConfirmXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderConfirmXML
        Dim Data As DataLayer = New DataLayer
        Data.SetOrderConfirm(StructConvert.ToSTIOrderConfirm(bstrOrder))

        SendToWebsocket(Data)
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderRejectXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderRejectXML
        Dim Data As DataLayer = New DataLayer
        Data.SetOrderReject(StructConvert.ToSTIOrderReject(bstrOrder))

        SendToWebsocket(Data)
    End Sub

    Private Sub TCSTIEvents_OnSTITradeUpdateXML(ByRef bstrTrade As String) Handles TCSTIEvents.OnSTITradeUpdateXML
        Dim Data As DataLayer = New DataLayer
        Data.SetTradeUpdate(StructConvert.ToSTITradeUpdate(bstrTrade))

        SendToWebsocket(Data)
    End Sub

    Private Sub TCSTIPosition_OnSTIPositionUpdateXML(ByRef bstrPosition As String) Handles TCSTIPosition.OnSTIPositionUpdateXML
        Dim Data As DataLayer = New DataLayer
        Data.SetPositionUpdate(StructConvert.ToSTIPositionUpdate(bstrPosition))

        SendToWebsocket(Data)
    End Sub

    Private Sub TCSTIAcctMaint_OnSTIAcctUpdateXML(ByRef bstrAcct As String) Handles TCSTIAcctMaint.OnSTIAcctUpdateXML
        Dim Data As DataLayer = New DataLayer
        Data.SetAccountUpdate(StructConvert.ToSTIAccountUpdate(bstrAcct))

        SendToWebsocket(Data)
    End Sub

    Private Sub SocketConnectButton_Click(sender As Object, e As EventArgs) Handles SocketConnectButton.Click
        SetConnectingStatus()

        Try
            TCWebSocket = New WebSocket("wss://" + EndpointInput.Text + "/" + TCTraderID)
            TCWebSocket.Open()
        Catch ex As Exception
            SetDisconnectedStatus()

            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TCWebSocket_Opened(sender As Object, e As EventArgs) Handles TCWebSocket.Opened
        SetConnectedStatus()

        SendMetadata()
        GetPositionList()
    End Sub

    Private Sub TCWebSocket_Closed(sender As Object, e As EventArgs) Handles TCWebSocket.Closed
        SetDisconnectedStatus()
    End Sub

    Private Sub TCWebSocket_MessageReceived(sender As Object, e As MessageReceivedEventArgs) Handles TCWebSocket.MessageReceived
        If (Not String.IsNullOrWhiteSpace(e.ToString)) Then
            ProcessMessage(e.Message)
        End If
    End Sub

    Private Sub SendToWebsocket(Data As DataLayer)
        Console.WriteLine(Data.ToJson)
        If TCWebSocket IsNot Nothing Then
            If TCWebSocket.State = WebSocketState.Open Then
                Data.SetServerTime(TCSTIApp.GetServerTime)
                TCWebSocket.Send(Data.ToJson)
            End If
        End If
    End Sub

    Private Sub ProcessMessage(Message As String)
        Dim MessageObject As Object = JsonConvert.DeserializeObject(Message)

        Dim FunctionString As String = Trim(MessageObject("__Function"))
        Dim Parameters As Object = MessageObject("__Parameters")

        Try
            Select Case FunctionString
                Case "SendMetadata"
                    SendMetadata()

                '@class STIApp
                Case "SendMessageBox"
                    SendMessageBox(Parameters)

                '@class STIAcctMaint
                Case "GetAccountInfo"
                    GetAccountInfo(Parameters)

                '@class STIAcctMaint
                Case "MaintainAccount"
                    MaintainAccount(Parameters)


                '@class STIAcctMaint
                Case "MaintainSymbolControl"
                    MaintainSymbolControl(Parameters)

                '@class STIOrder
                Case "ReplaceOrderStruct"
                    ReplaceOrder(Parameters)

                '@class STIOrder
                Case "SubmitOrderStruct"
                    SubmitOrder(Parameters)

                '@class STIOrderMaint
                Case "CancelAllOrders"
                    CancelAllOrders(Parameters)

                '@class STIOrderMaint
                Case "CancelOrder"
                    CancelOrder(Parameters)

                '@class STIOrderMaint
                Case "CancelOrderEx"
                    CancelOrderEx(Parameters)

                '@class STIOrderMaint
                Case "GetOrderInfo"
                    GetOrderInfo(Parameters)

                '@class STIOrderMaint
                Case "GetOrderList"
                    GetOrderList(Parameters)

                '@class STIOrderMaint
                Case "GetOrderListEx"
                    GetOrderListEx(Parameters)

                '@class STIOrderMaint
                Case "GetTradeListEx"
                    GetTradeListEx(Parameters)

                '@class STIPosition
                Case "GetPositionInfoStruct"
                    GetPositionInfoStruct(Parameters)

                '@class STIPosition
                Case "GetPositionList"
                    GetPositionList()

                '@class STIPosition
                Case "GetPosListByAccount"
                    GetPosListByAccount(Parameters)

                '@class STIPosition
                Case "GetPosListBySym"
                    GetPosListBySym(Parameters)

                Case Else
                    Dim Data As DataLayer = New DataLayer
                    Data.SetExceptionMessage("[" + FunctionString + "] Unsupported Function Call.")

                    SendToWebsocket(Data)
            End Select
        Catch ex As Exception
            Dim Data As DataLayer = New DataLayer
            Data.SetExceptionMessage(ex.Message)

            SendToWebsocket(Data)
        End Try
    End Sub


#Region "Process Message Functions"
    Public Sub SendMessageBox(Parameters As Object)
        Dim Trader As String = Parameters("Trader")
        Dim Text As String = Parameters("Text")

        TCSTIApp.SendMessageBox(Trader, Text)
    End Sub

    Public Sub GetAccountInfo(Parameters As Object)
        Dim Account As String = Parameters("Account")

        Dim Data As New DataLayer
        Data.SetAccountUpdate(TCSTIAcctMaint.GetAccountInfo(Account))

        SendToWebsocket(Data)
    End Sub

    Public Sub MaintainAccount(Parameters As Object)
        Dim Code As Integer = TCSTIAcctMaint.MaintainAccount(StructConvert.ToSTIAccountUpdate(Parameters.ToString))

        Dim Data As DataLayer = New DataLayer
        Data.SetMaintainAccountResponse(ErrorCodeHandler.MaintainAccountErrorMessage(Code))

        SendToWebsocket(Data)
    End Sub

    Public Sub MaintainSymbolControl(Parameters As Object)
        Dim Code As Integer = TCSTIAcctMaint.MaintainSymbolControl(StructConvert.ToSTISymbolControl(Parameters.ToString))

        Dim Data As DataLayer = New DataLayer
        Data.SetMaintainSymbolControlResponse(ErrorCodeHandler.MaintainAccountErrorMessage(Code))

        SendToWebsocket(Data)
    End Sub

    Public Sub ReplaceOrder(Parameters As Object)
        Dim OrderString As String = Parameters("Order")
        Dim OldOrderRecordID As Integer = Parameters("OldOrderRecordID")
        Dim OldClientOrderID As String = Parameters("OldClientOrderID")

        Dim OrderStruct As structSTIOrder = StructConvert.ToSTIOrder(OrderString)
        Dim Code As Integer = TCSTIOrder.ReplaceOrderStruct(OrderStruct, OldOrderRecordID, OldClientOrderID)

        If (Code <> 0) Then
            Dim Data As DataLayer = New DataLayer
            Data.SetOrderReject(ErrorCodeHandler.CreateOrderReject(Code, OrderStruct))

            SendToWebsocket(Data)
        End If
    End Sub

    Private Sub SubmitOrder(Parameters As Object)
        Dim OrderStruct As structSTIOrder = StructConvert.ToSTIOrder(Parameters.ToString)
        Dim Code As Integer = TCSTIOrder.SubmitOrderStruct(OrderStruct)

        If (Code <> 0) Then
            Dim Data As DataLayer = New DataLayer
            Data.SetOrderReject(ErrorCodeHandler.CreateOrderReject(Code, OrderStruct))

            SendToWebsocket(Data)
        End If
    End Sub

    Public Sub CancelAllOrders(Parameters As Object)
        TCSTIOrderMaint.CancelAllOrders(StructConvert.ToSTICancelAll(Parameters.ToString))
    End Sub

    Public Sub CancelOrder(Parameters As Object)
        Dim Account As String = Parameters("Account")
        Dim OrderRecordID As Integer = Parameters("OrderRecordID")
        Dim OldClientOrderID As String = Parameters("OldClientOrderID")
        Dim ClientOrderID As String = Parameters("ClientOrderID")

        TCSTIOrderMaint.CancelOrder(Account, OrderRecordID, OldClientOrderID, ClientOrderID)
    End Sub

    Public Sub CancelOrderEx(Parameters As Object)
        Dim Account As String = Parameters("Account")
        Dim OrderRecordID As Integer = Parameters("OrderRecordID")
        Dim OldClientOrderID As String = Parameters("OldClientOrderID")
        Dim ClientOrderID As String = Parameters("ClientOrderID")
        Dim Instrument As String = Parameters("Instrument")

        TCSTIOrderMaint.CancelOrderEx(Account, OrderRecordID, OldClientOrderID, ClientOrderID, Instrument)
    End Sub

    Public Sub GetOrderInfo(Parameters As Object)
        Dim Data As DataLayer = New DataLayer
        Data.SetOrderUpdate(TCSTIOrderMaint.GetOrderInfo(Parameters.ToString))

        SendToWebsocket(Data)
    End Sub

    Public Sub GetOrderList(Parameters As Object)
        Dim OpenOnly As Boolean = Parameters("OpenOnly")
        Dim OrderList() As structSTIOrderUpdate = Nothing

        TCSTIOrderMaint.GetOrderList(OpenOnly, OrderList)

        Dim Data As DataLayer = New DataLayer
        Data.SetOrderList(OrderList)

        SendToWebsocket(Data)
    End Sub

    Public Sub GetOrderListEx(Parameters As Object)
        Dim OrderList() As structSTIOrderUpdate = Nothing

        TCSTIOrderMaint.GetOrderListEx(StructConvert.ToSTIOrderFilter(Parameters.ToString), OrderList)

        Dim Data As DataLayer = New DataLayer
        Data.SetOrderList(OrderList)

        SendToWebsocket(Data)
    End Sub

    Public Sub GetTradeListEx(Parameters As Object)
        Dim TradeList() As structSTITradeUpdate = Nothing

        TCSTIOrderMaint.GetTradeListEx(StructConvert.ToSTITradeFilter(Parameters.ToString), TradeList)

        Dim Data As DataLayer = New DataLayer
        Data.SetTradeList(TradeList)
    End Sub

    Public Sub GetPositionInfoStruct(Parameters As Object)
        Dim Symbol As String = Parameters("Symbol")
        Dim Exchange As String = Parameters("Exchange")
        Dim Account As String = Parameters("Account")

        Dim Data As DataLayer = New DataLayer
        Data.SetPositionUpdate(TCSTIPosition.GetPositionInfoStruct(Symbol, Exchange, Account))

        SendToWebsocket(Data)
    End Sub

    Public Sub GetPositionList()
        Dim PositionList() As structSTIPositionUpdate = Nothing
        TCSTIPosition.GetPositionList(PositionList)

        Dim Data As DataLayer = New DataLayer
        Data.SetPositionList(PositionList)

        SendToWebsocket(Data)
    End Sub

    Public Sub GetPosListByAccount(Parameters As Object)
        Dim Account As String = Parameters("Account")

        Dim PositionList() As structSTIPositionUpdate = Nothing
        TCSTIPosition.GetPosListByAccount(Account, PositionList)

        Dim Data As DataLayer = New DataLayer
        Data.SetPositionList(PositionList)

        SendToWebsocket(Data)
    End Sub

    Public Sub GetPosListBySym(Parameters As Object)
        Dim Symbol As String = Parameters("Symbol")

        Dim PositionList() As structSTIPositionUpdate = Nothing
        TCSTIPosition.GetPosListBySym(Symbol, PositionList)

        Dim Data As DataLayer = New DataLayer
        Data.SetPositionList(PositionList)

        SendToWebsocket(Data)
    End Sub
#End Region
End Class