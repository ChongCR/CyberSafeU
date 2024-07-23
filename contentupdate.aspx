<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="contentupdate.aspx.cs" Inherits="WebApplication1.contentupdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="path_to_bootstrap_css/bootstrap.min.css" rel="stylesheet">
<script src="path_to_bootstrap_js/bootstrap.min.js"></script>
    
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
  .btn {
    padding: 0.5rem 1rem;
    border-radius: 0.25rem;
    font-size: 1rem;
    margin-top: 10px;
    width: 100%;
    text-transform: uppercase;
  }
  .btn-info {
    background-color: #17a2b8;
    border: none;
    color: white;
  }
  .btn-info:hover {
    background-color: #138496;
  }
  .btn-secondary {
    background-color: #6c757d;
    border: none;
    color: white;
  }
  .btn-secondary:hover {
    background-color: #545b62;
  }
  .button-group button {
    transition: background-color 0.3s ease;
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
    <div class="container-fluid text-dark mt-5">
        <div class="container m-3">
            <!-- Back button -->
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary w-25" Text="Back to Admin Home Page" OnClick="btnBackDashboard_Click" />
        </div>

     <div class="row">
           <div class="col-md-5">
            <div class="card h-100">
                <div class="card-body">
                    <div class="row">
                         <div class="col">
                             <center>
                                  <h4>Content Update</h4>
                             </center>
                         </div>
                    </div>

                    <div class="row">
                         <div class="col">
                             <center>
                                
                                 <img width="100px" src="imgs/contentupdate.png" />
                             </center>
                         </div>
                    </div>

                       <div class="row">
                         <div class="col">
                             <hr>
                         </div>
                    </div>

                 
                

               

                            

                    

                    <div class="row">
                         <div class="col-md-3">
                              <label>Content ID</label>
                            <div class="form-group">
                                <div class="input-group">
                                     <asp:TextBox Cssclass="form-control mr-1" ID="content_id_Details" runat="server" placeholder="Content ID" Enabled ="false"></asp:TextBox>
                                       
 
                                </div>
                               
                            </div>
                         </div>
                        
                        <div class="col-md-9">
                              <label>Content Title</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="content_title_Details" 
                                    runat="server" placeholder="Content Title" Enabled ="false"></asp:TextBox>
                            </div>
                         </div>

                         
                        
                    </div>


                     <div class="row">

                         <div class="col-md-4">
                              <label>Language</label>
                            <div class="form-group">
                                <asp:DropDownList  Cssclass="form-control" ID="language" runat="server" Enabled ="false">
                                    <asp:ListItem Text="English" Value="English" />
                                     <asp:ListItem Text="Chinese" Value="Chinese" />
                                     <asp:ListItem Text="Malay" Value="Malay" />

                                </asp:DropDownList>
                            </div>               
                         </div>



                         <div class="col-md-4">
                            <label>Category</label>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" ID="content_catDetails" runat="server" Enabled ="false">
                                    <asp:ListItem Text="News" Value="News" />
                                    <asp:ListItem Text="Article" Value="Article" />
                                    <asp:ListItem Text="Reference Material" Value="Reference Material" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <label>Item Type</label>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" ID="content_typeDetails" runat="server" Enabled ="false">
                                    <asp:ListItem Text="Thesis" Value="Thesis" />
                                    <asp:ListItem Text="Research Paper" Value="Research Paper" />
                                </asp:DropDownList>
                             
                            </div>
                        </div>

                    </div>



                        <div class="row">
                         <div class="col-12">

                              <label for="author_name_Details">Author Name</label>
                                <asp:TextBox CssClass="form-control" ID="author_name_Details" runat="server" placeholder="Author Name" Enabled ="false"></asp:TextBox>

                             
                         </div>

                           
                    </div>
                   
                    
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                             <label>Content Description</label>
                             <div class="form-group">
                                 <asp:TextBox Cssclass="form-control" ID="content_desc_Details" runat="server" placeholder="Content Description" TextMode="MultiLine" Enabled ="false"></asp:TextBox>
                             </div>
                            </div>
                            <div class="form-group">
                                <asp:Button ID="Button5" runat="server" Text="Content Image List" CssClass="btn btn-primary btn-block btn-sm" OnClick="btnUploadImage_Click" Enabled ="false"/>
                            </div> 
                            <div class="form-group">
                                <asp:Button ID="btnUploadVideo" runat="server" Text="Content Video List" CssClass="btn btn-secondary btn-block btn-sm" OnCLick="btnUploadVideo_Click" Enabled ="false" />
                            </div>
                        </div>
                    </div>


                   <div class="row">
                        <div class="col-6">
                            <asp:Button CssClass="btn btn-warning btn-block" ID="Button2" runat="server" Text="Update" Enabled="false" OnClick="btnUpdate_Click"/>
                        </div>
                        <div class="col-6">
                          <asp:Button CssClass="btn btn-danger btn-block" ID="Button3" runat="server" Text="Delete" OnClick="btnDeleteContent_Click" Enabled="false"  OnClientClick="return confirm('Are you sure you want to delete whole material?');"/>

                        </div>
                    </div>
                   

                </div>
            </div>

          

        </div>
         <div class="col-md-7"">
                <div class="card h-100">
                    <div class="card-body">
                        <h4 class="text-center">List of Reference Materials</h4>
                        <hr>

                        <div class="text-right mt-3 mb-2"> 
                           <button id="btnRefresh" runat="server" class="btn btn-secondary" onserverclick="btnRefresh_ServerClick" >
                            <i class="fas fa-sync"></i>
                        </button>


                            <asp:Button ID="btnShowModal" runat="server" Text="Create New Reference Material" 
                                CssClass="btn btn-success mr-1"  OnClick="btnShowCreateModal_Click"/>
                        </div>

                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                            AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"
                            placeholder="Search by Title or Author"></asp:TextBox>

                        <asp:GridView ID="GridViewRefMaterial" runat="server" CssClass="table table-bordered" 
                            AutoGenerateColumns="False" EmptyDataText="No reference material found."
                            DataKeyNames="content_id" AllowPaging="True" PageSize="5"
                            OnPageIndexChanging="GridViewRefMaterial_PageIndexChanging" OnRowDataBound="GridViewRefMaterial_RowDataBound"
                            AllowSorting="True" OnSorting="GridViewRefMaterial_Sorting"
                            OnSelectedIndexChanged="GridViewRefMaterial_SelectedIndexChanged">
                            <PagerStyle CssClass="grid-view-pager" />
                            <Columns>
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="content_title" HeaderText="Title" SortExpression="content_title" />
                                <asp:BoundField DataField="language" HeaderText="Language" SortExpression="language" />
                                <asp:BoundField DataField="author_name" HeaderText="Author" SortExpression="author_name" />
                                <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                       <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select" Text="Select" CssClass="btn btn-primary" />
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


        <div class="modal fade " id="imageListModal" tabindex="-1" role="dialog" aria-labelledby="imageListModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-xl" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="imageListModalLabel">Image List</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body text-center">
                                     <asp:Label ID="lblNoImages" runat="server" Text="" CssClass="text-danger"></asp:Label>
                                    <asp:GridView ID="GridViewImages" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"  DataKeyNames="Image_id"  OnRowCommand="GridViewImages_RowCommand" EmptyDataText="No images found.">
                                        <Columns>
                                             <asp:TemplateField HeaderText="No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ImageURL" HeaderText="Image URL" />
                                            <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:TemplateField HeaderText="Action">
                                         <ItemTemplate>
                                            <div class="btn-group" role="group" aria-label="Basic example">
                                                <button type="button" onclick="event.preventDefault(); showImage('<%# Eval("ImageURL") %>');" class="btn btn-primary">View</button>
                                                <asp:Button ID="btnDeleteImage" runat="server" Text="Delete" CommandName="DeleteImage" CommandArgument='<%# Eval("Image_id") %>' CssClass="btn btn-danger" OnClientClick="return confirm('Are you sure you want to delete this image?');" />
                                            </div>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="modal-footer">
                            <div class="input-group mb-3">
                                <div class="custom-file">
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="custom-file-input" accept="image/*" />
                                    <label class="custom-file-label" for="FileUpload1">Choose file</label>
                                </div>
                                 <asp:Button ID="Button4" runat="server" Text="Upload Image" CssClass="btn btn-primary  " OnClick="btnUploadImageModal_Click"  />
                                  <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            </div>
                                  
                                </div>
                            </div>
                        </div>
                    </div>


                   <!-- Modal for Displaying the Full Image -->
                    <div class="modal fade" id="imageViewModal" tabindex="-1" role="dialog" aria-labelledby="imageViewModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="imageViewModalLabel">Image View</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body text-center">
                                    <img id="imgViewModal" src="#" class="img-fluid" alt="Image"/>
                                </div>
                            </div>
                        </div>
                    </div>




                   <div class="modal fade" id="videoListModal" tabindex="-1" role="dialog" aria-labelledby="videoListModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="videoListModalLabel">Video List</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body text-center">
                                    <asp:GridView ID="GridViewVideos" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" DataKeyNames="video_id" OnRowCommand="GridViewVideos_RowCommand" EmptyDataText="No video found.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Video URL">
                                                <ItemTemplate>
                                                    <a href='<%# Eval("VideoURL") %>' target="_blank"><%# Eval("VideoURL") %></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>                                                   
                                                    <asp:Button ID="btnDeleteVideo" runat="server" Text="Delete" CommandName="DeleteVideo" CommandArgument='<%# Eval("video_id") %>' CssClass="btn btn-danger" OnClientClick="return confirm('Are you sure you want to delete this video?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <hr />
                                    <div class="form-group">
                                        <label for="txtVideoLink">Video Link:</label>
                                        <input type="text" id="txtVideoLink" runat="server" class="form-control" placeholder="Enter video link" />
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnAddVideoLink" runat="server" Text="Add Video Link" CssClass="btn btn-primary" OnClick="btnAddVideoLink_Click" />
                                    <button type="button" class="btn btn-secondary ml-3" data-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>



               

                    <!-- The Modal -->
                   
                             <div class="modal fade" id="createMaterialModal" tabindex="-1" role="dialog" aria-labelledby="addNewContentModalLabel" aria-hidden="true">
                                        <div class="modal-dialog modal-lg" role="document">
                                            <div class="modal-content text-dark">
                                                <div class="modal-header">
                                                    <h5 class="modal-title" id="addNewContentModalLabel">Create New Content</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                     <ContentTemplate>
                                                <div class="modal-body">                                                 
                                                     <p class="info-box">This is just a shortcut for creating a material. Please modify the details after creating.</p>
                                                    <div class="form-group">
                                                        <label for="txtNewContentTitle">Content Title</label>
                                                        <asp:TextBox ID="txtNewContentTitle2" runat="server" CssClass="form-control" placeholder="Content Title" />
                                                           <asp:Label ID="lblErrorContentTitle" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="ddlNewLanguage">Language</label>
                                                        <asp:DropDownList  Cssclass="form-control" ID="ddlNewLanguage2" runat="server">
                                                            <asp:ListItem Text="English" Value="English" />
                                                             <asp:ListItem Text="Chinese" Value="Chinese" />
                                                             <asp:ListItem Text="Malay" Value="Malay" />

                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="ddlNewCategory">Category</label>
                                                        <asp:DropDownList ID="ddlNewCategory2" runat="server" CssClass="form-control">
                                                           <asp:ListItem Text="News" Value="News" />
                                                            <asp:ListItem Text="Article" Value="Article" />
                                                            <asp:ListItem Text="Reference Material" Value="Reference Material" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="ddlNewItemtypelbl">Item Type</label>
                                                        <asp:DropDownList ID="ddlNewItemtype2" runat="server" CssClass="form-control">
                                                             <asp:ListItem Text="Thesis" Value="Thesis" />
                                                              <asp:ListItem Text="Research Paper" Value="Research Paper" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="txtNewAuthorName">Author Name</label>
                                                        <asp:TextBox ID="txtNewAuthorName2" runat="server" CssClass="form-control" placeholder="Author Name" />
                                                           <asp:Label ID="lblErrorAuthorName" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="txtNewContentDescription">Content Description</label>
                                                        <asp:TextBox ID="txtNewContentDescription2" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Content Description"></asp:TextBox>
                                                    </div>
                                                </div>
                                                     
                                                <div class="modal-footer">

                                                    <asp:Button ID="btnCreateContent2" runat="server" Text="Request Approval" CssClass="btn btn-primary" OnClick="btnCreateMaterial_Click" />
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                                </div>

                                                      </ContentTemplate>

                                                     
                                                        </asp:UpdatePanel>
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
