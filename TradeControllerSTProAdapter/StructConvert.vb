Imports SterlingLib

Public Class StructConvert
    Public Shared Function ToSTIAccountUpdate(AccountUpdate As String) As structSTIAcctUpdate
        Dim StringReader As New IO.StringReader(AccountUpdate)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTIAcctUpdate))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTISymbolControl(SymbolControl As String) As structSTISymbolControl
        Dim StringReader As New IO.StringReader(SymbolControl)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTISymbolControl))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTICancelAll(CancelAll As String) As structSTICancelAll
        Dim StringReader As New IO.StringReader(CancelAll)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTICancelAll))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTIOrder(Order As String) As structSTIOrder
        Dim StringReader As New IO.StringReader(Order)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTIOrder))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTIOrderFilter(OrderFilter As String) As structSTIOrderFilter
        Dim StringReader As New IO.StringReader(OrderFilter)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTIOrderFilter))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTIOrderConfirm(OrderConfirm As String) As structSTIOrderConfirm
        Dim StringReader As New IO.StringReader(OrderConfirm)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTIOrderConfirm))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTIOrderUpdate(OrderUpdate As String) As structSTIOrderUpdate
        Dim StringReader As New IO.StringReader(OrderUpdate)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTIOrderUpdate))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTIOrderReject(OrderReject As String) As structSTIOrderReject
        Dim StringReader As New IO.StringReader(OrderReject)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTIOrderReject))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTITradeFilter(TradeFilter As String) As structSTITradeFilter
        Dim StringReader As New IO.StringReader(TradeFilter)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTITradeFilter))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTITradeUpdate(TradeUpdate As String) As structSTITradeUpdate
        Dim StringReader As New IO.StringReader(TradeUpdate)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTITradeUpdate))

        Return XMLSerializer.Deserialize(StringReader)
    End Function

    Public Shared Function ToSTIPositionUpdate(PositionUpdate As String) As structSTIPositionUpdate
        Dim StringReader As New IO.StringReader(PositionUpdate)
        Dim XMLSerializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(GetType(structSTIPositionUpdate))

        Return XMLSerializer.Deserialize(StringReader)
    End Function
End Class
