<%@ Page Async="true" Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsAPIWebbtest._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Section for URL to PDF conversion -->
    <div>
        <asp:Label ID="lblUrl" runat="server" Text="Enter URL:"></asp:Label>
        <asp:TextBox ID="txtUrl" runat="server" Width="300"></asp:TextBox>
        <asp:Button ID="btnConvertUrl" runat="server" Text="Convert URL to PDF" OnClick="btnConvertUrl_Click" />
    </div>
    <br /><br />

    <!-- Section for HTML to PDF conversion -->
    <div>
        <asp:Label ID="lblHtml" runat="server" Text="Upload HTML file:"></asp:Label>
        <asp:FileUpload ID="htmlFileUpload" runat="server" />
        <asp:Button ID="btnConvertHtml" runat="server" Text="Convert HTML to PDF" OnClick="btnConvertHtml_Click" />
    </div>
    <br /><br />

    <asp:Literal ID="litMessage" runat="server" />

</asp:Content>
