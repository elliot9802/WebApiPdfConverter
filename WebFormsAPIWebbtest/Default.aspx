<%@ Page Async="true" Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsAPIWebbtest._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:TextBox ID="txtUrl" runat="server" Width="300"></asp:TextBox>
<asp:Button ID="btnConvertUrl" runat="server" Text="Convert URL to PDF" OnClick="btnConvertUrl_Click" />
<br /><br />

<asp:Literal ID="litMessage" runat="server" />


</asp:Content>
