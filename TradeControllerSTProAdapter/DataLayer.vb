'
'DEPRECATED
'

Imports Newtonsoft.Json
Imports SterlingLib

Public Class DataLayer
    Public ServerTime As String = Nothing
    Public Sub SetServerTime(Time As String)
        ServerTime = Time
    End Sub


    Public Accounts() As String = Nothing
    Public Sub SetAccountList(AccountList() As String)
        Accounts = AccountList
    End Sub


    Public Traders() As String = Nothing
    Public Sub SetTraderList(TraderList() As String)
        Traders = TraderList
    End Sub


    Public Destinations() As String = Nothing
    Public Sub SetDestinationList(DestinationList() As String)
        Destinations = DestinationList
    End Sub


    Public MaintainAccountResponse As String = Nothing
    Public Sub SetMaintainAccountResponse(Response As String)
        MaintainAccountResponse = Response
    End Sub


    Public MaintainSymbolControlResponse As String = Nothing
    Public Sub SetMaintainSymbolControlResponse(Response As String)
        MaintainSymbolControlResponse = Response
    End Sub


    Private SerializeAccountUpdate As Boolean = False
    Public Function ShouldSerializeAccountUpdate() As Boolean
        Return SerializeAccountUpdate
    End Function
    Public AccountUpdate As structSTIAcctUpdate = Nothing
    Public Sub SetAccountUpdate(AcctUpdateStruct As structSTIAcctUpdate)
        SerializeAccountUpdate = True
        AccountUpdate = AcctUpdateStruct
    End Sub


    Private SerializePositionUpdate As Boolean = False
    Public Function ShouldSerializePositionUpdate() As Boolean
        Return SerializePositionUpdate
    End Function
    Public PositionUpdate As structSTIPositionUpdate = Nothing
    Public Sub SetPositionUpdate(PositionUpdateStruct As structSTIPositionUpdate)
        SerializePositionUpdate = True
        PositionUpdate = PositionUpdateStruct
    End Sub


    Private SerializePositions As Boolean = False
    Public Function ShouldSerializePositions() As Boolean
        Return SerializePositions
    End Function
    Public Positions() As structSTIPositionUpdate = Nothing
    Public Sub SetPositionList(PositionList() As structSTIPositionUpdate)
        SerializePositions = True
        Positions = PositionList
    End Sub


    Private SerializeOrders As Boolean = False
    Public Function ShouldSerializeOrders() As Boolean
        Return SerializeOrders
    End Function
    Public Orders() As structSTIOrderUpdate = Nothing
    Public Sub SetOrderList(OrderList() As structSTIOrderUpdate)
        SerializeOrders = True
        Orders = OrderList
    End Sub


    Private SerializeOrderConfirm As Boolean = False
    Public Function ShouldSerializeOrderConfirm() As Boolean
        Return SerializeOrderConfirm
    End Function
    Public OrderConfirm As structSTIOrderConfirm = Nothing
    Public Sub SetOrderConfirm(OrderConfirmStruct As structSTIOrderConfirm)
        SerializeOrderConfirm = True
        OrderConfirm = OrderConfirmStruct
    End Sub


    Private SerializeOrderUpdate As Boolean = False
    Public Function ShouldSerializeOrderUpdate() As Boolean
        Return SerializeOrderUpdate
    End Function
    Public OrderUpdate As structSTIOrderUpdate = Nothing
    Public Sub SetOrderUpdate(OrderUpdateStruct As structSTIOrderUpdate)
        SerializeOrderUpdate = True
        OrderUpdate = OrderUpdateStruct
    End Sub


    Private SerializeOrderReject As Boolean = False
    Public Function ShouldSerializeOrderReject() As Boolean
        Return SerializeOrderReject
    End Function
    Public OrderReject As structSTIOrderReject = Nothing
    Public Sub SetOrderReject(OrderRejectStruct As structSTIOrderReject)
        SerializeOrderReject = True
        OrderReject = OrderRejectStruct
    End Sub


    Private SerializeTrades As Boolean = False
    Public Function ShouldSerializeTrades() As Boolean
        Return SerializeTrades
    End Function
    Public Trades() As structSTITradeUpdate = Nothing
    Public Sub SetTradeList(TradeList() As structSTITradeUpdate)
        SerializeTrades = True
        Trades = TradeList
    End Sub


    Private SerializeTradeUpdate As Boolean = False
    Public Function ShouldSerializeTradeUpdate() As Boolean
        Return SerializeTradeUpdate
    End Function
    Public TradeUpdate As structSTITradeUpdate = Nothing
    Public Sub SetTradeUpdate(TradeUpdateStruct As structSTITradeUpdate)
        SerializeTradeUpdate = True
        TradeUpdate = TradeUpdateStruct
    End Sub


    Public ExceptionMessage As String = Nothing
    Public Sub SetExceptionMessage(Message As String)
        ExceptionMessage = Message
    End Sub


    Public Function ToJson() As String
        Dim Settings As New JsonSerializerSettings
        With Settings
            .NullValueHandling = NullValueHandling.Ignore
        End With

        Return JsonConvert.SerializeObject(Me, Settings)
    End Function
End Class
