<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ViewCourse.aspx.cs" Inherits="WebApplication1.ViewCourse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <style>
        .course-container {
            display: flex;
            align-items: center;
            justify-content: center;
            margin-top: 20px;
        }
        .image-container {
            width: 50%;
            padding-right: 10px; 
        }
        .details-container {
            width: 50%;
            padding-left: 10px; 
        }
        .detail-item {
            display: flex;
            align-items: center;
            margin-bottom: 10px; 
        }
        .detail-label {
            font-weight: bold;
            margin-right: 5px; 
        }
        img.img-course {
            width: 100%;
            max-width: 400px;
            height: auto;
        }

        /* Custom CSS for nested GridView */
            .nested-table td {
                padding: 0.75rem;
                border: none; /* Removes the border from nested GridView cells */
            }

            /* Custom CSS for the main GridView */
            .gv-modules-header {
                background-color: #f7f7f7; /* Bootstrap-like header background */
            }

            .gv-modules-row, .gv-modules-alternating-row {
                border: 1px solid #ddd; /* Bootstrap-like row borders */
            }

            /* Additional Bootstrap styling adjustments */
            .table-hover tbody tr:hover {
                background-color: #f5f5f5; /* Bootstrap-like hover color */
            }


    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div class="container mt-4 text-dark">
        <div class="card">
            <div class="card-header text-center">
                Course Details
            </div>
            <div class="card-body">
                <div class="row">
                    <!-- Image container taking up 6 columns (50%) -->
                   <div class="col-6">
                    <asp:Image ID="imgCourseImage" runat="server" CssClass="img-fluid" alt="Course Image" />
                </div>

                    <!-- Details container also taking up 6 columns (50%) -->
                    <div class="col-2 text-end">
                        <p><strong>Course Title:</strong></p>
                        <p><strong>Code:</strong> </p>
                        <p><strong>Category:</strong> </p>
                        <p><strong>Level:</strong> </p>
                        <p><strong>Language:</strong> </p>
                        <p><strong>Status:</strong> </p>
                        <p><strong>Creation Date:</strong> </p>
                        <p><strong>Instructor ID:</strong> </p>
                        <p><strong>Required Hours:</strong> </p>
                    </div>
                      <div class="col-4">
                        <p> <span id="lblCourseName" runat="server"></span></p>
                        <p><span id="lblCourseCode" runat="server"></span></p>
                        <p> <span id="lblCourseCategory" runat="server"></span></p>
                        <p> <span id="lblCourseLevel" runat="server"></span></p>
                        <p> <span id="lblCourseLanguage" runat="server"></span></p>
                        <p><span id="lblStatus" runat="server"></span></p>
                        <p> <span id="lblCreationDate" runat="server"></span></p>
                        <p> <span id="lblInstructorID" runat="server"></span></p>
                        <p> <span id="lblRequiredHours" runat="server"></span></p>
                    </div>


                </div>
            </div>
        </div>
    </div>

    <div class="container mt-4 text-dark">
            <div class="card">
                <div class="card-header text-center">
                    Course Modules
                </div>
                <div class="card-body">
                     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                              DataKeyNames="CourseModuleID, AssociationCriteria"  
                              OnRowCommand="GridView1_RowCommand" 
                              CssClass="table table-bordered table-hover justify-content-center text-center">
                    <Columns>
                        <asp:TemplateField HeaderText="No">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CourseModuleNo" HeaderText="Module Number" />
                        <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                        <asp:BoundField DataField="ModuleContentTitle" HeaderText="Module Title" />
                        <asp:BoundField DataField="ModuleContentType" HeaderText="Module Type" />
                        <asp:BoundField DataField="AssociationCriteria" HeaderText="Association Criteria" Visible="False" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <div class="btn-group" role="group">                                                                 
                                 <asp:LinkButton ID="lnkView" runat="server" CommandName="View" Text="View" 
                                        CssClass="btn btn-info" CommandArgument='<%# Bind("AssociationCriteria") %>' />
                    
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
            </div>
        </div>
   

     <div class="container mt-4 text-dark">
            <div class="card">
                <div class="card-header text-center">
                    Course Modules
                </div>
                <div class="card-body">

                     <asp:GridView ID="GridViewAssessment" runat="server"  ViewStateMode="Enabled" AutoGenerateEditButton="false" AutoGenerateColumns="False"
                             DataKeyNames="CourseAssessmentID" OnRowCommand="GridViewAssessment_RowCommand" CssClass="table table-bordered table-hover justify-content-center  text-center">
                            <Columns>
                              
                                <asp:TemplateField HeaderText="No">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
        
                             
                                <asp:BoundField DataField="CourseAssessmentID" HeaderText="Assessment ID" SortExpression="AssessmentID" />
                                <asp:BoundField DataField="CourseCode" HeaderText="Course Code" SortExpression="CourseCode" />
                                  <asp:BoundField DataField="ModuleContentTitle" HeaderText="Module Content Title" />
                                <asp:BoundField DataField="AssessmentType" HeaderText="Assessment Type" />                               
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <div class="btn-group" role="group">                                        
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="View" Text="View"  CommandArgument='<%# Bind("CourseAssessmentID") %>' CssClass="btn btn-info" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>



                </div>
            </div>

           <div class="card-footer">
            <asp:Button ID="Button3" runat="server" Text="Back" CssClass="btn btn-secondary btn-block" OnClick="btnBack_Click" />
        </div>

        </div>

        




            <div class="modal fade" id="moduleContentModal" tabindex="-1" role="dialog" aria-labelledby="moduleContentModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content text-dark text-center">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h5 class="modal-title" id="moduleContentModalLabel">Module Content Details</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <!-- Modal Body -->
                        <div class="modal-body">
                            <!-- Module Content Title -->
                            <div class="form-group">
                                <label for="moduleContentTitle"><b>Module Content Title:</b></label>
                                <asp:Label ID="lblModuleContentTitle" runat="server" CssClass="form-control" />
                            </div>
                             <!-- Course Module No -->
                            <div class="form-group">
                                <label for="courseModuleNo"><b>Course Module No:</b></label>
                                <asp:Label ID="lblCourseModuleNo" runat="server" CssClass="form-control" />
                            </div>
                            <!-- Course Code -->
                            <div class="form-group">
                                <label for="courseCode"><b>Course Code:</b></label>
                                <asp:Label ID="Label1" runat="server" CssClass="form-control" />
                            </div>
                            <!-- Module Content Type -->
                            <div class="form-group">
                                <label for="moduleContentType"><b>Module Content Type:</b></label>
                                <asp:Label ID="lblModuleContentType" runat="server" CssClass="form-control" />
                            </div>
                            <!-- Module Content Video -->
                            <div class="form-group">
                                <label for="moduleContentVideo"><b>Module Content Video:</b></label>
                                <div runat="server" id="divModuleContentVideo" visible="false"></div>
                            </div>
                            <!-- Module Content Image -->
                            <div class="form-group">
                                <label for="moduleContentImage"><b>Module Content Image:</b></label>
                                <asp:Image ID="imgModuleContentImage" runat="server" CssClass="img-fluid" style="max-width: 100%; margin-bottom:5px; max-height: 300px; width: auto; height: auto; display: block;" />
                            </div>
                            <!-- Module Content Text -->
                            <div class="form-group">
                                <label for="moduleContentText"><b>Module Content Text:</b></label>
                                <div runat="server" id="divModuleContentText" class="form-control" style="padding-bottom:500px; overflow-y: auto;"></div>
                            </div>
                           
                        </div>
                        <!-- Modal Footer -->
                        <div class="modal-footer">
                            <asp:Button ID="btnCloseModal" runat="server" Text="Close" CssClass="btn btn-secondary" data-dismiss="modal" />
                            <!-- Optionally, add more buttons here -->
                        </div>
                    </div>
                </div>
            </div>


                    <!-- Modal Structure -->
                <div class="modal fade" id="detailsModal" tabindex="-1" role="dialog" aria-labelledby="detailsModalLabel" aria-hidden="true">
                  <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                      <div class="modal-header">
                        <h5 class="modal-title" id="detailsModalLabel">Module Content Details</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">&times;</span>
                        </button>
                      </div>
                      <div class="modal-body">
                            <asp:GridView ID="GridViewQuestions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="GridViewQuestions_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Question" HeaderText="Question" />
                                <asp:TemplateField HeaderText="Answers">
                                    <ItemTemplate>
                                        <div class="list-group">
                                            <asp:Repeater ID="RepeaterAnswers" runat="server">
                                               <ItemTemplate>
                                                    <div class="list-group-item d-flex justify-content-between align-items-center">
                                                        <%# DataBinder.Eval(Container.DataItem, "AnswerName") %>
                                                        <span class="badge <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCorrect")) ? "badge-success" : "badge-danger" %>">
                                                            <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCorrect")) ? "Correct" : "Incorrect" %>
                                                        </span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                      </div>
                    </div>
                  </div>
                </div>




                      





    <script>




    </script>
</asp:Content>
