<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="coursemanagement.aspx.cs" Inherits="WebApplication1.coursemanagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

          <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>

    <script src="path_to_jquery/jquery.min.js"></script>

    <style>

       

  .card {
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
    transition: 0.3s;
    border-radius: 10px;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  }
  .card:hover {
    box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
  }
  .card-title {
    font-size: 1.5rem;
    color: #333;
  }
  .info-section {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.5rem 0;
  }
  .info-label {
    font-weight: bold;
    margin-right: 1rem;
    color: #555;
  }
  .info-content {
    flex-grow: 1;
    text-align: right;
    color: #777;
  }
 
    .file-upload-wrapper {
        position: relative;
        overflow: hidden;
        display: inline-block;
    }
    .file-upload-wrapper .file-upload-input {
        position: absolute;
        top: 0;
        right: 0;
        margin: 0;
        padding: 0;
        font-size: 20px;
        cursor: pointer;
        opacity: 0;
        height: 100%;
        width: 100%;
    }

    .info-box {
        padding: 10px;
        margin-bottom: 15px;
        border: 1px solid #ffeeba;
        background-color: #fff3cd;
        color: #856404;
        border-radius: 4px;
        text-align: center;
        font-style: italic;
    }
</style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container m-3">
            <!-- Back button -->
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary w-25" Text="Back to Admin Home Page" OnClick="btnBackDashboard_Click" />
        </div>
     <div class="container-fluid text-dark align-items-center text-center mt-5">
  <div class="row justify-content-center">
    <div class="col-md-10 offset-md-1">
      <div class="card h-100">
        <div class="card-body">
            <h4 class="text-center">List of Courses</h4>
            <hr>

            <div class="text-right mt-3 mb-2"> 
                <button id="btnRefreshCourses" runat="server" class="btn btn-secondary" onserverclick="btnRefreshCourses_ServerClick">
                    <i class="fas fa-sync"></i>
                </button>

              <asp:Button ID="btnShowCreateCourseModal" runat="server" Text="Create New Course" 
                CssClass="btn btn-success mr-1" OnClick="btnShowCreateCourseModal_Click"/>

            </div>

            <asp:TextBox ID="txtSearchCourse" runat="server" CssClass="form-control" 
                AutoPostBack="true" OnTextChanged="txtSearchCourse_TextChanged"
                placeholder="Search by Course Code or Name"></asp:TextBox>

         <asp:GridView ID="GridViewCourses" runat="server" CssClass="table table-bordered" 
              AutoGenerateColumns="False" EmptyDataText="No courses found."
              DataKeyNames="CourseCode" AllowPaging="True" PageSize="5"
              OnPageIndexChanging="GridViewCourses_PageIndexChanging" 
              OnRowDataBound="GridViewCourses_RowDataBound"
              OnRowCommand="GridViewCourses_RowCommand"
              AllowSorting="True" OnSorting="GridViewCourses_Sorting">
    <PagerStyle CssClass="grid-view-pager" />
                    <Columns>
                        <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CourseCode" HeaderText="Code" SortExpression="CourseCode" />
                        <asp:BoundField DataField="CourseName" HeaderText="Name" SortExpression="CourseName" />
                        <asp:BoundField DataField="CourseCategory" HeaderText="Category" SortExpression="CourseCategory" />
                        <asp:BoundField DataField="CourseLevel" HeaderText="Level" SortExpression="CourseLevel" />
                        <asp:BoundField DataField="CourseLanguage" HeaderText="Language" SortExpression="CourseLanguage" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                   <asp:LinkButton ID="ViewButton" runat="server" CommandName="View" Text="View"
                CssClass="btn btn-info" CommandArgument='<%# Eval("CourseCode") %>' />
            &nbsp;
            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-warning" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
            &nbsp;
            <asp:Button ID="MarkAsCorrectButton" runat="server" Text="Approve" 
                CssClass="btn btn-success" OnClick="MarkAsCorrect_Click" CommandArgument='<%# Eval("CourseCode") %>' visible="false" />
            &nbsp;
            <asp:Button ID="MarkAsFalseButton" runat="server" Text="Reject" 
                CssClass="btn btn-danger" OnClick="MarkAsFalse_Click" CommandArgument='<%# Eval("CourseCode") %>' visible="false"/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle BackColor="#CCCCCC" />
                </asp:GridView>

                </div>
            </div>
        </div>


    
       
    </div>

         </div>
            <script type="text/javascript">



        function showImageModal(imageUrl) {
           
            $('#imgModalContent').attr('src', imageUrl);

           
            $('#imageModal').modal('show');
        }

   
        function showImage(url) {
         
            if (!url.match(/\.(jpeg|jpg|gif|png|bmp)$/i)) {
                url += '.jpg'; 
            }

            document.getElementById('imgViewModal').src = url;
            $('#imageViewModal').modal('show');
        }

        Swal.fire({
            title: title,
            text: message,
            icon: type,
            confirmButtonText: 'Ok'

        });

        function ShowVideoUploadModal() {
            $('#videoListModal').modal('show');
        }

        function showSuccessMessage() {
            Swal.fire({
                icon: "success",
                title: "Data refreshed!",
                timer: 1200,
                timerProgressBar: true,
                position: "top-end",
                showConfirmButton: false
            });
        }

            </script>

</asp:Content>
