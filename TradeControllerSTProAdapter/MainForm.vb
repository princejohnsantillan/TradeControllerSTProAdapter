Imports Newtonsoft.Json
Imports SterlingLib
Imports SuperSocket.ClientEngine
Imports WebSocket4Net

Public Class MainFrom

    Private WithEvents TCSTIAcctMaint As STIAcctMaint = Nothing

    Private WithEvents TCSTIEvents As STIEvents = Nothing

    Private WithEvents TCSTIPosition As STIPosition = Nothing

    Private WithEvents TCWebSocket As WebSocket = Nothing

    Private TCSTIApp As STIApp = Nothing

    Private TCSTIOrder As STIOrder = Nothing

    Private TCSTIOrderMaint As STIOrderMaint = Nothing

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
        MsgBox("Make sure Sterling Trader Pro or Elite is running.")
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

    Public Function NewWebSocketMessage() As WebSocketMessage
        Return New WebSocketMessage(TCTraderID)
    End Function


    Private Sub SendMetadata()
        Dim Metadata As Dictionary(Of String, String()) = New Dictionary(Of String, String())
        Dim AccountList() As String = Nothing
        Dim TraderList() As String = Nothing
        Dim DestinationList() As String = Nothing

        TCSTIAcctMaint.GetAccountList(AccountList)
        TCSTIApp.GetTraderList(TCTraderID, TraderList)
        TCSTIApp.GetDestinationList(DestinationList)

        Metadata("AccountList") = AccountList
        Metadata("TraderList") = TraderList
        Metadata("DestinationList") = DestinationList

        SendToWebSocket(NewWebSocketMessage.SetMetadata(Metadata))
    End Sub

    Private Sub TCSTIEvents_OnSTIShutdown() Handles TCSTIEvents.OnSTIShutdown
        SterlingIsNotRunning()
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderUpdateXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderUpdateXML
        SendToWebSocket(NewWebSocketMessage.SetOrderUpdate(StructConvert.ToSTIOrderUpdate(bstrOrder)))
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderConfirmXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderConfirmXML
        SendToWebSocket(NewWebSocketMessage.SetOrderConfirm(StructConvert.ToSTIOrderConfirm(bstrOrder)))
    End Sub

    Private Sub TCSTIEvents_OnSTIOrderRejectXML(ByRef bstrOrder As String) Handles TCSTIEvents.OnSTIOrderRejectXML
        SendToWebSocket(NewWebSocketMessage.SetOrderReject(StructConvert.ToSTIOrderReject(bstrOrder)))
    End Sub

    Private Sub TCSTIEvents_OnSTITradeUpdateXML(ByRef bstrTrade As String) Handles TCSTIEvents.OnSTITradeUpdateXML
        SendToWebSocket(NewWebSocketMessage.SetTradeUpdate(StructConvert.ToSTITradeUpdate(bstrTrade)))
    End Sub

    Private Sub TCSTIPosition_OnSTIPositionUpdateXML(ByRef bstrPosition As String) Handles TCSTIPosition.OnSTIPositionUpdateXML
        SendToWebSocket(NewWebSocketMessage.SetPositionUpdate(StructConvert.ToSTIPositionUpdate(bstrPosition)))
    End Sub

    Private Sub TCSTIAcctMaint_OnSTIAcctUpdateXML(ByRef bstrAcct As String) Handles TCSTIAcctMaint.OnSTIAcctUpdateXML
        SendToWebSocket(NewWebSocketMessage.SetAccountUpdate(StructConvert.ToSTIAccountUpdate(bstrAcct)))
    End Sub

    Private Sub SocketConnectButton_Click(sender As Object, e As EventArgs) Handles SocketConnectButton.Click
        SetConnectingStatus()

        Try
            TCWebSocket = New WebSocket("wss://" + Trim(EndpointInput.Text) + "/" + TCTraderID + "?AppVersion=" + ProductVersion)
            TCWebSocket.Open()
        Catch ex As Exception
            SetDisconnectedStatus()

            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TCWebSocket_Opened(sender As Object, e As EventArgs) Handles TCWebSocket.Opened
        SetConnectedStatus()

        SendMetadata()
    End Sub

    Private Sub TCWebSocket_Closed(sender As Object, e As EventArgs) Handles TCWebSocket.Closed
        SetDisconnectedStatus()
    End Sub

    Private Sub TCWebSocket_Error(sender As Object, e As ErrorEventArgs) Handles TCWebSocket.[Error]
        MsgBox(e.Exception.Message)
    End Sub

    Private Sub TCWebSocket_MessageReceived(sender As Object, e As MessageReceivedEventArgs) Handles TCWebSocket.MessageReceived
        If (Not String.IsNullOrWhiteSpace(e.Message)) Then
            ProcessMessage(e.Message)
        End If
    End Sub

    Private Sub SendToWebSocket(Message As WebSocketMessage)
        If TCWebSocket IsNot Nothing Then
            If TCWebSocket.State = WebSocketState.Open Then
                Message.SetServerTime(TCSTIApp.GetServerTime)
                TCWebSocket.Send(Message.ToJson)
            End If
        End If
    End Sub

    Private Sub ProcessMessage(Message As String)
        Dim MessageObject As Object = JsonConvert.DeserializeObject(Message)

        Dim EventName As String = MessageObject("Event")
        Dim DataObject As Object = MessageObject("Data")

        Try
            Select Case EventName
                Case "MsgBox"
                    MsgBox(DataObject)

                Case "SendMetadata"
                    SendMetadata()

                '@class STIApp
                Case "SendMessageBox"
                    SendMessageBox(DataObject)

                '@class STIAcctMaint
                Case "GetAccountInfo"
                    GetAccountInfo(DataObject)

                '@class STIAcctMaint
                Case "MaintainAccount"
                    MaintainAccount(DataObject)

                '@class STIAcctMaint
                Case "MaintainSymbolControl"
                    MaintainSymbolControl(DataObject)

                '@class STIOrder
                Case "ReplaceOrderStruct"
                    ReplaceOrder(DataObject)

                '@class STIOrder
                Case "SubmitOrderStruct"
                    SubmitOrder(DataObject)

                '@class STIOrderMaint
                Case "CancelAllOrders"
                    CancelAllOrders(DataObject)

                '@class STIOrderMaint
                Case "CancelOrder"
                    CancelOrder(DataObject)

                '@class STIOrderMaint
                Case "CancelOrderEx"
                    CancelOrderEx(DataObject)

                '@class STIOrderMaint
                Case "GetOrderInfo"
                    GetOrderInfo(DataObject)

                '@class STIOrderMaint
                Case "GetOrderList"
                    GetOrderList(DataObject)

                '@class STIOrderMaint
                Case "GetOrderListEx"
                    GetOrderListEx(DataObject)

                '@class STIOrderMaint
                Case "GetTradeListEx"
                    GetTradeListEx(DataObject)

                '@class STIPosition
                Case "GetPositionInfoStruct"
                    GetPositionInfoStruct(DataObject)

                '@class STIPosition
                Case "GetPositionList"
                    GetPositionList()

                '@class STIPosition
                Case "GetPosListByAccount"
                    GetPosListByAccount(DataObject)

                '@class STIPosition
                Case "GetPosListBySym"
                    GetPosListBySym(DataObject)

                Case Else
                    SendToWebSocket(NewWebSocketMessage.SetExceptionMessage("[" + EventName + "] Undefined Event."))
            End Select
        Catch ex As Exception
            SendToWebSocket(NewWebSocketMessage.SetExceptionMessage(ex.Message))
        End Try
    End Sub


