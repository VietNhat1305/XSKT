<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import {unitRoleModel} from "@/models/unitRoleModel";
import {lienHeModel} from "@/models/lienheModel";
import vue2Dropzone from "vue2-dropzone";
import {headerModel} from "@/models/headerModel";
import axios from "axios";
import {sliderModel} from "@/models/sliderModel";
export default {
  page: {
    title: "Quản lý thông tin trang",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, vueDropzone: vue2Dropzone},
  data() {
    return {
      title: "Quản lý thông tin trang ",
      items: [
        {
          text: "Liên hệ",
          href: "/lien-he",
          // active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      showModal: false,
      showModalSlider: false,
      showDetail: false,
      showDeleteModal: false,
      showDeleteHeader:false,
      submitted: false,
      modelHeader: headerModel.baseJson(),
      modelSlider: sliderModel.baseJson(),
      listRole: [],
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      todofilterOn: [],
      sortBy: "age",
      sortDesc: false,
      images: null,
      fields: [
        {
          key: 'STT', label: 'STT',
          class: "text-center td-stt",
          tdClass: 'align-middle',
          thClass: 'hidden-sortable'
        },
        {
          key: "file",
          label: "Hình ảnh",
          class: "text-center",
          thClass: 'hidden-sortable'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: "text-center",
          thClass: 'hidden-sortable',
          thStyle: {width: '80px', minWidth: '60px'},
        }
      ],
      listFileSilder : [],
      listFileHeader : [],
      url : `${process.env.VUE_APP_API_URL}filesminio/view/`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}filesminio/upload`,
        acceptedFiles: ".jpg,.jpeg,.png,.gif",
        thumbnailHeight: 100,
        thumbnailWidth: 300,
        maxFiles: 5,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true
      },
      image: "",
      file: "",
    };
  },
  created() {
    this.getThongTinHeader();
    this.getThongTinSlider();
    const user = {
      id: 1,
      name: 'Victory Osayi',
      is_editor: true,
      is_admin: false,
      // you can have role based permission list or access control list possibly from database
      permissions: [
        'manage-HEADER-654863bd4e06313c529c0109', 
        'create-HEADER-654863bd4e06313c529c0109',
        'update-HEADER-654863bd4e06313c529c0109',
        'delete-HEADER-654863bd4e06313c529c0109'
      ]
    }
  },
  watch: {
    showModal(status) {
      if (status == false) this.modelHeader =headerModel.baseJson() ;
    },
    showModalSlider(status) {
      if (status == false) this.modelSlider = sliderModel.baseJson() ;
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
  methods: {
    uploadFileHeader(files,response) {
      let fileSuccess = response.data;
      console.log('log suscess', response.data)
      if (response.code == 0){
        this.modelHeader.file = {
          fileId: fileSuccess.fileId,
          fileName: fileSuccess.fileName,
          ext: fileSuccess.ext,
          path: fileSuccess.path
        };
      }
      console.log('log model file', this.modelHeader.file)

    },
    uploadFileSlider(files,response) {
      let fileSuccess = response.data;
      console.log('log suscess', response.data)
      if (response.code == 0){
        this.modelSlider.files.push({
          fileId: fileSuccess.fileId,
          fileName: fileSuccess.fileName,
          ext: fileSuccess.ext,
          path: fileSuccess.path
        });
      }
      console.log('log model file', this.model.file)

    },
    removeThisFile(files, error, xhr) {
      let fileCongViec = JSON.parse(files.xhr.response);
      if (fileCongViec.data && fileCongViec.data.fileId) {
        let idFile = fileCongViec.data.fileId;
        let resultData = this.model.file.filter(x => {
          return x.fileId != idFile;
        })
        this.model.file = resultData;
        //  console.log('log model file remove', this.modelContent.files)
      }

      // let fileCongViec = JSON.parse(files.xhr.response);
      // if (fileCongViec.data && fileCongViec.data.fileId) {
      //   let idFile = fileCongViec.data.fileId;
      //   // Call your delete API here
      //   axios.post(`${process.env.VUE_APP_API_URL}filesminio/delete/${idFile}`).then((response) => {
      //     let resultData = this.model.file.filter((x) => {
      //       return x.fileId != idFile;
      //     });
      //     this.model.file = resultData;
      //     console.log('log model file remove', this.model.file);
      //   }).catch((error) => {
      //     // Handle error here
      //     console.error('Error deleting file:', error);
      //   });
      // }
    },
    sendingEvent(file, xhr, formData) {
      formData.append("files", file);
    },
    async getThongTinHeader(){
      let promise =  this.$store.dispatch("headerStore/getAll")
      return promise.then(resp => {
        if(resp.data == null){
          return []
        }else{
          if (resp.data != null )
          {
            this.listFileHeader = []
            this.listFileHeader.push(resp.data)
          }
        }
      })
    },
    async getThongTinSlider(){
      let promise =  this.$store.dispatch("sliderStore/getAll")
      return promise.then(resp => {
        if(resp.data == null){
          return []
        }else{
          if (resp.data != null )
          {
            this.listFileSilder = []
            this.listFileSilder = resp.data;
          }
        }
      })
    },
    async handleSubmitHeader(e) {
      console.log("LOG THONG TIN " ,this.modelHeader)
      e.preventDefault();
      this.submitted = true;
      console.log("LOG INVALID 36165 : ", this.m);
      if (this.modelHeader == null || (this.modelHeader != null && this.modelHeader.file == null)) {
        this.$store.dispatch("snackBarStore/addNotify", {
          message: "Vui lòng chọn hình ảnh trước khi lưu ",
          code: 20,
        });
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        await this.$store.dispatch("headerStore/create", this.modelHeader).then((res) => {
          if (res.code === 0) {
            this.modelHeader = null
            this.showModal = false;
            this.getThongTinHeader();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
        loader.hide();
        this.submitted = false;

      }
      this.submitted = false;
    },

    async handleSubmitSlider(e) {
      console.log("LOG THONG TIN " ,this.modelSlider)
      e.preventDefault();
      this.submitted = true;
      console.log("LOG INVALID 36165 : ", this.m);
      if (this.modelSlider == null || (this.modelSlider != null && this.modelSlider.files == null)) {
        this.$store.dispatch("snackBarStore/addNotify", {
          message: "Vui lòng chọn hình ảnh trước khi lưu ",
          code: 20,
        });
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        await this.$store.dispatch("sliderStore/create", this.modelSlider).then((res) => {
          if (res.code === 0) {
            this.modelSlider = null
            this.showModalSlider = false;
            this.getThongTinSlider();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
        loader.hide();
        this.submitted = false;
      }
      this.submitted = false;
    },
    handleShowDeleteModal(id) {
      this.modelSlider._id = id;
      this.showDeleteModal = true;
    },
    async handleDelete() {
      if (this.modelSlider._id != 0 && this.modelSlider._id != null && this.modelSlider._id) {
        await this.$store.dispatch("sliderStore/delete",{_id :  this.modelSlider._id}).then((res) => {
          if (res.code===0) {
            this.getThongTinSlider();
            this.showDeleteModal = false;
          }
          var a = {
            message: res.message,
            code: res.code
          };
          // console.log("LOG A : " ,a)
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code
          });
        });
      }
    },
    handleShowDeleteModalHeader(id) {
      this.modelHeader._id = id;
      this.showDeleteHeader = true;
    },
    async handleDeleteHeader() {
      if (this.modelHeader._id != 0 && this.modelHeader._id != null && this.modelHeader._id) {
        await this.$store.dispatch("headerStore/delete",{_id :  this.modelHeader._id}).then((res) => {
          if (res.code===0) {
            this.getThongTinHeader();
            this.showDeleteHeader = false;
          }
          var a = {
            message: res.message,
            code: res.code
          };
          // console.log("LOG A : " ,a)
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code
          });
        });
      }
    },

  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="card">
      <div class="card-body">
        <form ref="formContainer">
          <div class="col-sm-12">
            <div class="text-sm-end">
              <b-button
                  v-if="$can('manage-HEADER-654863bd4e06313c529c0109') || $can('create-HEADER-654863bd4e06313c529c0109') || $can('update-HEADER-654863bd4e06313c529c0109')"
                  type="button"
                  class="btn cs-btn-primary btn-rounded mb-2 me-2"
                  size="sm"
                  @click="showModal = true"
              >
                <i class="mdi mdi-plus me-1"></i> Cập nhật Header
              </b-button>
              <b-modal
                  v-model="showModal"
                  title="Cập nhật Header"
                  title-class="text-black font-18"
                  body-class="p-3"
                  hide-footer
                  centered
                  no-close-on-backdrop
                  size="lg"
              >
                <form @submit.prevent="handleSubmitHeader"
                      ref="formContainer"
                >
                  <div class="row">
                    <div class="col-12" >
                        <label class="text-left">Chọn ảnh kích thước 1920 x 300</label>
                        <vue-dropzone
                            id="dropzone"
                            ref="myVueDropzone"
                            :use-custom-slot="true"
                            :options="dropzoneOptions"
                            v-on:vdropzone-sending="sendingEvent"
                            v-on:vdropzone-removed-file="removeThisFile"
                            v-on:vdropzone-success="uploadFileHeader"
                        >
                          <div class="dropzone-custom-content">
                            <div class="mb-1">
                              <i class="display-4 text-muted bx bxs-cloud-upload"></i>
                            </div>
                            <h4>Kéo thả tệp hoặc click vào đây để tải tệp tin.</h4>
                          </div>
                        </vue-dropzone>
                      </div>

                  </div>
                  <div class="text-end pt-2 mt-3">
                    <b-button variant="light" @click="showModal = false" class="border-0">
                      Đóng
                    </b-button>
                    <b-button  type="submit" variant="success" class="ms-1 cs-btn-primary">Lưu
                    </b-button>
                  </div>
                </form>
              </b-modal>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mt-4">
                  <div class="col-sm-12 col-md-6">
                    <div
                        class="col-sm-12 d-flex justify-content-left align-items-center"
                    >

                    </div>
                  </div>
                </div>
                <div class="table-responsive mb-0">
                  <b-table
                      class="datatables table-admin"
                      :items="listFileHeader"
                      :fields="fields"
                      striped
                      bordered
                      responsive="sm"
                      :per-page="perPage"
                      :current-page="currentPage"
                      :sort-by.sync="sortBy"
                      :sort-desc.sync="sortDesc"
                      :filter="filter"
                      :filter-included-fields="filterOn"
                      ref="tblListHeader"
                      primary-key="id"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <template v-slot:cell(file)="data">
                      <b-card-img
                          :src="url + data.item.file.fileId"
                          class="rounded-0 image-pakn"
                          :height="300"
                          :width="300"
                      ></b-card-img>
                    </template>
                      <template v-slot:cell(process)="data">
                        <button
                            v-if="$can('manage-HEADER-654863bd4e06313c529c0109') || $can('delete-HEADER-654863bd4e06313c529c0109')"
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm"
                            v-on:click="handleShowDeleteModalHeader(data.item._id)"
                        >
                          <i class="fas fa-trash-alt text-danger me-1"></i>
                        </button>
                    </template>
                  </b-table>
                </div>


              </div>
            </div>
          </div>

          <!-- <div class="col-sm-12 mt-4">
            <div class="text-sm-end">
              <b-button
                  type="button"
                  class="btn cs-btn-primary btn-rounded me-2"
                  size="sm"
                  @click="showModalSlider = true"
              >
                <i class="mdi mdi-plus me-1"></i> Cập nhật Slider
              </b-button>
              <b-modal
                  v-model = "showModalSlider"
                  title="Cập nhật Slider"
                  title-class="text-black font-18"
                  body-class="p-3"
                  hide-footer
                  centered
                  no-close-on-backdrop
                  size="lg"
              >
                <form @submit.prevent="handleSubmitSlider"
                      ref="formContainer"
                >
                  <div class="row">
                    <div class="col-12" >
                      <label class="text-left">Chọn ảnh </label>

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

                  </div>
                  <div class="text-end pt-2 mt-3">
                    <b-button variant="light" @click="showModal = false" class="border-0">
                      Đóng
                    </b-button>
                    <b-button  type="submit" variant="success" class="ms-1 cs-btn-primary">Lưu
                    </b-button>
                  </div>
                </form>
              </b-modal>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mt-4">
                  <div class="col-sm-12 col-md-6">
                    <div
                        class="col-sm-12 d-flex justify-content-left align-items-center"
                    >

                    </div>
                  </div>
                </div>
                <div class="table-responsive mb-0">
                  <b-table
                      class="datatables"
                      :items="listFileSilder"
                      :fields="fields"
                      striped
                      bordered
                      responsive="sm"
                      :per-page="perPage"
                      :current-page="currentPage"
                      :sort-by.sync="sortBy"
                      :sort-desc.sync="sortDesc"
                      :filter="filter"
                      :filter-included-fields="filterOn"
                      ref="tblListSlider"
                      primary-key="id"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <template v-slot:cell(file)="data">
                      <b-card-img
                          :src="url + data.item.file.fileId"
                          class="rounded-0 image-pakn"
                          :height="300"
                          :width="300"
                      ></b-card-img>
                    </template>
                    <template v-slot:cell(process)="data">
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          v-on:click="handleShowDeleteModal(data.item._id)"
                      >
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>


              </div>
            </div>
          </div> -->

        </form>
        <b-modal
            v-model="showDeleteModal"
            centered
            title="Xóa dữ liệu"
            title-class="font-18"
            no-close-on-backdrop
        >
          <p>
            Dữ liệu xóa sẽ không được phục hồi!
          </p>
          <template #modal-footer>
            <button v-b-modal.modal-close_visit
                    class="btn btn-outline-info m-1"
                    v-on:click="showDeleteModal = false">
              Đóng
            </button>
            <button v-b-modal.modal-close_visit
                    class="btn btn-danger btn m-1"
                    v-on:click="handleDelete">
              Xóa
            </button>
          </template>
        </b-modal>
        <b-modal
            v-model="showDeleteHeader"
            centered
            title="Xóa dữ liệu"
            title-class="font-18"
            no-close-on-backdrop
        >
          <p>
            Dữ liệu xóa sẽ không được phục hồi!
          </p>
          <template #modal-footer>
            <button v-b-modal.modal-close_visit
                    class="btn btn-outline-info m-1"
                    v-on:click="showDeleteHeader = false">
              Đóng
            </button>
            <button v-b-modal.modal-close_visit
                    class="btn btn-danger btn m-1"
                    v-on:click="handleDeleteHeader">
              Xóa
            </button>
          </template>
        </b-modal>
      </div>
    </div>
  </Layout>
</template>
