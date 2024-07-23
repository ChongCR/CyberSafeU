<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="courseoverview.aspx.cs" Inherits="WebApplication1.courseoverview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container-fluid text-dark">
    <div class="col-lg-12 py-3">
        <center>
            <h3>Featured Courses</h3>
            <hr />
        </center>
    </div>

    <div class="row">
        <div class="col-lg-3"></div>

        <div class="col-lg-6">
            <div class="row justify-content-center">
                <asp:Repeater ID="rptCourses" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-4 mb-4">
                            <div class="card">
                                <a href='<%# "course.aspx?courseID=" + Eval("CourseID") %>' style="text-decoration: none; color: black">
                                    <img class="card-img-top" src='<%# Eval("ImageURL") %>' alt="Card image cap">
                                    <div class="card-body">
                                        <div>
                                            <hr />
                                        </div>
                                        <h5 class="card-title">
                                            <asp:Label ID="lblCourseName" runat="server" Text='<%# Eval("CourseName") %>'></asp:Label>
                                        </h5>
                                        <p>
                                            <asp:Label ID="lblCourseLevel" runat="server" Text='<%# Eval("CourseLevel") %>'></asp:Label>
                                        </p>
                                        <p>
                                            <asp:Label ID="lblCourseLanguage" runat="server" Text='<%# Eval("CourseLanguage") %>'></asp:Label>
                                        </p>
                                    </div>
                                    <div class="card-footer">
                                        <p>
                                            <asp:Label class="text-muted" ID="lblCourseCategory" runat="server" Text='<%# Eval("CourseCategory") %>'></asp:Label>
                                        </p>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div class="col-lg-3"></div> <!-- Empty space on the right -->
    </div>

    <blockquote class="blockquote text-center py-3">
        <p class="mb-0">"Security is not a product, but a process." </p>
        <footer class="blockquote-footer">- Bruce Schneier</footer>
    </blockquote>

    <!-- Other content goes here -->

</div>








</asp:Content>
