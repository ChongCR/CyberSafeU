<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" enableEventValidation="true" CodeBehind="course.aspx.cs" Inherits="WebApplication1.course" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>

        /* CSS in your <style> tag or external stylesheet */
.content-title {
    /* other styles */
    margin-bottom: 1rem; /* Adds space below the title */
}

.content-image, .embed-responsive {
    margin-bottom: 1rem; /* Adds space below image and video */
}

.content-text {
    padding-top: 1rem; /* Adds space above the text */
}


     #contentDisplay {
    background: #f9f9f9;
    border-radius: 8px;
    padding: 20px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
    margin-bottom: 30px;
    }

    /* Title styling */
    .content-title {
        font-size: 24px;
        color: #333;
        margin-bottom: 15px;
    }

    /* Image styling */
    .content-image {
        max-width: 50%; /* Make image smaller */
        height: auto;
        margin-bottom: 15px;
        display: inline-block; /* To center in the div */
    }

    /* Video styling */
    .content-video {
        max-width: 100%;
        height: auto;
        margin-bottom: 15px;
        display: block;
        margin-left: auto;
        margin-right: auto;
    }

    /* Text content styling */
    .content-text {
        font-size: 16px;
        line-height: 1.6;
        color: #555;
        text-align: justify; /* Justify the text content for a cleaner look */
    }

        /* Sidebar */
        .sidebar {
            position: fixed;
            top: 0;
            bottom: 150px;
            left: 0;
            padding: 58px 0 0; /* Height of navbar */
           
            width: 240px;
            z-index: 600;
        }

        @media (max-width: 991.98px) {
            .sidebar {
                width: 100%;
            }
        }
            .content-item {
                width: 100%;
                text-align: left;
                margin: 0 !important;
                border: none;
                background-color: #f8f8f8;
                color: #333;
                border-radius: 4px;
                margin-bottom: 5px; /* Spacing between items */
            }

            .content-item:hover {
                background-color: #e7e7e7;
            }
            .card-body {
                padding: 0 !important;
            }


      
    </style>

    <div class="container-fluid text-dark">
        <!-- Sidebar -->
       <nav id="sidebarMenu" class="collapse d-lg-block sidebar collapse bg-white">
    <div class="list-group list-group-flush mx-3 mt-4">
        <button class="btn btn-light" type="button" data-toggle="collapse" 
                data-target=".multi-collapse" aria-expanded="false" 
                aria-controls="multiCollapseExample1 multiCollapseExample2 multiCollapseExample3 multiCollapseExample4">
                <asp:Literal ID="litCourseTitle" runat="server"></asp:Literal>
        </button>

     
      <div id="accordion">
         <asp:Repeater ID="rptModules" runat="server" OnItemDataBound="rptModules_ItemDataBound">

                <ItemTemplate>
                    <div class="card">
                        <div class="card-header" id='heading<%# Eval("ModuleNo") %>'>
                            <h5 class="mb-0">
                                <button class="btn btn-link" type="button" data-toggle="collapse" data-target='#collapse<%# Eval("ModuleNo") %>' aria-expanded="true" aria-controls='collapse<%# Eval("ModuleNo") %>'>
                                    Module <%# Eval("ModuleNo") %>
                                </button>
                            </h5>
                        </div>
                        <div id='collapse<%# Eval("ModuleNo") %>' class="collapse" aria-labelledby='heading<%# Eval("ModuleNo") %>' data-parent="#accordion">
                            <div class="card-body">
                                <asp:Repeater ID="rptModuleContent" runat="server" DataSource='<%# Eval("ModuleContents") %>'>
                                    <ItemTemplate>
                                        <ul class="list-group">
                                            <li class="list-group-item">
                                                <asp:LinkButton ID="lnkModuleContent" runat="server" 
                                                    CommandArgument='<%# Eval("ModuleContentID") %>' 
                                                    CommandName="SelectContent" 
                                                    Text='<%# Eval("ModuleContentTitle") %>' />
                                            </li>
                                        </ul>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div class="card">
                <div class="card-body">
                    <asp:Button ID="btnAssessment" runat="server" CssClass="btn btn-danger btn-block" CommandName="ChangeView" CommandArgument="2" OnCommand="btnAssessment_Command" Text="Assessment" />
                </div>
            </div>




        </div>





    </div>
</nav>
        <!-- Sidebar -->

         <asp:MultiView ID="multiView" runat="server">

        <asp:View ID="view1" runat="server">
                     <main style="margin-top: 30px;">
                <div class="container pt-4">
                    <div class="mt-4" id="contentDisplay" runat="server">
                        <h2 id="lblModuleContentTitle" runat="server" class="text-center content-title"></h2>
                         <hr>
                        <div class="text-center">
                            <asp:Image ID="imgModuleContentImage" runat="server" CssClass="img-fluid content-image" Visible="false" />
                        </div>
                         <hr>
                       <div class="embed-responsive embed-responsive-16by9">
                            <iframe id="iframeVideo" runat="server" class="embed-responsive-item" allowfullscreen></iframe>
                        </div>
                         <hr>
                        <div class="content-text">
                            <asp:Literal ID="ltModuleContentText" runat="server" Text=""></asp:Literal>
                        </div>
                    </div>
                </div>
            </main>

        </asp:View>


                <asp:View ID="view2" runat="server">
                     <main style="margin-top: 30px;">
                <div class="container pt-4 text-center">
                                <asp:GridView ID="gvAssessmentList" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowCommand="gvAssessmentList_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="AssessmentID" HeaderText="Assessment ID" />
                        <asp:BoundField DataField="AssessmentTitle" HeaderText="Assessment Title" />
                        <asp:BoundField DataField="AssessmentType" HeaderText="Assessment Type" />
                        <asp:BoundField DataField="ModuleNo" HeaderText="Module No" />
                        <asp:TemplateField HeaderText="First Try Score">
                            <ItemTemplate>
                                <%# Eval("FirstTryScore") != DBNull.Value ? String.Format("{0:0.##}%", Eval("FirstTryScore")) : "N/A" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Second Try Score">
                            <ItemTemplate>
                                <%# Eval("SecondTryScore") != DBNull.Value ? String.Format("{0:0.##}%", Eval("SecondTryScore")) : "N/A" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <%# GetCourseStatus(Eval("FirstTryScore"), Eval("SecondTryScore")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnViewAssessment" runat="server" CommandName="ViewAssessment" 
                                            CommandArgument='<%# Bind("AssessmentID") %>' Text="Begin" 
                                            CssClass="btn btn-primary" 
                                            Enabled='<%# Eval("FirstTryScore") == DBNull.Value || Eval("SecondTryScore") == DBNull.Value %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>



                </div>

                       

            </main>

        </asp:View>



      </asp:MultiView>
  



   
    </div>


    <script type="text/javascript">
    function toggleContent(header) {
        // Find the next sibling element which is the div containing the module content
        var contentDiv = header.nextElementSibling;

        // Toggle the display of the div
        if (contentDiv.style.display === "none") {
            contentDiv.style.display = "block";
        } else {
            contentDiv.style.display = "none";
        }
    }
    </script>


</asp:Content>