#Region "Process Message Functions"
    Public Sub SendMessageBox(DataObject As Object)
        Dim Trader As String = DataObject("Trader")
        Dim Text As String = DataObject("Text")

        TCSTIApp.SendMessageBox(Trader, Text)
    End Sub

    Public Sub GetAccountInfo(DataObject As Object)
        Dim Account As String = DataObject("Account")

        SendToWebSocket(NewWebSocketMessage.SetAccountUpdate(TCSTIAcctMaint.GetAccountInfo(Account)))
    End Sub

    Public Sub MaintainAccount(DataObject As Object)
        Dim ErrorCode As Integer = TCSTIAcctMaint.MaintainAccount(StructConvert.ToSTIAccountUpdate(DataObject.ToString))

        SendToWebSocket(NewWebSocketMessage.SetMaintainAccountResponse(ErrorCodeHandler.MaintainAccountErrorMessage(ErrorCode)))
    End Sub

    Public Sub MaintainSymbolControl(DataObject As Object)
        Dim ErrorCode As Integer = TCSTIAcctMaint.MaintainSymbolControl(StructConvert.ToSTISymbolControl(DataObject.ToString))

        SendToWebSocket(NewWebSocketMessage.SetMaintainSymbolControlResponse(ErrorCodeHandler.MaintainAccountErrorMessage(ErrorCode)))
    End Sub

    Public Sub ReplaceOrder(DataObject As Object)
        Dim OrderString As String = DataObject("Order")
        Dim OldOrderRecordID As Integer = DataObject("OldOrderRecordID")
        Dim OldClientOrderID As String = DataObject("OldClientOrderID")

        Dim OrderStruct As structSTIOrder = StructConvert.ToSTIOrder(OrderString)
        Dim ErrorCode As Integer = TCSTIOrder.ReplaceOrderStruct(OrderStruct, OldOrderRecordID, OldClientOrderID)

        If (ErrorCode <> 0) Then
            SendToWebSocket(NewWebSocketMessage.SetOrderReject(ErrorCodeHandler.CreateOrderReject(ErrorCode, OrderStruct)))
        End If
    End Sub

    Private Sub SubmitOrder(DataObject As Object)
        Dim OrderStruct As structSTIOrder = StructConvert.ToSTIOrder(DataObject.ToString)
        Dim ErrorCode As Integer = TCSTIOrder.SubmitOrderStruct(OrderStruct)

        If (ErrorCode <> 0) Then
            SendToWebSocket(NewWebSocketMessage.SetOrderReject(ErrorCodeHandler.CreateOrderReject(ErrorCode, OrderStruct)))
        End If
    End Sub

    Public Sub CancelAllOrders(DataObject As Object)
        TCSTIOrderMaint.CancelAllOrders(StructConvert.ToSTICancelAll(DataObject.ToString))
    End Sub

    Public Sub CancelOrder(DataObject As Object)
        Dim Account As String = DataObject("Account")
        Dim OrderRecordID As Integer = DataObject("OrderRecordID")
        Dim OldClientOrderID As String = DataObject("OldClientOrderID")
        Dim ClientOrderID As String = DataObject("ClientOrderID")

        TCSTIOrderMaint.CancelOrder(Account, OrderRecordID, OldClientOrderID, ClientOrderID)
    End Sub

    Public Sub CancelOrderEx(DataObject As Object)
        Dim Account As String = DataObject("Account")
        Dim OrderRecordID As Integer = DataObject("OrderRecordID")
        Dim OldClientOrderID As String = DataObject("OldClientOrderID")
        Dim ClientOrderID As String = DataObject("ClientOrderID")
        Dim Instrument As String = DataObject("Instrument")

        TCSTIOrderMaint.CancelOrderEx(Account, OrderRecordID, OldClientOrderID, ClientOrderID, Instrument)
    End Sub

    Public Sub GetOrderInfo(DataObject As Object)
        Dim ClientOrderID As String = DataObject("ClientOrderID")

        SendToWebSocket(NewWebSocketMessage.SetOrderUpdate(TCSTIOrderMaint.GetOrderInfo(ClientOrderID)))
    End Sub

    Public Sub GetOrderList(DataObject As Object)
        Dim OpenOnly As Boolean = DataObject("OpenOnly")
        Dim OrderList() As structSTIOrderUpdate = Nothing

        TCSTIOrderMaint.GetOrderList(OpenOnly, OrderList)

        SendToWebSocket(NewWebSocketMessage.SetOrderList(OrderList))
    End Sub

    Public Sub GetOrderListEx(DataObject As Object)
        Dim OrderList() As structSTIOrderUpdate = Nothing

        TCSTIOrderMaint.GetOrderListEx(StructConvert.ToSTIOrderFilter(DataObject.ToString), OrderList)

        SendToWebSocket(NewWebSocketMessage.SetOrderList(OrderList))
    End Sub

    Public Sub GetTradeListEx(DataObject As Object)
        Dim TradeList() As structSTITradeUpdate = Nothing

        TCSTIOrderMaint.GetTradeListEx(StructConvert.ToSTITradeFilter(DataObject.ToString), TradeList)

        SendToWebSocket(NewWebSocketMessage.SetTradeList(TradeList))
    End Sub

    Public Sub GetPositionInfoStruct(DataObject As Object)
        Dim Symbol As String = DataObject("Symbol")
        Dim Exchange As String = DataObject("Exchange")
        Dim Account As String = DataObject("Account")

        SendToWebSocket(NewWebSocketMessage.SetPositionUpdate(TCSTIPosition.GetPositionInfoStruct(Symbol, Exchange, Account)))
    End Sub

    Public Sub GetPositionList()
        Dim PositionList() As structSTIPositionUpdate = Nothing
        TCSTIPosition.GetPositionList(PositionList)

        SendToWebSocket(NewWebSocketMessage.SetPositionList(PositionList))
    End Sub

    Public Sub GetPosListByAccount(DataObject As Object)
        Dim Account As String = DataObject("Account")

        Dim PositionList() As structSTIPositionUpdate = Nothing
        TCSTIPosition.GetPosListByAccount(Account, PositionList)

        SendToWebSocket(NewWebSocketMessage.SetPositionList(PositionList))
    End Sub

    Public Sub GetPosListBySym(DataObject As Object)
        Dim Symbol As String = DataObject("Symbol")

        Dim PositionList() As structSTIPositionUpdate = Nothing
        TCSTIPosition.GetPosListBySym(Symbol, PositionList)

        SendToWebSocket(NewWebSocketMessage.SetPositionList(PositionList))
    End Sub
#End Region
End Class