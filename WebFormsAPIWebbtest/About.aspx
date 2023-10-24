<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebFormsAPIWebbtest.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <h3>Converting URL or HTML files to PDF files through an API</h3>
        <p>Converting the user entered URL or file to a new PDF file that the user can download, through the Syncfusion HtmlConverter library.</p>
    </main>
</asp:Content>
