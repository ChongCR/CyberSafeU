<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="announcement.aspx.cs" Inherits="WebApplication1.announcement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

          <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>


    <style>

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


  .btn-toggle-status {
    background-color: #4CAF50;
    border: none;
    color: white;
    padding: 15px 32px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
    border-radius: 4px;
    box-shadow: 0 2px 4px 0 rgba(0,0,0,0.2);
    transition: background-color 0.3s, box-shadow 0.3s;
}

.btn-toggle-status:hover {
    background-color: #45a049; 
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
}

.btn-toggle-status:focus {
    outline: none; 
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
                    <h4 class="card-title text-center mb-4">Announcement Details</h4>
                    <hr />

                    <div class="form-row">
                      <!-- Announcement ID -->
                      <div class="form-group col-12">
                        <label class="info-label">Announcement ID:</label>
                        <span class="info-content" id="spanAnnId" runat="server"></span>
                      </div>
                    </div>

                    <div class="form-row">
                      <!-- Announcement Title -->
                      <div class="form-group col-12">
                        <label class="info-label">Title:</label>
                        <asp:TextBox Cssclass="form-control" ID="ann_title" runat="server" placeholder="Announcement Title" Enabled="false"></asp:TextBox>
                      </div>
                    </div>

                    <div class="form-row">
                      <!-- Announcer -->
                      <div class="form-group col-12">
                        <label class="info-label">Announcer:</label>
                        <asp:DropDownList CssClass="form-control" ID="announcer" runat="server" Enabled="false"></asp:DropDownList>
                      </div>
                    </div>

                    <div class="form-row">
                      <!-- Announcement Content -->
                      <div class="form-group col-12">
                        <label class="info-label">Content:</label>
                        <asp:TextBox Cssclass="form-control" ID="ann_content" runat="server" placeholder="Announcement Content" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                      </div>
                    </div>

                  

                    <div class="form-group">
                                <asp:Button ID="Button5" runat="server" Text="Content Image List" CssClass="btn btn-primary btn-block btn-sm" OnClick="btnUploadImage_Click" Enabled ="false"/>
                            </div> 
                    <div class="form-group">
                            <asp:Button ID="btnUploadVideo" runat="server" Text="Content Video List" CssClass="btn btn-secondary btn-block btn-sm" OnCLick="btnUploadVideo_Click" Enabled ="false" />
                    </div>
                    <div class="form-row">

                        <!-- Announcement Status -->
                        <div class="form-group col-12">
                            <label class="info-label">Status:</label>
                            <span class="info-content" id="spanAnnStatus" runat="server"></span>
                         <asp:Button ID="btnToggleStatus" runat="server" CssClass="btn btn-toggle-status" OnClick="ToggleAnnouncementStatus" Text="Switch Status" Enabled="false"/>

                        </div>
                        </div>

                    <div class="form-row">
                      <!-- Publish Button -->
                      <div class="form-group col-6">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary btn-block" OnClick="btnUpdate_Click" />
                      </div>
                      <!-- Delete Button -->
                      <div class="form-group col-6">
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-block" OnClick="btnDelete_Click" />
                      </div>
                    </div>
                  </div>
                </div>
     </div>


          <div class="col-md-7"">
                  <div class="card h-100">
                    <div class="card-body">
                      <h4 class="text-center">List of Announcements</h4>
                      <hr />

                      <div class="text-right mt-3 mb-2"> 
                        <button id="btnRefresh" runat="server" class="btn btn-secondary" onserverclick="btnRefresh_ServerClick">
                          <i class="fas fa-sync"></i> Refresh
                        </button>

                        <asp:Button ID="btnShowModal" runat="server" Text="Create New Announcement" CssClass="btn btn-success mr-1" OnClick="btnShowCreateModal_Click" />
                      </div>

                      <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" placeholder="Search by Title or Content"></asp:TextBox>

                      <asp:GridView ID="GridViewAnnouncements" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" EmptyDataText="No announcements found."
                        DataKeyNames="ann_id" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridViewAnnouncements_PageIndexChanging" OnRowDataBound="GridViewAnnouncements_RowDataBound"
                        AllowSorting="True" OnSorting="GridViewAnnouncements_Sorting" OnSelectedIndexChanged="GridViewAnnouncements_SelectedIndexChanged">
                        <PagerStyle CssClass="grid-view-pager" />
                        <Columns>
                          <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                              <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                          </asp:TemplateField>
                          <asp:BoundField DataField="ann_title" HeaderText="Title" SortExpression="ann_title" />
                          <asp:BoundField DataField="ann_content" HeaderText="Content" SortExpression="ann_content" />
                          <asp:BoundField DataField="announcer" HeaderText="Announcer" SortExpression="announcer" />
                          <asp:BoundField DataField="ann_status" HeaderText="Status" SortExpression="ann_status" />
                          <asp:BoundField DataField="date_posted" HeaderText="Date Posted" SortExpression="date_posted" />

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

            <div class="modal fade" id="createAnnouncementModal" tabindex="-1" role="dialog" aria-labelledby="addNewAnnouncementModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="addNewAnnouncementModalLabel">Create New Announcement</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="modal-body">                                                 
                                        <p class="info-box">This is a shortcut form for creating a new announcement. Please edit after creating.</p>
                                        <div class="form-group">
                                            <label for="txtNewAnnouncementTitle" class="text-dark">Announcement Title</label>
                                            <asp:TextBox ID="txtNewAnnouncementTitle" runat="server" CssClass="form-control" placeholder="Announcement Title" />
                                        </div>
                                       
                                        <div class="form-group">
                                            <label for="ddlAnnouncer"  class="text-dark">Announcer</label>
                                            <asp:DropDownList ID="ddlAnnouncer" runat="server" CssClass="form-control">
                                            
                                            </asp:DropDownList>
                                        </div>
                                       

                                         <div class="form-group">
                                            <label for="txtNewAnnouncementContent"  class="text-dark">Announcement Content</label>
                                            <asp:TextBox ID="txtNewAnnouncementContent" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Announcement Content"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnCreateAnnouncement" runat="server" Text="Create Announcement" CssClass="btn btn-primary" OnClick="btnCreateAnnouncement_Click" />
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                                    <asp:GridView ID="GridViewAnnouncementImages" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"  DataKeyNames="Image_id"  OnRowCommand="GridViewImages_RowCommand" EmptyDataText="No images found.">
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
                                    <asp:GridView ID="GridViewAnnouncementVideos" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" DataKeyNames="video_id" OnRowCommand="GridViewVideos_RowCommand" EmptyDataText="No video found.">
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
