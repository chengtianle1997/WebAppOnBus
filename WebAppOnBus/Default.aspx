<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAppOnBus._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div style="position: absolute; height: 10000px; width: 20000px; top: 0px; left: 0px ;background:#293B4D">
                                          
    </div>
    <%--<asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" BackColor="White" BorderColor="Black" Height="22px" RenderingMode="Table" Width="300px" OnMenuItemClick="Menu1_MenuItemClick" style="position: absolute;top: 90px; left: 30px">
        <Items>
                 <asp:MenuItem Selected="True" Text="轨道2D图像" Value="1"></asp:MenuItem>
                 <asp:MenuItem Text="轨道3D图像" Value="2"></asp:MenuItem>

             </Items>
    </asp:Menu>--%>

     <div style="position: absolute; height: 50px; width:1200px; top: 75px; left: 90px;">
         <label ID="btnstart"  class="btn btn-lg btn-default"  role="button" style="width:95px;height:40px;background:white;color:#293B4D;font-size :12px;font-weight:bold" onclick="startapp(this)">开始采集</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <%--<button ID="btnstop"  class="btn btn-lg btn-default"  role="button" style="width:95px;height:40px;background:white;color:#293B4D;font-size :12px;font-weight:bold"  onclick="stopapp(this)">停止采集</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
         <button ID="btn1"  class="btn btn-lg btn-default"  role="button" style="width:95px;height:40px;background:white;color:#293B4D;font-size :12px;font-weight:bold" Value="1" onclick="Menu1_MenuItemClick1">隧道2D图像</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <button ID="btn2" class="btn btn-lg btn-default"  role="button" style="width:95px;height:40px;background:white;color:#293B4D;font-size :12px;font-weight:bold" Value="2" onclick="Menu1_MenuItemClick2">隧道3D图像</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

         <label ID="condlabel"  class="btn btn-lg btn-default"  role="button" style="width:290px;height:40px;background:white;color:#293B4D;font-size :12px;font-weight:bold" onclick="startapp(this)"></label>
    </div>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <div style="position: absolute; top:-35px; left: 0px;">
            
                
                     <div ID="Block1" class="pic" style="position: absolute; top: 192px; left: 24px;">
                        
                         <asp:Image ID="Camera_Image1" runat="server" Height="205px" Width="259px" />                                                       
         
                     </div>

                     <div ID="Block2" class="pic" style="position: absolute; top: 192px; left: 384px;">
                         
                         <asp:Image ID="Camera_Image2" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block3" class="pic" style="position: absolute; top: 192px; left: 744px;">
                         
                         <asp:Image ID="Camera_Image3" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block4" class="pic" style="position: absolute; top: 417px; left: 24px;">
                         
                         <asp:Image ID="Camera_Image4" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block5" class="pic" style="position: absolute; top: 417px; left: 384px;">
                         
                         <asp:Image ID="Camera_Image5" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block6" class="pic" style="position: absolute; top: 417px; left: 744px;">
                         
                         <asp:Image ID="Camera_Image6" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block7" class="pic" style="position: absolute; top: 642px; left: 24px;">
                         
                         <asp:Image ID="Camera_Image7" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block8" class="pic" style="position: absolute; top: 642px; left: 384px;">
                         
                         <asp:Image ID="Camera_Image8" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block9" class="pic" style="position: absolute; top: 642px; left: 744px;">
                         
                         <asp:Image ID="Camera_Image9" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>




           







                     

                    <div class="lab" style="position: absolute; top: 192px; left: 290px;height:225px; width:80px">
                       
                         <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">1</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera1_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>
                        
                   
                     </div>
                   
                     <div class="lab" style="position: absolute; top: 417px; left: 290px;height:225px;width:80px">
                      
                         <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">4</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera4_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>     
                         
                     </div>

                 <div class="lab" style="position: absolute; top: 642px; left: 290px;height:225px;width:80px">
                      <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">7</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera7_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>
                     </div>

                  <div class="lab" style="position: absolute; top: 192px; left: 650px;height:225px;width:80px">
                       
                     <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">2</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera2_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>
                        
                     </div>

                 <div class="lab" style="position: absolute; top: 417px; left: 650px;height:225px;width:80px">
                       
                  <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">5</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera5_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>
                        
                     </div>

                 <div class="lab" style="position: absolute; top: 642px; left: 650px;height:225px;width:80px">
                <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">8</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera8_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>
                        
                     </div>
            
                      <div class="lab" style="position: absolute; top: 192px; left: 1010px;width:80px">
                     <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">3</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera3_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>
                     </div>
                 
                          <div class="lab" style="position: absolute; top: 417px; left: 1010px;height:225px;width:80px">
                     <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">6</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera6_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>
                     </div>

                 <div class="lab" style="position: absolute; top: 642px; left: 1010px;height:225px;width:80px">
                             <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">相机编号</a></p>
                        
                        <p><a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold">9</a></p>
                        
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#3caad4;color:white;font-size :11px;font-weight:bold">帧数:</a> </p>
                         
                        <p> <a class="btn btn-lg btn-primary" href="#" role="button" style="width:85px;height:35px;background:#57aa72;color:white;font-size :11px;font-weight:bold"></a> </p>

                       <asp:Label ID="Camera9_Frame" runat="server" Text="0" style="position:absolute;top:145px;left:40px; color:white;font-size :11px;font-weight:bold"></asp:Label>
                     </div>


                 <div style="position: absolute; left: 1130px; top: 192px; width: 366px; height: 550px">
                     <asp:Image ID="Image10" runat="server" Height="509px" Width="359px" />
                 </div>
                <%-- <img id="testimg" style="height:205px;width:259px;" src="" />--%>


        <!--选项单-->
            
          <div style="position: absolute; height: 130px; width: 339px; top: 722px; left: 1150px ;background:rgba(57, 71, 86, 0.50)">
          <div style="position: absolute; height: 130px; width: 20px; top: 0px; left: -20px ;background:rgba(57, 71, 86, 0.50)">
          </div>
              
           <div class="checkbox">
          <label style="color:white;font-size :11px;font-weight:bold">
          <input type="checkbox" value="pointcloud" ID="checkbox1" runat="server" checked="true" onclick="setcloudchange(this)">点云
          </label>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
            
            <div class="checkbox">
          <label style="color:white;font-size :11px;font-weight:bold">
          <input type="checkbox" value="denoise" ID="checkbox2" runat="server" checked="false" onclick="setfilterchange(this)">降噪
          </label>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
         
            <div class="checkbox">
          <label style="color:white;font-size :11px;font-weight:bold">
          <input type="checkbox" value="fitting" ID="checkbox3" runat="server" checked="false" onclick="setfitchange(this)">拟合
          </label>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
          <div class="checkbox">
          <label style="color:white;font-size :11px;font-weight:bold">
          <input type="checkbox" value="fitting" ID="checkbox4" runat="server" checked="false" onclick="setidenchange(this)">标注
          </label>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
            </div>
        </asp:View>


        <asp:View ID="View2" runat="server">
            

        </asp:View>
    </asp:MultiView>
    

   <script>
       function startapp(button) {
           $.ajax({
                    "dataType": "text",
                    "async": "false",
                    "url": "actionhandler.ashx",
                    "type": "POST",
                    "data": { action: "startcamera" },
                    success: function (data) {
                        //alert(button.innerHTML);
                        //document.getElementById("condlabel").innerText = "123456"
                        if (data=="True") {                         
                            button.innerHTML = "开始采集"
                            
                        }
                        else if (data=="False") {
                            button.innerHTML = "停止采集"
                        }
                    }
                })
       }
       function setcond() {
           $.ajax({
               "dataType": "text",
               "async": "false",
               "url": "actionhandler.ashx",
               "type": "POST",
               "data": { action: "setcond" },
               success: function (data) {
                   document.getElementById("condlabel").innerText = data      
               }
           })
           
       }
       function setcloudchange(checkbox) {
           if (checkbox.checked == true) {
               //alert("cloud on");
               $.ajax({
                   "dataType": 'text',
                   "async": false,
                   "url": "2dmodelhandler.ashx",
                   "type": "POST",
                   "data": { action: "cloudon" },
                   "success": function (data) {

                   }
               });
           }
           else if (checkbox.checked == false) {
               //alert("cloud off")
               $.ajax({
                   "dataType": 'text',
                   "async": false,
                   "url": "2dmodelhandler.ashx",
                   "type": "POST",
                   "data": { action: "cloudoff" },
                   "success": function (data) {

                   }
               });
           }
       }
       function setfilterchange(checkbox) {
           if (checkbox.checked == true) {
               //alert("cloud on");
               $.ajax({
                   "dataType": 'text',
                   "async": false,
                   "url": "2dmodelhandler.ashx",
                   "type": "POST",
                   "data": { action: "filteron" },
                   "success": function (data) {

                   }
               });
           }
           else if (checkbox.checked == false) {
               //alert("cloud off")
               $.ajax({
                   "dataType": 'text',
                   "async": false,
                   "url": "2dmodelhandler.ashx",
                   "type": "POST",
                   "data": { action: "filteroff" },
                   "success": function (data) {

                   }
               });
           }
       }
       function setfitchange(checkbox) {
           if (checkbox.checked == true) {
               //alert("cloud on");
               $("#MainContent_checkbox4").attr("checked", true);
               $("#MainContent_checkbox4").attr("disabled", false);
               $.ajax({
                   "dataType": 'text',
                   "async": false,
                   "url": "2dmodelhandler.ashx",
                   "type": "POST",
                   "data": { action: "fiton" },
                   "success": function (data) {

                   }
               });
           }
           else if (checkbox.checked == false) {
               //alert("cloud off")
               $("#MainContent_checkbox4").attr("checked", false);
               $("#MainContent_checkbox4").attr("disabled", true);
               $.ajax({
                   "dataType": 'text',
                   "async": false,
                   "url": "2dmodelhandler.ashx",
                   "type": "POST",
                   "data": { action: "fitoff" },
                   "success": function (data) {

                   }
               });
           }
       }
       function setidenchange(checkbox) {
           if (checkbox.checked == true) {
               //alert("cloud on");
               $.ajax({
                   "dataType": 'text',
                   "async": false,
                   "url": "2dmodelhandler.ashx",
                   "type": "POST",
                   "data": { action: "idenon" },
                   "success": function (data) {

                   }
               });
           }
           else if (checkbox.checked == false) {
               //alert("cloud off")
               $.ajax({
                   "dataType": 'text',
                   "async": false,
                   "url": "2dmodelhandler.ashx",
                   "type": "POST",
                   "data": { action: "idenoff" },
                   "success": function (data) {

                   }
               });
           }
       }
       
       
       $(document).ready(function () {
           //初始化控制模块
           $.ajax({
               "dataType": "text",
               "async": "false",
               "url": "actionhandler.ashx",
               "type": "POST",
               "data": { action: "startaction" },
               success: function (data) {
                   //document.getElementById("condlabel").innerText = data      
               }
           })

           //初始化图传
           //alert('complete');
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "startServer" },
               "success": function (data) {

                   //alert(data);
                   setInterval(SetImage0, 300);
                   setInterval(SetImage1, 300);
                   setInterval(SetImage2, 300);
                   setInterval(SetImage3, 300);
                   setInterval(SetImage4, 300);
                   setInterval(SetImage5, 300);
                   setInterval(SetImage6, 300);
                   setInterval(SetImage7, 300);
                   setInterval(SetImage8, 300);

                   setInterval(SetLabel0, 300);
                   setInterval(SetLabel1, 300);
                   setInterval(SetLabel2, 300);
                   setInterval(SetLabel3, 300);
                   setInterval(SetLabel4, 300);
                   setInterval(SetLabel5, 300);
                   setInterval(SetLabel6, 300);
                   setInterval(SetLabel7, 300);
                   setInterval(SetLabel7, 300);
                   setInterval(SetLabel8, 300);

                   setInterval(setcond, 300);
               }
           });
           //初始化数传
            $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "2dmodelhandler.ashx",
               "type": "POST",
               "data": { action: "startServer" },
               "success": function (data) {

                   //alert(data);
                   setInterval(Set2DImage, 100);
                 
               }
           });
       });

       function Set2DImage() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "2dmodelhandler.ashx",
               "type": "POST",
               "data": { action: "get2DImage" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Image10").attr("src", data);

                   //setTimeout(Set2DImage, 100);
               }
           });
       }

       

       function SetImage0() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage0" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image9").attr("src", data);

                   //setTimeout(SetImage0, 100);
               }
           });
       }

       function SetImage1() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage1" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image1").attr("src", data);

                   //setTimeout(SetImage1, 100);
               }
           });
       }

       function SetImage2() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage2" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image2").attr("src", data);

                   //setTimeout(SetImage2, 100);
               }
           });
       }

       function SetImage3() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage3" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image3").attr("src", data);

                   //setTimeout(SetImage3, 100);
               }
           });
       }

       function SetImage4() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage4" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image4").attr("src",data);

                  // setTimeout(SetImage4, 100);
               }
           });
       }

       function SetImage5() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage5" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image5").attr("src", data);

                   //setTimeout(SetImage5, 100);
               }
           });
       }

       function SetImage6() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage6" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image6").attr("src", data);

                   //setTimeout(SetImage6, 100);
               }
           });
       }

       function SetImage7() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage7" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image7").attr("src", data);

                   //setTimeout(SetImage7, 100);
               }
           });
       }


       function SetImage8() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage8" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image8").attr("src", data);

                   //setTimeout(SetImage8, 100);
               }
           });
       }

       function  SetLabel0() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe0" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera9_Frame").html(data);

                   //setTimeout(SetLabel0, 100);
               }
           });
       }

        function SetLabel1() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe1" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera1_Frame").html(data);

                   //setTimeout(SetLabel1, 100);
               }
           });
       }

        function SetLabel2() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe2" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera2_Frame").html(data);

                   //setTimeout(SetLabel2, 100);
               }
           });
       }

        function SetLabel3() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe3" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera3_Frame").html(data);

                   //setTimeout(SetLabel3, 100);
               }
           });
       }

       function SetLabel4() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe4" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera4_Frame").html(data);

                   //setTimeout(SetLabel4, 100);
               }
           });
       }

        function SetLabel5() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe5" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera5_Frame").html(data);

                   //setTimeout(SetLabel5, 100);
               }
           });
       }

        function SetLabel6() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe6" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera6_Frame").html(data);

                   //setTimeout(SetLabel6, 100);
               }
           });
       }

        function SetLabel7() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe7" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera7_Frame").html(data);

                   //setTimeout(SetLabel7, 100);
               }
           });
       }

        function SetLabel8() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe8" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera8_Frame").html(data);

                   //setTimeout(SetLabel8, 100);
               }
           });
       }
       
           
   </script>

</asp:Content>
