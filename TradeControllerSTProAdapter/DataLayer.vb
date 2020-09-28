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


    Public MaintainSymbolControlResponse As Integer = Nothing
    Public Sub SetMaintainSymbolControlResponse(Code As Integer)
        MaintainSymbolControlResponse = Code
    End Sub


    Public AccountUpdate As structSTIAcctUpdate = Nothing
    Public Sub SetAccountUpdate(AcctUpdateStruct As structSTIAcctUpdate)
        AccountUpdate = AcctUpdateStruct
    End Sub


    Public PositionUpdate As structSTIPositionUpdate = Nothing
    Public Sub SetPositionUpdate(PositionUpdateStruct As structSTIPositionUpdate)
        PositionUpdate = PositionUpdateStruct
    End Sub


    Public Positions() As structSTIPositionUpdate = Nothing
    Public Sub SetPositionList(PositionList() As structSTIPositionUpdate)
        Positions = PositionList
    End Sub


    Public Orders() As structSTIOrderUpdate = Nothing
    Public Sub SetOrderList(OrderList() As structSTIOrderUpdate)
        Orders = OrderList
    End Sub


    Public OrderConfirm As structSTIOrderConfirm = Nothing
    Public Sub SetOrderConfirm(OrderConfirmStruct As structSTIOrderConfirm)
        OrderConfirm = OrderConfirmStruct
    End Sub


    Public OrderUpdate As structSTIOrderUpdate = Nothing
    Public Sub SetOrderUpdate(OrderUpdateStruct As structSTIOrderUpdate)
        OrderUpdate = OrderUpdateStruct
    End Sub


    Public OrderReject As structSTIOrderReject = Nothing
    Public Sub SetOrderReject(OrderRejectStruct As structSTIOrderReject)
        OrderReject = OrderRejectStruct
    End Sub


    Public Trades() As structSTITradeUpdate = Nothing
    Public Sub SetTradeList(TradeList() As structSTITradeUpdate)
        Trades = TradeList
    End Sub


    Public TradeUpdate As structSTITradeUpdate = Nothing
    Public Sub SetTradeUpdate(TradeUpdateStruct As structSTITradeUpdate)
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
