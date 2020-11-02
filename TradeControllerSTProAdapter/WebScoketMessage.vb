Imports Newtonsoft.Json
Imports SterlingLib

Public Class WebSocketMessage
    <JsonProperty("event")>
    Public EventName As String = Nothing

    <JsonProperty("channel")>
    Public ChannelName As String = Nothing

    <JsonProperty("data")>
    Public Data As Object = Nothing

    <JsonProperty("servertime")>
    Public ServerTime As String = Nothing

    Public Sub New(Channel As String)
        ChannelName = Channel
    End Sub

    Public Sub SetServerTime(Time As String)
        ServerTime = Time
    End Sub

    Public Function SetMetadata(Metadata As Dictionary(Of String, String())) As WebSocketMessage
        EventName = "Metadata"
        Data = Metadata
        Return Me
    End Function

    Public Function SetAccountList(AccountList() As String) As WebSocketMessage
        EventName = "AccountList"
        Data = AccountList
        Return Me
    End Function

    Public Function SetTraderList(TraderList() As String) As WebSocketMessage
        EventName = "TraderList"
        Data = TraderList
        Return Me
    End Function

    Public Function SetDestinationList(DestinationList() As String) As WebSocketMessage
        EventName = "DestinationList"
        Data = DestinationList
        Return Me
    End Function

    Public Function SetMaintainAccountResponse(Response As String) As WebSocketMessage
        EventName = "MaintainAccountResponse"
        Data = Response
        Return Me
    End Function

    Public Function SetMaintainSymbolControlResponse(Response As String) As WebSocketMessage
        EventName = "MaintainSymbolControlResponse"
        Data = Response
        Return Me
    End Function

    Public Function SetAccountUpdate(AcctUpdateStruct As structSTIAcctUpdate) As WebSocketMessage
        EventName = "AccountUpdate"
        Data = AcctUpdateStruct
        Return Me
    End Function

    Public Function SetPositionUpdate(PositionUpdateStruct As structSTIPositionUpdate) As WebSocketMessage
        EventName = "PositionUpdate"
        Data = PositionUpdateStruct
        Return Me
    End Function

    Public Function SetPositionList(PositionList() As structSTIPositionUpdate) As WebSocketMessage
        EventName = "PositionList"
        Data = PositionList
        Return Me
    End Function

    Public Function SetOrderList(OrderList() As structSTIOrderUpdate) As WebSocketMessage
        EventName = "OrderList"
        Data = OrderList
        Return Me
    End Function

    Public Function SetOrderConfirm(OrderConfirmStruct As structSTIOrderConfirm) As WebSocketMessage
        EventName = "OrderConfirm"
        Data = OrderConfirmStruct
        Return Me
    End Function

    Public Function SetOrderUpdate(OrderUpdateStruct As structSTIOrderUpdate) As WebSocketMessage
        EventName = "OrderUpdate"
        Data = OrderUpdateStruct
        Return Me
    End Function

    Public Function SetOrderReject(OrderRejectStruct As structSTIOrderReject) As WebSocketMessage
        EventName = "OrderReject"
        Data = OrderRejectStruct
        Return Me
    End Function

    Public Function SetTradeList(TradeList() As structSTITradeUpdate) As WebSocketMessage
        EventName = "TradeList"
        Data = TradeList
        Return Me
    End Function

    Public Function SetTradeUpdate(TradeUpdateStruct As structSTITradeUpdate) As WebSocketMessage
        EventName = "TradeUpdate"
        Data = TradeUpdateStruct
        Return Me
    End Function

    Public Function SetExceptionMessage(Message As String) As WebSocketMessage
        EventName = "ExceptionMessage"
        Data = Message
        Return Me
    End Function

    Public Function SetMessage(Message As String) As WebSocketMessage
        EventName = "Message"
        Data = Message
        Return Me
    End Function

    Public Function ToJson() As String
        Dim Settings As New JsonSerializerSettings
        With Settings
            .NullValueHandling = NullValueHandling.Ignore
        End With

        Return JsonConvert.SerializeObject(Me, Settings)
    End Function
End Class
