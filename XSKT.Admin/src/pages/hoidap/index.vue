<script>
import Layout from "@/pages/congdan/layout";
import {hoiDapModel} from "@/models/hoiDapModel";
import {notifyModel} from "@/models/notifyModel";
import vue2Dropzone from "vue2-dropzone";
import 'vue2-dropzone/dist/vue2Dropzone.min.css'
import axios from "axios";

/**
 * Crypto ICO-landing page
 */
export default {
  components: { Layout,vueDropzone: vue2Dropzone,
    // 'ckeditor-nuxt': () => {
    //   return import('@blowstack/ckeditor-nuxt')
    // },
  },
  data() {
    return {
      data: [],
      apiView: `${process.env.VUE_APP_API_URL}filesminio/view/`,
      // url: `${process.env.VUE_APP_API_URL}files/view`,
      totalRows: 1,
      numberOfElement: 1,
      perPage: 4,
      currentPage: 1,
      sortBy: "age",
      sortDesc: false,
      model:null,
      modelHoiDap : hoiDapModel.baseJson(),
      filterOn: [],
      pageOptions: [5,10,25,50,100],
      listMenu : [],
      menu : null,
      listDanhMuc :[],
      urlFile:`${process.env.VUE_APP_API_URL}filesminio/view`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}filesminio/upload`,
        acceptedFiles: ".jpg,.jpeg,.png,.gif,.pdf,.doc,.docx,.xls,.xlsx, .zip",
        thumbnailHeight: 100,
        thumbnailWidth: 300,
        maxFiles: 30,
        maxFilesize: 50,
        maxFileSizeInMB:50,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true
      },
    };
  },
  computed: {

  },
  watch: {
    $route(to, from) {
      this.getGioiThieu();
    },
    currentPage:{
      deep: true,
      handler(val){
      //  console.log("this.perpage", this.currentPage);
        this.getGioiThieu();
      }
    }
  },
  created() {
    this.getMenu();
    this.GetDanhSach();
  },
  destroyed() {
    window.removeEventListener("scroll", this.windowScroll);
  },
  mounted() {
    this.start = new Date(this.starttime).getTime();
    this.end = new Date(this.endtime).getTime();
    // Update the count down every 1 second
  },
  methods: {
    async GetDanhSach(){
      await this.$store.dispatch("menuCongDanStore/getDanhMuc").then((res) => {
        this.listDanhMuc = res.data;
        //    console.log("LOG DANH MUC : ", this.listDanhMuc)
      })
    },
    async getMenu(){
      let menuStorage = localStorage.getItem("flatten-menu");
    //  console.log("LOG GET MENU STOREAGE :  ",menuStorage)
      if (menuStorage) {
        this.listMenu= JSON.parse(menuStorage);
        this.menu =  JSON.parse(menuStorage).find(item => item.link ===window.location.pathname);
     //   console.log("LOG ROUTER :  ", this.menu)

        this.getGioiThieu();
      }
    },
    async getGioiThieu(){
      const params = {
        start: this.currentPage,
        limit: this.perPage,
        sortDesc: true,
        menuId: (this.menu != null && this.menu.id != null) ? this.menu.id : this.$route.params.id
      };
      await this.$store.dispatch("contentStore/getPagingParamsHoiDapCongDan",params).then((res) => {
       // console.log("MENU: ", res.data)
        this.model = res.data.data;
        this.totalRows = res.data.totalRows;
        this.numberOfElement = res.data.data.length;
      })
    },
    handleShowRegisterModal(){
      this.$store.dispatch("snackBarStore/showRegisterModal", !this.$store.state.snackBarStore.registerModal)
    //  console.log("abc")
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },
    /**
     * Window scroll method
     */
    windowScroll() {
      const navbar = document.getElementById("navbar");
      if (
          document.body.scrollTop >= 50 ||
          document.documentElement.scrollTop >= 50
      ) {
        navbar.classList.add("nav-sticky");
      } else {
        navbar.classList.remove("nav-sticky");
      }
    },
    /**
     * Toggle menu
     */
    toggleMenu() {
      document.getElementById("topnav-menu-content").classList.toggle("show");
    },
    nextSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getNextPage());
    },
    prevSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getPreviousPage());
    },
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    async HandleSubmit(evt) {
      this.modelHoiDap.menu=[];
      this.modelHoiDap.menu.push({id : "6544b83c1f222fcfc365c914" , name : "Hỏi đáp"});
        await this.$store
            .dispatch("contentStore/createHoiDap", this.modelHoiDap)
            .then((res) => {
              if (res.code === 0) {
               // console.log("LOG CODE = 0 ")
                this.modelHoiDap = hoiDapModel.baseJson();
                this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
                return;
              }
              this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
            });
     },

     getColorWithExtFile(ext) {
      if (ext == '.png' || ext == '.jpg'|| ext == '.jpeg' )
        return 'text-danger';

    },

    getIconWithExtFile(ext) {
      if (ext == '.png' || ext == '.jpg'|| ext == '.jpeg')
        return 'mdi mdi-file-image-outline';
    },
    sendingEvent(files, xhr, formData) {
      formData.append("files", files);
    },
    uploadFileSlider(files,response) {
      let fileSuccess = response.data;
    //  console.log('log suscess', response.data)
      if (response.code == 0){
        this.modelHoiDap.files.push({
          fileId: fileSuccess.fileId,
          fileName: fileSuccess.fileName,
          ext: fileSuccess.ext,
          path: fileSuccess.path
        });
      }
   //   console.log('log model file', this.modelHoiDap.files)

    },
    removeThisFile(files, error, xhr) {
      let fileCongViec = JSON.parse(files.xhr.response);
      if (fileCongViec.data && fileCongViec.data.fileId) {
        let idFile = fileCongViec.data.fileId;
        let resultData = this.modelHoiDap.files.filter(x => {
          return x.fileId != idFile;
        })
        this.modelHoiDap.files = resultData;
      //  console.log('log model file remove', this.modelHoiDap.files)
      }
    },

    deleteFile(){
      if (this.modelHoiDap != null && this.modelHoiDap.files != null)
      {
      //  console.log("LOG this.modelContent : ", this.modelHoiDap)
        axios.post(`${process.env.VUE_APP_API_URL}filesminio/delete/${this.modelHoiDap.files.fileId}`).then((response) => {
          this.modelHoiDap.files = null;
        //  console.log('log model file remove', this.modelHoiDap.files);
        }).catch((error) => {
          // Handle error here
          console.error('Error deleting file:', error);
        });
      }
    },

    getUrl(item) {
      // console.log("LOG DANH MUC CLICK", item)
      if (item.link.indexOf("/{id}") < 0  && item.level === 0)
      {
        // console.log("LOG ROUTER IF LAYOUT  : ", item)
        this.idMenu = item.id;
        //  console.log("LOG ITEM : ", item.link)
        this.$router.push(item.link);
      } else if (item.link.indexOf("/{id}") > 0 && item.level === 0){
        this.idMenu = item.id;
        // console.log("LOG ROUTER IF ELSE  LAYOUT  : ", item.link.replace("{id}",  item.id))
        this.$router.push(item.link.replace("{id}",  item.id));
      }else {
        // console.log("LOG ROUTER ELSE LAYOUT  : ", item.link +   item.id)
        this.idMenu = item.id;
        this.$router.push(item.link +  "/" + item.id);
      }
    },

  },
};
</script>

<template>
  <Layout>
    <section class="section p-2 bg-white" id="about">
      <div class="row align-items-center">
        <div>
          <div class="row">
            <div class="col-12">
              <div class="row">
                <div class="col-lg-12">
                  <div class="container">
                    <div class="row">
                      <form role="form" >
                        <div class="col-12 mt-3">
                          <div class="row mb-3">
                            <div class="col-12 mb-2">
                              <div class="cs-title-box">
                                <div class="cs-title py-2">
                                  <a href="">
                                    <i class="mdi mdi-star ic-item"></i>
                                    <span style="color: #fff; font-size: 16px;">ĐẶT CÂU HỎI</span>
                                  </a>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div class="row mb-3">
                            <div class="col-md-3 col-lg-2">
                              <label class="text-black"> Họ và tên </label>
                            </div>
                            <div class="col-md-9 col-lg-10">
                              <div >
                                <input
                                v-model="modelHoiDap.hoVaTen"
                                    class="form-control"
                                    type="search"
                                    placeholder="Nhập họ và tên..."
                                />
                              </div>
                            </div>
                          </div>
                          <!-- Số điện thoại -->
                          <div class="row mb-3">
                            <div class="col-md-3 col-lg-2">
                              <label class="text-black"> Số điện thoại </label>
                            </div>
                            <div class="col-md-9 col-lg-10">
                              <div >
                                <input
                                    v-model="modelHoiDap.soDienThoai"
                                    class="form-control"
                                    v-mask="'(###) # ### ####'"
                                    type="text"
                                    placeholder="Nhập số điện thoại..."
                                />
                              </div>
                            </div>
                          </div>
                          <div class="row mb-3">
                            <div class="col-md-3 col-lg-2">
                              <label class="text-black"> Địa chỉ </label>
                            </div>
                            <div class="col-md-9 col-lg-10">
                              <div >
                                <input
                                    v-model="modelHoiDap.diaChi"
                                    class="form-control"
                                    type="text"
                                    placeholder="Nhập địa chỉ..."
                                />
                              </div>
                            </div>
                          </div>
                          <div class="row mb-3">
                            <div class="col-md-3 col-lg-2">
                              <label class="text-black"> Email </label>
                            </div>
                            <div class="col-md-9 col-lg-10">
                              <div >
                                <input
                                    v-model="modelHoiDap.email"
                                    class="form-control"
                                    type="email"
                                    placeholder="Nhập địa chỉ email..."
                                />
                              </div>
                            </div>
                          </div>
                          <div class="row mb-3">
                            <div class="col-md-3 col-lg-2">
                              <label class="text-black"> Nội dung câu hỏi </label>
                              <span
                                  class="text-danger"
                              >*</span
                              >
                            </div>
                            <div class="col-md-9 col-lg-10">
                              <div >
                                <textarea
                                    v-model="modelHoiDap.moTa"
                                    class="form-control"
                                    type="search"
                                    placeholder="Nhập nội dung câu hỏi"
                                >
                                </textarea>
                              </div>
                            </div>
                          </div>
                          <div class="col-12" >
                            <template>
                              <div style="margin-top: 20px;">
                                <vue-dropzone
                                    id="dropzone"
                                    ref="myVueDropzone"
                                    :use-custom-slot="true"
                                    :options="dropzoneOptions"
                                    v-on:vdropzone-sending="sendingEvent"
                                    v-on:vdropzone-removed-file="removeThisFile"
                                    v-on:vdropzone-success="uploadFileSlider"
                                >
                                  <div class="dropzone-custom-content">
                                    <div class="mb-1">
                                      <i class="display-4 text-muted bx bxs-cloud-upload"></i>
                                    </div>
                                    <h4>Kéo thả tệp hoặc click vào đây để tải tệp tin.</h4>
                                  </div>
                                </vue-dropzone>
                              </div>
                            </template>
                          </div>
                          <div class="row mt-3">
                            <div class="col-12">
                              <div class="mb-3 row">
                                <div class="col-md-12" style="text-align: center;">
                                  <b-button @click="HandleSubmit"
                                    style="width: 200px;
                                    border-radius: 20px;
                                    background-color: #05912a;
                                    border: none;"
                                  >
                                    GỬI CÂU HỎI
                                  </b-button>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </form>
                    </div>
                    <div class="row">
                      <div class="col-lg-9">
                        <div class="col-12 mb-2">
                          <div class="cs-title-box">
                            <div class="cs-title py-2">
                              <a href="">
                                <i class="mdi mdi-star ic-item"></i>
                                <span style="color: #fff; font-size: 16px;">HỎI ĐÁP</span>
                              </a>
                            </div>
                          </div>
                        </div>
                        <section>
                          <div class="row">
                            <div class="col-lg-12">
                              <div
                                  v-for="(item, index) in model"
                                  :key="index"
                                  class="row"
                              >
                                <div class="col-md-12 mb-4">
                                  <b-card no-body style="border-radius: 5px;">
                                    <b-row no-gutters class="align-items-center">
                                      <b-col md="3">
                                        <router-link
                                            :to="{
                                                    path: `/hoi-dap/chi-tiet/${item._id}`,
                                                    }"
                                        >
                                          <div v-if="!item.fileImage">
                                            <div class="float-left">
                                              <img
                                                  src="@/assets/images/hoidap.jpg"
                                                  alt="Không có ảnh"
                                                  class="rounded-0 w-100"
                                                  style="height: 250px; padding: 10px;"
                                              >
                                            </div>
                                          </div>
                                          <div v-else>
                                            <b-card-img
                                                :src="apiView + item.fileImage.fileId"
                                                class="rounded-0"
                                                :height="250"
                                                :width="250"
                                            ></b-card-img>
                                          </div>
                                        </router-link>
                                      </b-col>
                                      <b-col md="9" style="padding: 10px;">
                                        <router-link
                                            :to="{
                                                    path: `/hoi-dap/chi-tiet/${item._id}`,
                                                    }"
                                        >
                                          <b-card-body class="" :title="`Câu hỏi: ${item.moTa}`" style="color: #000 !important;">
                                            <p class="card-text">
                                              <small class="fs-13" style="font-style: italic; color: #d60604">
                                                Ngày hỏi: {{item.createdAtShow}}
                                              </small>
                                            </p>
                                            <b-card-text
                                                class="custom-content"
                                            >
                                              <p style="padding-right: 10px;  color: #495057 !important; font-size: 16px;">
                                                Câu trả lời: {{ item.noiDung }}
                                              </p>
                                            </b-card-text>

                                            <b-button
                                                pill
                                                class="px-4 fs-13"
                                                size="sm"
                                                style="
                                                      float: left;
                                                      background-color:#efc62c;
                                                      color: #000 !important;
                                                      border: none;
                                                      "
                                            >
                                              Chi tiết
                                              <i class="mdi mdi-arrow-right ps-2"></i>
                                            </b-button>
                                          </b-card-body>
                                        </router-link>
                                      </b-col>
                                    </b-row>
                                  </b-card>
                                </div>
                              </div>
                            </div>
                          </div>

                        </section>
                        <div class="row mb-3">
                          <b-col>
                            <div>Hiển thị {{numberOfElement}} trên tổng số {{totalRows}} dòng</div>
                          </b-col>
                          <div class="col">
                            <div
                                class="dataTables_paginate paging_simple_numbers float-end">
                              <ul class="pagination pagination-rounded mb-0">
                                <!-- pagination -->
                                <b-pagination
                                    v-model="currentPage"
                                    :total-rows="totalRows"
                                    :per-page="perPage"
                                ></b-pagination>
                              </ul>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="col-lg-3">
                        <div class="category">
                          <div class="cate-title">
                            Danh Mục
                          </div>
                          <div v-for ="(item,index) in listDanhMuc" :key="index"  class="cate-list">
                            <ul style="cursor: pointer">
                              <li>
                                <i class="mdi mdi-chevron-double-right"></i>
                                <a @click="getUrl(item)">{{item.label}}</a>
                              </li>
                            </ul>
                          </div>
                        </div>
                      </div>
                    </div>

                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </Layout>
</template>
<style>

.category{
  background-color: #f9f9f9;
}

.cate-title{
  background-color: #05912a;
  color: #fff;
  padding: 10px;
  font-size: 20px;
}

.cate-list ul li{
  list-style: none;
  border-bottom: 1px dashed #d0cfcf;
  padding: 10px 0 10px 0;
}

.cate-list ul li a{
  margin-left: 10px;
  font-size: 14px;
  color: #78797C;
}

/* .cs-title-box .cs-title .ic-item {
  background-color: #fff;
  color: #d60604;
  padding: 5px 7px;
  border-radius: 50px;
  margin-right: 10px;
} */

.btn-yellow{
  background-color: #EFC62C;
  border: none;
  border-radius: 0 !important;
  color: #000 !important;
}

.btn-yellow:hover{
  background-color: #ffc800;
  border: none;
}

.color-primary {
  /*color: #28883b;*/
  color: #2b569a;
}

.bg-primary {
  /*background-color: #28883b !important;*/
  background-color: #2b569a !important;
}


.w-10 {
  width: 10%;
}
.w-80 {
  width: 80%;
}
.w-90 {
  width: 90%;
}

.block-ellipsis {
  display: block;
  display: -webkit-box;
  max-width: 100%;
  font-size: 14px;
  line-height: 1.4;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
}

tr {
  vertical-align: middle !important;
  box-shadow: 0 0 1rem rgb(18 38 63 / 3%) !important;
}

.bg-ico-primary {
  height: 340px;
}
.ribbons {
  /*background: linear-gradient(45deg, #940012, #F6C6C6);*/
  position: absolute;
  padding: 10px;
  font-weight: bold;
  color: #fff;
  border-radius: 5px;
  top: -18px;
  left: 20px;
  background-color: #2b569a;
  box-shadow: rgba(255, 255, 255, 0) 0px 4px 6px -1px, rgba(255, 255, 255, 0.5) 0px 2px 4px -1px;
}

@media only screen and (max-width: 425px){
  .create-at{
    text-align: start !important;
    margin-bottom: 5px;
  }
}


section.bg-ico-primary {
  padding-top: 100px;
}
.btn-detail {
  background:#2b569a;
  border: none;
}

.btn-secondary {
  --bs-btn-bg: #2b569a;
  --bs-btn-hover-bg: #537961;
}

.custom-content{
  display: -webkit-box !important;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.card{
  box-shadow: 0 0.75rem 1.5rem rgb(18 38 63 / 17%) !important;
  margin-bottom: 0px !important;
  min-height: 250px;
}
/* .card-body{
    padding: 0px
} */
.card-title{
  font-size: 16px;
}

@media only screen and (max-width: 768px){
  .card-body{
    padding: 20px;
  }
}

.active > .page-link {
  background-color: #EFC62C !important;
}
</style>
